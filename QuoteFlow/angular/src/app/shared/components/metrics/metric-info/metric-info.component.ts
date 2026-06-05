import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-metric-info',
  standalone: true,
  imports: [CommonModule, NgbTooltipModule],
  template: `
    <div class="d-flex align-items-start cursor-pointer" [ngClass]="containerClass">
      <i [ngClass]="iconClass" class="me-1 mt-n1"></i>
      <div>
        <div class="d-flex align-items-center mb-1" [ngClass]="labelContainerClass">
          <span class="me-2 fw-bold small" [ngClass]="labelClass">{{ label }}:</span>
          <span
            class="fw-bold small"
            [ngClass]="[valueClass, customStatusClass]"
            [ngbTooltip]="showTooltip ? tooltipText : null"
            [placement]="tooltipPlacement">
            {{ formatValue(value) }}{{ suffix }}
          </span>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .metric-placeholder {
        color: #6c757d;
      }
      .status-good {
        color: #198754 !important;
      }
      .status-warning {
        color: #fd7e14 !important;
      }
      .status-exceeded {
        color: #dc3545 !important;
      }
      .status-info {
        color: #0d6efd !important;
      }
    `,
  ],
})
export class MetricInfoComponent {
  @Input() label: string = '';
  @Input() value: number | string = '';
  @Input() suffix: string = '';
  @Input() iconClass: string = 'bi bi-info-circle text-primary';
  @Input() containerClass: string = '';
  @Input() labelContainerClass: string = '';
  @Input() labelClass: string = '';
  @Input() valueClass: string = 'metric-placeholder';
  @Input() customStatusClass: string = '';
  @Input() showTooltip: boolean = false;
  @Input() tooltipText: string = '';
  @Input() tooltipPlacement: string = 'top';
  @Input() formatter: (value: number | string) => string = value => {
    if (typeof value === 'number') {
      return value.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 0 });
    }
    return String(value);
  };

  formatValue(value: number | string): string {
    return this.formatter(value);
  }
}
