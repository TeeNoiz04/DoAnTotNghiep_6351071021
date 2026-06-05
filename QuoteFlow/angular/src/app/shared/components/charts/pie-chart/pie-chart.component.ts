import { ChangeDetectionStrategy, Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Chart, ChartConfiguration, ChartData, ChartType, Plugin } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CoreModule } from '@abp/ng.core';
import DatalabelsPlugin from 'chartjs-plugin-datalabels';

import { ArcElement, DoughnutController, PieController, Tooltip, Legend } from 'chart.js';
Chart.register(ArcElement, DoughnutController, PieController, Tooltip, Legend, DatalabelsPlugin);

export interface PieChartDataItem {
  label: string;
  value: number;
  color?: string;
}

export interface PieChartOptions {
  doughnut?: boolean;
  cutoutPercentage?: number;
  showLegend?: boolean;
  legendPosition?: 'top' | 'bottom' | 'left' | 'right';
  showPercentage?: boolean;
  responsive?: boolean;
  aspectRatio?: number;
  centerText?: string;
  maxLegendHeight?: number;
  legendFontSize?: number;
}

@Component({
  selector: 'app-pie-chart',
  standalone: true,
  imports: [CommonModule, CoreModule, ThemeSharedModule, BaseChartDirective],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './pie-chart.component.html',
  styleUrls: ['./pie-chart.component.scss'],
})
export class PieChartComponent implements OnInit, OnChanges {
  @Input() data: PieChartDataItem[] = [];
  @Input() options: PieChartOptions = {};
  @Input() height: string = '300px';
  @Input() width: string = '100%';
  @Input() showTotal: boolean = false;

  chartData: ChartData<'pie' | 'doughnut'> = {
    labels: [],
    datasets: [],
  };

  chartType: ChartType = 'pie';
  chartOptions: ChartConfiguration['options'] = {};
  plugins: Plugin[] = [];
  centerTextLines: string[] = [];

  ngOnInit(): void {
    this.initializeChart();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['data'] || changes['options']) {
      this.initializeChart();
    }
  }

  private initializeChart(): void {
    if (this.options?.centerText) {
      this.centerTextLines = this.options.centerText.split('\n').map(line => this.truncateTextIfNeeded(line));
    } else {
      this.centerTextLines = [];
    }
    this.chartType = this.options?.doughnut ? 'doughnut' : 'pie';
    this.transformData();
    this.configureChartOptions();
  }

  private truncateTextIfNeeded(text: string): string {
    const MAX_LENGTH = 12;
    return text.length > MAX_LENGTH ? `${text.substring(0, MAX_LENGTH)}...` : text;
  }

  private transformData(): void {
    if (!this.data || this.data.length === 0) {
      this.chartData = {
        labels: [],
        datasets: [
          {
            data: [],
            backgroundColor: [],
          },
        ],
      };
      return;
    }

    const labels = this.data.map(item => item.label);
    const values = this.data.map(item => item.value);
    const colors = this.data.map(item => item.color || this.getRandomColor());

    this.chartData = {
      labels: labels,
      datasets: [
        {
          data: values,
          backgroundColor: colors,
          hoverOffset: 4,
          borderWidth: 1,
          borderColor: '#ffffff',
        },
      ],
    };
  }

  private configureChartOptions(): void {
    let cutoutPercentage = this.options?.cutoutPercentage;
    if (this.options?.doughnut) {
      cutoutPercentage = this.options?.centerText ? Math.max(cutoutPercentage || 50, 50) : cutoutPercentage || 50;
    }

    this.chartOptions = {
      responsive: this.options?.responsive !== false,
      maintainAspectRatio: false,
      cutout: this.options?.doughnut ? `${cutoutPercentage}%` : undefined,
      plugins: {
        legend: {
          display: false,
        },
        tooltip: {
          callbacks: {
            label: context => {
              const label = context.label || '';
              const value = context.raw as number;
              const datasetIndex = context.datasetIndex;

              const dataset = this.chartData.datasets[datasetIndex];
              const dataArray = dataset.data as number[];
              const total = dataArray.reduce((a, b) => a + b, 0);

              const percentage = total > 0 ? Math.round((value * 100) / total) : 0;

              return this.options?.showPercentage
                ? `${label}: ${this.formatNumber(value)} (${percentage}%)`
                : `${label}: ${this.formatNumber(value)}`;
            },
          },
        },
        datalabels: {
          display: this.options?.showPercentage === true,
          color: '#fff',
          font: {
            weight: 'bold',
            size: 11,
          },
          formatter: (value: number, context: { dataset: { data: number[] } }) => {
            const dataArray = context.dataset.data as number[];
            const total = dataArray.reduce((a, b) => a + b, 0);

            const percentage = total > 0 ? Math.round((value * 100) / total) : 0;
            return percentage > 5 ? `${percentage}%` : '';
          },
        },
      },
      animation: {
        duration: 500,
      },
    } as ChartConfiguration['options'];
  }

  private getRandomColor(): string {
    const letters = '0123456789ABCDEF';
    let color = '#';
    for (let i = 0; i < 6; i++) {
      color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
  }

  public formatNumber(value: number): string {
    return new Intl.NumberFormat('en-US').format(value);
  }
}
