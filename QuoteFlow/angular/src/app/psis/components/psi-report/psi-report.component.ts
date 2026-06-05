import { PageModule } from '@abp/ng.components/page';
import { CoreModule, ListService } from '@abp/ng.core';
import { DateAdapter, ThemeSharedModule, TimeAdapter } from '@abp/ng.theme.shared';
import { ChangeDetectionStrategy, Component, ViewEncapsulation } from '@angular/core';
import {
  NgbCollapseModule,
  NgbDateAdapter,
  NgbDatepickerModule,
  NgbDropdownModule,
  NgbTimeAdapter,
  NgbTimepickerModule,
  NgbTooltip,
  NgbTooltipModule,
} from '@ng-bootstrap/ng-bootstrap';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { TableEdgeScrollerComponent } from '@app/shared/components/table-edge-scroller/table-edge-scroller.component';
import {
  AbstractPSIReportComponent,
  ChildComponentDependencies,
  ChildTabDependencies,
} from './psi-report.abstract.component';
import { PsiReportFilterComponent } from './psi report filter/psi-report-filter.component';
import { PsiReportViewService } from '../../services/psi-report.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-psi-report',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    CommonModule,
    ...ChildTabDependencies,
    NgbCollapseModule,
    NgbDatepickerModule,
    NgbTimepickerModule,
    NgbDropdownModule,
    NgxValidateCoreModule,
    PageModule,
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    MatCheckboxModule,
    NgbTooltipModule,
    PsiReportFilterComponent,
    NgxDatatableModule,
    TableEdgeScrollerComponent,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [
    ListService,
    PsiReportViewService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './psi-report.component.html',
  styleUrls: ['./psi-report.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class PSIReportComponent extends AbstractPSIReportComponent {
  groupedData: any[] = [];
  totalRow: any = null;

  override ngOnInit() {
    super.ngOnInit();
    this.service.data = [];
  }

  get totalLabel(): string {
    return this.currentMaterialType === 'FA' ? 'Total FA' : 'Total SW';
  }

  ceil(value: number | null | undefined): number {
    if (value === null || value === undefined) {
      return 0;
    }
    return Math.ceil(value);
  }

  override updatePagedData() {
    console.log('📊 updatePagedData - Material Type:', this.currentMaterialType);
    console.log('🏷️ Total Label:', this.totalLabel);

    const items = this.service.data && Array.isArray(this.service.data) ? this.service.data : [];
    this.pagedData = items;
    this.groupDataByMaterialGroup();
    this.calculateTotalRow();
  }

  private groupDataByMaterialGroup() {
    if (!this.pagedData || this.pagedData.length === 0) {
      this.groupedData = [];
      return;
    }

    const groups = new Map();

    this.pagedData.forEach(item => {
      const materialGroup = item.materialGroupPSI || 'Unknown';
      const description = item.description || '';

      if (!groups.has(materialGroup)) {
        groups.set(materialGroup, {
          materialGroupPSI: materialGroup,
          poIssue: null,
          arriveToWH: null,
          sales: null,
          stock: null,
        });
      }

      const group = groups.get(materialGroup);

      if (description.toLowerCase().includes('po issue')) {
        group.poIssue = item;
      } else if (description.toLowerCase().includes('arrive to wh')) {
        group.arriveToWH = item;
      } else if (description.toLowerCase().includes('sales')) {
        group.sales = item;
      } else if (description.toLowerCase().includes('stock')) {
        group.stock = item;
      }
    });

    this.groupedData = Array.from(groups.values());
  }

  private calculateTotalRow() {
    if (!this.pagedData || this.pagedData.length === 0) {
      this.totalRow = null;
      return;
    }

    const total = {
      poIssue: this.initializeTotalObject(),
      arriveToWH: this.initializeTotalObject(),
      sales: this.initializeTotalObject(),
      stock: this.initializeTotalObject(),
    };

    this.pagedData.forEach(item => {
      const description = (item.description || '').toLowerCase();
      let targetCategory = null;

      if (description.includes('po issue')) {
        targetCategory = total.poIssue;
      } else if (description.includes('arrive to wh')) {
        targetCategory = total.arriveToWH;
      } else if (description.includes('sales')) {
        targetCategory = total.sales;
      } else if (description.includes('stock')) {
        targetCategory = total.stock;
      }

      if (targetCategory) {
        this.addToTotal(targetCategory, item);
      }
    });

    this.totalRow = total;
  }

  private initializeTotalObject(): any {
    return {
      planApril: 0,
      resultApril: 0,
      planMay: 0,
      resultMay: 0,
      planJune: 0,
      resultJune: 0,
      planJuly: 0,
      resultJuly: 0,
      planAugust: 0,
      resultAugust: 0,
      planSeptember: 0,
      resultSeptember: 0,
      planOctober: 0,
      resultOctober: 0,
      planNovember: 0,
      resultNovember: 0,
      planDecember: 0,
      resultDecember: 0,
      planJan: 0,
      resultJan: 0,
      planFeb: 0,
      resultFeb: 0,
      planMarch: 0,
      resultMarch: 0,
    };
  }

  private addToTotal(target: any, source: any) {
    const months = [
      'April',
      'May',
      'June',
      'July',
      'August',
      'September',
      'October',
      'November',
      'December',
      'Jan',
      'Feb',
      'March',
    ];

    months.forEach(month => {
      const planKey = `plan${month}`;
      const resultKey = `result${month}`;

      target[planKey] += source[planKey] || 0;
      target[resultKey] += source[resultKey] || 0;
    });
  }

  trackByMaterialGroup(index: number, item: any): string {
    return item.materialGroupPSI;
  }

  get totalCount(): number {
    return Array.isArray(this.service.data) ? this.service.data.length : 0;
  }

  get hasData(): boolean {
    return this.pagedData && this.pagedData.length > 0;
  }

  get Math() {
    return Math;
  }
}
