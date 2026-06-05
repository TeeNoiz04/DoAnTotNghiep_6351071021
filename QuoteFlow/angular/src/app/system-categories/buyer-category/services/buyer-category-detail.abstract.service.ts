import { ListService, TrackByService } from '@abp/ng.core';
import { inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { finalize, tap } from 'rxjs/operators';

import { FormMode } from '@app/shared/enums/form-mode';
import { BuyerService } from '../../../proxy/buyers/buyer.service';
import type { BuyerDto } from '../../../proxy/buyers/models';
import { GetPriceFromOptions } from './get-price-from';

export abstract class AbstractBuyerCategoryDetailViewService {
  protected readonly fb = inject(FormBuilder);
  protected readonly track = inject(TrackByService);

  public readonly proxyService = inject(BuyerService);
  public readonly list = inject(ListService);

  isBusy = false;
  isVisible = false;
  selected = {} as any;
  form: FormGroup | undefined;

  public getPriceFromOptions = GetPriceFromOptions;
  protected createRequest() {
    const formValues = {
      ...this.form.getRawValue(),
    };

    if (this.selected) {
      return this.proxyService.update(this.selected.id, {
        ...formValues,
        concurrencyStamp: this.selected.concurrencyStamp,
      });
    }

    return this.proxyService.create(formValues);
  }

  buildForm(mode: FormMode = FormMode.New) {
    const { code, name, address, contactInfo, priceColumn, note, isDeactive } = this.selected || {};

    this.form = this.fb.group({
      code: [code ?? null, [Validators.required, Validators.maxLength(50)]],
      name: [name ?? null, [Validators.required, Validators.maxLength(500)]],
      address: [address ?? null, [Validators.maxLength(500)]],
      contactInfo: [contactInfo ?? null, [Validators.maxLength(4000)]],
      priceColumn: [priceColumn ?? 0, [Validators.min(0), Validators.max(32767)]],
      note: [note ?? null, [Validators.maxLength(4000)]],
      isDeactive: [isDeactive ?? false, []],
    });
    // if (mode === FormMode.Edit) {
    //   FormGroupHelper.toggleFormControls(this.form.get('code'), true);
    // }
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
