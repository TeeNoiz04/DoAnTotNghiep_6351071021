import { Injectable } from '@angular/core';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';
import { BehaviorSubject, Observable, Subject } from 'rxjs';

export interface PdfExportOptions {
  filename?: string;
  margin?: number;
  includeTimestamp?: boolean;
  includePageNumbers?: boolean;
  includeRequestNo?: boolean;
  customHeaderText?: string;
  customFooterText?: string;
  optimization?: 'standard' | 'high' | 'performance';
  paperSize?: 'a4' | 'letter' | 'legal';
}

@Injectable({
  providedIn: 'root',
})
export class PdfExportService {
  private isLoading = new BehaviorSubject<boolean>(false);
  public loading$ = this.isLoading.asObservable();

  private pdfPreviewSubject = new Subject<Blob>();
  public pdfPreview$ = this.pdfPreviewSubject.asObservable();

  private lastPreviewedPdfBlob: Blob;

  constructor() {
    this.pdfPreview$.subscribe(blob => {
      this.lastPreviewedPdfBlob = blob;
    });
  }

  previewPDF(content: HTMLElement, options?: PdfExportOptions): Observable<Blob> {
    const defaultOptions: PdfExportOptions = {
      filename: 'document.pdf',
      margin: 15,
      includeTimestamp: true,
      includePageNumbers: true,
      includeRequestNo: true,
      optimization: 'standard',
      paperSize: 'a4',
    };

    const config = { ...defaultOptions, ...options };
    this.showLoadingIndicator('Preparing PDF preview...');

    const pdf = new jsPDF({
      orientation: 'portrait',
      unit: 'mm',
      format: config.paperSize,
    });

    const margin = config.margin;
    const pdfWidth = pdf.internal.pageSize.getWidth();
    const pdfHeight = pdf.internal.pageSize.getHeight();
    const contentWidth = pdfWidth - margin * 2;
    const contentHeight = pdfHeight - margin * 2;

    const formattedDateTime = config.includeTimestamp ? this.formatDateTime(new Date()) : '';
    const wrapper = this.prepareContentForPDF(content, contentWidth);
    this.optimizePageBreaks(wrapper);
    const previewSubject = new Subject<Blob>();

    setTimeout(async () => {
      try {
        const mmToPx = 3.78;
        const pageHeightPx = contentHeight * mmToPx;
        const totalHeight = wrapper.scrollHeight;
        const totalPages = Math.ceil(totalHeight / pageHeightPx);

        for (let pageNum = 0; pageNum < totalPages; pageNum++) {
          if (pageNum > 0) {
            pdf.addPage();
          }

          const yStart = pageNum * pageHeightPx;

          const pageCanvas = await html2canvas(wrapper, {
            scale:
              config.optimization === 'high' ? 3 : config.optimization === 'performance' ? 1 : 2,
            useCORS: true,
            logging: false,
            allowTaint: true,
            backgroundColor: 'white',
            windowWidth: wrapper.scrollWidth,
            height: pageHeightPx,
            y: yStart,
          });

          const imgData = pageCanvas.toDataURL('image/jpeg', 1.0);
          pdf.addImage(imgData, 'JPEG', margin, margin, contentWidth, contentHeight);

          if (config.includeTimestamp || config.includePageNumbers) {
            this.addPageMetadata(pdf, {
              dateTimeText: formattedDateTime,
              currentPage: pageNum + 1,
              totalPages: totalPages,
              requestNo: options['requestNo'],
              pdfWidth,
              pdfHeight,
            });
          }
        }
        const pdfBlob = pdf.output('blob');

        this.pdfPreviewSubject.next(pdfBlob);
        previewSubject.next(pdfBlob);
        previewSubject.complete();
      } catch (error) {
        console.error('Error generating PDF preview:', error);
        previewSubject.error(error);
      } finally {
        this.hideLoadingIndicator();
        document.body.removeChild(wrapper);
      }
    }, 500);

    return previewSubject.asObservable();
  }

