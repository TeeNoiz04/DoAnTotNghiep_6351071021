import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { SaleOrdersManagementViewService } from '@app/sale-orders/services/sale-orders-management.service';
import { InputNumberComponent } from '@app/shared/components/input-number/input-number.component';
import { EscCloseModalDirective } from '@app/shared/esc-close-modal/esc-close-modal.directive';
import { NumberHelper } from '@app/shared/helpers/number-helper';
import { StatusLabelComponent } from '@app/shared/status/components/status-label.component';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { SaleOrderDetailDto, SaleOrderDetailUpdateDto } from '@proxy/sale-orders/sale-order-details';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-edit-sale-order-item-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModalModule,
    CoreModule,
    ThemeSharedModule,
    StatusLabelComponent,
    InputNumberComponent,
    EscCloseModalDirective,
  ],
  templateUrl: './edit-sale-order-item-dialog.component.html',
  styleUrls: ['./edit-sale-order-item-dialog.component.scss'],
})
export class EditSaleOrderItemDialogComponent implements OnInit {
  @Input() isVisible = false;
  @Input() saleOrderDetail: SaleOrderDetailDto;

  @Output() visibleChange = new EventEmitter<boolean>();
  @Output() itemUpdated = new EventEmitter<SaleOrderDetailDto>();

  public readonly service = inject(SaleOrdersManagementViewService);
  private readonly toasterService = inject(ToasterService);
  private readonly fb = inject(FormBuilder);

  isBusy = false;
  form: FormGroup;

  ngOnInit(): void {
    this.buildForm();
    this.patchForm();
  }

  buildForm(): void {
    this.form = this.fb.group({
      price: [{ value: null, disabled: true }, [Validators.min(0)]],
      qty: [{ value: '', disabled: true }, []],
      amount: [{ value: '', disabled: true }, []],
      extrafee: [{ value: null, disabled: true }, []],
      note: ['', Validators.maxLength(500)],
    });
  }

  patchForm(): void {
    if (!this.saleOrderDetail) return;

    let price = this.saleOrderDetail.price || 0;
    if (typeof price === 'string') {
      price = parseFloat((price as string).replace(/,/g, ''));
    }

    this.form?.patchValue({
      price: price,
      qty: this.saleOrderDetail?.qty || '0',
      extrafee: this.saleOrderDetail?.extrafee || '0',
      amount: this.saleOrderDetail?.amount || '0',
      note: this.saleOrderDetail?.note || '',
    });
  }

  closeDialog(): void {
    this.isVisible = false;
    this.visibleChange.emit(false);
    this.form.reset();
  }

  onSave(): void {
    if (this.form.invalid) {
      Object.keys(this.form.controls).forEach(key => {
        const control = this.form.get(key);
        control.markAsTouched();
      });
      this.toasterService.warn('Please correct the form errors before saving', 'Validation Error');
      return;
    }

    this.isBusy = true;

    const formValue = this.form.value;
    const price = typeof formValue.price === 'string' ? parseFloat(formValue.price.replace(/,/g, '')) : formValue.price;
    const updateDto: SaleOrderDetailUpdateDto = {
      saleOrderId: this.saleOrderDetail.saleOrderId,
      dpoDetailId: this.saleOrderDetail.id,
      price: this.saleOrderDetail.price,
      extrafee: this.saleOrderDetail.extrafee,
      note: formValue.note,
      concurrencyStamp: this.saleOrderDetail.concurrencyStamp,
    };

    this.service.proxyService
      .updateNote(this.saleOrderDetail.id, updateDto)
      .pipe(finalize(() => (this.isBusy = false)))
      .subscribe({
        next: result => {
          this.toasterService.success('Sale order item updated successfully', 'Success');
          this.itemUpdated.emit(result);
          this.closeDialog();
        },
        error: error => {
          console.error('Error updating sale order item:', error);
          this.toasterService.error('Failed to update sale order item', 'Error');
        },
      });
  }

  onPriceChange(val: any) {
    const numericPrice = NumberHelper.convertToNumber(val?.target?.value);

    this.form.get('amount')?.setValue(numericPrice * (this.saleOrderDetail?.qty || 0));
  }
}
