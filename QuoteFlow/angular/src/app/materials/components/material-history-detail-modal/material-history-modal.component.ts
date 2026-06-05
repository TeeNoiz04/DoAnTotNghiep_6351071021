import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbpWindowService, CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { MaterialService } from '../../../proxy/materials/material.service';
import type { HistoryTrackingDto } from '../../../proxy/history-trackings/models';
import type { MaterialDto } from '../../../proxy/materials/models';
import {
  AppAdvancedDataTableComponent,
  AppTableColumnGroupDirective,
  AppTableColumnDirective,
} from '@app/shared/components';
import { finalize } from 'rxjs';
@Component({
  selector: 'app-material-history-modal',
  standalone: true,
  imports: [
    CommonModule,
    CoreModule,
    ThemeSharedModule,
    AppAdvancedDataTableComponent,
    AppTableColumnGroupDirective,
    AppTableColumnDirective,
  ],
  templateUrl: './material-history-modal.component.html',
  styleUrls: ['./material-history-modal.component.scss'],
})
export class MaterialHistoryModalComponent {
  visible = false;
  loading = false;
  tableLoading = false;
  isExportToExcelBusy = false;

  materialItem: MaterialDto | null = null;
  historyData: HistoryTrackingDto[] = [];
  pageSize = 50;
  currentPage = 1;
  filteredHistoryData: HistoryTrackingDto[] = [];

  constructor(
    private materialService: MaterialService,
    private abpWindow: AbpWindowService,
  ) {}

  formatActionStatus = (value: string) => {
    if (!value) return '-';

    const formattedValue = value.toLowerCase().replace(/\b\w/g, char => char.toUpperCase());

    let badgeClass = 'badge';

    if (value === 'UPDATE STATUS') {
      badgeClass += ' bg-primary';
    } else if (value === 'UPDATE PRICE') {
      badgeClass += ' bg-success';
    } else {
      badgeClass += ' bg-secondary';
    }

    return `<span class="${badgeClass}">${formattedValue}</span>`;
  };

  open(material: MaterialDto) {
    this.materialItem = material;
    this.visible = true;
    this.loadHistoryData();
  }

  closeModal() {
    this.visible = false;
    this.materialItem = null;
    this.historyData = [];
    this.filteredHistoryData = [];
  }

  private loadHistoryData() {
    if (!this.materialItem?.golfaCode) return;

    this.loading = true;
    this.tableLoading = true;

    this.materialService.getListMaterialHistory(this.materialItem.golfaCode).subscribe({
      next: response => {
        this.historyData = response;
        this.filteredHistoryData = response;
        this.loading = false;
        this.tableLoading = false;
      },
      error: error => {
        console.error('Error loading history data:', error);
        this.loading = false;
        this.tableLoading = false;
      },
    });
  }

  exportToExcel() {
    if (!this.materialItem?.golfaCode) {
      console.error('Material code not found');
      return;
    }

    this.isExportToExcelBusy = true;

    // Nếu chỉ cần golfaCode:
    this.materialService
      .getMaterialHistoryAsExcel(this.materialItem.golfaCode)
      .pipe(finalize(() => (this.isExportToExcelBusy = false)))
      .subscribe({
        next: (blob: Blob) => {
          const fileName = `Material_History_${this.materialItem!.golfaCode}_${new Date().toISOString().split('T')[0]}.xlsx`;
          this.abpWindow.downloadBlob(blob, fileName);
        },
        error: err => console.error('Export error:', err),
      });
  }

  // Formatters for table columns
  formatQuantity = (value: any): string => {
    if (value === null || value === undefined) return '-';
    return new Intl.NumberFormat('en-US', {
      minimumFractionDigits: 0,
      maximumFractionDigits: 0,
    }).format(value);
  };

  formatModifiedDate = (value: any): string => {
    if (!value) return '-';
    const date = new Date(value);
    return new Intl.DateTimeFormat('en-GB', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
      hour12: false,
    }).format(date);
  };
  get totalItems(): number {
    return this.historyData?.length ?? 0;
  }

  get totalPages(): number {
    return Math.max(1, Math.ceil(this.totalItems / this.pageSize));
  }

  get startItem(): number {
    if (this.totalItems === 0) return 0;
    return (this.currentPage - 1) * this.pageSize + 1;
  }

  get endItem(): number {
    return Math.min(this.totalItems, this.currentPage * this.pageSize);
  }

  applyPagination() {
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    this.filteredHistoryData = this.historyData.slice(start, end);
  }

  goToPage(page: number) {
    if (page < 1 || page > this.totalPages || page === this.currentPage) return;
    this.currentPage = page;
    this.applyPagination();
  }

  prevPage() {
    if (this.currentPage <= 1) return;
    this.currentPage--;
    this.applyPagination();
  }

  nextPage() {
    if (this.currentPage >= this.totalPages) return;
    this.currentPage++;
    this.applyPagination();
  }

  goToFirst() {
    this.goToPage(1);
  }

  goToLast() {
    this.goToPage(this.totalPages);
  }

  get pagesToShow(): Array<number> {
    const pages: number[] = [];
    const maxButtons = 7;
    const total = this.totalPages;
    if (total <= maxButtons) {
      for (let i = 1; i <= total; i++) pages.push(i);
      return pages;
    }

    const half = Math.floor(maxButtons / 2);
    let start = Math.max(1, this.currentPage - half);
    let end = Math.min(total, this.currentPage + half);

    if (this.currentPage - start < half) {
      end = Math.min(total, end + (half - (this.currentPage - start)));
    }
    if (end - this.currentPage < half) {
      start = Math.max(1, start - (half - (end - this.currentPage)));
    }

    if (start > 1) {
      pages.push(1);
      if (start > 2) pages.push(-1);
    }

    for (let p = start; p <= end; p++) pages.push(p);

    if (end < total) {
      if (end < total - 1) pages.push(-1);
      pages.push(total);
    }

    return pages;
  }
}