  exportToPDF(content: HTMLElement, options?: PdfExportOptions): Promise<void> {
    // Set default options if not provided
    const defaultOptions: PdfExportOptions = {
      filename: 'document.pdf',
      margin: 15,
      includeTimestamp: true,
      includePageNumbers: true,
      includeRequestNo: true,
      optimization: 'standard',
      paperSize: 'a4',
    };

    const config = { ...defaultOptions, ...options };

    // Show loading indicator
    this.showLoadingIndicator('Preparing PDF...');

    // Create PDF document
    const pdf = new jsPDF({
      orientation: 'portrait',
      unit: 'mm',
      format: config.paperSize,
    });

    // Define margins and content area
    const margin = config.margin;
    const pdfWidth = pdf.internal.pageSize.getWidth();
    const pdfHeight = pdf.internal.pageSize.getHeight();
    const contentWidth = pdfWidth - margin * 2;
    const contentHeight = pdfHeight - margin * 2;

    const formattedDateTime = config.includeTimestamp ? this.formatDateTime(new Date()) : '';
    const wrapper = this.prepareContentForPDF(content, contentWidth);

    this.optimizePageBreaks(wrapper);

    return new Promise((resolve, reject) => {
      setTimeout(async () => {
        try {
          const mmToPx = 3.78;
          const pageHeightPx = contentHeight * mmToPx;
          const totalHeight = wrapper.scrollHeight;
          const totalPages = Math.ceil(totalHeight / pageHeightPx);

          // Process each page
          for (let pageNum = 0; pageNum < totalPages; pageNum++) {
            if (pageNum > 0) {
              pdf.addPage();
            }

            const yStart = pageNum * pageHeightPx;

            const pageCanvas = await html2canvas(wrapper, {
              scale:
                config.optimization === 'high' ? 3 : config.optimization === 'performance' ? 1 : 2,
              useCORS: true,
              logging: false,
              allowTaint: true,
              backgroundColor: 'white',
              windowWidth: wrapper.scrollWidth,
              height: pageHeightPx,
              y: yStart,
            });

            const imgData = pageCanvas.toDataURL('image/jpeg', 1.0);
            pdf.addImage(imgData, 'JPEG', margin, margin, contentWidth, contentHeight);

            if (config.includeTimestamp || config.includePageNumbers) {
              // Add page metadata (timestamp, page numbers, etc.)
              this.addPageMetadata(pdf, {
                dateTimeText: formattedDateTime,
                currentPage: pageNum + 1,
                totalPages: totalPages,
                requestNo: options['requestNo'],
                pdfWidth,
                pdfHeight,
              });
            }
          }

          // Save the PDF
          pdf.save(config.filename);
          resolve();
        } catch (error) {
          console.error('Error generating PDF:', error);
          reject(error);
        } finally {
          // Clean up
          this.hideLoadingIndicator();
          document.body.removeChild(wrapper);
        }
      }, 500);
    });
  }

  exportPreviouslyPreviewedPDF(filename: string): void {
    const link = document.createElement('a');
    link.href = window.URL.createObjectURL(this.lastPreviewedPdfBlob);
    link.download = filename;
    link.click();
  }

