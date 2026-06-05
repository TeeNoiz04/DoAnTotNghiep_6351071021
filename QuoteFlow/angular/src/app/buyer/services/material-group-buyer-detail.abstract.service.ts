import { ListService, TrackByService } from '@abp/ng.core';
import { inject } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormGroup,
  ValidationErrors,
  Validators,
} from '@angular/forms';

import { finalize, tap } from 'rxjs/operators';

import { FormMode } from '@app/shared/enums/form-mode';
import { FormGroupHelper } from '@app/shared/helpers/form-group-helper';
import {
  MaterialGroupBuyerCreatesDto,
  MaterialGroupBuyerDto,
  MaterialGroupBuyerService,
} from '@proxy/material-group-buyers';
import { LookupDto } from '@proxy/shared';
import { BuyerDto } from '@proxy/buyers';

export abstract class AbstractMaterialGroupBuyerDetailViewService {
  public readonly fb = inject(FormBuilder);
  protected readonly track = inject(TrackByService);

  public readonly proxyService = inject(MaterialGroupBuyerService);
  public readonly list = inject(ListService);

  isBusy = false;
  isVisible = false;
  selected = {} as any;
  form: FormGroup | undefined;
  buyerOptions: LookupDto<string>[] = [];
  materialGroupOptions: LookupDto<string>[] = [];

  protected createRequest() {
    const formValues = {
      ...this.form.getRawValue(),
    };

    const buyer = this.buyerOptions.find(b => b.id === formValues.buyerId);

    const materialGroups = formValues.materialGroups
      .map(id => {
        const found = this.materialGroupOptions.find(g => g.id === id);
        return found
          ? {
              materialGroupId: found.id,
              materialGroupCode: found.displayCode,
            }
          : null;
      })
      .filter(x => x !== null);

    const item = {
      buyerId: formValues.buyerId,
      buyerShortName: this.selected ? this.selected.buyerShortName : buyer.displayCode,
      materialGroups: materialGroups,
    };

    if (this.selected) {
      return this.proxyService.update(item);
    }

    const createParam: MaterialGroupBuyerCreatesDto = {
      ...item,
    };
    return this.proxyService.create(createParam);
  }

  atLeastOneCheckboxCheckedValidator(control: AbstractControl): ValidationErrors | null {
    const formArray = control as any;
    return formArray.length > 0 ? null : { required: true };
  }

  buildForm(mode: FormMode = FormMode.New) {
    const { id, shortName } = this.selected || {};

    this.form = this.fb.group({
      buyerId: [id ?? null, [Validators.required]],
      buyerShortName: [shortName ?? null, []],
      materialGroups: this.fb.array([], this.atLeastOneCheckboxCheckedValidator),
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

  update(record: BuyerDto) {
    this.selected = record;

    this.proxyService.getListByBuyer(this.selected?.id).subscribe({
      next: result => {
        this.showForm(FormMode.Edit);
        this.patchValue(result);
      },
      error: error => {
        console.error('Error loading buyers:', error);
      },
    });
  }

  patchValue(materialGroupBuyer: MaterialGroupBuyerDto[]) {
    FormGroupHelper.toggleFormControls(this.form.get('buyerId'), true);
    // extract only IDs for checkbox selection
    const selectedIds = materialGroupBuyer.map(m => m.materialGroupId);
    // clear existing FormArray first
    const formArray = this.form.get('materialGroups') as FormArray;
    formArray.clear();
    // push controls for each selected ID
    selectedIds.forEach(id => formArray.push(this.fb.control(id)));
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
