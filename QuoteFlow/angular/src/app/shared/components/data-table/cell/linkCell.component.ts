import { Component, EventEmitter, Output, Input } from '@angular/core';
import { TextCellComponent } from './textCell.component';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-link-cell',
  standalone: true,
  template: `<a
    [routerLink]="getLink(value, entry)"
    (click)="clickAction.emit(); $event.stopPropagation()"
    class="link-cell {{ cssClass }}">
    {{ formatter(value, entry) }}
  </a>`,
  imports: [CommonModule, RouterModule],
})
export class LinkCellComponent extends TextCellComponent {
  @Output() clickAction = new EventEmitter<any>();
  @Input() getLink: (value: any, entry: any) => string;
}
