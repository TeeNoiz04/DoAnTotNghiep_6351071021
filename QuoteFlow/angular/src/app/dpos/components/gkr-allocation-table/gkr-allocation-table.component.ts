import { CommonModule, LocationStrategy } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import {
  AppAdvancedDataTableComponent,
  AppTableColumnDirective,
  CellClickEvent,
  RowExpansionEvent,
  SelectionChangeEvent,
} from '@app/shared/components/advanced-data-table';
import { AppSubtableColumnsDirective } from '@app/shared/components/advanced-data-table/directives/subtable-columns.directive';
import { NumberHelper } from '@app/shared/helpers/number-helper';
import { DpoGkrAllocationDto } from '@proxy/dpos';

@Component({
  selector: 'app-gkr-allocation-table',
  templateUrl: './gkr-allocation-table.component.html',
  styleUrls: ['./gkr-allocation-table.component.scss'],
  standalone: true,
  imports: [CommonModule, AppAdvancedDataTableComponent, AppTableColumnDirective, AppSubtableColumnsDirective],
})
export class GkrAllocationTableComponent implements OnChanges {
  @Input() data: DpoGkrAllocationDto[] = [];
  @Input() height: string = 'auto';
  @Input() maxHeight: string | null = '400px';
  @Input() enableSelection: boolean = false;
  @Input() showLinkedNote: boolean = false;

  @Output() selectionChange = new EventEmitter<DpoGkrAllocationDto | null>();
  @Output() viewDetail = new EventEmitter<DpoGkrAllocationDto>();

  protected readonly router = inject(Router);
  protected readonly locationStrategy = inject(LocationStrategy);

  tableData: any[] = [];
  selectedItem: DpoGkrAllocationDto | null = null;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['data'] && this.data) {
      this.tableData = this.data.map(item => ({
        ...item,
        selected: false,
      }));
    }
  }

  // Formatter for Material Type with color
  formatMaterialType = (value: any, row: any): string => {
    return row.materialType || '';
  };

  // Get cell class for Material Type column
  getMaterialTypeClass = (value: any, row: any): string => {
    const materialType = row.materialType;
    return materialType ? `badge bg-material-${materialType.toLowerCase()}` : '';
  };

  // Formatter for Buyer
  formatBuyer = (value: any, row: DpoGkrAllocationDto): string => {
    if (row.buyerTypeDescription && row.buyerShortName) {
      return `${row.buyerTypeDescription} - ${row.buyerShortName}`;
    }
    return row.buyerShortName || '';
  };

  formatNumber = (value: any): string => {
    if (value == null || isNaN(value)) {
      return '';
    }

    return NumberHelper.convertToFormattedNumber(value, 0);
  };

  // Formatter for dates
  formatDate = (value: any): string => {
    return value ? new Date(value).toLocaleDateString('en-GB') : '';
  };

  // Handle selection change
  onSelectionChange(event: SelectionChangeEvent): void {
    if (!this.enableSelection) return;

    // Only allow single selection
    if (event.selectedItems.length > 1) {
      // Keep only the last selected item
      const lastSelected = event.selectedItems[event.selectedItems.length - 1];
      //   this.tableData.forEach(item => {
      //     item.selected = item === lastSelected;
      //   });
      this.selectedItem = lastSelected;
    } else if (event.selectedItems.length === 1) {
      this.selectedItem = event.selectedItems[0];
    } else {
      this.selectedItem = null;
    }

    this.selectionChange.emit(this.selectedItem);
  }

  // Handle action click (View Detail button)
  onCellClick(event: CellClickEvent): void {
    if (event.field !== 'dpoNo') return;

    this.viewDetail.emit(event.row);

    // Assuming event.row.id is the <id> you need
    const id = event.row.id;

    // 2. Create the UrlTree from your route array structure
    const urlTree = this.router.createUrlTree([AppRoutes.GKR.BASE, id]);

    // 3. Convert the UrlTree into a full path string (e.g., /gkr-base/12345)
    // The LocationStrategy ensures the path includes the base path if needed
    const url = this.locationStrategy.prepareExternalUrl(this.router.serializeUrl(urlTree));

    // 4. Open the generated URL in a new tab
    window.open(url, '_blank');
  }

  // Custom actions for the action column
  getCustomActions() {
    return [
      {
        id: 'viewDetail',
        label: 'View Detail',
        icon: 'fa fa-eye',
        tooltip: 'View GKR Detail',
        cssClass: '',
      },
    ];
  }

  // Determine if row can expand (only if it has details)
  canExpandRow = (row: DpoGkrAllocationDto): boolean => {
    return row.allocationDetails && row.allocationDetails.length > 0;
  };

  // Handle row expansion events
  onRowExpansion(event: RowExpansionEvent): void {
    if (event.expanded && event.type === 'subtable') {
    }
  }
}
