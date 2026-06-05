import { CoreModule, ListService } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { ChangeDetectionStrategy, Component, inject, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormGroup, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgbNavModule, NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule } from '@angular/common';
import { PageModule } from '@abp/ng.components/page';
import { ActivatedRoute, Router } from '@angular/router';

import { finalize } from 'rxjs';
import { AppRoutes } from '@app/app.routes';
import { StatusLabelComponent } from '@app/shared/status/components/status-label.component';
import { ExpandablePanelV2Component } from '@app/shared/components/expandable-panel-v2/expandable-panel-v2.component';
import { TitleService } from '@app/shared/services/title/title.service';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { ImportBatchRequestDetailViewService } from '../services/import-batch-request-detail.service';
import { ImportBatchRequestColumns, ImportBatchRequestType } from './import-batch-request/batch-request.types';
import { SpoBatchRequestService } from '@proxy/spo-batch-requests';

@Component({
  selector: 'app-import-batch-request-detail-modal',
  changeDetection: ChangeDetectionStrategy.Default,
  standalone: true,
  imports: [
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    CommonModule,
    PageModule,
    ReactiveFormsModule,
    FormsModule,
    NgbNavModule,
    StatusLabelComponent,
    ExpandablePanelV2Component,
    NgbTooltip,
  ],
  providers: [ListService, ImportBatchRequestDetailViewService],
  templateUrl: './import-batch-request-detail.component.html',
  styleUrls: ['./import-batch-request-detail.component.scss'],
})
export class ImportBatchRequestDetailComponent implements OnInit {
  protected readonly router = inject(Router);
  protected route = inject(ActivatedRoute);
  public readonly service = inject(ImportBatchRequestDetailViewService);
  public readonly titleService = inject(TitleService);
  public readonly loadingService = inject(LoadingService);
  public readonly proxyService = inject(SpoBatchRequestService);
  public readonly confirmation = inject(ConfirmationService);
  public readonly toast = inject(ToasterService);

  protected readonly title = 'SPO Batch Request Detail';
  pageSize = 50;
  pageIndex = 0;
  pagedRows: any[] = [];

  // Search
  searchTerm: string = '';
  allRows: any[] = [];
  filteredRows: any[] = [];
  searchableFields = ['spoCode', 'golfaCode', 'note'];

  // ✅ Checkbox selection
  selectedItemIds: string[] = [];

  form: FormGroup = new FormGroup({});
  requestId: string | null;
  importType = ImportBatchRequestType;
  columns = ImportBatchRequestColumns;

  @ViewChild('customCellTemplate', { static: true }) customCellTemplate: TemplateRef<any>;

  ngOnInit() {
    this.titleService.setTitle('SPO Batch Request Detail | SPO Batch Request Management');

    this.requestId = this.route.snapshot.paramMap.get('id');
    if (this.requestId) {
      this.loadData();
    }
  }

  private loadData(): void {
    this.service.isBusy = true;
    this.service
      .loadDetailsAndImports(this.requestId)
      .pipe(finalize(() => (this.service.isBusy = false)))
      .subscribe({
        next: ({ details }) => {
          this.service.selected = details;
          this.service.buildForm();

          this.allRows = this.service?.selected?.spoBatchRequestDetails || [];
          this.filteredRows = [...this.allRows];
          this.selectedItemIds = [];
          this.updatePagedRows();

          if (this.columns?.[details.importType]?.length > 0) {
            this.columns?.[details.importType]?.forEach(col => {
              col.cellTemplate = this.customCellTemplate;
            });
          }
        },
        error: error => {
          console.error('Error loading data', error);
        },
      });
  }

  updatePagedRows() {
    const start = this.pageIndex * this.pageSize;
    this.pagedRows = this.filteredRows.slice(start, start + this.pageSize);
  }

  onPage(event: any) {
    this.pageIndex = event.offset;
    this.updatePagedRows();
  }

  onSearch(event: any) {
    const term = this.searchTerm.toLowerCase().trim();
    if (!term) {
      this.filteredRows = [...this.allRows];
    } else {
      this.filteredRows = this.allRows.filter(row =>
        this.searchableFields.some(field => {
          const value = row[field];
          if (value === null || value === undefined) return false;
          return value.toString().toLowerCase().includes(term);
        }),
      );
    }
    this.pageIndex = 0;
    this.updatePagedRows();
  }

  clearSearch() {
    this.searchTerm = '';
    this.filteredRows = [...this.allRows];
    this.pageIndex = 0;
    this.updatePagedRows();
  }

  isItemDeletable(row: any): boolean {
    return row.status === 'NOT_START';
  }

  get deletableRows(): any[] {
    return this.filteredRows.filter(row => this.isItemDeletable(row));
  }

  get isAllSelected(): boolean {
    return this.deletableRows.length > 0 && this.deletableRows.every(row => this.selectedItemIds.includes(row.id));
  }

  get isIndeterminate(): boolean {
    const count = this.deletableRows.filter(row => this.selectedItemIds.includes(row.id)).length;
    return count > 0 && count < this.deletableRows.length;
  }

  toggleSelectAll(checked: boolean): void {
    if (checked) {
      const allDeletableIds = this.deletableRows.map(row => row.id);
      this.selectedItemIds = [...new Set([...this.selectedItemIds, ...allDeletableIds])];
    } else {
      const deletableIds = this.deletableRows.map(row => row.id);
      this.selectedItemIds = this.selectedItemIds.filter(id => !deletableIds.includes(id));
    }
  }

  toggleSelectItem(id: string, checked: boolean): void {
    if (checked) {
      this.selectedItemIds = [...this.selectedItemIds, id];
    } else {
      this.selectedItemIds = this.selectedItemIds.filter(i => i !== id);
    }
  }

  onDeleteSelectedItems(): void {
    if (!this.selectedItemIds.length) {
      this.toast.warn('Please select at least one item to delete.');
      return;
    }

    this.confirmation
      .warn('Are you sure you want to delete the selected items?', 'Confirm Delete', {
        yesText: '::Yes',
        cancelText: '::No',
      })
      .subscribe(result => {
        if (result === Confirmation.Status.confirm) {
          this.loadingService.show();
          this.proxyService
            .deleteBatchRequestItems(this.requestId, this.selectedItemIds)
            .pipe(finalize(() => this.loadingService.hide()))
            .subscribe({
              next: () => {
                this.toast.success('Items deleted successfully.');

                const remainingRows = this.allRows.filter(row => !this.selectedItemIds.includes(row.id));

                if (remainingRows.length === 0) {
                  this.goBack();
                } else {
                  this.loadData();
                }
              },
              error: err => {
                const message = err?.error?.error?.message || 'Failed to delete items. Please try again.';
                this.toast.error(message);
              },
            });
        }
      });
  }

  goBack(): void {
    const currentUrl = this.router.url;
    const returnStatus = this.route.snapshot.queryParams['returnStatus'];

    const routesMap = [
      {
        condition:
          currentUrl.includes(AppRoutes.SPECIAL_PRICE_OFFERS.BASE) &&
          currentUrl.includes(AppRoutes.SPECIAL_PRICE_OFFERS.DETAILS_BATCH_REQUEST.BASE),
        route: [AppRoutes.SPECIAL_PRICE_OFFERS.BASE, AppRoutes.SPECIAL_PRICE_OFFERS.BATCH_REQUEST.BASE],
      },
    ];
    for (const route of routesMap) {
      if (route.condition) {
        this.router.navigate(route.route, {
          queryParams: { status: returnStatus ?? 'IN_PROGRESS' },
        });
        break;
      }
    }
  }
}
