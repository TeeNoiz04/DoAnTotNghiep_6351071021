import { AbpWindowService, ListService, PagedResultDto } from '@abp/ng.core';
import { ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { DatePipe } from '@angular/common';
import { inject } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { PSIService, PSIExportDataDto, PSIByProductExportInput } from '@proxy/psis';
import { finalize } from 'rxjs';

export abstract class AbstractPsiReportViewService {
  protected readonly list = inject(ListService);
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly loadingService = inject(LoadingService);
  protected readonly fb = inject(FormBuilder);
  public readonly toast = inject(ToasterService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly abpWindowService = inject(AbpWindowService);
  protected readonly datePipe = inject(DatePipe);
  public readonly proxyService = inject(PSIService);

  isExportToExcelBusy = false;

  data: PSIExportDataDto[] = [];

  filters: PSIByProductExportInput = {
    materialType: '',
    fiscalYear: '',
    maxResultCount: DEFAULT_PAGE_SIZE,
    skipCount: 0,
  };

  formatDate(datetime: string, format: string = 'yyyy-MM-dd'): string {
    return datetime ? this.datePipe.transform(datetime, format) : null;
  }

  clearFilters() {
    this.filters = {
      materialType: '',
      fiscalYear: '',
      maxResultCount: DEFAULT_PAGE_SIZE,
      skipCount: 0,
    };
    this.data = [];
  }

  loadData(filters: PSIByProductExportInput) {
    this.loadingService.show();
    this.filters = { ...this.filters, ...filters };

    this.proxyService
      .getListPSIReportData(this.filters)
      .pipe(finalize(() => this.loadingService.hide()))
      .subscribe({
        next: (result: PSIExportDataDto[]) => {
          this.data = result;
        },
        error: error => {
          console.error('Error loading PSI Report:', error);
          this.toast.error('Failed to load report data. Please try again.');
        },
      });
  }

  exportToExcel(filters: PSIByProductExportInput): void {
    if (this.isExportToExcelBusy) {
      this.toast.error('Export is already in progress. Please wait.');
      return;
    }

    this.isExportToExcelBusy = true;

    const exportFilters = {
      materialType: filters.materialType,
      fiscalYear: filters.fiscalYear,
    } as PSIByProductExportInput;

    this.proxyService
      .getPSIByProductExport(exportFilters)
      .pipe(finalize(() => (this.isExportToExcelBusy = false)))
      .subscribe({
        next: (blob: Blob) => {
          const now = new Date();
          const dd = String(now.getDate()).padStart(2, '0');
          const mm = String(now.getMonth() + 1).padStart(2, '0');
          const yy = String(now.getFullYear()).slice(-2);
          this.abpWindowService.downloadBlob(blob, `PSI-Report-${dd}${mm}${yy}.xlsx`);
          this.toast.success('Report exported successfully');
        },
        error: error => {
          console.error('Export failed:', error);
          this.toast.error('Failed to export report. Please try again.');
        },
      });
  }
}
