import { inject } from '@angular/core';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import { filter, switchMap } from 'rxjs/operators';
import { CustomerDto, CustomerService } from '@proxy/customers';
import { ListService } from '@abp/ng.core';

export abstract class AbstractCustomerViewService {
  protected readonly proxyService = inject(CustomerService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly list = inject(ListService);

  delete(record: CustomerDto) {
    this.confirmationService
      .warn('::DeleteConfirmationMessage', '::AreYouSure', { messageLocalizationParams: [] })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => this.proxyService.delete(record.id)),
      )
      .subscribe(this.list.get);
  }
}
