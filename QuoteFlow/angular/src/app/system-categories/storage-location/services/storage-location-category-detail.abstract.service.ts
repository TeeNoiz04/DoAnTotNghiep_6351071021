import { ListService, TrackByService } from '@abp/ng.core';
import { inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { finalize, tap } from 'rxjs/operators';

import { FormMode } from '@app/shared/enums/form-mode';
import { FormGroupHelper } from '@app/shared/helpers/form-group-helper';
import {
  StockCategoryCreateDto,
  StockCategoryDto,
  StockCategoryService,
  StockCategoryUpdateDto,
} from '@proxy/stock-categories';

export abstract class AbstractStorageLocationDetailViewService {
  protected readonly fb = inject(FormBuilder);
  protected readonly track = inject(TrackByService);

  public readonly proxyService = inject(StockCategoryService);
  public readonly list = inject(ListService);

  isBusy = false;
  isVisible = false;
  selected = {} as any;
  form: FormGroup | undefined;

  protected createRequest() {
    const formValues = {
      ...this.form.getRawValue(),
    };

    const item = {
      stockCode: formValues.stockCode,
      sapCode: formValues.sapCode,
      stockName: formValues.stockName,
      note: formValues.note,
      isDeactive: formValues.isDeactive,
      mainStock: formValues.mainStock,
      damagedStock: formValues.damagedStock,
      foc: formValues.foc,
      sortOrder: formValues.sortOrder,
    };

    if (this.selected) {
      const updateParam: StockCategoryUpdateDto = {
        ...item,
        concurrencyStamp: this.selected.concurrencyStamp,
      };

      return this.proxyService.update(this.selected.id, updateParam);
    }

    const createParam: StockCategoryCreateDto = {
      ...item,
    };
    return this.proxyService.create(createParam);
  }

  buildForm(mode: FormMode = FormMode.New) {
    const { stockCode, stockName, note, isDeactive, mainStock, damagedStock, foc, sapCode } =
      this.selected || {};

    this.form = this.fb.group({
      stockCode: [stockCode ?? null, [Validators.required, Validators.maxLength(50)]],
      sapCode: [sapCode ?? null, []],
      stockName: [stockName ?? null, [Validators.required, Validators.maxLength(500)]],
      note: [note ?? null, [Validators.maxLength(4000)]],
      isDeactive: [isDeactive ?? false, []],
      mainStock: [mainStock ?? false, []],
      foc: [foc ?? false, []],
      damagedStock: [damagedStock ?? false, []],
    });

    if (mode === FormMode.Edit) {
      FormGroupHelper.toggleFormControls(this.form.get('stockCode'), true);
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

  update(record: StockCategoryDto) {
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
