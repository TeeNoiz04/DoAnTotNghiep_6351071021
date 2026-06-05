import { Injectable, inject, Type } from '@angular/core';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { Observable, from } from 'rxjs';
import { map } from 'rxjs/operators';
import { DialogComponent } from '../../components/dialog/dialog.component';
import { DialogConfig } from '../../models/dialog-config.model';

@Injectable({
  providedIn: 'root',
})
export class DialogService {
  private readonly modalService = inject(NgbModal);

  open<T, R = any>(component: Type<T>, config?: DialogConfig): Observable<R> {
    const modalOptions: NgbModalOptions = {
      centered: config?.centered ?? true,
      backdrop: config?.backdrop ?? 'static',
      size: config?.size ?? 'lg',
      windowClass: config?.windowClass ?? '',
      keyboard: config?.keyboard ?? true,
    };

    const modalRef = this.modalService.open(DialogComponent, modalOptions);
    const dialogInstance = modalRef.componentInstance as DialogComponent;

    dialogInstance.title = config?.title ?? '';
    dialogInstance.showClose = config?.showClose ?? true;
    dialogInstance.contentComponent = component;
    dialogInstance.contentData = config?.data ?? {};
    dialogInstance.closeBtnLabel = config?.closeBtnLabel;
    dialogInstance.confirmBtnLabel = config?.confirmBtnLabel;
    dialogInstance.showConfirmBtn = !!config?.confirmBtnLabel;
    dialogInstance.customClass = config?.customClass ?? '';
    return from(modalRef.result).pipe(map(result => result as R));
  }

  confirm(title: string, message: string, confirmBtnLabel = 'Confirm'): Observable<boolean> {
    const modalRef = this.modalService.open(DialogComponent, {
      centered: true,
      backdrop: 'static',
      size: 'sm',
    });

    const dialogInstance = modalRef.componentInstance as DialogComponent;
    dialogInstance.title = title;
    dialogInstance.message = message;
    dialogInstance.showClose = true;
    dialogInstance.closeBtnLabel = 'Cancel';
    dialogInstance.confirmBtnLabel = confirmBtnLabel;
    dialogInstance.showConfirmBtn = true;
    dialogInstance.isConfirmation = true;

    return from(modalRef.result).pipe(map(result => result === true));
  }
}