  private prepareContentForPDF(content: HTMLElement, contentWidth: number): HTMLElement {
    // Create wrapper element
    const wrapper = document.createElement('div');
    wrapper.className = 'pdf-content-wrapper';
    wrapper.style.width = `${contentWidth}mm`;
    wrapper.style.padding = '0';
    wrapper.style.margin = '0';
    wrapper.style.backgroundColor = 'white';
    const clonedContent = content.cloneNode(true) as HTMLElement;

    const styleElement = document.createElement('style');
    styleElement.textContent = `
    .pdf-content-wrapper * {
      -webkit-print-color-adjust: exact !important;
      print-color-adjust: exact !important;
      box-sizing: border-box !important;
    }

    .pdf-content-wrapper table,
    .pdf-content-wrapper .equal-col-table {
      page-break-inside: auto !important;
    }

    .pdf-content-wrapper tr,
    .pdf-content-wrapper .table-row,
    .pdf-content-wrapper .avoid-break {
      page-break-inside: avoid !important;
      break-inside: avoid !important;
    }

    .pdf-content-wrapper td,
    .pdf-content-wrapper th,
    .pdf-content-wrapper .table-cell {
      word-wrap: break-word !important;
      word-break: normal !important;
      overflow-wrap: break-word !important;
      vertical-align: top !important;
      line-height: 1.3 !important;
    }

    /* Add padding to ensure text has room */
    // .pdf-content-wrapper .table-cell {
    //   padding-top: 3px !important;
    //   padding-bottom: 3px !important;
    // }

    .pdf-content-wrapper .long-text {
      min-height: 11px !important;
      white-space: normal !important;
    }

    .pdf-content-wrapper .request-detail-container {
      page-break-inside: avoid;
    }

    .pdf-content-wrapper hr.separator {
      break-before: avoid !important;
      break-after: avoid !important;
    }

    .pdf-content-wrapper h4,
    .pdf-content-wrapper h5,
    .pdf-content-wrapper .big-header,
    .pdf-content-wrapper .header-row,
    .pdf-content-wrapper .table-header {
      page-break-after: avoid !important;
      break-after: avoid !important;
    }

    .pdf-content-wrapper .force-break {
      page-break-before: always !important;
      break-before: page !important;
      height: 1px;
      display: block;
    }

    .pdf-content-wrapper .empty-section {
      display: none !important;
    }
  `;

    clonedContent.appendChild(styleElement);
    wrapper.appendChild(clonedContent);

    wrapper.style.position = 'absolute';
    wrapper.style.left = '-9999px';
    document.body.appendChild(wrapper);

    const tableCells = wrapper.querySelectorAll('.table-cell, td, th');
    tableCells.forEach(cell => {
      if (cell.textContent && cell.textContent.trim().length > 30) {
        cell.classList.add('long-text');
      }
    });

    this.removeEmptySeparators(wrapper);

    return wrapper;
  }

  private optimizePageBreaks(container: HTMLElement): void {
    const mmToPx = 3.78;
    const pageHeightPx = (210 - 30) * mmToPx;

    this.cleanupEmptySections(container);

    this.optimizeTableCells(container);

    const sectionHeaders = container.querySelectorAll('.request-detail-container');
    sectionHeaders.forEach(section => {
      // Skip hidden sections
      if ((section as HTMLElement).style.display === 'none') {
        return;
      }

      const headerElement = section.querySelector('.big-header');
      if (headerElement) {
        headerElement.classList.add('avoid-break');

        const sectionPos = section.getBoundingClientRect().top % pageHeightPx;
        const spaceLeft = pageHeightPx - sectionPos;

        const hasContent = this.sectionHasContent(section);

        if (spaceLeft < 80 && hasContent) {
          const forceBreak = document.createElement('div');
          forceBreak.className = 'force-break';
          section.parentNode?.insertBefore(forceBreak, section);
        }
      }
    });

    const tables = container.querySelectorAll('table, .equal-col-table');
    tables.forEach(table => {
      const tableRect = table.getBoundingClientRect();
      const tableHeight = tableRect.height;
      const tablePos = tableRect.top % pageHeightPx;
      const spaceLeft = pageHeightPx - tablePos;

      if (tableHeight > pageHeightPx * 0.7 && spaceLeft < tableHeight && spaceLeft < 120) {
        const forceBreak = document.createElement('div');
        forceBreak.className = 'force-break';
        table.parentNode?.insertBefore(forceBreak, table);
      }

      if (table.classList.contains('equal-col-table')) {
        const rows = table.querySelectorAll('.table-row');
        rows.forEach(row => {
          row.classList.add('avoid-break');

          const rowRect = row.getBoundingClientRect();
          const rowPosition = rowRect.top % pageHeightPx;
          const rowHeight = rowRect.height;
          const rowSpaceLeft = pageHeightPx - rowPosition;

          if (rowHeight < pageHeightPx * 0.8 && rowHeight > rowSpaceLeft && rowSpaceLeft < 50) {
            const forceBreak = document.createElement('div');
            forceBreak.className = 'force-break';
            row.parentNode?.insertBefore(forceBreak, row);
          }
        });
      } else {
        const rows = table.querySelectorAll('tr');
        rows.forEach(row => {
          row.classList.add('avoid-break');
        });
      }
    });

    this.detectContentSplitsAndFixBreaks(container, pageHeightPx);
  }

