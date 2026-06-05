import { Component, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartData, ChartType } from 'chart.js';
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';

@Component({
  selector: 'app-base-chart',
  standalone: true,
  imports: [CommonModule, CoreModule, ThemeSharedModule, BaseChartDirective],
  template: `
    <div class="chart-container" [style.height]="height" [class.scrollable]="enableScrolling">
      <div class="chart-scroll-container" [style.width]="scrollWidth" [style.height]="'100%'" *ngIf="enableScrolling">
        <canvas baseChart [data]="chartData" [options]="chartOptions" [type]="chartType"></canvas>
      </div>
      <div class="chart-regular-container" [style.height]="'100%'" *ngIf="!enableScrolling">
        <canvas baseChart [data]="chartData" [options]="chartOptions" [type]="chartType"></canvas>
      </div>
    </div>
  `,
  styles: [
    `
      .chart-container {
        position: relative;
        width: 100%;
        height: 100%;
        min-height: 300px;
        display: flex;
        flex-direction: column;
      }

      .chart-container.scrollable {
        overflow-x: auto;
        overflow-y: hidden;
      }

      .chart-scroll-container,
      .chart-regular-container {
        position: relative;
        flex: 1;
        width: 100%;
        height: 100%;
      }
    `,
  ],
})
export class BaseChartComponent implements OnInit, OnChanges {
  @Input() chartData: ChartData = {
    labels: [],
    datasets: [],
  };

  @Input() chartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
  };

  @Input() chartType: ChartType = 'bar';
  @Input() height: string = '100%';
  @Input() width: string = '100%';
  @Input() enableScrolling: boolean = false;
  @Input() scrollWidth: string = 'auto';

  ngOnInit(): void {
    this.initializeDefaultOptions();
    this.calculateScrollWidth();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['chartType'] || changes['chartData']) {
      this.initializeDefaultOptions();
      this.calculateScrollWidth();
    }
  }

  private calculateScrollWidth(): void {
    if (this.enableScrolling && this.chartData?.labels?.length) {
      const itemCount = this.chartData.labels.length;
      if (itemCount > 10) {
        const minWidth = Math.max(itemCount * 60, 800);
        this.scrollWidth = `${minWidth}px`;
      }
    }
  }

  private initializeDefaultOptions(): void {
    const baseOptions: ChartConfiguration['options'] = {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: {
          display: true,
          position: 'top' as const,
        },
        tooltip: {
          enabled: true,
          mode: 'index' as const,
          intersect: false,
        },
      },
    };

    this.chartOptions = {
      ...baseOptions,
      ...this.chartOptions,
    };
  }
}
