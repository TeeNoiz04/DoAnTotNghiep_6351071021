import { PageModule } from '@abp/ng.components/page';
import { AbpWindowService, CoreModule, ListService, TrackByService } from '@abp/ng.core';
import { DateAdapter, ThemeSharedModule, TimeAdapter, ToasterService } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AppPermissions } from '@app/app.permissions';
import { LookupService } from '@app/proxy/general-lookups';
import { ExcelValidationResult } from '@app/proxy/shared/excels/models';
import { SpecialInputPriceDetailImportDto } from '@app/proxy/special-input-prices/models';
import { AuditInfoColumnComponent } from '@app/shared/components/audit-info-column';
import { ErrorDisplayComponent } from '@app/shared/components/error-display/error-display.component';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { UsernamePipe } from '@app/shared/pipes/username.pipe';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { TitleService } from '@app/shared/services/title/title.service';
import { StatusLabelComponent } from '@app/shared/status/components/status-label.component';
import {
  NgbCollapseModule,
  NgbDateAdapter,
  NgbDatepickerModule,
  NgbDropdownModule,
  NgbModal,
  NgbTimeAdapter,
  NgbTimepickerModule,
  NgbTooltip,
  NgbTooltipModule,
} from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { TemplateService } from '@proxy/general-templates';
import { SpecialInputPriceService } from '@proxy/special-input-prices';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { finalize } from 'rxjs';
import { SpecialInputPriceFilterService } from '../services/special-input-price-filter.service';
import { AddSpecialInputPriceModalComponent } from './add-special-input-price-modal/add-special-input-price-modal.component';
import {
  EditSpecialInputPriceData,
  EditSpecialInputPriceModalComponent,
} from './edit-special-input-price-modal/edit-special-input-price-modal.component';
import { ImportSpecialInputPriceModalComponent } from './import-special-input-price-modal/import-special-input-price-modal.component';
import { ResultImportSpecialInputPriceComponent } from './result-import-special-input-price/result-import-special-input-price.component';
import { SpecialInputPriceFilterComponent } from './special-input-price-filter/special-input-price-filter.component';

@Component({
  selector: 'app-special-input-price',
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    CommonModule,
    NgbCollapseModule,
    NgbDatepickerModule,
    NgbTimepickerModule,
    NgbDropdownModule,
    NgbTooltipModule,
    NgxValidateCoreModule,
    PageModule,
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    NgSelectModule,
    ErrorDisplayComponent,
    SpecialInputPriceFilterComponent,
    ImportSpecialInputPriceModalComponent,
    ResultImportSpecialInputPriceComponent,
    AddSpecialInputPriceModalComponent,
    EditSpecialInputPriceModalComponent,
    UsernamePipe,
    StatusLabelComponent,
    AuditInfoColumnComponent,
    NgbTooltip,
  ],
  providers: [
    ListService,
    LookupService,
    LoadingService,
    SpecialInputPriceService,
    SpecialInputPriceFilterService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './special-input-price.component.html',
  styleUrls: [`special-input-price.component.scss`],
  standalone: true,
})
export class SpecialInputPriceComponent implements OnInit {
  protected readonly modal = inject(NgbModal);
  protected readonly list = inject(ListService);
  protected readonly lookupService = inject(LookupService);
  protected readonly loadingService = inject(LoadingService);
  protected readonly specialInputPriceService = inject(SpecialInputPriceService);
  protected readonly toast = inject(ToasterService);
  public readonly track = inject(TrackByService);
  protected readonly fb = inject(FormBuilder);
  protected readonly titleService = inject(TitleService);
  protected readonly filterService = inject(SpecialInputPriceFilterService);
  public readonly templateService = inject(TemplateService);
  protected readonly abpWindowService = inject(AbpWindowService);

  @ViewChild('importSpecialInputPrice') importSpecialInputPrice: ImportSpecialInputPriceModalComponent | undefined;

  // Modal states
  showImportModal = false;
  showAddModal = false;
  showEditModal = false;
  editSpecialInputPriceData: EditSpecialInputPriceData | null = null;
  showResultImportModal = false;
  AppPermissions = AppPermissions;

  // Import workflow state
  importBusy = false;
  submitBusy = false;
  resultImport: ExcelValidationResult<SpecialInputPriceDetailImportDto> | undefined;

  title = 'Special Input Price';

  ngOnInit() {
    this.titleService.setTitle(this.title);
    this.list.maxResultCount = DEFAULT_PAGE_SIZE;
    // Initialize the filter service
    this.filterService.initialize({
      buildForm: true,
      syncFromQuery: true,
      autoSearch: false,
    });

    // Hook to query parameters
    this.filterService.hookToQuery();
  }

  // Data access through filter service
  get data() {
    return this.filterService.data;
  }

  // Search method
  onSearch() {
    this.filterService.search();
  }

