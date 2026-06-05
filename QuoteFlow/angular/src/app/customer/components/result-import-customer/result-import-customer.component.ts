import { CoreModule } from '@abp/ng.core';
import { Component, inject, Input } from '@angular/core';
import { FormBuilder, FormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { CustomerImportDto } from '@proxy/customers';
import { ExcelValidationResult } from '@proxy/shared/excels';
import { CustomerItemsComponent } from './customer-items/customer-items.component';

@Component({
  selector: 'app-result-import-customer',
  templateUrl: './result-import-customer.component.html',
  styleUrls: ['./result-import-customer.component.scss'],
  standalone: true,
  providers: [],
  imports: [FormsModule, NgbModule, NgSelectModule, MatCheckboxModule, CoreModule, CustomerItemsComponent],
})
export class ResultImportCustomerComponent {
  @Input() resultImport: ExcelValidationResult<CustomerImportDto> | undefined;
  @Input() height: string = '350px';

  private readonly fb = inject(FormBuilder);

  isCardCollapsed: { [key: string]: boolean } = {
    customerItems: false,
  };
  isLoading = false;

  toggleCardCollapse(cardId: string): void {
    this.isCardCollapsed[cardId] = !this.isCardCollapsed[cardId];
  }
}
