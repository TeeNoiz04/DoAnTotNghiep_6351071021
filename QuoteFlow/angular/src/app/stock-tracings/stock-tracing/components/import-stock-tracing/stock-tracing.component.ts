import { PageModule } from '@abp/ng.components/page';
import { CoreModule, ListService } from '@abp/ng.core';
import { DateAdapter, ThemeSharedModule, TimeAdapter } from '@abp/ng.theme.shared';
import { ChangeDetectionStrategy, Component, ViewChild } from '@angular/core';
import { ErrorDisplayComponent } from '@app/shared/components/error-display/error-display.component';
import { AuditInfoColumnComponent } from '@app/shared/components/audit-info-column';
import {
  NgbCollapseModule,
  NgbDateAdapter,
  NgbDatepickerModule,
  NgbDropdownModule,
  NgbTimeAdapter,
  NgbTimepickerModule,
  NgbTooltip,
} from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { ReportTypePipe } from '@shared/pipes/report-type.pipe';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { StockTracingDetailViewService } from '../../services/stock-tracing-detail.service';
import { StockTracingViewService } from '../../services/stock-tracing.service';
import { ImportStockComponent } from './import-stock/import-stock.component';
import { ResultImportStockTracingComponent } from './result-import-stock-tracing/result-import-strock-tracing.component';
import { StockTracingFilterComponent } from './stock-tracing-filter/stock-tracing-filter.component';
import {
  AbstractStockTracingComponent,
  ChildComponentDependencies,
  ChildTabDependencies,
} from './stock-tracing.abstract.component';
import { ImportStockTracingType } from './stock-tracing.types';

