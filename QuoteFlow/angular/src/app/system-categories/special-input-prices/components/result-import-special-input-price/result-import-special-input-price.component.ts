import { CoreModule } from '@abp/ng.core';
import { CommonModule } from '@angular/common';
import { Component, inject, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, FormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { ExcelValidationResult } from '@app/proxy/shared/excels/models';
import { SpecialInputPriceDetailImportDto } from '@app/proxy/special-input-prices/models';
import {
  AppAdvancedDataTableComponent,
  AppTableColumnDirective,
  AppTableColumnGroupDirective,
} from '@app/shared/components/advanced-data-table';
import { NumberHelper } from '@app/shared/helpers/number-helper';
import { TableFilterPipe } from '@app/shared/pipes/table-filter.pipe';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';

@Component({
  selector: 'app-result-import-special-input-price',
  templateUrl: './result-import-special-input-price.component.html',
  styleUrls: ['./result-import-special-input-price.component.scss'],
  standalone: true,
  providers: [],
  imports: [
    CommonModule,
    FormsModule,
    NgbModule,
    NgSelectModule,
    MatCheckboxModule,
    CoreModule,
    AppAdvancedDataTableComponent,
    AppTableColumnDirective,
    AppTableColumnGroupDirective,
  ],
})
export class ResultImportSpecialInputPriceComponent implements OnChanges {
  @Input() resultImport: ExcelValidationResult<SpecialInputPriceDetailImportDto> | undefined;

  readonly fb = inject(FormBuilder);
  private readonly tableFilterPipe = new TableFilterPipe();

  public fileImport: File | null = null;
  searchText = '';
  showFilter: 'all' | 'valid' | 'error' = 'all';

  isLoading = false;
  validationErrors: { [key: string]: boolean } = {};

  // Static data property like the working price offer component
  filteredItems: any[] = [];

  // Search configuration
  searchColumns = [
    'rowData.materialCode',
    'rowData.modelName',
    'rowData.currency',
    'rowData.standardInputPrice',
    'rowData.specialInputPrice',
  ];

  get specialInputPriceData(): SpecialInputPriceDetailImportDto | undefined {
    if (!this.resultImport?.listData?.length) return undefined;
    return this.resultImport.listData[0]?.rowData;
  }

  get totalItems(): number {
    return this.resultImport?.listData?.length || 0;
  }

  get validItemsCount(): number {
    return this.resultImport?.listData?.filter(item => !item.hasErrors)?.length || 0;
  }

  get invalidItemsCount(): number {
    return this.totalItems - this.validItemsCount;
  }

  get hasErrors(): boolean {
    return this.invalidItemsCount > 0;
  }

  get canSubmit(): boolean {
    return this.validItemsCount > 0;
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.resultImport && this.resultImport) {
      this.updateFilteredItems();
    }
  }

  private updateFilteredItems(): void {
    let items = this.resultImport?.listData || [];

    // Apply status filter
    if (this.showFilter === 'valid') {
      items = items.filter(item => !item.hasErrors);
    } else if (this.showFilter === 'error') {
      items = items.filter(item => item.hasErrors);
    }

    // Apply search filter
    if (this.searchText && this.searchText.trim() !== '') {
      items = this.tableFilterPipe.transform(items, this.searchText, this.searchColumns);
    }

    const newListData =
      items?.map((item, index) => ({
        ...item,
        rowData: {
          ...item.rowData,
          status: item?.hasErrors ? 'INVALID' : 'VALID',
          no: index + 1, // Add row number
        },
      })) || [];

    this.filteredItems = newListData;
  }

  // Formatter functions for advanced data table
  formatNumber = (value: any): string => {
    return value ? NumberHelper.convertToFormattedNumber(value, 0) : '0';
  };

  formatCurrency = (value: any): string => {
    return value
      ? `${new Intl.NumberFormat('en-US', {
          minimumFractionDigits: 0,
          maximumFractionDigits: 0,
        }).format(value)}`
      : '';
  };

  formatSpecialPrice = (value: any): string => {
    return value
      ? `${new Intl.NumberFormat('en-US', {
          minimumFractionDigits: 2,
          maximumFractionDigits: 2,
        }).format(value)}`
      : '';
  };

  formatValidationStatus = (hasErrors: boolean): string => {
    return hasErrors ? 'Invalid' : 'Valid';
  };

  formatRowNumber = (value: any, item: any): string => {
    // Find the original index in the full list
    const originalIndex = this.resultImport?.listData?.findIndex(original => original === item) ?? -1;
    return (originalIndex + 1).toString();
  };

  formatErrors = (errors: string[]): string => {
    if (!errors || errors.length === 0) {
      return '✓ Valid';
    }
    return errors.join('; ');
  };

  // Row click handler (required for advanced data table)
  onRowClick(event: any): void {
    // Empty handler - expansion is handled automatically by advanced data table
  }

  // Filter and search methods
  setFilter(filter: 'all' | 'valid' | 'error'): void {
    this.showFilter = filter;
    this.updateFilteredItems();
  }

  clearSearch(): void {
    this.searchText = '';
    this.updateFilteredItems();
  }

  onSearchTextChange(): void {
    this.updateFilteredItems();
  }

  getImportSummary() {
    return {
      totalItems: this.totalItems,
      validItems: this.validItemsCount,
      invalidItems: this.invalidItemsCount,
      hasErrors: this.hasErrors,
      canSubmit: this.canSubmit,
    };
  }
}
