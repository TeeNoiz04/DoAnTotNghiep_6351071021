import { CoreModule } from '@abp/ng.core';
import { CommonModule } from '@angular/common';
import { Component, inject, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { PriceOfferDetailViewService } from '@app/price-offers/services/price-offer-detail.service';
import { StatusLabelComponent } from '@app/shared/status/components/status-label.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { PriceOfferImportDto } from '@proxy/price-offers';
import { PriceOfferCustomerImportDto } from '@proxy/price-offers/price-offer-customers';
import { ExcelValidationResult } from '@proxy/shared/excels';

@Component({
  selector: 'app-customer-information-table',
  templateUrl: './customer-information-table.component.html',
  standalone: true,
  styleUrls: ['./customer-information-table.component.scss'],
  imports: [FormsModule, NgbModule, NgSelectModule, MatCheckboxModule, CoreModule, CommonModule, StatusLabelComponent],
})
export class CustomerInformationTableComponent implements OnInit, OnChanges {
  @Input() resultImport: ExcelValidationResult<PriceOfferImportDto> | undefined;
  @Input() enableErrorHandling: boolean = false;
  @Input() useServiceData: boolean = true;

  customers: ExcelValidationResult<PriceOfferCustomerImportDto> | undefined;
  public readonly service = inject(PriceOfferDetailViewService);
  expandedRows: { [key: number]: boolean } = {};

  ngOnInit(): void {
    if (this.useServiceData) {
      this.service.buildDetailForm();
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (!this.useServiceData && changes['resultImport'] && this.resultImport) {
      if (this.resultImport.singleRow) {
        this.customers = this.resultImport.listData[0].rowData?.customers;

        if (this.customers?.listData) {
          this.customers.listData = this.customers.listData.map(item => {
            return {
              ...item,
              status: item.hasErrors ? 'INVALID' : 'VALID',
            };
          });
        }
      }
    }
  }

  hasAnyErrors(): boolean {
    if (!this.enableErrorHandling) return false;
    return (this.customers?.listData || []).some(row => row.hasErrors);
  }

  countErrorRows(): number {
    if (!this.enableErrorHandling) return 0;
    return (this.customers?.listData || []).filter(row => row.hasErrors).length;
  }

  toggleRowExpansion(row: any): void {
    if (!this.enableErrorHandling || !row.hasErrors) return;

    const rowIndex = row.rowIndex || (this.customers?.listData || []).indexOf(row);
    this.expandedRows[rowIndex] = !this.expandedRows[rowIndex];
  }

  formatCellValue(field: string, row: any): string {
    if (!row) return '';
    const value = this.useServiceData ? row[field] : row.rowData?.[field];

    if (this.enableErrorHandling && row.hasErrors) {
      const hasPropertyError = row.errors?.some((error: string) => error.toLowerCase().includes(field.toLowerCase()));

      if (hasPropertyError) {
        return `<span class="error-indicator">${value || ''}
          <i class="bi bi-exclamation-circle-fill text-danger ms-1"></i>
        </span>`;
      }
    }

    return value || '';
  }

  getComponentData(field: string, row: any): any {
    if (!row) return '';
    const value = this.useServiceData ? row[field] : row[field];
    if (value === '' || value === null || value === undefined) {
      return '';
    }

    if (field === 'status') {
      return value;
    }

    return value;
  }

  getCellClass(field: string): string {
    if (field === 'customerTaxCode') {
      return 'text-center';
    }
    return 'text-start';
  }

  getDataSource(): any[] {
    return this.useServiceData ? this.service.customers?.items || [] : this.customers?.listData || [];
  }
}
