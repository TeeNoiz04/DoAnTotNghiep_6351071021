import { CommonModule } from '@angular/common';
import { Component, ElementRef, Input, OnChanges, SimpleChanges, TemplateRef, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgbModule, NgbPopover } from '@ng-bootstrap/ng-bootstrap';

interface ProcessedError {
  where?: string;
  message: string;
  isFullWidth: boolean;
  rowNumber?: number; // Thêm field để lưu số row
  originalIndex: number; // Lưu index gốc
}

@Component({
  selector: 'app-error-display',
  standalone: true,
  imports: [CommonModule, NgbModule, FormsModule],
  templateUrl: './error-display.component.html',
  styleUrls: ['./error-display.component.scss'],
})
export class ErrorDisplayComponent implements OnChanges {
  @Input() fileName?: string;
  @Input() errors: string[] = [];
  @Input() customMessage?: string;
  @Input() showIcon: boolean = true;
  @Input() showFileName: boolean = true;
  @Input() layout: 'horizontal' | 'vertical' = 'horizontal';
  @Input() popoverPlacement: 'auto' | 'top' | 'bottom' | 'left' | 'right' = 'auto';
  @Input() popoverClass: string = 'custom-error-popover';
  @Input() showViewAllThreshold: number = 20;
  @Input() showExport: boolean = true;
  @Input() popoverMaxErrors: number = 200;
  @Input() modalPageSize: number = 100;

  @ViewChild('errorPopoverTemplate', { static: true }) errorPopoverTemplate!: TemplateRef<object>;
  @ViewChild('popoverContent') popoverContent!: ElementRef;
  @ViewChild('searchInput') searchInput!: ElementRef<HTMLInputElement>;

  processedErrors: ProcessedError[] = [];
  popoverErrors: ProcessedError[] = [];
  filteredErrors: ProcessedError[] = [];
  paginatedErrors: ProcessedError[] = [];
  searchTerm: string = '';
  isModalOpen: boolean = false;

  // Pagination
  currentPage: number = 1;
  totalPages: number = 1;

  // Scroll indicators for popover
  hasScrollTop: boolean = false;
  hasScrollBottom: boolean = false;

