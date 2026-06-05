import { PageModule } from '@abp/ng.components/page';
import { CoreModule, LocalizationModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppPermissions } from '@app/app.permissions';
import {
  BarChartComponent,
  LineChartComponent,
  PieChartComponent,
  PieChartDataItem,
  PieChartOptions,
} from '@app/shared/components/charts';
import { ExpandablePanelV2Component } from '@app/shared/components/expandable-panel-v2/expandable-panel-v2.component';
import { TitleService } from '@app/shared/services/title/title.service';
import { NgbDateAdapter, NgbDateNativeAdapter, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import {
  DashboardService,
  SaleResultBaseDto,
  SaleResultBuyerDto,
  SaleResultMaterialGroupDto,
  SaleResultPODto,
} from '@proxy/dashboards';
import { DateRangePickerModule } from '@volo/abp.commercial.ng.ui';
import { AuditLoggingModule } from '@volo/abp.ng.audit-logging';
import { ChartData } from 'chart.js';
import { catchError, finalize, forkJoin, of } from 'rxjs';

enum EMaterialType {
  FA = 'FA',
  LVS = 'LVS',
}

// Color palettes for charts
const MATERIAL_GROUP_COLORS = [
  '#4CAF50',
  '#8BC34A',
  '#CDDC39',
  '#FFEB3B',
  '#FFC107',
  '#FF9800',
  '#FF5722',
  '#795548',
  '#9E9E9E',
  '#607D8B',
];

const DISTRIBUTOR_COLORS = [
  '#3F51B5',
  '#2196F3',
  '#03A9F4',
  '#00BCD4',
  '#009688',
  '#4CAF50',
  '#8BC34A',
  '#CDDC39',
  '#FFEB3B',
  '#FFC107',
];

export interface MaterialGroupSalesData {
  materialType: string;
  materialGroup: string;
  saleResult: number;
}

export interface DistributorSalesData {
  buyerCode: string;
  materialType: string;
  saleResult: number;
}

export interface DashboardData {
  faChartData: ChartData;
  lvsChartData: ChartData;
  materialGroupSales: MaterialGroupSalesData[];
  distributorSales: DistributorSalesData[];
}

@Component({
  selector: 'app-host-dashboard',
  templateUrl: './host-dashboard.component.html',
  styleUrls: ['./host-dashboard.component.scss'],
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }],
  standalone: true,
  imports: [
    CommonModule,
    PageModule,
    FormsModule,
    ReactiveFormsModule,
    NgxValidateCoreModule,
    DateRangePickerModule,
    AuditLoggingModule,
    LocalizationModule,
    CoreModule,
    ThemeSharedModule,
    NgbModule,
    BarChartComponent,
    LineChartComponent,
    NgSelectModule,
    PieChartComponent,
    ExpandablePanelV2Component,
  ],
})
export class HostDashboardComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  public readonly titleService = inject(TitleService);
  private readonly dashboardService = inject(DashboardService);

  dateFilterForm: FormGroup;
  fiscalYears: number[] = [];

  FAChartData: ChartData = {
    labels: [],
    datasets: [],
  };

  LVSChartData: ChartData = {
    labels: [],
    datasets: [],
  };

  // PO
  POFAChartData: ChartData = {
    labels: [],
    datasets: [],
  };
  POLVSChartData: ChartData = {
    labels: [],
    datasets: [],
  };

  loading: boolean = false;

  materialGroupFAData: PieChartDataItem[] = [];
  materialGroupLVSData: PieChartDataItem[] = [];

  materialGroupFAOptions: PieChartOptions = {
    doughnut: false,
    cutoutPercentage: 60,
    showLegend: true,
    legendPosition: 'bottom',
    showPercentage: true,
    responsive: true,
    aspectRatio: 1,
    maxLegendHeight: 100,
    legendFontSize: 10,
  };

  materialGroupLVSOptions: PieChartOptions = {
    doughnut: false,
    cutoutPercentage: 60,
    showLegend: true,
    legendPosition: 'bottom',
    showPercentage: true,
    responsive: true,
    aspectRatio: 1,
    maxLegendHeight: 100,
    legendFontSize: 10,
  };

  distributorFAData: PieChartDataItem[] = [];
  distributorLVSData: PieChartDataItem[] = [];

  distributorFAOptions: PieChartOptions = {
    doughnut: false,
    cutoutPercentage: 60,
    showLegend: true,
    legendPosition: 'bottom',
    showPercentage: true,
    responsive: true,
    aspectRatio: 1,
    maxLegendHeight: 100,
    legendFontSize: 10,
  };

  distributorLVSOptions: PieChartOptions = {
    doughnut: false,
    cutoutPercentage: 60,
    showLegend: true,
    legendPosition: 'bottom',
    showPercentage: true,
    responsive: true,
    aspectRatio: 1,
    maxLegendHeight: 100,
    legendFontSize: 10,
  };

  materialGroupSalesData: MaterialGroupSalesData[] = [];
  distributorSalesData: DistributorSalesData[] = [];
  materialGroupDataError: string | null = null;
  distributorDataError: string | null = null;
  AppPermissions = AppPermissions;

  ngOnInit(): void {
    this.titleService.setTitle('Dashboard');

    this.generateFiscalYears();
    this.buildForm();
    this.loadChartData();
    this.loadPieChartData();
  }

  get hasDataFA(): boolean {
    return (
      this.FAChartData &&
      this.FAChartData.datasets &&
      this.FAChartData.datasets.some(dataset => dataset.data && dataset.data.length > 0)
    );
  }

  get hasDataLVS(): boolean {
    return (
      this.LVSChartData &&
      this.LVSChartData.datasets &&
      this.LVSChartData.datasets.some(dataset => dataset.data && dataset.data.length > 0)
    );
  }

  get hasDataPOFA(): boolean {
    return (
      this.POFAChartData &&
      this.POFAChartData.datasets &&
      this.POFAChartData.datasets.some(dataset => dataset.data && dataset.data.length > 0)
    );
  }

  get hasDataPOLVS(): boolean {
    return (
      this.POLVSChartData &&
      this.POLVSChartData.datasets &&
      this.POLVSChartData.datasets.some(dataset => dataset.data && dataset.data.length > 0)
    );
  }

  get hasDataMaterialGroupFA(): boolean {
    return this.materialGroupFAData && this.materialGroupFAData.length > 0;
  }

  get hasDataMaterialGroupLVS(): boolean {
    return this.materialGroupLVSData && this.materialGroupLVSData.length > 0;
  }

  get hasDataDistributorFA(): boolean {
    return this.distributorFAData && this.distributorFAData.length > 0;
  }

  get hasDataDistributorLVS(): boolean {
    return this.distributorLVSData && this.distributorLVSData.length > 0;
  }

  buildForm(): void {
    const now = new Date();
    const currentYear = now.getFullYear();
    const currentMonth = now.getMonth();

    const currentFiscalYear = currentMonth < 3 ? currentYear - 1 : currentYear;

    this.dateFilterForm = this.fb.group({
      fiscalYear: [currentFiscalYear],
    });
  }

  refreshData(): void {
    this.materialGroupDataError = null;
    this.distributorDataError = null;
    this.loadChartData();
    this.loadPieChartData();
  }

  generateFiscalYears(): void {
    const currentYear = new Date().getFullYear();
    for (let year = currentYear - 5; year <= currentYear + 1; year++) {
      this.fiscalYears.push(year);
    }
    this.fiscalYears.sort((a, b) => b - a);
  }

  loadChartData(): void {
    this.loading = true;
    try {
      const fiscalYear = this.dateFilterForm.get('fiscalYear')?.value || new Date().getFullYear();
      this.dashboardService
        .saleResultBase(fiscalYear)
        .pipe(finalize(() => (this.loading = false)))
        .subscribe({
          next: (response: SaleResultBaseDto[]) => {
            this.FAChartData = this.processChartData(response, EMaterialType.FA);
            this.LVSChartData = this.processChartData(response, EMaterialType.LVS);
          },
          error: error => {
            console.error('Failed to load sale result data:', error);
          },
        });

      this.dashboardService
        .poResult(fiscalYear)
        .pipe(finalize(() => (this.loading = false)))
        .subscribe({
          next: (response: SaleResultPODto[]) => {
            this.POFAChartData = this.processPOApiData(response, EMaterialType.FA);
            this.POLVSChartData = this.processPOApiData(response, EMaterialType.LVS);
          },
          error: error => {
            console.error('Failed to load PO result data:', error);
          },
        });
    } finally {
      this.loading = false;
    }
  }

  private processPOApiData(data: SaleResultPODto[], materialType: EMaterialType): ChartData {
    const filteredData = data.filter(item => {
      const types = item.materialType?.split(',') || [];
      return types.includes(materialType);
    });

    const labels = filteredData.map(item => item.material_Group);
    const planValues = filteredData.map(item => {
      return item.plan ? parseFloat(item.plan) : 0;
    });

    const resultValues = filteredData.map(item => item.saleResult);
    const achievementValues = filteredData.map(item => {
      const planValue = item.plan ? parseFloat(item.plan) : 0;
      return planValue > 0 ? Math.round((item.saleResult / planValue) * 100) : 0;
    });

    return {
      labels,
      datasets: [
        {
          label: 'Plan',
          data: planValues,
          backgroundColor: 'rgba(54, 162, 235, 0.5)',
          borderColor: 'rgb(54, 162, 235)',
          borderWidth: 1,
        },
        {
          label: 'PO Result',
          data: resultValues,
          backgroundColor: 'rgba(255, 99, 132, 0.5)',
          borderColor: 'rgb(255, 99, 132)',
          borderWidth: 1,
        },
        {
          label: 'Achievement (%)',
          data: achievementValues,
          backgroundColor: 'rgba(75, 192, 192, 0.5)',
          borderColor: 'rgb(75, 192, 192)',
          borderWidth: 1,
          type: 'line',
          yAxisID: 'percentage',
        },
      ],
    };
  }

  loadPieChartData(): void {
    this.loading = true;
    this.materialGroupDataError = null;
    this.distributorDataError = null;

    const fiscalYear = this.dateFilterForm.get('fiscalYear')?.value || new Date().getFullYear();
    forkJoin({
      materialGroups: this.dashboardService.saleResultByMaterialGroup(fiscalYear).pipe(
        catchError(error => {
          console.error('Failed to load material group data:', error);
          this.materialGroupDataError = 'Failed to load material group data. Using sample data instead.';
          return of([] as SaleResultMaterialGroupDto[]);
        }),
      ),
      distributors: this.dashboardService.saleResultByBuyer(fiscalYear).pipe(
        catchError(error => {
          console.error('Failed to load distributor data:', error);
          this.distributorDataError = 'Failed to load distributor data. Using sample data instead.';
          return of([] as SaleResultBuyerDto[]);
        }),
      ),
    })
      .pipe(
        finalize(() => {
          this.loading = false;
        }),
      )
      .subscribe({
        next: results => {
          this.materialGroupSalesData = results.materialGroups.map(item => ({
            materialType: item.materialType || '',
            materialGroup: item.material_Group || '',
            saleResult: item.saleResult || 0,
          }));

          this.distributorSalesData = results.distributors.map(item => ({
            buyerCode: item.buyerCode || '',
            materialType: item.materialType || '',
            saleResult: item.saleResult || 0,
          }));

          this.processMaterialGroupData();
          this.processDistributorData();
        },
      });
  }

  private processChartData(data: SaleResultBaseDto[], materialType: EMaterialType): ChartData {
    const filteredData = data.filter(item => item.materialType === materialType);
    const labels = filteredData.map(item => item.buyerCode);

    const targetValues = filteredData.map(item => item.saleTarget);
    const resultValues = filteredData.map(item => item.saleResult);
    const achievementValues = filteredData.map(item => Math.round((item.saleResult / item.saleTarget) * 100));

    return {
      labels,
      datasets: [
        {
          label: 'Sales Target',
          data: targetValues,
          backgroundColor: 'rgba(54, 162, 235, 0.5)',
          borderColor: 'rgb(54, 162, 235)',
          borderWidth: 1,
        },
        {
          label: 'Sales Result',
          data: resultValues,
          backgroundColor: 'rgba(255, 99, 132, 0.5)',
          borderColor: 'rgb(255, 99, 132)',
          borderWidth: 1,
        },
        {
          label: 'Achievement (%)',
          data: achievementValues,
          backgroundColor: 'rgba(75, 192, 192, 0.5)',
          borderColor: 'rgb(75, 192, 192)',
          borderWidth: 1,
          type: 'line',
          yAxisID: 'percentage',
        },
      ],
    };
  }

  private processMaterialGroupData(): void {
    // Process FA material groups
    this.materialGroupFAData = this.transformToChartData(
      this.materialGroupSalesData.filter(item => item.materialType === EMaterialType.FA),
      'materialGroup',
      'saleResult',
      MATERIAL_GROUP_COLORS,
    );

    // Process LVS material groups
    this.materialGroupLVSData = this.transformToChartData(
      this.materialGroupSalesData.filter(item => item.materialType === EMaterialType.LVS),
      'materialGroup',
      'saleResult',
      MATERIAL_GROUP_COLORS,
    );

    const faTotalSales = this.materialGroupFAData.reduce((sum, item) => sum + item.value, 0);
    const lvsTotalSales = this.materialGroupLVSData.reduce((sum, item) => sum + item.value, 0);

    this.materialGroupFAOptions = {
      ...this.materialGroupFAOptions,
      centerText: `Total\n${this.formatNumber(faTotalSales)}`,
    };

    this.materialGroupLVSOptions = {
      ...this.materialGroupLVSOptions,
      centerText: `Total\n${this.formatNumber(lvsTotalSales)}`,
    };
  }

  private processDistributorData(): void {
    // Process FA distributors
    this.distributorFAData = this.transformToChartData(
      this.distributorSalesData.filter(item => item.materialType === EMaterialType.FA),
      'buyerCode',
      'saleResult',
      DISTRIBUTOR_COLORS,
    );

    // Process LVS distributors
    this.distributorLVSData = this.transformToChartData(
      this.distributorSalesData.filter(item => item.materialType === EMaterialType.LVS),
      'buyerCode',
      'saleResult',
      DISTRIBUTOR_COLORS,
    );

    const faTotalSales = this.distributorFAData.reduce((sum, item) => sum + item.value, 0);
    const lvsTotalSales = this.distributorLVSData.reduce((sum, item) => sum + item.value, 0);

    this.distributorFAOptions = {
      ...this.distributorFAOptions,
      centerText: `Total\n${this.formatNumber(faTotalSales)}`,
    };

    this.distributorLVSOptions = {
      ...this.distributorLVSOptions,
      centerText: `Total\n${this.formatNumber(lvsTotalSales)}`,
    };
  }

  private transformToChartData(
    data: any[],
    labelField: string,
    valueField: string,
    colorPalette: string[],
  ): PieChartDataItem[] {
    const groupedData = data.reduce((acc, item) => {
      const key = item[labelField];
      if (!acc[key]) {
        acc[key] = 0;
      }
      acc[key] += item[valueField];
      return acc;
    }, {});

    return Object.keys(groupedData)
      .map((key, index) => ({
        label: key,
        value: groupedData[key],
        color: colorPalette[index % colorPalette.length],
      }))
      .sort((a, b) => b.value - a.value);
  }

  private formatNumber(value: number): string {
    return new Intl.NumberFormat('en-US').format(value);
  }
}
