import { CoreModule, ListService } from '@abp/ng.core';
import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { ChangeDetectionStrategy, Component, inject, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { NgbNavModule, NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule } from '@angular/common';
import { PageModule } from '@abp/ng.components/page';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize } from 'rxjs';
import { AppRoutes } from '@app/app.routes';
import { PSIDetailViewService } from '@app/psis/services/psi-detail.service';
import { HistoryModalComponent } from '@app/shared/components/history-modal/history-modal.component';
import { StatusLabelComponent } from '@app/shared/status/components/status-label.component';
import { ApproversModalComponent } from '@app/shared/components/approvers-modal/approvers-modal.component';
import { ApprovalCommentModalComponent } from '@app/shared/components/approval-comment/approval-comment.component';
import { ExpandablePanelV2Component } from '@app/shared/components/expandable-panel-v2/expandable-panel-v2.component';
import { TitleService } from '@app/shared/services/title/title.service';

export interface TableColumn {
  prop: string;
  name: string;
  minWidth?: number;
  sortable?: boolean;
  format?: string;
  frozenLeft?: boolean;
  cellTemplate?: TemplateRef<any>;
}

interface PSIDetail {
  materialGroup?: string; // Make it optional to match PSIDetailDto
  month?: number; // Make it optional to match PSIDetailDto
  plan?: number; // Make it optional to match PSIDetailDto
}

interface PSIRow {
  materialGroup: string;
  [key: string]: string | number; // For dynamic month columns
}

@Component({
  selector: 'app-psi-detail',
  changeDetection: ChangeDetectionStrategy.Default,
  standalone: true,
  imports: [
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    CommonModule,
    PageModule,
    ReactiveFormsModule,
    NgbNavModule,
    HistoryModalComponent,
    StatusLabelComponent,
    ApproversModalComponent,
    ApprovalCommentModalComponent,
    ExpandablePanelV2Component,
    NgbTooltip,
  ],
  providers: [ListService, PSIDetailViewService],
  templateUrl: './psi-detail.component.html',
  styleUrls: ['./psi-detail.component.scss'],
})
export class PSIDetailComponent implements OnInit {
  protected readonly router = inject(Router);
  protected route = inject(ActivatedRoute);
  public readonly service = inject(PSIDetailViewService);
  public readonly toast = inject(ToasterService);
  public readonly titleService = inject(TitleService);

  protected readonly title = 'PSI - Detail';
  private readonly fiscalOrder = [4, 5, 6, 7, 8, 9, 10, 11, 12, 1, 2, 3];
  private readonly monthNames = new Map<number, string>([
    [1, 'Jan'],
    [2, 'Feb'],
    [3, 'March'],
    [4, 'April'],
    [5, 'May'],
    [6, 'June'],
    [7, 'July'],
    [8, 'August'],
    [9, 'September'],
    [10, 'October'],
    [11, 'November'],
    [12, 'December'],
  ]);
  columns: TableColumn[] = [];
  rows: PSIRow[] = [];
  form: FormGroup = new FormGroup({});
  requestId: string | null;

  showHistoryModal = false;
  approvalHistories = [];

  @ViewChild('approversModalComponent', { static: false }) approversModalComponent: ApproversModalComponent;
  @ViewChild('customCellTemplate', { static: true }) customCellTemplate: TemplateRef<unknown>;

  getRowClass = (row: PSIRow) => {
    if (row.materialGroup === '') {
      return 'total-row';
    }
    return '';
  };

  ngOnInit() {
    this.titleService.setTitle('PSI Detail | PSI');

    this.requestId = this.route.snapshot.paramMap.get('id');
    if (this.requestId) {
      this.service.isBusy = true;

      this.service
        .loadDetailsAndImports(this.requestId)
        .pipe(finalize(() => (this.service.isBusy = false)))
        .subscribe({
          next: ({ item }) => {
            this.service.selected = item;
            this.service.buildForm();
            this.buildTable(item.details);
          },
          error: error => {
            console.error('Error loading data', error);
          },
        });
    }
  }

  goBack(): void {
    const currentUrl = this.router.url;

    const routesMap = [
      {
        condition:
          currentUrl.includes(AppRoutes.PSI.BASE) &&
          currentUrl.includes(AppRoutes.PSI.DETAILS.BASE) &&
          currentUrl.includes(AppRoutes.PSI.MY_APPROVALS.BASE),
        route: [AppRoutes.PSI.BASE, AppRoutes.PSI.MY_APPROVALS.BASE],
      },
      {
        condition: currentUrl.includes(AppRoutes.PSI.BASE) && currentUrl.includes(AppRoutes.PSI.DETAILS.BASE),
        route: [AppRoutes.PSI.BASE, AppRoutes.PSI.LIST.BASE],
      },
    ];

    for (const route of routesMap) {
      if (route.condition) {
        this.router.navigate(route.route);
        break;
      }
    }
  }

  private buildTable(details: PSIDetail[]) {
    const months = Array.from(new Set(details.map(d => d.month).filter(m => m != null) as number[]));

    const sorted = this.fiscalOrder.filter(m => months.includes(m));

    this.columns = [
      { prop: 'materialGroup', name: 'Material Group', frozenLeft: true, minWidth: 150 },
      ...sorted.map(m => ({
        prop: this.monthNames.get(m)!,
        name: this.monthNames.get(m)!,
        format: 'number',
        minWidth: 100,
      })),
    ];

    // Step 1: Build the map
    const map = details.reduce(
      (acc, cur) => {
        const key = cur.materialGroup;
        if (!acc[key]) acc[key] = { materialGroup: key };
        const name = this.monthNames.get(cur.month);
        if (name) acc[key][name] = cur.plan ?? 0;
        return acc;
      },
      {} as Record<string, PSIRow>,
    );

    // Step 2: Initialize totals
    const monthlyTotals: PSIRow = { materialGroup: '' }; // Empty materialGroup for total row
    for (const name of this.monthNames.values()) {
      monthlyTotals[name] = 0;
    }

    // Step 3: Calculate totals
    Object.values(map).forEach((row: PSIRow) => {
      for (const name of this.monthNames.values()) {
        monthlyTotals[name] = (monthlyTotals[name] as number) + ((row[name] as number) ?? 0);
      }
    });

    // Step 4: Convert map to array and insert total at the beginning
    this.rows = [monthlyTotals, ...Object.values(map)];
  }

  showHistory() {
    this.showHistoryModal = true;
    this.approvalHistories = this.service.selected?.approvalHistories;
  }

  closeHistoryDialog(): void {
    this.showHistoryModal = false;
    this.approvalHistories = [];
  }

  performAction(action: string) {
    this.service.performAction(action);
  }

  loadApprovalList() {
    if (this.service?.selected?.id) {
      this.service.getListApprovers(this.service?.selected?.id).subscribe({
        next: approvers => {
          this.approversModalComponent.openModal(approvers);
        },
        error: () => {
          this.toast.error('Failed to load approvers.');
        },
      });
    }
  }
}