  private optimizeTableCells(container: HTMLElement): void {
    const cells = container.querySelectorAll('td, th, .table-cell');

    cells.forEach((cell: HTMLElement) => {
      cell.style.wordBreak = 'break-word';
      cell.style.overflowWrap = 'break-word';

      const textNodes = Array.from(cell.childNodes).filter(
        node =>
          node.nodeType === Node.TEXT_NODE ||
          (node.nodeType === Node.ELEMENT_NODE &&
            !['BR', 'HR', 'IMG'].includes((node as Element).tagName)),
      );

      if (textNodes.length > 0) {
        cell.style.minHeight = '11px';

        if (cell.textContent && cell.textContent.trim().length > 50) {
          cell.style.whiteSpace = 'normal';
          const computedStyle = getComputedStyle(cell);
          if (parseFloat(computedStyle.lineHeight) < 1.2) {
            cell.style.lineHeight = '1.2';
          }
        }
      }
    });

    const rows = container.querySelectorAll('.table-row, tr');
    rows.forEach((row: HTMLElement) => {
      const hasMultilineContent = Array.from(row.querySelectorAll('.table-cell, td')).some(cell => {
        const text = cell.textContent?.trim() || '';
        return text.length > 40 || text.includes('\n');
      });

      if (hasMultilineContent) {
        row.style.marginBottom = '4px';
      }
    });
  }

  private detectContentSplitsAndFixBreaks(container: HTMLElement, pageHeightPx: number): void {
    const contentSections = Array.from(container.querySelectorAll('.request-detail-container'));

    contentSections.forEach((section: HTMLElement) => {
      if (section.clientHeight < 40) return;

      const sectionTop = this.getElementOffset(section).top % pageHeightPx;
      const sectionBottom = sectionTop + section.clientHeight;

      if (
        sectionTop < pageHeightPx &&
        sectionBottom > pageHeightPx &&
        section.clientHeight < pageHeightPx
      ) {
        const spaceUsedOnFirstPage = pageHeightPx - sectionTop;
        const proportionOnFirstPage = spaceUsedOnFirstPage / section.clientHeight;

        if (proportionOnFirstPage < 0.25) {
          const forceBreak = document.createElement('div');
          forceBreak.className = 'force-break';
          section.parentNode?.insertBefore(forceBreak, section);
        } else if (proportionOnFirstPage > 0.75) {
          section.classList.add('avoid-break');
        }
      }
    });
  }

  private addPageMetadata(
    pdf: jsPDF,
    options: {
      dateTimeText: string;
      currentPage: number;
      totalPages: number;
      requestNo?: string;
      pdfWidth: number;
      pdfHeight: number;
    },
  ): void {
    const { dateTimeText, currentPage, totalPages, requestNo, pdfWidth, pdfHeight } = options;

    pdf.setFont('helvetica', 'normal');
    pdf.setFontSize(8);
    pdf.setTextColor(100, 100, 100);

    if (dateTimeText) {
      pdf.text(dateTimeText, pdfWidth - 40, pdfHeight - 10);
    }

    pdf.text(`Page ${currentPage} of ${totalPages}`, pdfWidth / 2, pdfHeight - 10, {
      align: 'center',
    });

    if (requestNo) {
      pdf.text(requestNo, 15, pdfHeight - 10);
    }
  }

  private formatDateTime(date: Date): string {
    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const year = date.getFullYear();
    let hours = date.getHours();
    const ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12;
    const minutes = date.getMinutes().toString().padStart(2, '0');

    return `${day}/${month}/${year} ${hours}:${minutes} ${ampm}`;
  }

  private getElementOffset(element: HTMLElement): { top: number; left: number } {
    const rect = element.getBoundingClientRect();
    const scrollLeft = window.pageXOffset || document.documentElement.scrollLeft;
    const scrollTop = window.pageYOffset || document.documentElement.scrollTop;

    return {
      top: rect.top + scrollTop,
      left: rect.left + scrollLeft,
    };
  }

  private showLoadingIndicator(message: string = 'Loading...'): void {
    this.isLoading.next(true);

    // Create a loading indicator if it doesn't exist
    if (!document.querySelector('.pdf-loading-indicator')) {
      const loadingIndicator = document.createElement('div');
      loadingIndicator.className = 'pdf-loading-indicator';
      loadingIndicator.textContent = message;
      loadingIndicator.style.position = 'fixed';
      loadingIndicator.style.top = '50%';
      loadingIndicator.style.left = '50%';
      loadingIndicator.style.transform = 'translate(-50%, -50%)';
      loadingIndicator.style.padding = '20px';
      loadingIndicator.style.background = 'rgba(0,0,0,0.7)';
      loadingIndicator.style.color = 'white';
      loadingIndicator.style.borderRadius = '5px';
      loadingIndicator.style.zIndex = '9999';
      document.body.appendChild(loadingIndicator);
    }
  }

