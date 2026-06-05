import { CoreModule } from '@abp/ng.core';
import { Component, inject, Input } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { SaleOrdersManagementViewService } from '@app/sale-orders/services/sale-orders-management.service';
import { AutoResizeDirective } from '@app/shared/directives/autoResize.directive';
import { AutoAddMissingItemDirective } from '@app/shared/directives/ng-select-auto-add-missing-item.directive';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { LookupDto } from '@proxy/shared';

@Component({
  selector: 'app-sale-orders-form',
  templateUrl: './sale-orders-form.component.html',
  standalone: true,
  styleUrls: ['./sale-orders-form.component.scss'],
  imports: [
    FormsModule,
    NgbModule,
    NgSelectModule,
    MatCheckboxModule,
    ReactiveFormsModule,
    CoreModule,
    AutoResizeDirective,
    AutoAddMissingItemDirective,
  ],
})
export class SaleOrdersFormComponent {
  public readonly service = inject(SaleOrdersManagementViewService);
  @Input() isNewRequestPage = false;
  statusOptions = [
    {
      value: 'IN_PROGRESS',
      label: 'In Progress',
    },
    {
      value: 'CLOSED',
      label: 'Closed',
    },
  ];

  vatOptions = [
    { label: '10%', value: 0.1 },
    { label: '8%', value: 0.08 },
    { label: '5%', value: 0.05 },
    { label: '0%', value: 0 },
    { label: 'KCT', value: -1 },
  ];

  get f() {
    return this.service.saleOrderForm.controls;
  }

  onBuyerTypeChange(event: LookupDto<string>): void {
    if (event) {
      this.service.saleOrderForm?.patchValue({
        buyer: '',
        buyerTypeId: event.id,
      });
      this.service.getBuyerLookupByBuyerType(event.id);
    }
  }

  onBuyerChange(event: LookupDto<string>): void {}

  onMissingBuyerItem(item: any) {
    this.service.buyerTypeOptions = [item, ...this.service.buyerTypeOptions];
  }
}
