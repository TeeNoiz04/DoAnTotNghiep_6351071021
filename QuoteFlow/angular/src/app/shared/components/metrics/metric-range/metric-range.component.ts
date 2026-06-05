import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, SimpleChanges, TemplateRef, ViewChild } from '@angular/core';
import { NumberHelper } from '@app/shared/helpers/number-helper';
import { NgbModule, NgbPopover } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-metric-range',
  standalone: true,
  imports: [CommonModule, NgbModule],
  templateUrl: './metric-range.component.html',
  styleUrl: './metric-range.component.scss',
})
export class MetricRangeComponent implements OnChanges {
  @Input() label: string = '';
  @Input() minValue: number = 0;
  @Input() currentValue: number = 0;
  @Input() maxValue: number = 100;
  @Input() iconClass: string = 'bi bi-graph-up text-primary';
  @Input() containerClass: string = '';
  @Input() labelContainerClass: string = '';
  @Input() labelClass: string = '';
  @Input() valueClass: string = '';
  @Input() scaleContainerClass: string = '';
  @Input() showPopover: boolean = true;
  @Input() popoverPlacement: 'auto' | 'top' | 'bottom' | 'left' | 'right' = 'top';
  @Input() popoverClass: string = 'custom-metric-popover';
  @Input() customStatusClass: string = '';

  // Custom labels for tooltip
  @Input() minLabel: string = 'Minimum';
  @Input() maxLabel: string = 'Maximum';
  @Input() currentLabel: string = 'Current';
  @Input() remainingAddLabel: string = 'Remaining to Add';
  @Input() remainingReduceLabel: string = 'Amount to Reduce';

  // Formatting options
  @Input() formatter: (value: number) => string = value => NumberHelper.convertToFormattedNumber(value, 0) || '0';
  @Input() abbreviationDecimalPlaces: number = 1;
  @Input() abbreviationThreshold: number = 1000;

  // Template and popover references
  @ViewChild('popoverTemplate', { static: true }) popoverTemplate!: TemplateRef<object>;
  @ViewChild('popoverTrigger', { static: false }) popoverTrigger!: NgbPopover;

  // Calculated values
  currentPosition: number = 0;
  statusClass: string = 'status-good';
  isOverflow: boolean = false;
  overflowExtension: number = 0;

  // Popover state management
  private isPopoverOpen: boolean = false;

  // Expose Math for template use
  protected readonly Math = Math;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['currentValue'] || changes['minValue'] || changes['maxValue'] || changes['customStatusClass']) {
      this.updateCalculatedValues();
    }
  }

  private updateCalculatedValues(): void {
    this.currentPosition = this.calculateCurrentPosition();
    this.statusClass = this.calculateStatusClass();
    this.isOverflow = this.calculateOverflow();
    this.overflowExtension = this.calculateOverflowExtension();
  }

  formatValue(value: number): string {
    return this.formatter(value);
  }

  formatAbbreviatedValue(value: number): string {
    return NumberHelper.abbreviateNumber(value, {
      decimalPlaces: this.abbreviationDecimalPlaces,
      threshold: this.abbreviationThreshold,
    });
  }

  getRemainingToMin(): number {
    return this.currentValue - this.minValue;
  }

  getRemainingToMax(): number {
    return this.maxValue - this.currentValue;
  }

  private calculateCurrentPosition(): number {
    const range = this.maxValue - this.minValue;
    if (range === 0) return 50; // Center position if no range

    const currentRelativeToMin = this.currentValue - this.minValue;
    const position = (currentRelativeToMin / range) * 100;

    // Return position as percentage, can exceed 0-100%
    return Math.max(0, Math.min(position, 100));
  }

  private calculateStatusClass(): string {
    if (this.customStatusClass) {
      return this.customStatusClass;
    }

    if (this.currentValue < this.minValue) {
      return 'status-below';
    } else if (this.currentValue > this.maxValue) {
      return 'status-above';
    } else {
      return 'status-good';
    }
  }

  private calculateOverflow(): boolean {
    return this.currentValue < this.minValue || this.currentValue > this.maxValue;
  }

  private calculateOverflowExtension(): number {
    if (!this.isOverflow) return 0;

    const range = this.maxValue - this.minValue;
    if (range === 0) return 0;

    if (this.currentValue > this.maxValue) {
      // Overflow to the right
      const excess = this.currentValue - this.maxValue;
      return (excess / range) * 100;
    } else if (this.currentValue < this.minValue) {
      // Overflow to the left
      const deficit = this.minValue - this.currentValue;
      return (deficit / range) * 100;
    }

    return 0;
  }

  onMetricClick(): void {
    if (!this.showPopover || !this.popoverTrigger) return;

    if (this.isPopoverOpen) {
      this.popoverTrigger.close();
    } else {
      this.popoverTrigger.open();
    }
    this.isPopoverOpen = !this.isPopoverOpen;
  }

  onPopoverShown(): void {
    this.isPopoverOpen = true;
  }

  onPopoverHidden(): void {
    this.isPopoverOpen = false;
  }
}
