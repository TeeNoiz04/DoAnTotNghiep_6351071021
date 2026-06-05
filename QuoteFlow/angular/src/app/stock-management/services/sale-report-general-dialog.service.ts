import { AbpWindowService } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { SaleReportInput, SalesAssignmentService } from '@proxy/sales-assignments';
import { finalize, Observable, of } from 'rxjs';
import { DialogService } from '../../shared/services/dialog/dialog.service';
import { SaleReportGeneralDialogComponent } from '../components/sale-report-general-dialog/sale-report-general-dialog.component';

interface SaleReportFilter {
  fromDate?: string;
  toDate?: string;
  invoiceFromDate?: string;
  invoiceToDate?: string;
}

@Injectable({
  providedIn: 'root',
})
export class SaleReportGeneralDialogService {
  private readonly dialogService = inject(DialogService);
  private readonly router = inject(Router);
  private readonly toast = inject(ToasterService);
  private readonly service = inject(SalesAssignmentService);
  protected readonly abpWindowService = inject(AbpWindowService);

  private isExportToExcelBusy = false;

  openSaleReportDialog(): void {
    this.dialogService
      .open(SaleReportGeneralDialogComponent, {
        title: 'Sale Report (R05)',
        size: 'lg',
        confirmBtnLabel: null,
      })
      .subscribe({
        next: () => {
          // Modal closed - result will contain final data if modal was closed normally
        },
        error: () => {
          // Handle dialog errors silently or log to external service
        },
      });
  }

  getReportGenerationFunction(): (filterData: SaleReportFilter) => Observable<Blob> {
    return (filterData: SaleReportFilter) => this.performSaleReportGeneration(filterData);
  }

  private performSaleReportGeneration(filterData: SaleReportFilter): Observable<Blob> {
    return this.generateSaleReport(filterData);
  }

  private generateSaleReport(filterData: SaleReportFilter): Observable<Blob> {
    if (this.isExportToExcelBusy) {
      this.toast.error('Export is already in progress. Please wait.');
      return of(null);
    }

    this.isExportToExcelBusy = true;
    const payload = {
      fromDate: filterData.fromDate,
      toDate: filterData.toDate,
      invoiceFromDate: filterData.invoiceFromDate,
      invoiceToDate: filterData.invoiceToDate,
    } as SaleReportInput;
    return this.service.getListSaleReportGeneralAsExcel(payload).pipe(
      finalize(() => {
        this.isExportToExcelBusy = false;
      }),
    );
  }
}
