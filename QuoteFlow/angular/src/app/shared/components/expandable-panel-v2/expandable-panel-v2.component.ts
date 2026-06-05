import { Component, EventEmitter, Input, OnInit, Output, ContentChild, TemplateRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbCollapseModule } from '@ng-bootstrap/ng-bootstrap';
import { animate, state, style, transition, trigger } from '@angular/animations';

@Component({
  selector: 'app-expandable-panel-v2',
  standalone: true,
  imports: [CommonModule, NgbCollapseModule],
  templateUrl: './expandable-panel-v2.component.html',
  styleUrls: ['./expandable-panel-v2.component.scss'],
  animations: [
    trigger('rotateIcon', [
      state('collapsed', style({ transform: 'rotate(0deg)' })),
      state('expanded', style({ transform: 'rotate(180deg)' })),
      transition('collapsed <=> expanded', animate('300ms ease-in-out')),
    ]),
    trigger('fadeInOut', [
      state('void', style({ opacity: 0, height: '0px' })),
      state('*', style({ opacity: 1, height: '*' })),
      transition('void <=> *', animate('300ms ease-in-out')),
    ]),
  ],
})
export class ExpandablePanelV2Component implements OnInit {
  @Input({ required: true }) headerText!: string;
  @Input({ required: true }) panelId!: string;
  @Input() defaultCollapsed: boolean = false;
  @Input() headerStyle: { [key: string]: string | number } | null = null;
  @Input() panelStyle: { [key: string]: string | number } | null = null;
  @Input() panelClass: string = '';
  @Input() showIcon: boolean = true;
  @Input() badgeCount: string | number | null = null;
  @Input() selectedCount: string | number | null = null;
  @Input() badgeClass: string = 'bg-secondary';
  @Input() icon: string = '';
  @Input() actions: unknown[] = [];
  @Input() isDisabled: boolean = false;
  @ContentChild('headerActions') headerActionsTemplate: TemplateRef<unknown> | undefined;

  @Output() expanded = new EventEmitter<void>();
  @Output() collapsed = new EventEmitter<void>();
  @Output() toggleChange = new EventEmitter<boolean>();

  isCollapsed: boolean = false;
  animationState: 'collapsed' | 'expanded' = 'collapsed';

  ngOnInit(): void {
    this.isCollapsed = this.defaultCollapsed;
    this.animationState = this.isCollapsed ? 'collapsed' : 'expanded';
  }

  toggle(event?: Event): void {
    if (this.isDisabled) return;

    if (event) {
      event.preventDefault();
      event.stopPropagation();
    }

    this.isCollapsed = !this.isCollapsed;
    this.animationState = this.isCollapsed ? 'collapsed' : 'expanded';
    this.toggleChange.emit(this.isCollapsed);

    if (this.isCollapsed) {
      this.collapsed.emit();
    } else {
      this.expanded.emit();
    }
  }

  expand(): void {
    if (this.isDisabled || !this.isCollapsed) return;

    this.isCollapsed = false;
    this.animationState = 'expanded';
    this.expanded.emit();
    this.toggleChange.emit(this.isCollapsed);
  }

  collapse(): void {
    if (this.isDisabled || this.isCollapsed) return;

    this.isCollapsed = true;
    this.animationState = 'collapsed';
    this.collapsed.emit();
    this.toggleChange.emit(this.isCollapsed);
  }
}