@Component({
  selector: 'app-stock-tracing',
  changeDetection: ChangeDetectionStrategy.Default,
  standalone: true,
  imports: [
    ...ChildTabDependencies,
    NgbCollapseModule,
    NgbDatepickerModule,
    NgbTimepickerModule,
    NgbDropdownModule,
    NgxValidateCoreModule,
    PageModule,
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    StockTracingFilterComponent,
    ReportTypePipe,
    ImportStockComponent,
    ResultImportStockTracingComponent,
    ErrorDisplayComponent,
    AuditInfoColumnComponent,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [
    ListService,
    StockTracingViewService,
    StockTracingDetailViewService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './stock-tracing.component.html',
  styleUrls: ['./stock-tracing.component.scss'],
})
export class StockTracingComponent extends AbstractStockTracingComponent {
  @ViewChild('importStockTracing') importStockTracing: ImportStockComponent | undefined;
  verifyData() {
    const file = this.importStockTracing?.fileImport;
    const values = this.importStockTracing?.importForm.getRawValue();

    const fromDate = values?.fromDate;
    const toDate = values?.toDate;
    const entered = values?.entered;
    const note = values?.note;

    this.resultImportDelivery = undefined;
    this.resultImportInventory = undefined;
    this.resultImportReceipt = undefined;

    if (this.importMode !== ImportStockTracingType.Inventory) {
      if (!file || !(file instanceof File) || !fromDate || !toDate) {
        this.toast.error('Please select a file and fill in all required fields before submitting.');
        return;
      }

      const from = new Date(fromDate);
      const to = new Date(toDate);

      if (from >= to) {
        this.toast.error("The 'from date' must be earlier than the 'to date'.");
        return;
      }
    }
    if (this.importMode == ImportStockTracingType.Inventory) {
      if (!entered) {
        this.toast.error('Please select a file and fill in all required fields before submitting.');
        return;
      }
    }

    this.stockTracingInformation = {
      file: file,
      fromDate: fromDate,
      toDate: toDate,
      entered: entered,
      note: note,
    };

    this.loadingService.show();
    const formData = new FormData();
    formData.append('file', file);

    switch (this.importMode) {
      case ImportStockTracingType.Delivery:
        this.proxyService.validateAndStockTracingDelivery(formData, fromDate, toDate, note).subscribe({
          next: result => {
            result.listData = result.listData.map(row => ({
              ...row,
              rowData: {
                ...row.rowData,
                productionDate: this.formatDate(row.rowData.productionDate),
              },
            }));
            this.resultImportDelivery = result;
            this.showResultImportStockTracing = true;
            this.loadingService.hide();
          },
          error: this.handleImportError.bind(this),
        });
        break;

      case ImportStockTracingType.Inventory:
        this.proxyService.validateAndStockTracingInventory(formData, entered, note).subscribe({
          next: result => {
            result.listData = result.listData.map(row => ({
              ...row,
              rowData: {
                ...row.rowData,
                productionDate: this.formatDate(row.rowData.productionDate),
              },
            }));

            this.resultImportInventory = result;
            this.showResultImportStockTracing = true;
            this.loadingService.hide();
          },
          error: this.handleImportError.bind(this),
        });
        break;

      case ImportStockTracingType.Receipt:
        this.proxyService.validateAndStockTracingReceipt(formData, fromDate, toDate, note).subscribe({
          next: result => {
            result.listData = result.listData.map(row => ({
              ...row,
              rowData: {
                ...row.rowData,
                productionDate: this.formatDate(row.rowData.productionDate),
              },
            }));
            this.resultImportReceipt = result;
            this.showResultImportStockTracing = true;
            this.loadingService.hide();
          },
          error: this.handleImportError.bind(this),
        });
        break;

      default:
        this.toast.error('Invalid import type.');
        this.loadingService.hide();
        break;
    }
  }

  private handleImportError(error: any) {
    console.error('Error during import:', error);
    this.loadingService.hide();
    this.resultImportDelivery = undefined;
    this.resultImportInventory = undefined;
    this.resultImportReceipt = undefined;
    this.toast.error('Failed to process the file. Please check the file format and try again.');
  }

  submitting = false;
  onSubmitImport() {
    this.submitting = true;
    const values = this.importStockTracing?.importForm.getRawValue();
    switch (this.importMode) {
      case ImportStockTracingType.Delivery:
        {
          this.proxyService
            .importStockTracingDelivery(this.resultImportDelivery, values.fromDate, values.toDate, values.note)
            .subscribe({
              next: () => {
                this.toast.success('Stock tracing imported successfully.');
                this.resultImportDelivery = undefined;
                this.showImportStock = false;
                this.showResultImportStockTracing = false;
                this.submitting = false;
                this.service.hookToQuery().subscribe();
              },
              error: () => {
                this.submitting = false;
                this.toast.error('Failed to import Stock tracing. Please try again.');
              },
            });
        }
        break;
      case ImportStockTracingType.Receipt:
        {
          this.proxyService
            .importStockTracingReceipt(this.resultImportReceipt, values.fromDate, values.toDate, values.note)
            .subscribe({
              next: () => {
                this.toast.success('Stock tracing imported successfully.');
                this.resultImportReceipt = undefined;
                this.showImportStock = false;
                this.showResultImportStockTracing = false;
                this.submitting = false;
                this.service.hookToQuery().subscribe();
              },
              error: () => {
                this.submitting = false;
                this.toast.error('Failed to import Stock tracing. Please try again.');
              },
            });
        }
        break;
      case ImportStockTracingType.Inventory:
        {
          this.proxyService
            .importStockTracingInvantory(this.resultImportInventory, values.entered, values.note)
            .subscribe({
              next: () => {
                this.toast.success('Stock tracing imported successfully.');
                this.resultImportInventory = undefined;
                this.showImportStock = false;
                this.showResultImportStockTracing = false;
                this.submitting = false;
                this.service.hookToQuery().subscribe();
              },
              error: () => {
                this.submitting = false;
                this.toast.error('Failed to import Stock tracing. Please try again.');
              },
            });
        }
        break;
      default:
        this.submitting = false;
        break;
    }
  }
  private formatDate(dateString?: string): string | undefined {
    if (!dateString) return undefined;
    const date = new Date(dateString);
    if (isNaN(date.getTime())) return dateString;

    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }
}
