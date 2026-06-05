import { CommonModule } from '@angular/common';
import { Component, ElementRef, Input, OnChanges, SimpleChanges, TemplateRef, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgbModule, NgbPopover } from '@ng-bootstrap/ng-bootstrap';

interface ProcessedWarning {
  where?: string;
  message: string;
  isFullWidth: boolean;
  rowNumber?: number;
  originalIndex: number;
}

@Component({
  selector: 'app-warning-display',
  standalone: true,
  imports: [CommonModule, NgbModule, FormsModule],
  templateUrl: './warning-display.component.html',
  styleUrls: ['./warning-display.component.scss'],
})
export class WarningDisplayComponent implements OnChanges {
  @Input() warnings: string[] = [];
  @Input() customMessage?: string;
  @Input() showIcon: boolean = true;
  @Input() popoverPlacement: 'auto' | 'top' | 'bottom' | 'left' | 'right' = 'auto';
  @Input() popoverClass: string = 'custom-warning-popover';
  @Input() popoverMaxWarnings: number = 200;

  @ViewChild('warningPopoverTemplate', { static: true }) warningPopoverTemplate!: TemplateRef<object>;
  @ViewChild('popoverContent') popoverContent!: ElementRef;

  processedWarnings: ProcessedWarning[] = [];
  popoverWarnings: ProcessedWarning[] = [];

  hasScrollTop: boolean = false;
  hasScrollBottom: boolean = false;

  private activePopover?: NgbPopover;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['warnings']) {
      this.processWarnings();
    }
  }

  get warningCount(): number {
    return this.warnings?.length || 0;
  }

  get displayMessage(): string {
    return this.customMessage || `Warnings: ${this.warningCount}`;
  }

  get hasWarnings(): boolean {
    return this.warningCount > 0;
  }

  private processWarnings(): void {
    if (!this.warnings || this.warnings.length === 0) {
      this.processedWarnings = [];
      this.popoverWarnings = [];
      return;
    }

    const warnings = this.warnings.map((warning, index) => {
      const rowMatch = warning.match(/^Row (\d+):/);
      const rowNumber = rowMatch ? parseInt(rowMatch[1], 10) : undefined;

      const cleanWarning = warning.replace(/^Row \d+:\s*/, '');
      const whereMatch = cleanWarning.match(/^\[([^\]]+)\]\s*(.+)$/);

      if (whereMatch) {
        return {
          where: whereMatch[1],
          message: whereMatch[2],
          isFullWidth: false,
          rowNumber,
          originalIndex: index,
        };
      } else {
        return {
          message: warning,
          isFullWidth: true,
          rowNumber,
          originalIndex: index,
        };
      }
    });

    this.processedWarnings = warnings.sort((a, b) => {
      if (a.rowNumber !== undefined && b.rowNumber !== undefined) return a.rowNumber - b.rowNumber;
      if (a.rowNumber !== undefined) return -1;
      if (b.rowNumber !== undefined) return 1;
      return a.originalIndex - b.originalIndex;
    });

    this.popoverWarnings = this.processedWarnings.slice(0, this.popoverMaxWarnings);
  }

  togglePopover(popover: NgbPopover): void {
    if (popover.isOpen()) {
      popover.close();
      this.activePopover = undefined;
    } else {
      popover.open();
      this.activePopover = popover;
    }
  }

  onPopoverScroll(event: Event): void {
    const element = event.target as HTMLElement;
    const scrollTop = element.scrollTop;
    const scrollHeight = element.scrollHeight;
    const clientHeight = element.clientHeight;
    this.hasScrollTop = scrollTop > 10;
    this.hasScrollBottom = scrollTop + clientHeight < scrollHeight - 10;
  }
}
