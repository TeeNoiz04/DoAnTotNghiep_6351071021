import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';

export interface FilterField {
  key: string;
  label: string;
  type: 'checkbox' | 'select' | 'multiselect' | 'radio' | 'daterange' | 'numberrange';
  options?: Array<{ value: any; label: string }>;
  value?: any;
  placeholder?: string;
  col?: string; // Bootstrap column class, e.g., 'col-md-3', 'col-lg-4'
  icon?: string; // Bootstrap icon class for checkbox toggle, e.g., 'bi-check-circle', 'bi-star'
  checkedIcon?: string; // Optional different icon when checked
}

@Component({
  selector: 'app-filter-pane',
  standalone: true,
  imports: [CommonModule, FormsModule, NgbModule, NgSelectModule],
  templateUrl: './filters-pane.component.html',
  styleUrls: ['./filters-pane.component.scss'],
  animations: [],
})
export class FilterPaneComponent implements OnInit {
  @Input() filterFields: FilterField[] = [];
  @Input() isOpen = false;
  @Input() showApplyButton = false;
  @Input() showClearButton = true;

  @Output() filterChange = new EventEmitter<{ [key: string]: any }>();
  @Output() applyClick = new EventEmitter<{ [key: string]: any }>();
  @Output() clearClick = new EventEmitter<void>();

  ngOnInit() {
    // Initialize values for multiselect and range types
    this.filterFields.forEach(field => {
      if (field.type === 'multiselect' && !field.value) {
        field.value = [];
      }
      if ((field.type === 'daterange' || field.type === 'numberrange') && !field.value) {
        field.value = {};
      }
    });
  }

  onFilterChange() {
    if (!this.showApplyButton) {
      // Emit changes immediately for real-time filtering
      this.emitFilters();
    }
  }

  applyFilters() {
    this.emitFilters();
    this.applyClick.emit(this.getFilterValues());
  }

  clearAllFilters() {
    this.filterFields.forEach(field => {
      if (field.type === 'checkbox') {
        field.value = false;
      } else if (field.type === 'multiselect') {
        field.value = [];
      } else if (field.type === 'daterange' || field.type === 'numberrange') {
        field.value = {};
      } else {
        field.value = null;
      }
    });
    this.emitFilters();
    this.clearClick.emit();
  }

  private emitFilters() {
    this.filterChange.emit(this.getFilterValues());
  }

  private getFilterValues(): { [key: string]: any } {
    const values: { [key: string]: any } = {};
    this.filterFields.forEach(field => {
      values[field.key] = field.value;
    });
    return values;
  }

  // Checkbox helper methods
  toggleCheckbox(field: FilterField) {
    field.value = !field.value;
    this.onFilterChange();
  }

  getCheckboxIcon(field: FilterField): string {
    const defaultIcon = 'bi bi-circle';
    const defaultCheckedIcon = 'bi bi-check-circle-fill';

    if (field.value) {
      return field.checkedIcon || field.icon || defaultCheckedIcon;
    }
    return field.icon || defaultIcon;
  }
}
