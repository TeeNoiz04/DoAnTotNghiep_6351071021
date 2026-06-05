import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { UsernamePipe } from '@app/shared/pipes/username.pipe';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-audit-info-column',
  standalone: true,
  imports: [CommonModule, NgbModule, UsernamePipe],
  templateUrl: './audit-info-column.component.html',
  styleUrl: './audit-info-column.component.scss',
})
export class AuditInfoColumnComponent {
  @Input() creationTime?: string | Date;
  @Input() creatorName?: string;
  @Input() lastModificationTime?: string | Date;
  @Input() lastModifierName?: string;

  get hasModificationInfo(): boolean {
    return !!(this.lastModificationTime && this.lastModifierName);
  }

  get formattedCreationTime(): string {
    if (!this.creationTime) return '';
    const date = typeof this.creationTime === 'string' ? new Date(this.creationTime) : this.creationTime;
    return this.formatDate(date);
  }

  get formattedModificationTime(): string {
    if (!this.lastModificationTime) return '';
    const date =
      typeof this.lastModificationTime === 'string' ? new Date(this.lastModificationTime) : this.lastModificationTime;
    return this.formatDate(date);
  }

  private formatDate(date: Date): string {
    if (!date || isNaN(date.getTime())) return '';
    return date
      .toLocaleDateString('en-GB', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit',
        hour12: false,
      })
      .replace(/(\d{2})\/(\d{2})\/(\d{4}),\s*(\d{2}):(\d{2})/, '$1/$2/$3 $4:$5');
  }
}
