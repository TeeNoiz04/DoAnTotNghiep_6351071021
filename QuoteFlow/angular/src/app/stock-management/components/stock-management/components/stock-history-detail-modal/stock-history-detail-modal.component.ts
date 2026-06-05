import { AbpWindowService } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  AppAdvancedDataTableComponent,
  AppTableColumnDirective,
  AppTableColumnGroupDirective,
} from '@app/shared/components/advanced-data-table';
import { ExpandablePanelComponent } from '@app/shared/components/expandable-panel/expandable-panel.component';
import { EscCloseModalDirective } from '@app/shared/esc-close-modal/esc-close-modal.directive';
import { DateHelper } from '@app/shared/helpers/date-helper';
import { TableFilterPipe } from '@app/shared/pipes/table-filter.pipe';
import { UsernamePipe } from '@app/shared/pipes/username.pipe';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { LookupService } from '@proxy/general-lookups';
import { HistoryTrackingDto } from '@proxy/history-trackings';
import { LookupDto } from '@proxy/shared';
import { StockManagementListDto, StockManagementService } from '@proxy/stock-managements';
import { catchError, EMPTY, finalize, tap } from 'rxjs';
@Component({
  selector: 'app-stock-history-detail-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ThemeSharedModule,
    NgSelectModule,
    AppAdvancedDataTableComponent,
    AppTableColumnDirective,
    AppTableColumnGroupDirective,
    ExpandablePanelComponent,
    NgbDatepickerModule,
    EscCloseModalDirective,
  ],
  templateUrl: './stock-history-detail-modal.component.html',
  styleUrls: ['./stock-history-detail-modal.component.scss'],
})
export class StockHistoryDetailModalComponent implements OnInit {
  private fb = inject(FormBuilder);
  private readonly service = inject(StockManagementService);
  private readonly lookupService = inject(LookupService);
  private readonly abpWindowService = inject(AbpWindowService);
  private readonly usernamePipe = new UsernamePipe();
  private readonly tableFilterPipe = new TableFilterPipe();

  @Input() visible: boolean = false;
  @Input() materialItem: StockManagementListDto = undefined;
  @Input() stockCode: string = '';
  @Input() golfaCode: string = '';

  @Output() visibleChange = new EventEmitter<boolean>();

  serverFilterForm: FormGroup;
  loading = false;
  historyData: HistoryTrackingDto[] = [];
  filteredHistoryData: HistoryTrackingDto[] = [];
  tableLoading = false;
  searchText = '';
  stockCategoryOptions: LookupDto<string>[] = [];
  isExportToExcelBusy = false;

  ngOnInit(): void {
    if (this.visible) {
      this.buildForm();
      this.loadStockCategories();
      this.loadHistoryData();
    }
  }

  onCancel(): void {
    this.closeModal();
  }

  closeModal(): void {
    this.visible = false;
    this.visibleChange.emit(false);
    this.resetFilters();
  }

  buildForm() {
    // Default "From" date to 6 months ago
    const sixMonthsAgo = new Date();
    sixMonthsAgo.setMonth(sixMonthsAgo.getMonth() - 6);

    // Set stock code from input or material item
    const defaultStockCode = this.stockCode || '';

    this.serverFilterForm = this.fb.group({
      actionFrom: [DateHelper.formatDateGmt7(sixMonthsAgo)],
      actionTo: [''],
      stockCode: [defaultStockCode],
    });
  }

