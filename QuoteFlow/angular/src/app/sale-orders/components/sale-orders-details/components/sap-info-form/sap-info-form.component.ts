import { Component, inject, Input } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from '@abp/ng.core';
import { NgSelectModule } from '@ng-select/ng-select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SaleOrdersManagementViewService } from '@app/sale-orders/services/sale-orders-management.service';
import { AutoResizeDirective } from '@app/shared/directives/autoResize.directive';

@Component({
  selector: 'app-sap-info-form',
  templateUrl: './sap-info-form.component.html',
  standalone: true,
  styleUrls: ['./sap-info-form.component.scss'],
  imports: [
    FormsModule,
    NgbModule,
    NgSelectModule,
    MatCheckboxModule,
    ReactiveFormsModule,
    CoreModule,
    AutoResizeDirective,
  ],
})
export class SAPInformationFormComponent {
  public readonly service = inject(SaleOrdersManagementViewService);
  @Input() isNewRequestPage = false;
  get f() {
    return this.service.saleOrderForm.controls;
  }
}
