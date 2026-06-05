import { inject } from '@angular/core';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import { filter, switchMap } from 'rxjs/operators';
import { BuyerDto, BuyerService } from '@proxy/buyers';

export abstract class AbstractBuyerViewService {
  protected readonly proxyService = inject(BuyerService);
  protected readonly confirmationService = inject(ConfirmationService);

  delete(record: BuyerDto) {
    this.confirmationService
      .warn('::DeleteConfirmationMessage', '::AreYouSure', { messageLocalizationParams: [] })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => this.proxyService.delete(record.id)),
      )
      .subscribe();
  }
}
