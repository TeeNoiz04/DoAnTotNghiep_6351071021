import { ListService, TrackByService } from '@abp/ng.core';
import { inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { finalize, tap } from 'rxjs/operators';

import { FormMode } from '@app/shared/enums/form-mode';
import {
  StockCategoryCreateDto,
  StockCategoryDto,
  StockCategoryUpdateDto,
} from '@proxy/stock-categories';
import {
  SystemConfigurationCreateDto,
  SystemConfigurationDto,
  SystemConfigurationService,
  SystemConfigurationUpdateDto,
} from '@proxy/system-configurations';
import { NumberHelper } from '@app/shared/helpers/number-helper';

export abstract class AbstractSystemConfigurationDetailViewService {
  protected readonly fb = inject(FormBuilder);
  protected readonly track = inject(TrackByService);

  public readonly proxyService = inject(SystemConfigurationService);
  public readonly list = inject(ListService);

  isBusy = false;
  isVisible = false;
  selected = {} as SystemConfigurationDto;
  form: FormGroup | undefined;

  protected createRequest() {
    const formValues = {
      ...this.form.getRawValue(),
    };

    const item = {
      cfgKey: this.selected?.cfgKey,
      cfgValue: formValues.cfgValue,
      // description: formValues.description,
      isSystemCfg: this.selected?.isSystemCfg,
    };

    if (this.selected) {
      const updateParam: SystemConfigurationUpdateDto = {
        ...item,
      };

      return this.proxyService.update(this.selected.id, updateParam);
    }

    const createParam: SystemConfigurationCreateDto = {
      ...item,
    };
    return this.proxyService.create(createParam);
  }

  buildForm(mode: FormMode = FormMode.New) {
    const { cfgValue, isSystemCfg } = this.selected || {};

    this.form = this.fb.group({
      cfgValue: [cfgValue ?? null, [Validators.required]],
      isSystemCfg: [isSystemCfg ?? false, []],
    });
  }

  showForm(mode: FormMode = FormMode.New) {
    this.buildForm(mode);
    this.isVisible = true;
  }

  create() {
    this.selected = undefined;
    this.showForm();
  }

  update(record: SystemConfigurationDto) {
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
