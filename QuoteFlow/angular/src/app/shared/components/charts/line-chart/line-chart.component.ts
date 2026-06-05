import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChartConfiguration, ChartData } from 'chart.js';
import { BaseChartComponent } from '../base-chart/base-chart.component';

@Component({
  selector: 'app-line-chart',
  standalone: true,
  imports: [CommonModule, BaseChartComponent],
  template: `
    <app-base-chart
      [chartData]="chartData"
      [chartOptions]="lineChartOptions"
      [chartType]="'line'"
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
export class LineChartComponent implements OnInit {
  @Input() chartData: ChartData = {
    labels: [],
    datasets: [],
  };

  @Input() height: string = '100%';
  @Input() enableScrolling: boolean = true;
  @Input() scrollWidth: string = 'auto';

  @Input() lineChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    elements: {
      line: {
        tension: 0.4,
      },
      point: {
        radius: 4,
        hoverRadius: 6,
      },
    },
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
              const yAxisID = (context.dataset as any).yAxisID;
              if (yAxisID === 'percentage') {
                label += context.parsed.y + '%';
              } else {
                label += new Intl.NumberFormat('en-US').format(context.parsed.y);
              }
            }
            return label;
          },
        },
      },
    },
  };

  ngOnInit(): void {
    const itemCount = this.chartData?.labels?.length || 0;
    this.enableScrolling = itemCount > 10;
    if (this.chartData?.datasets?.length) {
      this.chartData.datasets.forEach(dataset => {
        const typedDataset = dataset as any;

        if (typedDataset.type === 'bar') {
          typedDataset.yAxisID = 'y';
        } else if (!typedDataset.yAxisID) {
          typedDataset.yAxisID = 'percentage';
        }
      });
    }
  }
}