  // Navigation method for Smart Back Button integration
  onViewDetails(id: string) {
    this.filterService.navigateToDetail(id, 'details');
  }

  // Import methods
  onImport() {
    this.showImportModal = true;
  }

  isExportBusy = false;
  onExport() {
    if (this.isExportBusy) return;
    this.isExportBusy = true;

    if (this.filterService.searchForm) {
      this.filterService['filters'] = this.filterService['mapFormToFilters'](
        this.filterService.searchForm.getRawValue(),
      );
    }

    const filterExport = {
      ...this.filterService.filters,
      skipCount: null,
      maxResultCount: null,
    };

    this.specialInputPriceService
      .getInputPriceAsExcel(filterExport)
      .pipe(
        finalize(() => {
          this.isExportBusy = false;
        }),
      )
      .subscribe({
        next: (blob: Blob) => {
          this.abpWindowService.downloadBlob(blob, `SpecialInputPrices_${this.formatExportFileName()}.xlsx`);
        },
        error: err => console.error('Export failed:', err),
      });
  }

  private formatExportFileName(): string {
    const now = new Date();
    const dateStr = now.toISOString().split('T')[0]; // YYYY-MM-DD
    return `${dateStr}`;
  }

  onImportVerify() {
    const file = this.importSpecialInputPrice?.fileImport;
    const isValidForm = this.importSpecialInputPrice?.importForm.valid;

    if (file && file instanceof File && isValidForm) {
      this.loadingService.show();
      this.importBusy = true;
      const formData = new FormData();
      formData.append('file', file);
      this.specialInputPriceService
        .validateAndParseSpecialInputPriceDetails(formData)
        .pipe(
          finalize(() => {
            this.importBusy = false;
            this.loadingService.hide();
            this.clearFile();
          }),
        )
        .subscribe({
          next: response => {
            this.resultImport = response;
            this.showResultImportModal = true;
            this.showImportModal = false;
          },
          error: () => {
            this.resultImport = undefined;
            this.toast.error('Failed to process the file. Please check the file format and try again.');
          },
        });
    } else {
      this.resultImport = undefined;
      if (!file) {
        this.toast.error('Please select a file before submitting.');
      } else {
        this.toast.error('Please select a valid file.');
        this.importSpecialInputPrice?.importForm.markAllAsTouched();
      }
    }
  }

  onSubmitImport() {
    if (!this.resultImport) {
      this.toast.error('No data to import.');
      return;
    }

    this.loadingService.show();
    this.submitBusy = true;

    this.specialInputPriceService
      .importSpecialInputPriceDetails(this.resultImport)
      .pipe(
        finalize(() => {
          this.submitBusy = false;
          this.loadingService.hide();
        }),
      )
      .subscribe({
        next: () => {
          this.toast.success('Special Input Price imported successfully.');
          this.resultImport = undefined;
          this.showResultImportModal = false;
          this.showImportModal = false;
          this.filterService.search();
        },
        error: () => {
          this.toast.error('Failed to import Special Input Price. Please try again.');
        },
      });
  }

  onBackFromResult() {
    this.showResultImportModal = false;
    this.showImportModal = true;
  }

  // CRUD methods
  onAdd() {
    this.showAddModal = true;
  }

  onEdit(item: any) {
    this.editSpecialInputPriceData = {
      id: item.id,
      accountNo: item.accountNo,
      customerName: item.accountName,
      projectName: item.projectName,
      note: item.note,
      materialType: item.materialType,
      supplierId: item.supplierId,
      supplierBUId: item.supplierBUId,
      currency: item.currency,
      validFrom: item.validFrom,
      validTo: item.validTo,
      status: item.status,
      concurrencyStamp: item.concurrencyStamp,
    };
    this.showEditModal = true;
  }

  onAddSpecialInputPriceConfirmed(data: any) {
    // Handle successful add
    this.toast.success('Special input price added successfully.');
    this.filterService.search(); // Refresh the data
  }

  onEditSpecialInputPriceConfirmed(data: any) {
    // Handle successful edit
    this.toast.success('Special input price updated successfully.');
    this.filterService.search(); // Refresh the data
  }

  delete(item: any) {
    this.filterService.delete(item);
  }

  // Template download
  onDownLoadTemplate() {
    this.templateService.getSpecialInputPriceTemplate().subscribe({
      next: (blob: Blob) => this.downloadBlob(blob, `Template_SpecialInputPrice.xlsx`),
      error: err => console.error('Error downloading template:', err),
    });
  }

  private downloadBlob(blob: Blob, fileName: string) {
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = fileName;
    a.click();
    window.URL.revokeObjectURL(url);
  }

  // Clear filters
  clearFilters() {
    this.filterService.clearFilters();
  }

  private clearFile() {
    // Clear the file from the import form to prevent errors
    if (this.importSpecialInputPrice) {
      this.importSpecialInputPrice.resetFile();
    }
  }
}