  private hideLoadingIndicator(): void {
    this.isLoading.next(false);

    const loadingIndicator = document.querySelector('.pdf-loading-indicator');
    if (loadingIndicator) {
      document.body.removeChild(loadingIndicator);
    }
  }

  private cleanupEmptySections(container: HTMLElement): void {
    const containerSections = container.querySelectorAll('.request-detail-container');
    containerSections.forEach((section: HTMLElement) => {
      const hasText = Array.from(section.querySelectorAll('*')).some(
        el => el.textContent && el.textContent.trim().length > 0,
      );

      const hasVisibleElements = section.querySelectorAll('img, table, canvas').length > 0;
      if (!hasText && !hasVisibleElements) {
        section.style.display = 'none';
      }

      const header = section.querySelector('.big-header');
      const contentAfterHeader = header
        ? Array.from(section.children).filter(
            el => !el.classList.contains('big-header') && el.textContent?.trim().length > 0,
          ).length > 0
        : false;

      if (header && !contentAfterHeader && !hasVisibleElements) {
        section.style.display = 'none';
      }
    });

    const allElements = Array.from(container.querySelectorAll('*'));
    for (let i = 0; i < allElements.length - 1; i++) {
      if (
        allElements[i].classList.contains('force-break') &&
        allElements[i + 1] instanceof HTMLElement &&
        (allElements[i + 1] as HTMLElement).style.display === 'none'
      ) {
        (allElements[i] as HTMLElement).style.display = 'none';
      }
    }

    const forceBreaks = Array.from(container.querySelectorAll('.force-break'));
    for (let i = 0; i < forceBreaks.length - 1; i++) {
      if (forceBreaks[i].nextElementSibling === forceBreaks[i + 1]) {
        (forceBreaks[i + 1] as HTMLElement).style.display = 'none';
      }
    }
  }

  private getPreviousVisibleSibling(element: Element): Element | null {
    let sibling = element.previousElementSibling;
    while (sibling) {
      if (getComputedStyle(sibling).display !== 'none') {
        return sibling;
      }
      sibling = sibling.previousElementSibling;
    }
    return null;
  }

  private getNextVisibleSibling(element: Element): Element | null {
    let sibling = element.nextElementSibling;
    while (sibling) {
      if (getComputedStyle(sibling).display !== 'none') {
        return sibling;
      }
      sibling = sibling.nextElementSibling;
    }
    return null;
  }

  private hasVisibleContent(element: Element): boolean {
    if (element.textContent && element.textContent.trim().length > 0) {
      return true;
    }

    const visibleChildren = element.querySelectorAll('img, table, canvas');
    return visibleChildren.length > 0;
  }

  private sectionHasContent(section: Element): boolean {
    if (section.textContent && section.textContent.trim().length > 0) {
      const headerTextContent = Array.from(
        section.querySelectorAll('.big-header, h1, h2, h3, h4, h5, h6'),
      )
        .map(el => el.textContent?.trim() || '')
        .join('');

      if (section.textContent.trim().length > headerTextContent.length) {
        return true;
      }
    }

    const visibleElements = section.querySelectorAll('img, table, canvas, .table-row');
    if (visibleElements.length > 0) {
      return true;
    }

    const childElements = Array.from(section.children);
    for (const child of childElements) {
      if (this.hasVisibleContent(child)) {
        return true;
      }
    }

    return false;
  }

  private removeEmptySeparators(container: HTMLElement): void {
    const separators = container.querySelectorAll('hr.separator');

    separators.forEach(separator => {
      const prevSibling = this.getPreviousVisibleSibling(separator);
      const nextSibling = this.getNextVisibleSibling(separator);
      if (
        (!prevSibling || !this.hasVisibleContent(prevSibling)) &&
        (!nextSibling || !this.hasVisibleContent(nextSibling))
      ) {
        separator.classList.add('empty-section');
      }
    });
  }
}
