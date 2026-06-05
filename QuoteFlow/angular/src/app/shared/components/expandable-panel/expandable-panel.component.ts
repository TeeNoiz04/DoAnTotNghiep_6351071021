import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbCollapseModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-expandable-panel',
  standalone: true,
  imports: [CommonModule, NgbCollapseModule],
  templateUrl: './expandable-panel.component.html',
  styleUrls: ['./expandable-panel.component.scss'],
})
export class ExpandablePanelComponent implements OnInit {
  @Input({ required: true }) headerText!: string;
  @Input({ required: true }) panelId!: string;
  @Input() defaultCollapsed: boolean = false;
  @Input() headerStyle: string = 'background-color: #498f51; color: #fff;';
  @Input() panelStyle: string = '';
  @Input() panelClass: string = '';
  @Input() showIcon: boolean = true;

  /**
   * Position of the icon - 'start' or 'end'
   */
  @Input() iconPosition: 'start' | 'end' = 'end';
  @Output() expanded = new EventEmitter<void>();
  @Output() collapsed = new EventEmitter<void>();

  isCollapsed: boolean = false;
  headerClasses: string = '';

  constructor() {}

  ngOnInit(): void {
    this.isCollapsed = this.defaultCollapsed;
    this.updateHeaderClasses();
  }

  toggle(event?: Event): void {
    if (event) {
      event.preventDefault();
      event.stopPropagation();
    }

    this.isCollapsed = !this.isCollapsed;
    this.updateHeaderClasses();

    if (this.isCollapsed) {
      this.collapsed.emit();
    } else {
      this.expanded.emit();
    }
  }

  expand(): void {
    if (this.isCollapsed) {
      this.isCollapsed = false;
      this.updateHeaderClasses();
      this.expanded.emit();
    }
  }

  collapse(): void {
    if (!this.isCollapsed) {
      this.isCollapsed = true;
      this.updateHeaderClasses();
      this.collapsed.emit();
    }
  }

  private updateHeaderClasses(): void {
    this.headerClasses = this.isCollapsed ? 'panel-header-collapsed' : 'panel-header-expanded';
  }
}
