import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-header',
  template: '',
  standalone: true,
})
export class HeaderTableComponent {
  @Input() title: string;
  @Input() name: string;
  @Input() hasTooltip: string;
  @Input() className: string;
  @Input() tooltip: string;
  @Input() sortable: boolean = true;
  @Input() filter: boolean = false;
  @Input() group?: string;
  @Input() parentGroup?: string;
}
