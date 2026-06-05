import { AbpWindowService, ListService, PermissionService, TrackByService } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Directive, inject, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { TitleService } from '@app/shared/services/title/title.service';
import { PsiReportViewService } from '../../services/psi-report.service';
import { finalize } from 'rxjs';
import { PsiReportFilterComponent } from './psi report filter/psi-report-filter.component';

export const ChildTabDependencies = [];
export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractPSIReportComponent implements OnInit {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(PsiReportViewService);
  public readonly permissionService = inject(PermissionService);
  protected readonly router = inject(Router);
  protected readonly toast = inject(ToasterService);
  protected readonly loadingService = inject(LoadingService);
  public readonly titleService = inject(TitleService);
  public readonly abpWindowService = inject(AbpWindowService);

  pageNumber = 0;
  pageSize = DEFAULT_PAGE_SIZE;
  pagedData = [];
  protected title = 'PSI Report by Product';

  currentMaterialType: string = 'FA';

  @ViewChild('psiReportFilter') psiReportFilter: PsiReportFilterComponent | undefined;

  ngOnInit() {
    this.titleService.setTitle(this.title);
  }

  clearFilters() {
    if (!this.psiReportFilter) {
      return;
    }
    this.psiReportFilter.form.reset();
    this.service.clearFilters();
    this.pageNumber = 0;
    this.currentMaterialType = 'FA';
    this.updatePagedData();
  }

  onSearch() {
    if (!this.psiReportFilter) {
      return;
    }

    const values = this.psiReportFilter.form.getRawValue();

    this.currentMaterialType = values.materialType || 'FA';
    console.log('🔍 Search with Material Type:', this.currentMaterialType);

    this.service.filters = {
      fiscalYear: values.fiscalYear,
      materialType: values.materialType,
      skipCount: 0,
      maxResultCount: this.pageSize,
    };

    this.loadingService.show();

    this.service.proxyService.getListPSIReportData(this.service.filters).subscribe({
      next: res => {
        this.service.data = res;
        this.pageNumber = 0;
        this.updatePagedData();
        this.loadingService.hide();
      },
      error: err => {
        console.error('Error loading PSI Report:', err);
        this.toast.error('Failed to load PSI Report. Please try again.');
        this.loadingService.hide();
      },
    });
  }

  exportToExcel(): void {
    this.service.isExportToExcelBusy = true;
    const values = this.psiReportFilter.form.getRawValue();

    const exportFilter = {
      fiscalYear: values.fiscalYear,
      materialType: values.materialType,
      skipCount: 0,
      maxResultCount: DEFAULT_PAGE_SIZE,
    };

    this.service.proxyService
      .getPSIByProductExport(exportFilter)
      .pipe(finalize(() => (this.service.isExportToExcelBusy = false)))
      .subscribe({
        next: (blob: Blob) => {
          this.abpWindowService.downloadBlob(blob, `PSI_Report_${this.formatExportFileName()}.xlsx`);
          this.toast.success('Report exported successfully');
        },
        error: error => {
          console.error('Export failed:', error);
          this.toast.error('Failed to export report. Please try again.');
        },
      });
  }

  private formatExportFileName(): string {
    const now = new Date();
    const dateStr = now.toISOString().split('T')[0];
    return `${dateStr}`;
  }

  onPage(event: any) {
    this.pageNumber = event.offset;

    if (!this.psiReportFilter) {
      return;
    }

    const values = this.psiReportFilter.form.getRawValue();

    this.currentMaterialType = values.materialType || 'FA';

    const filters = {
      fiscalYear: values.fiscalYear,
      materialType: values.materialType,
      skipCount: this.pageNumber * this.pageSize,
      maxResultCount: this.pageSize,
    };

    this.loadingService.show();

    // Use getListPSIReportData from PSIService
    this.service.proxyService.getListPSIReportData(filters).subscribe({
      next: res => {
        this.service.data = res;
        this.updatePagedData();
        this.loadingService.hide();
      },
      error: err => {
        console.error('Error loading PSI Report:', err);
        this.loadingService.hide();
      },
    });
  }

  updatePagedData() {
    this.pagedData = Array.isArray(this.service.data) ? this.service.data : [];
  }

  get totalCount(): number {
    return Array.isArray(this.service.data) ? this.service.data.length : 0;
  }

  get hasData(): boolean {
    return this.pagedData && this.pagedData.length > 0;
  }

  get Math() {
    return Math;
  }
}
