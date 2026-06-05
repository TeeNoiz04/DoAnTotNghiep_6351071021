import { Directive, ElementRef, EventEmitter, Input, Output } from '@angular/core';
import { fromEvent, debounceTime } from 'rxjs';

interface RowData {
  [key: string]: unknown;
}

@Directive({
  selector: '[appNgxDatatableDblClick]',
  standalone: true,
})
export class NgxDatatableDoubleClickDirective {
  @Input() debounceTime = 300; // milliseconds
  @Input() enabled = true;
  @Output() rowDblClick = new EventEmitter<RowData>();

  private readonly DOUBLE_CLICK_EVENT = 'dblclick';

  constructor(private el: ElementRef) {
    this.initializeDoubleClickListener();
  }

  private initializeDoubleClickListener(): void {
    try {
      fromEvent(this.el.nativeElement, this.DOUBLE_CLICK_EVENT)
        .pipe(debounceTime(this.debounceTime))
        .subscribe((event: Event) => {
          if (!this.enabled) return;

          const row = this.findRowElement(event.target as HTMLElement);
          if (row) {
            const rowData = this.extractRowData(row);
            this.rowDblClick.emit(rowData);
          }
        });
    } catch (error) {
      console.error('Error initializing double click listener:', error);
    }
  }

  private findRowElement(element: HTMLElement | null): HTMLElement | null {
    while (element && !element.classList.contains('datatable-row-wrapper')) {
      element = element.parentElement;
    }
    return element;
  }

  private extractRowData(row: HTMLElement): RowData {
    try {
      const rowScope = (row as any).__ngContext__;
      return rowScope ? rowScope.row : {};
    } catch (error) {
      console.error('Error extracting row data:', error);
      return {};
    }
  }
}
