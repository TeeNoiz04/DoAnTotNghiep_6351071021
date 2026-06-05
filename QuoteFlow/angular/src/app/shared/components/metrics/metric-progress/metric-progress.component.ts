import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, SimpleChanges, TemplateRef, ViewChild } from '@angular/core';
import { NumberHelper } from '@app/shared/helpers/number-helper';
import { NgbModule, NgbPopover } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-metric-progress',
  standalone: true,
  imports: [CommonModule, NgbModule],
  templateUrl: './metric-progress.component.html',
  styleUrl: './metric-progress.component.scss',
})
export class MetricProgressComponent implements OnChanges {
  @Input() label: string = '';
  @Input() currentValue: number = 0;
  @Input() maxValue: number | string = 100;
  @Input() iconClass: string = 'bi bi-graph-up text-primary';
  @Input() containerClass: string = '';
  @Input() labelContainerClass: string = '';
  @Input() labelClass: string = '';
  @Input() valueClass: string = '';
  @Input() progressContainerClass: string = '';
  @Input() progressBarClass: string = '';
  @Input() showPopover: boolean = true;
  @Input() popoverPlacement: 'auto' | 'top' | 'bottom' | 'left' | 'right' = 'top';
  @Input() popoverClass: string = 'custom-metric-popover';
  @Input() formatter: (value: number) => string = value => NumberHelper.convertToFormattedNumber(value, 0) || '0';
  @Input() abbreviationDecimalPlaces: number = 1;
  @Input() abbreviationThreshold: number = 1000;
  @Input() warningThreshold: number = 80; // Percentage threshold for warning status
  @Input() customStatusClass: string = ''; // Allow custom status override
  @Input() suffix: string = ''; // Suffix to append to the formatted value

  // Custom labels for tooltip
  @Input() maxLabel: string = 'Maximum';
  @Input() currentLabel: string = 'Current';
  @Input() remainingLabel: string = 'Remaining';
  @Input() exceededLabel: string = 'Exceeded by';

  // Template and popover references
  @ViewChild('popoverTemplate', { static: true }) popoverTemplate!: TemplateRef<object>;
  @ViewChild('popoverTrigger', { static: false }) popoverTrigger!: NgbPopover;

  progressPercentage: number = 0;
  statusClass: string = 'status-good';

  // Popover state management
  private isPopoverOpen: boolean = false;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['currentValue'] || changes['maxValue'] || changes['warningThreshold'] || changes['customStatusClass']) {
      this.updateCalculatedValues();
    }
  }

  private updateCalculatedValues(): void {
    // 2. Only calculate progress and status if maxValue is a number
    if (typeof this.maxValue === 'number') {
      this.progressPercentage = this.calculateProgressPercentage();
      this.statusClass = this.calculateStatusClass();
    } else {
      // Handle case when maxValue is a string (e.g., 'N/A' or 'Unlimited')
      this.progressPercentage = 100; // Or some other default visual state
      this.statusClass = 'status-good'; // Default status
    }
  }

  // Helper function to safely get the numeric maxValue for calculations
  private getNumericMaxValue(): number {
    return typeof this.maxValue === 'number' ? this.maxValue : 0;
  }

  formatValue(value: number | string): string {
    if (typeof value === 'string') {
      return value;
    }
    return this.formatter(value);
  }

  formatAbbreviatedValue(value: number | string): string {
    if (typeof value === 'string') {
      return value;
    }
    return NumberHelper.abbreviateNumber(value, {
      decimalPlaces: this.abbreviationDecimalPlaces,
      threshold: this.abbreviationThreshold,
    });
  }

  private calculateProgressPercentage(): number {
    const numericMaxValue = this.getNumericMaxValue();
    if (numericMaxValue === 0) return 100;
    return Math.min((this.currentValue / numericMaxValue) * 100, 100);
  }

  private calculateStatusClass(): string {
    if (this.customStatusClass) {
      return this.customStatusClass;
    }

    const numericMaxValue = this.getNumericMaxValue();

    if (numericMaxValue === 0) {
      // If max is 0 (and is a number), treat as exceeded or a special status
      return this.currentValue > 0 ? 'status-exceeded' : 'status-good';
    }

    const percentage = (this.currentValue / numericMaxValue) * 100;

    if (this.currentValue >= numericMaxValue) {
      return 'status-exceeded';
    } else if (percentage >= this.warningThreshold) {
      return 'status-warning';
    } else {
      return 'status-good';
    }
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
