import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-metrics-panel',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div [ngClass]="containerClass">
      <div [ngClass]="rowClass">
        <div [ngClass]="contentClass">
          <div [ngClass]="metricsContainerClass">
            <ng-content></ng-content>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .metrics-container-default {
        display: flex;
        flex-wrap: wrap;
        gap: 1rem;
        align-items: flex-start;
      }

      @media (max-width: 768px) {
        .metrics-container-default {
          gap: 0.75rem;
        }
      }

      @media (max-width: 576px) {
        .metrics-container-default {
          flex-direction: column;
          gap: 0.5rem;
        }
      }
    `,
  ],
})
export class MetricsPanelComponent {
  @Input() containerClass: string = 'row mb-2 align-items-center';
  @Input() rowClass: string = '';
  @Input() contentClass: string = 'col-12';
  @Input() metricsContainerClass: string = 'metrics-container-default';
}
