import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChartConfiguration, ChartData } from 'chart.js';
import { BaseChartComponent } from '../base-chart/base-chart.component';

@Component({
  selector: 'app-bar-chart',
  standalone: true,
  imports: [CommonModule, BaseChartComponent],
  template: `
    <app-base-chart
      [chartData]="chartData"
      [chartOptions]="barChartOptions"
      [chartType]="'bar'"
      [height]="height"
      [enableScrolling]="enableScrolling"
      [scrollWidth]="scrollWidth">
    </app-base-chart>
  `,
  styles: [
    `
      :host {
        display: block;
        width: 100%;
        height: 100%;
      }
    `,
  ],
})
export class BarChartComponent implements OnInit {
  @Input() chartData: ChartData = {
    labels: [],
    datasets: [],
  };

  @Input() height: string = '100%';
  @Input() enableScrolling: boolean = true;
  @Input() scrollWidth: string = 'auto';

  @Input() barChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    scales: {
      x: {
        grid: {
          display: false,
        },
        ticks: {
          maxRotation: 45,
          minRotation: 45,
        },
      },
      y: {
        beginAtZero: true,
        title: {
          display: true,
          text: 'Sales Value',
        },
      },
      percentage: {
        beginAtZero: true,
        position: 'right',
        title: {
          display: true,
          text: 'Achievement (%)',
        },
        max: 100,
        grid: {
          display: false,
        },
      },
    },
    plugins: {
      legend: {
        display: true,
        position: 'top',
      },
      tooltip: {
        callbacks: {
          label: function (context) {
            let label = context.dataset.label || '';
            if (label) {
              label += ': ';
            }
            if (context.parsed.y !== null) {
              if (context.dataset.yAxisID === 'percentage') {
                label += context.parsed.y + '%';
              } else {
                label += new Intl.NumberFormat('en-US').format(context.parsed.y);
              }
            }
            return label;
          },
        },
      },
      datalabels: {
        display: false,
        anchor: 'end',
        align: 'top',
        formatter: (value, context) => {
          return new Intl.NumberFormat('en-US').format(value);
        },
        font: {
          weight: 'bold',
          size: 11,
        },
        padding: 4,
      },
    },
  };

  ngOnInit(): void {
    const itemCount = this.chartData?.labels?.length || 0;
    this.enableScrolling = itemCount > 10;
    if (itemCount > 10) {
      this.barChartOptions = {
        ...this.barChartOptions,
        datasets: {
          bar: {
            barPercentage: 0.8,
            categoryPercentage: 0.8,
          },
        },
      };
    }

    if (this.chartData?.datasets?.length) {
      this.chartData.datasets.forEach(dataset => {
        const typedDataset = dataset as any;
        if (typedDataset.type === 'line' && !typedDataset.yAxisID) {
          typedDataset.yAxisID = 'percentage';
        }
      });
    }
  }
}
