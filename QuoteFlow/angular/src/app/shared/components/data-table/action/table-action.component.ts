import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-table-action',
  template: ``,
  standalone: true,
})
export class TableActionComponent {
  @Input() name: string;
  @Input() title: string;
  @Input() condition: boolean;
  @Input() readOnly: boolean;
  @Output() doAction = new EventEmitter<any>();
}
