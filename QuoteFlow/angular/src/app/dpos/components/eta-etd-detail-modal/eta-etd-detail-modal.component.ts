import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DPOLockOnOrderStockDto, DPOLockStockEtaEtdDto, DPOService } from '@app/proxy/dpos';
import { DPODetailDto } from '@app/proxy/dpos/dpodetails/models';
import {
  AppAdvancedDataTableComponent,
  AppTableColumnDirective,
  AppTableColumnGroupDirective,
} from '@app/shared/components/advanced-data-table';
import { NgSelectModule } from '@ng-select/ng-select';
import { catchError, EMPTY, finalize, tap } from 'rxjs';

export interface LockStockDetailResult {
  confirmed: boolean;
  stockCategoryId?: string;
  lockQty?: number;
  note?: string;
}

@Component({
  selector: 'app-eta-etd-detail-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ThemeSharedModule,
    NgSelectModule,
    AppAdvancedDataTableComponent,
    AppTableColumnDirective,
    AppTableColumnGroupDirective,
  ],
  templateUrl: './eta-etd-detail-modal.component.html',
  styleUrls: ['./eta-etd-detail-modal.component.scss'],
})
export class ETAETDDetailModalComponent implements OnInit {
  private readonly dpoService = inject(DPOService);

  @Input() visible: boolean = false;
  @Input() dpoDetail: DPODetailDto | null = null;
  @Input() dpoId: string = '';
  @Input() dpoDetailId: string = '';
  @Input() poDetail: DPOLockOnOrderStockDto | null = null;

  @Output() visibleChange = new EventEmitter<boolean>();

  loading = false;

  // Table data
  etaEtdData: DPOLockStockEtaEtdDto[] = [];
  tableLoading = false;

  ngOnInit(): void {
    if (this.visible) {
      this.loadETAETDData();
    }
  }

  onCancel(): void {
    this.closeModal();
  }

  closeModal(): void {
    this.visible = false;
    this.visibleChange.emit(false);
  }

  private loadETAETDData(): void {
    if (!this.dpoDetailId || !this.poDetail) {
      return;
    }

    this.tableLoading = true;
    this.dpoService
      .getListLockStockEtaEtd(this.dpoDetailId, this.poDetail.poDetailId)
      .pipe(
        tap(result => {
          this.etaEtdData = result?.items || [];
        }),
        catchError(error => {
          console.error('Error loading lock stock data:', error);
          this.etaEtdData = [];
          return EMPTY;
        }),
        finalize(() => {
          this.tableLoading = false;
        }),
      )
      .subscribe();
  }

  // Formatters for table columns
  formatQuantity = (val: number): string => {
    return val ? new Intl.NumberFormat('en-US').format(val) : '0';
  };

  formatDate = (value: any): string => {
    return value ? new Date(value).toLocaleDateString('en-GB') : '';
  };
}
