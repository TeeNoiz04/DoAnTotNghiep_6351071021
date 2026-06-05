import { inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import {
  SaleOrderCreateDto,
  SaleOrderDto,
  SaleOrderService,
  SaleOrderUpdateDto,
} from '@proxy/sale-orders';
import { AppPermissions } from '@app/app.permissions';
import { PermissionService } from '@abp/ng.core';

export abstract class AbstractSaleOrdersManagementDetailViewService {
  form: FormGroup;

  isBusy = false;
  isModalBusy = false;

  selected: SaleOrderDto;
  showDetails = false;

  protected readonly permissionService = inject(PermissionService);
  protected readonly proxyService = inject(SaleOrderService);
  private readonly fb = inject(FormBuilder);

  buildForm(): void {
    this.form = this.fb.group({
      soNo: [null, [Validators.required]],
      sosapNo: [null],
      materialType: [null],
      buyerId: [null],
      buyerCode: [null],
      buyerName: [null],
      orderDate: [null],
      statusCode: [null],
      stockCategoryId: [null],
      sO_VAT: [null],
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

    const request = this.form.value as SaleOrderCreateDto;
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
    } as SaleOrderUpdateDto;

    this.proxyService
      .update(this.selected.id, request)
      .pipe(finalize(() => (this.isModalBusy = false)))
      .subscribe(() => {
        this.isModalBusy = false;
      });
  }

  openModal(selected?: SaleOrderDto): void {
    this.buildForm();
    this.selected = selected;

    if (selected?.id) {
      this.form.patchValue({
        ...selected,
      });
    }
  }

  get canDelete(): (row: any) => boolean {
    return (row: any) =>
      this.permissionService.getGrantedPolicy(AppPermissions.SaleOrders.Delete) &&
      row?.statusCode !== 'CLOSED';
  }
}
