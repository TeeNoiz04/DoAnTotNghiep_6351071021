import { CommonModule } from '@angular/common';
import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-action-cell',
  templateUrl: 'actionCell.component.html',
  styleUrls: ['actionCell.component.scss'],
  standalone: true,
  imports: [CommonModule],
})
export class ActionCellComponent {
  @Input() remove: boolean;
  @Input() edit: boolean;
  @Input() readOnly: boolean;

  @Output() removeAction = new EventEmitter<any>();
  @Output() editAction = new EventEmitter<any>();
}