  private loadStockCategories(): void {
    this.lookupService.getStockCategoryLookup().subscribe({
      next: result => {
        this.stockCategoryOptions = result.items || [];

        // Load damaged stock categories and merge
        this.lookupService.getDamagedStockCategoryLookup().subscribe({
          next: damagedResult => {
            this.stockCategoryOptions = [...this.stockCategoryOptions, ...(damagedResult.items || [])];

            // Set default stock code after all categories are loaded
            const materialStockCode = this.stockCategoryOptions.find(x => x.id === this.materialItem?.stockCategoryId);
            const defaultStockCode = this.stockCode || materialStockCode || '';

            if (defaultStockCode) {
              this.serverFilterForm.patchValue({ stockCode: defaultStockCode });
            }
          },
          error: error => {
            console.error('Error loading damaged stock categories:', error);

            // Still set default stock code even if damaged categories fail
            const materialStockCode = this.stockCategoryOptions.find(x => x.id === this.materialItem?.stockCategoryId);
            const defaultStockCode = this.stockCode || materialStockCode || '';

            if (defaultStockCode) {
              this.serverFilterForm.patchValue({ stockCode: defaultStockCode });
            }
          },
        });
      },
      error: error => {
        console.error('Error loading stock categories:', error);
      },
    });
  }

  private loadHistoryData(): void {
    if (!this.materialItem && !this.golfaCode) {
      return;
    }

    const formValues = this.serverFilterForm.value;
    const golfaCode = this.golfaCode || this.materialItem?.golfaCode;
    const stockCode = formValues.stockCode;

    if (!golfaCode || !stockCode) {
      return;
    }

    this.tableLoading = true;

    const input = {
      stockCode: stockCode,
      golfaCode: golfaCode,
      actionFrom: formValues.actionFrom || undefined,
      actionTo: formValues.actionTo || undefined,
      note: undefined,
    };

    this.service
      .getStockHistory(input)
      .pipe(
        tap(result => {
          this.historyData = result.items || [];
          this.applySearchFilter();
        }),
        catchError(error => {
          console.error('Error loading stock history data:', error);
          this.historyData = [];
          this.filteredHistoryData = [];
          return EMPTY;
        }),
        finalize(() => {
          this.tableLoading = false;
        }),
      )
      .subscribe();
  }

  onApplyServerFilter(): void {
    this.loadHistoryData();
  }

  private applySearchFilter(): void {
    this.filteredHistoryData = this.tableFilterPipe.transform(this.historyData, this.searchText);
  }

  onSearchChange(): void {
    this.applySearchFilter();
  }

  clearSearch(): void {
    this.searchText = '';
    this.applySearchFilter();
  }

  exportToExcel(): void {
    if (!this.materialItem && !this.golfaCode) {
      return;
    }

    this.isExportToExcelBusy = true;
    const formValues = this.serverFilterForm.value;
    const golfaCode = this.golfaCode || this.materialItem?.golfaCode;
    const stockCode = formValues.stockCode;

    if (!golfaCode || !stockCode) {
      this.isExportToExcelBusy = false;
      return;
    }

    const input = {
      stockCode: stockCode,
      golfaCode: golfaCode,
      actionFrom: formValues.actionFrom || undefined,
      actionTo: formValues.actionTo || undefined,
      note: undefined,
    };

    this.service
      .getStockHistoryAsExcel(input)
      .pipe(
        finalize(() => {
          this.isExportToExcelBusy = false;
        }),
      )
      .subscribe({
        next: result => {
          const fileName = `Stock_History_${golfaCode}_${stockCode}_${new Date().toISOString().split('T')[0]}.xlsx`;
          this.abpWindowService.downloadBlob(result, fileName);
        },
        error: error => {
          console.error('Error exporting stock history to Excel:', error);
        },
      });
  }

  private resetFilters(): void {
    this.searchText = '';
    this.historyData = [];
    this.filteredHistoryData = [];
  }

  // Formatters for table columns
  formatQuantity = (val: number): string => {
    return val !== null && val !== undefined ? new Intl.NumberFormat('en-US').format(val) : '';
  };

  formatDate = (val: string): string => {
    return val ? new Date(val).toLocaleDateString('en-GB') : '';
  };

  formatModifiedDate = (val: string): string => {
    return val
      ? new Date(val).toLocaleDateString('en-GB') +
          ' ' +
          new Date(val).toLocaleTimeString('en-GB', { hour: '2-digit', minute: '2-digit' })
      : '';
  };

  formatUsername = (val: string): string => {
    return val ? this.usernamePipe.transform(val) : '';
  };

  onClose(): void {
    this.closeModal();
  }
}
