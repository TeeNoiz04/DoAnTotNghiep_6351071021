import { inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ListService, PagedResultDto, TrackByService } from '@abp/ng.core';

import { finalize, map, tap } from 'rxjs/operators';

import { FormMode } from '@app/shared/enums/form-mode';
import { GetPriceFromOptions as GetAppliedPriceFromOptions } from './get-price-from';
import { CustomerService } from '@proxy/customers';
import {
  GetSystemCategoriesInput,
  SystemCategoryDto,
  SystemCategoryService,
} from '@proxy/system-categories';
import { SystemCategories } from '@proxy';
import { CategoryTypes } from '@app/system-categories/system-category.model';
import { Observable } from 'rxjs';
import { BuyerDto, BuyerService } from '@proxy/buyers';
import { FormGroupHelper } from '@app/shared/helpers/form-group-helper';
import { TaxCodeValidator } from '@app/shared/validators/tax-code.validator';

export abstract class AbstractBuyerDetailViewService {
  protected readonly fb = inject(FormBuilder);
  protected readonly track = inject(TrackByService);

  public readonly proxyService = inject(BuyerService);
  protected readonly proxySystemCategoryService = inject(SystemCategoryService);
  public readonly list = inject(ListService);

  isBusy = false;
  isVisible = false;
  selected = {} as any;
  form: FormGroup | undefined;

  public getAppliedPriceFromOptions = GetAppliedPriceFromOptions;
  protected createRequest() {
    const formValues = {
      ...this.form.getRawValue(),
    };

    if (this.selected) {
      return this.proxyService.update(this.selected.id, {
        ...formValues,
        creditLimit:
          formValues.creditLimit != null
            ? typeof formValues.creditLimit === 'string'
              ? parseFloat(formValues.creditLimit.replace(/,/g, ''))
              : formValues.creditLimit
            : null,
        creditExposure:
          formValues.creditExposure != null
            ? typeof formValues.creditExposure === 'string'
              ? parseFloat(formValues.creditExposure.replace(/,/g, ''))
              : formValues.creditExposure
            : null,
        concurrencyStamp: this.selected.concurrencyStamp,
      });
    }
    return this.proxyService.create({
      ...formValues,
      creditLimit: parseFloat(formValues.creditLimit?.replace(/,/g, '')),
      creditExposure: parseFloat(formValues.creditExposure?.replace(/,/g, '')),
    });
  }

  buildForm(mode: FormMode = FormMode.New) {
    const {
      buyerTypeId,
      buyerTypeCode,
      buyerCode,
      shortName,
      fullName,
      taxCode,
      address,
      contactPerson,
      contactEmail,
      contactPhoneNumber,
      paymentTermCode,
      paymentTermDescription,
      creditLimit,
      creditExposure,
      appliedPrice,
      deactive,
      note,
    } = this.selected || {};

    this.form = this.fb.group({
      buyerTypeId: [buyerTypeId ?? null, Validators.required],
      buyerTypeCode: [buyerTypeCode ?? null, Validators.required],
      buyerCode: [buyerCode ?? null, Validators.required],
      appliedPrice: [appliedPrice ?? null, Validators.required],
      shortName: [shortName ?? null],
      fullName: [fullName ?? null],
      taxCode: [taxCode ?? null, [Validators.required, TaxCodeValidator()]],
      address: [address ?? null],
      contactPerson: [contactPerson ?? null],
      contactEmail: [contactEmail ?? null],
      contactPhoneNumber: [contactPhoneNumber ?? null],
      paymentTermCode: [paymentTermCode ?? null],
      paymentTermDescription: [paymentTermDescription ?? null],
      creditLimit: [creditLimit ?? null],
      creditExposure: [creditExposure ?? null], //parseFloat(formValues.value?.replace(/,/g, '')),
      deactive: [deactive ?? false],
      note: [note ?? null],
    });

    const buyerCodeCtrl = this.form.get('buyerCode');
    if (mode === FormMode.Edit) {
      FormGroupHelper.toggleFormControls(buyerCodeCtrl!, true); // disable khi Edit
    } else {
      FormGroupHelper.toggleFormControls(buyerCodeCtrl!, false); // enable khi New
    }
  }

  showForm(mode: FormMode = FormMode.New) {
    this.buildForm(mode);
    this.isVisible = true;
  }

  create() {
    this.selected = undefined;
    this.showForm();
  }

  update(record: BuyerDto) {
    this.selected = record;
    this.showForm(FormMode.Edit);
  }

  hideForm() {
    this.isVisible = false;
  }

  submitForm() {
    if (this.form.invalid) return;

    this.isBusy = true;
    const request = this.createRequest().pipe(
      finalize(() => (this.isBusy = false)),
      tap(() => this.hideForm()),
    );
    request.subscribe(this.list.get);
  }

  changeVisible($event: boolean) {
    this.isVisible = $event;
  }
}
