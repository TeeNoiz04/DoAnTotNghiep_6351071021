import { inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { MaterialService } from '@proxy/materials/material.service';
import type { MaterialCreateDto, MaterialDto, MaterialUpdateDto } from '@proxy/materials/models';

export abstract class AbstractStockManagementDetailViewService {
  form: FormGroup;

  isBusy = false;
  isModalBusy = false;

  selected?: MaterialDto;
  showDetails = false;

  protected readonly proxyService = inject(MaterialService);
  private readonly fb = inject(FormBuilder);

  buildForm(): void {
    this.form = this.fb.group({
      golfaCode: [null, [Validators.required]],
      model: [null, [Validators.required]],
      sapCode: [null],
      descriptionEN: [null],
      descriptionVN: [null],
      descriptionGroup: [null],
      spec1: [null],
      spec2: [null],
      spec3: [null],
      spec4: [null],
      unit: [null],
      origin: [null],
      kind: [null],
      materialType: [null],
      materialStatus: [null, [Validators.required]],
      productHierarchy: [null],
      materialGroup: [null],
      factory: [null],
      vendor: [null],
      epa: [false, [Validators.required]],
      leadTime: [null],
      maxlot: [null],
      stockValueWarning: [null],
      hsCode: [null],
      incoterms: [null],
      importDuty: [null],
      refExchangeRate: [null],
      inputPrice: [null],
      currency: [null],
      standardPrice: [0, [Validators.required]],
      buyerPrice1: [null],
      buyerPrice2: [null],
      buyerPrice3: [null],
      buyerPrice4: [null],
      buyerPrice5: [null],
      vat: [null],
      landingCost: [null],
      marginAllowSale: [null],
      marginAllowManager: [null],
      note: [null],
      concurrencyStamp: [null],
    });
  }

  closeDetails(): void {
    this.selected = undefined;
    this.showDetails = false;
  }

  create(): void {
    if (this.form.invalid) {
      return;
    }

    this.isModalBusy = true;

    const request = this.form.value as MaterialCreateDto;
    this.proxyService
      .create(request)
      .pipe(finalize(() => (this.isModalBusy = false)))
      .subscribe(() => {
        this.form.reset();
        this.isModalBusy = false;
      });
  }

  update(): void {
    if (this.form.invalid || !this.selected) {
      return;
    }

    this.isModalBusy = true;

    const request = {
      ...this.form.value,
      concurrencyStamp: this.selected.concurrencyStamp,
    } as MaterialUpdateDto;

    this.proxyService
      .update(this.selected.id, request)
      .pipe(finalize(() => (this.isModalBusy = false)))
      .subscribe(() => {
        this.isModalBusy = false;
      });
  }

  openModal(selected?: MaterialDto): void {
    this.buildForm();
    this.selected = selected;

    if (selected?.id) {
      this.form.patchValue({
        ...selected,
      });
    }
  }
}
