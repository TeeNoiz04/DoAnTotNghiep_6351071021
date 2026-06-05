import { Injectable } from '@angular/core';
import { PdfPreviewComponent } from '@app/shared/components/pdf-preview/pdf-preview.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PdfPreviewModalService {
  constructor(private modalService: NgbModal) {}

  private modalClosedSubject = new Subject<void>();
  public modalClosed$ = this.modalClosedSubject.asObservable();

  closeModal() {
    this.modalClosedSubject.next();
  }

  openPdfPreview(pdfBlob?: Blob, filename?: string) {
    const modalRef = this.modalService.open(PdfPreviewComponent, {
      size: 'xl',
      centered: true,
      backdrop: 'static',
      // windowClass: 'pdf-preview-modal-fullscreen', => full-screen
      // fullscreen: true, => full-screen
      keyboard: true,
    });

    if (pdfBlob) {
      modalRef.componentInstance.pdfBlob = pdfBlob;
    }

    if (filename) {
      modalRef.componentInstance.filename = filename;
    }

    return modalRef.result;
  }
}
