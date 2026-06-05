import { Directive, OnDestroy, OnInit, ViewChild, inject } from '@angular/core';

import { ListService, PermissionService, TrackByService } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Router } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import { PSIExtendedService } from '@app/proxy-custom/psi/psi-extended.service';
import { PSIViewService } from '@app/psis/services/psi.service';
import { ApproversModalComponent } from '@app/shared/components/approvers-modal/approvers-modal.component';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { downloadBlob } from '@app/shared/helpers/file-helper';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { TitleService } from '@app/shared/services/title/title.service';
import { TemplateService } from '@proxy/general-templates';
import { PSIDto, PSIService } from '@proxy/psis';
import { Observable, Subscription } from 'rxjs';
import { PSIImportModalComponent } from './psi-import-modal/psi-import-modal.component';
import { ImportPSIOptions, ImportPSIType, ImportPSITypeOption, ImportResultMap } from './psi.types';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractPSIComponent implements OnInit, OnDestroy {
  protected readonly router = inject(Router);
  protected readonly loadingService = inject(LoadingService);
  public readonly list = inject(ListService);
  public readonly toast = inject(ToasterService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(PSIViewService);
  public readonly permissionService = inject(PermissionService);
  public readonly proxyService = inject(PSIService);
  public readonly proxyExtendedService = inject(PSIExtendedService);
  public readonly templateService = inject(TemplateService);
  public readonly titleService = inject(TitleService);

  protected title = '::PSI Upload';

  importMode = '';
  importInformation = {};

  showImportPSI = false;
  showResultImport = false;

  showHistoryModal = false;
  approvalHistories = [];

  importPSITypeSelected: ImportPSITypeOption | undefined;
  importPSIOptions: ImportPSITypeOption[] = ImportPSIOptions;
  resultImports: Partial<ImportResultMap> = {};

  private subscriptions: Subscription[] = [];

  private readonly monthNames = new Map<number, string>([
    [1, 'Jan'],
    [2, 'Feb'],
    [3, 'March'],
    [4, 'April'],
    [5, 'May'],
    [6, 'June'],
    [7, 'July'],
    [8, 'August'],
    [9, 'September'],
    [10, 'October'],
    [11, 'November'],
    [12, 'December'],
  ]);

  @ViewChild('approversModalComponent', { static: false }) approversModalComponent: ApproversModalComponent;
  @ViewChild('importPSI') importPSI: PSIImportModalComponent | undefined;

  ngOnInit() {
    this.titleService.setTitle('PSI Upload');

    this.list.maxResultCount = DEFAULT_PAGE_SIZE;
    this.service.hookToQuery();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  clearFilters() {
    this.service.clearFilters();
  }

  onSearch() {
    this.list.get();
  }

  performAction(action: string, row: PSIDto) {
    this.service.performAction(action, row);
  }

  getDetailUrl(request: PSIDto): string {
    if (request?.id) {
      return this.router.serializeUrl(
        this.router.createUrlTree([
          AppRoutes.PSI.BASE,
          AppRoutes.DETAILS_WITH_ID(request.id),
          AppRoutes.PSI.DETAILS.BASE,
        ]),
      );
    } else {
      console.error('Unknown key account:', request.id);
      return '';
    }
  }

  showHistory(record: PSIDto) {
    this.showHistoryModal = true;
    this.approvalHistories = record?.approvalHistories;
  }

  closeHistoryDialog(): void {
    this.showHistoryModal = false;
    this.approvalHistories = [];
  }

  exportToExcel() {
    this.service.exportToExcel();
  }

  onCloseImportModal(): void {
    this.showImportPSI = false;
    this.importPSI?.importForm.reset();
  }

  onCloseImportResultModal(): void {
    this.loadingService.hide();
    this.showResultImport = false;
    this.resultImports = {} as Partial<ImportResultMap>;
    this.importPSI?.clearSelectedFile();
  }

  transformDetailsListData(detailsListData: any[], monthNames: Map<number, string>) {
    // Step 1: Build the map
    const aggregated = detailsListData.reduce(
      (acc, { rowData, errors }) => {
        const { materialGroup, month, plan } = rowData;
        const key = materialGroup;
        if (!acc[key]) acc[key] = { materialGroup: key, errors: [] };

        // Collect errors for each group
        acc[key]?.errors?.push(...errors);

        const monthName = monthNames.get(month);
        if (monthName) acc[key][monthName] = plan ?? 0;

        return acc;
      },
      {} as Record<string, any>,
    );

    // Step 2: Initialize totals
    const monthlyTotals: Record<string, any> = { materialGroup: 'Total Amount (Mil VNĐ)' };
    for (const name of this.monthNames.values()) {
      monthlyTotals[name] = 0;
    }

    // Step 3: Calculate totals
    Object.values(aggregated).forEach((row: any) => {
      for (const name of this.monthNames.values()) {
        monthlyTotals[name] += row[name] ?? 0;
      }
    });

    // Step 4: Convert map to array and insert total at the beginning
    const result = [monthlyTotals, ...Object.values(aggregated)];

    return Object.values(result);
  }

  verifyData() {
    const file = this.importPSI?.fileImport;
    const values = this.importPSI?.importForm.getRawValue();

    const { financeYear, note } = values || {};
    var materialType = this.importPSITypeSelected?.label;

    if (!file || !(file instanceof File) || !financeYear || !note || !materialType) {
      this.toast.error('Please select a file and fill in all required fields before submitting.');
      return;
    }

    this.importInformation = { file, note, financeYear, materialType };

    this.loadingService.show();
    const formData = new FormData();
    formData.append('file', file);

    const importHandlers = {
      [ImportPSIType.PSI_FA]: () =>
        this.proxyExtendedService.validateAndParseFAManual(file, { fy: financeYear, note, materialType }),
      [ImportPSIType.PSI_LVS]: () =>
        this.proxyExtendedService.validateAndParseLVSManual(file, { fy: financeYear, note, materialType }),
    };

    const handler = importHandlers[this.importMode];

    if (!handler) {
      this.toast.error('Invalid import type.');
      this.loadingService.hide();
      return;
    }

    handler().subscribe({
      next: result => {
        if (result?.listData?.length) {
          result.listData = result.listData.map((row: any) => {
            const detailsListData = row?.rowData?.details?.listData;
            const rows = detailsListData ? this.transformDetailsListData(detailsListData, this.monthNames) : [];
            return { ...row, rows };
          });

          const flattenedRows = result?.listData?.[0] || [];

          if (flattenedRows?.rows?.length > 0) {
            result.listData = flattenedRows?.rows?.map((item: any, index: number): any => ({
              errors: item?.errors || [],
              hasErrors: item?.errors?.length > 0,
              rowData: {
                ...item,
                ...flattenedRows?.rowData,
              },
              rowIndex: index + 1,
            }));
          }
        }

        this.resultImports[this.importMode] = result;
        this.showResultImport = true;

        this.loadingService.hide();
      },
      error: this.handleImportError.bind(this),
    });
  }

  onBackFromResult() {
    // Clear the result import data
    this.resultImports = undefined;
    this.showResultImport = false;

    // Clear the file from the import form to prevent errors
    if (this.importPSI) {
      this.importPSI?.clearSelectedFile();
    }
  }

  onSubmitImport() {
    // const values = this.importPSI?.importForm.getRawValue();
    const resultImport = this.resultImports[this.importMode];

    const importHandlers: Partial<Record<ImportPSIType, () => Observable<any>>> = {
      [ImportPSIType.PSI_FA]: () => this.proxyService.importFA(resultImport),
      [ImportPSIType.PSI_LVS]: () => this.proxyService.importLVS(resultImport),
    };

    const requestFn = importHandlers[this.importMode];

    if (!requestFn) {
      this.toast.error('Invalid import type.');
      return;
    }

    requestFn().subscribe({
      next: () => {
        this.toast.success('PSI imported successfully.');
        this.resultImports = {} as Partial<ImportResultMap>;
        this.showImportPSI = false;
        this.showResultImport = false;
        this.service.hookToQuery();
      },
      error: () => {
        this.toast.error('Failed to import PSI. Please try again.');
      },
    });
  }

  private handleImportError() {
    this.loadingService.hide();
    this.resultImports = {} as Partial<ImportResultMap>;
    this.toast.error('Failed to process the file. Please check the file format and try again.');
  }

  onOpenImportPSI(val: ImportPSITypeOption) {
    this.importMode = val.value;
    this.importPSITypeSelected = val;
    this.showImportPSI = true;
  }

  onOpenDownloadPSI(option: ImportPSITypeOption) {
    this.templateService.getTemplatePSI(option.value).subscribe({
      next: (blob: Blob) => downloadBlob(blob, `Template_${option.value}.xlsx`),
      error: err => console.error('Error downloading template:', err),
    });
  }

  loadApprovalList(rowData: PSIDto) {
    if (rowData?.id) {
      this.service.getListApprovers(rowData.id).subscribe({
        next: approvers => {
          this.approversModalComponent.openModal(approvers);
        },
        error: () => {
          this.toast.error('Failed to load approvers.');
        },
      });
    }
  }
}
