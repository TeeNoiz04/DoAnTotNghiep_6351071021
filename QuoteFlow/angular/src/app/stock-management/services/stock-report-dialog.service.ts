import { AbpWindowService } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { finalize, Observable, of } from 'rxjs';
import { DialogService } from '../../shared/services/dialog/dialog.service';
import { InventoryReportDialogComponent } from '../components/inventory-report-dialog/inventory-report-dialog.component';
import { StockManagementService } from '@proxy/stock-managements';

interface InventoryReportFilter {
  materialCode?: string;
  inventoryCategory?: string;
  materialGroup?: string;
}

interface StockReportFilter {
  materialType?: string;
  supplierCode?: string;
  materialGroup?: string;
}

@Injectable({
  providedIn: 'root',
})
export class StockReportDialogService {
  private readonly dialogService = inject(DialogService);
  private readonly router = inject(Router);
  private readonly toast = inject(ToasterService);
  private readonly service = inject(StockManagementService);
  protected readonly abpWindowService = inject(AbpWindowService);

  private isExportToExcelBusy = false;

  openInventoryReportDialog(): void {
    this.dialogService
      .open(InventoryReportDialogComponent, {
        title: 'Inventory Report (R15)',
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

  getReportGenerationFunction(): (filterData: InventoryReportFilter) => Observable<Blob> {
    return (filterData: InventoryReportFilter) => this.performInventoryReportGeneration(filterData);
  }

  private performInventoryReportGeneration(filterData: InventoryReportFilter): Observable<Blob> {
    return this.generateInventoryReport(filterData);
  }

  private generateInventoryReport(filterData: InventoryReportFilter): Observable<Blob> {
    if (this.isExportToExcelBusy) {
      this.toast.error('Export is already in progress. Please wait.');
      return of(null);
    }

    this.isExportToExcelBusy = true;
    const payload = {
      materialCode: filterData.materialCode,
      inventoryCategory: filterData.inventoryCategory,
      materialGroup: filterData.materialGroup,
      skipCount: 0,
      maxResultCount: 1000,
    };
    return this.service.getListExcelInventoryReport(payload).pipe(
      finalize(() => {
        this.isExportToExcelBusy = false;
      }),
    );
  }
}