  // Reference to active popover
  private activePopover?: NgbPopover;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['errors']) {
      this.processErrors();
      this.filterErrors();
    }
  }

  get errorCount(): number {
    return this.errors?.length || 0;
  }

  get displayMessage(): string {
    return this.customMessage || `Errors: ${this.errorCount}`;
  }

  get hasErrors(): boolean {
    return this.errorCount > 0;
  }

  get shouldShowViewAllButton(): boolean {
    return this.errorCount > this.showViewAllThreshold;
  }

  private processErrors(): void {
    if (!this.errors || this.errors.length === 0) {
      this.processedErrors = [];
      this.popoverErrors = [];
      return;
    }

    // Process và extract row numbers
    const errors = this.errors.map((error, index) => {
      // Extract row number từ đầu error message
      const rowMatch = error.match(/^Row (\d+):/);
      const rowNumber = rowMatch ? parseInt(rowMatch[1], 10) : undefined;

      const cleanError = error.replace(/^Row \d+:\s*/, '');
      const whereMatch = cleanError.match(/^\[([^\]]+)\]\s*(.+)$/);

      if (whereMatch) {
        return {
          where: whereMatch[1],
          message: whereMatch[2],
          isFullWidth: false,
          rowNumber,
          originalIndex: index,
        };
      } else {
        return {
          message: error,
          isFullWidth: true,
          rowNumber,
          originalIndex: index,
        };
      }
    });

    // Sắp xếp theo row number
    this.processedErrors = errors.sort((a, b) => {
      // Nếu cả hai đều có row number, sắp xếp theo row number
      if (a.rowNumber !== undefined && b.rowNumber !== undefined) {
        return a.rowNumber - b.rowNumber;
      }
      // Nếu chỉ a có row number, đưa a lên trước
      if (a.rowNumber !== undefined) {
        return -1;
      }
      // Nếu chỉ b có row number, đưa b lên trước
      if (b.rowNumber !== undefined) {
        return 1;
      }
      // Nếu cả hai đều không có row number, giữ nguyên thứ tự ban đầu
      return a.originalIndex - b.originalIndex;
    });

    // Limit popover to first N errors for performance
    this.popoverErrors = this.processedErrors.slice(0, this.popoverMaxErrors);
  }

  togglePopover(popover: NgbPopover): void {
    if (popover.isOpen()) {
      popover.close();
      this.activePopover = undefined;
    } else {
      popover.open();
      this.activePopover = popover;
    }
  }

  // Popover scroll handling for visual indicators
  onPopoverScroll(event: Event): void {
    const element = event.target as HTMLElement;
    this.updateScrollIndicators(element);
  }

  private updateScrollIndicators(element: HTMLElement): void {
    if (!element) return;

    const scrollTop = element.scrollTop;
    const scrollHeight = element.scrollHeight;
    const clientHeight = element.clientHeight;

    this.hasScrollTop = scrollTop > 10;
    this.hasScrollBottom = scrollTop + clientHeight < scrollHeight - 10;
  }

  // Modal methods
  openErrorModal(): void {
    this.isModalOpen = true;
    this.currentPage = 1;
    this.filterErrors();

    // Focus search input after modal opens
    setTimeout(() => {
      if (this.searchInput) {
        this.searchInput.nativeElement.focus();
      }
    }, 100);

    // Prevent body scroll when modal is open
    document.body.style.overflow = 'hidden';
  }

  openErrorModalAndClosePopover(): void {
    // Close the popover first
    if (this.activePopover && this.activePopover.isOpen()) {
      this.activePopover.close();
      this.activePopover = undefined;
    }

    // Then open the modal
    this.openErrorModal();
  }

  closeErrorModal(): void {
    this.isModalOpen = false;
    this.searchTerm = '';
    this.currentPage = 1;
    this.filterErrors();

    // Restore body scroll
    document.body.style.overflow = '';
  }

  // Search and filter methods
  filterErrors(): void {
    if (!this.searchTerm || this.searchTerm.trim() === '') {
      this.filteredErrors = [...this.processedErrors];
    } else {
      const searchLower = this.searchTerm.toLowerCase().trim();
      this.filteredErrors = this.processedErrors.filter(error => {
        const whereMatch = error.where?.toLowerCase().includes(searchLower);
        const messageMatch = error.message?.toLowerCase().includes(searchLower);
        return whereMatch || messageMatch;
      });
    }

    // Reset to first page when filtering
    this.currentPage = 1;
    this.updatePagination();
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.filterErrors();
    if (this.searchInput) {
      this.searchInput.nativeElement.focus();
    }
  }

  // Pagination methods
  updatePagination(): void {
    this.totalPages = Math.ceil(this.filteredErrors.length / this.modalPageSize);
    const startIndex = (this.currentPage - 1) * this.modalPageSize;
    const endIndex = startIndex + this.modalPageSize;
    this.paginatedErrors = this.filteredErrors.slice(startIndex, endIndex);
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  nextPage(): void {
    this.goToPage(this.currentPage + 1);
  }

  previousPage(): void {
    this.goToPage(this.currentPage - 1);
  }

  get startIndex(): number {
    return (this.currentPage - 1) * this.modalPageSize + 1;
  }

  get endIndex(): number {
    return Math.min(this.currentPage * this.modalPageSize, this.filteredErrors.length);
  }

  get pageNumbers(): number[] {
    const pages: number[] = [];
    const maxVisible = 5;
    let start = Math.max(1, this.currentPage - Math.floor(maxVisible / 2));
    let end = Math.min(this.totalPages, start + maxVisible - 1);

    if (end - start + 1 < maxVisible) {
      start = Math.max(1, end - maxVisible + 1);
    }

    for (let i = start; i <= end; i++) {
      pages.push(i);
    }
    return pages;
  }

  // Export methods
  exportErrorsAsCSV(): void {
    if (this.errorCount === 0) return;

    const csvContent = this.generateCSV();
    const fileName = this.fileName ? `${this.fileName}_errors.csv` : 'errors.csv';
    this.downloadFile(csvContent, fileName, 'text/csv');
  }

  exportErrorsAsTXT(): void {
    if (this.errorCount === 0) return;

    const txtContent = this.generateTXT();
    const fileName = this.fileName ? `${this.fileName}_errors.txt` : 'errors.txt';
    this.downloadFile(txtContent, fileName, 'text/plain');
  }

  private generateCSV(): string {
    // CSV header
    let csv = '"#","Location","Error Message"\n';

    // CSV rows
    this.processedErrors.forEach((error, index) => {
      const rowNum = index + 1;
      const location = error.where ? `"${this.escapeCsvValue(error.where)}"` : '""';
      const message = `"${this.escapeCsvValue(error.message)}"`;
      csv += `${rowNum},${location},${message}\n`;
    });

    return csv;
  }

  private generateTXT(): string {
    let txt = '';

    if (this.fileName) {
      txt += `Error Report: ${this.fileName}\n`;
      txt += '='.repeat(60) + '\n\n';
    }

    txt += `Total Errors: ${this.errorCount}\n`;
    txt += `Generated: ${new Date().toLocaleString()}\n\n`;
    txt += '='.repeat(60) + '\n\n';

    this.processedErrors.forEach((error, index) => {
      txt += `Error #${index + 1}\n`;
      if (error.where) {
        txt += `Location: ${error.where}\n`;
      }
      txt += `Message: ${error.message}\n`;
      txt += '-'.repeat(60) + '\n\n';
    });

    return txt;
  }

  private escapeCsvValue(value: string): string {
    // Escape double quotes by doubling them
    return value.replace(/"/g, '""');
  }

  private downloadFile(content: string, fileName: string, mimeType: string): void {
    const blob = new Blob([content], { type: mimeType });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    window.URL.revokeObjectURL(url);
  }
}
