import { ListService, TrackByService } from '@abp/ng.core';
import { inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { finalize, tap } from 'rxjs/operators';

import type {
  SupplierBUCreateDto,
  SupplierBUDto,
  SupplierBUUpdateDto,
} from '../../../proxy/supplier-bus/models';
import { SupplierBUService } from '../../../proxy/supplier-bus/supplier-bu.service';

export abstract class AbstractFactoryDetailViewService {
  protected readonly fb = inject(FormBuilder);
  protected readonly track = inject(TrackByService);
  public readonly proxyService = inject(SupplierBUService);
  public readonly list = inject(ListService);

  public isBusy = false;
  public isVisible = false;
  selected = {} as SupplierBUDto;
  form: FormGroup | undefined;

  buildForm() {
    const {
      supplierBUCode,
      supplierBURemarks,
      orderMethod,
      poTemplate,
      contact,
      email,
      incoTerm,
      paymentTermCode,
      paymentDescription,
      currency,
      materialType,
      supplierId,
      supplierCode,
      supplierShortName,
      supplierAddress,
      sortOrder,
      fascmVendorCode,
      fascmBuyerCode,
      fascmConsigneeCode,
      fascmSectionCode,
      fascmPaymentTerm,
      fascmFreightMethod,
      fascmDeliveryTerms,
      fascmPlaceOfDeliveryTerms,
      fascmShippingMarkCode,
    } = this.selected || {};

    this.form = this.fb.group({
      supplierBUCode: [supplierBUCode ?? null, [Validators.required]],
      supplierBURemarks: [supplierBURemarks ?? null],
      orderMethod: [orderMethod ?? null],
      poTemplate: [poTemplate ?? null],
      contact: [contact ?? null],
      email: [email ?? null, [Validators.email]],
      incoTerm: [incoTerm ?? null],
      paymentTermCode: [paymentTermCode ?? null],
      paymentDescription: [paymentDescription ?? null],
      currency: [currency ?? null],
      materialType: [materialType ?? null],
      supplierId: [supplierId ?? null],
      supplierCode: [supplierCode ?? null],
      supplierShortName: [supplierShortName ?? null],
      supplierAddress: [supplierAddress ?? null],
      sortOrder: [sortOrder ?? 0],
      fascmVendorCode: [fascmVendorCode ?? null],
      fascmBuyerCode: [fascmBuyerCode ?? null],
      fascmConsigneeCode: [fascmConsigneeCode ?? null],
      fascmSectionCode: [fascmSectionCode ?? null],
      fascmPaymentTerm: [fascmPaymentTerm ?? null],
      fascmFreightMethod: [fascmFreightMethod ?? null],
      fascmDeliveryTerms: [fascmDeliveryTerms ?? null],
      fascmPlaceOfDeliveryTerms: [fascmPlaceOfDeliveryTerms ?? null],
      fascmShippingMarkCode: [fascmShippingMarkCode ?? null],
    });
  }

  protected createRequest() {
    const formValues = {
      ...this.form.getRawValue(),
    };

    if (this.selected?.id) {
      const updateParam: SupplierBUUpdateDto = {
        ...formValues,
        concurrencyStamp: this.selected.concurrencyStamp,
      };

      return this.proxyService.update(this.selected.id, updateParam);
    }

    const createParam: SupplierBUCreateDto = {
      ...formValues,
    };

    return this.proxyService.create(createParam);
  }

  showForm() {
    this.buildForm();
    this.isVisible = true;
  }

  create() {
    this.selected = undefined as any;
    this.showForm();
  }

  update(record: SupplierBUDto) {
    this.selected = record;

    this.showForm();
  }

  submitForm() {
    if (!this.form || this.form.invalid) return;

    this.isBusy = true;

    const request = this.createRequest().pipe(
      finalize(() => (this.isBusy = false)),
      tap(() => (this.isVisible = false)),
    );

    request.subscribe(this.list.get);
  }

  changeVisible($event: boolean) {
    this.isVisible = $event;
  }
}
