import { Component, ElementRef, ViewChild, Input, OnInit, AfterViewInit, OnDestroy, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbActiveModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { PdfExportService } from '@app/shared/services/pdf-export/pdf-export.service';

@Component({
  selector: 'app-pdf-preview',
  standalone: true,
  imports: [CommonModule, NgbModule],
  template: `
    <div class="modal-header pdf-preview-header">
      <h4 class="modal-title">PDF Preview: {{ filename }}</h4>
      <div class="preview-actions">
        <button type="button" class="btn btn-sm btn-outline-primary me-2" (click)="downloadPdf()">
          <i class="bi bi-download"></i> Download
        </button>
        <button type="button" class="btn-close" aria-label="Close" (click)="activeModal.dismiss()"></button>
      </div>
    </div>
    <div class="modal-body pdf-preview-body">
      <div class="pdf-preview-container">
        <div class="pdf-viewer">
          <iframe #pdfFrame class="pdf-frame" [style.height.px]="iframeHeight"></iframe>
        </div>
      </div>
    </div>
    <div class="modal-footer pdf-preview-footer">
      <div class="d-flex justify-content-between w-100">
        <div>
          <span class="text-warning">
            <i class="bi bi-info-circle"></i>
            Please review the PDF carefully for any page break issues.
          </span>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      :host {
        display: flex;
        flex-direction: column;
        height: 100%;
        max-height: 100vh;
        overflow: hidden;
      }

      .pdf-preview-header {
        flex: 0 0 auto;
        padding: 0.75rem 1rem;
        border-bottom: 1px solid #dee2e6;
      }

      .preview-actions {
        display: flex;
        align-items: center;
      }

      .pdf-preview-body {
        flex: 1 1 auto;
        padding: 0;
        overflow: hidden;
        display: flex;
        flex-direction: column;
      }

      .pdf-preview-container {
        flex: 1;
        display: flex;
        flex-direction: column;
        height: 100%;
      }

      .pdf-viewer {
        flex: 1;
        height: 100%;
        position: relative;
      }

      .pdf-frame {
        width: 100%;
        height: 100%;
        border: none;
        background-color: #f8f9fa;
      }

      .pdf-preview-footer {
        flex: 0 0 auto;
        padding: 0.5rem 1rem;
        border-top: 1px solid #dee2e6;
        font-size: 0.875rem;
      }
    `,
  ],
})
export class PdfPreviewComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('pdfFrame') pdfFrame: ElementRef;
  @Input() pdfBlob: Blob;
  @Input() filename: string = 'document.pdf';

  private subscription: Subscription;
  private objectUrl: string;
  private pendingBlob: Blob | null = null;
  public iframeHeight: number;

  constructor(
    public activeModal: NgbActiveModal,
    private pdfExportService: PdfExportService,
  ) {}

  @HostListener('window:resize')
  onResize() {
    this.calculateIframeHeight();
  }

  ngOnInit(): void {
    this.calculateIframeHeight();

    if (!this.pdfBlob) {
      this.subscription = this.pdfExportService.pdfPreview$.subscribe(blob => {
        this.pdfBlob = blob;
        if (this.pdfFrame) {
          this.loadPdfFromBlob(blob);
        } else {
          this.pendingBlob = blob;
        }
      });
    }
  }

  ngAfterViewInit(): void {
    this.calculateIframeHeight();
    if (this.pdfBlob) {
      this.loadPdfFromBlob(this.pdfBlob);
    } else if (this.pendingBlob) {
      this.loadPdfFromBlob(this.pendingBlob);
      this.pendingBlob = null;
    }
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
    if (this.objectUrl) {
      URL.revokeObjectURL(this.objectUrl);
    }
  }

  calculateIframeHeight(): void {
    const headerFooterHeight = 120;
    this.iframeHeight = window.innerHeight - headerFooterHeight;
  }

  loadPdfFromBlob(blob: Blob): void {
    if (!this.pdfFrame) {
      console.warn('PDF frame reference not yet available');
      this.pendingBlob = blob;
      return;
    }
    this.objectUrl = URL.createObjectURL(blob);
    this.pdfFrame.nativeElement.src = this.objectUrl;
  }

  downloadPdf(): void {
    if (this.pdfBlob) {
      const link = document.createElement('a');
      link.href = URL.createObjectURL(this.pdfBlob);
      link.download = this.filename;
      link.click();
    } else {
      this.pdfExportService.exportPreviouslyPreviewedPDF(this.filename);
    }
  }
}
