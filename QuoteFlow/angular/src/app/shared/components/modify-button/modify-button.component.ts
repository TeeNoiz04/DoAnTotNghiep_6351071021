import { PageModule } from '@abp/ng.components/page';
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { Component, inject, Input, OnDestroy } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import { RequestDetailsDto } from '@app/requests/request/my-request-details.model';
import { RequestContextService } from '@app/requests/request/services/request-context.service';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { NgbDropdownModule, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { BusinessTripRequestService } from '@proxy/business-trip-requests';
import { EntertainmentRequestService } from '@proxy/entertainment-requests';
import { PromotionRequestService } from '@proxy/promotion-requests';
import { PurchaseRequestNonExpService } from '@proxy/purchase-request-non-exps';
import { PurchaseRequestService } from '@proxy/purchase-requests';
import { RequestModificationDto, RequestType } from '@proxy/requests';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-modify-button',
  standalone: true,
  imports: [
    CoreModule,
    ThemeSharedModule,
    PageModule,
    CommercialUiModule,
    NgbDropdownModule,
    NgSelectModule,
    ReactiveFormsModule,
  ],
  providers: [RequestContextService],
  templateUrl: './modify-button.component.html',
  styleUrls: ['./modify-button.component.scss'],
})
export class ModifyButtonComponent implements OnDestroy {
  @Input() requestDetails: RequestDetailsDto | undefined;
  @Input() requestType: RequestType | undefined;
  public readonly toast = inject(ToasterService);
  public readonly router = inject(Router);
  public readonly purchaseNonRequestService = inject(PurchaseRequestNonExpService);
  public readonly promotionRequestService = inject(PromotionRequestService);
  public readonly purchaseRequestService = inject(PurchaseRequestService);
  public readonly entertainmentRequestService = inject(EntertainmentRequestService);
  public readonly businessTripRequestService = inject(BusinessTripRequestService);
  private loadingService = inject(LoadingService);

  openModifyModal = false;
  isModifyModalBusy = false;
  modifyReason: string | null;
  options: NgbModalOptions = {
    size: 'xl',
    animation: true,
    centered: true,
    backdrop: 'static',
    keyboard: false,
    modalDialogClass: 'modify-request-modal',
  };

  private subscriptions: Subscription[] = [];

  constructor() {}

  showModifyModal() {
    this.openModifyModal = true;
  }

  onCloseModifyModal() {
    this.openModifyModal = false;
  }

  onSubmitModifyRequest() {
    if (!this.modifyReason) {
      return;
    }
    this.isModifyModalBusy = true;
    this.loadingService.show();
    const payload: RequestModificationDto = {
      id: this.requestDetails.id,
      comment: this.modifyReason,
      concurrencyStamp: this.requestDetails.concurrencyStamp,
    };
    switch (this.requestType) {
      case RequestType.Promotion:
        this.modifyPromotionRequest(payload);
        break;
      case RequestType.BusinessTrip:
        this.modifyBusinessTripRequest(payload);
        break;
      case RequestType.Entertainment:
        this.modifyEntertainmentRequest(payload);
        break;
      case RequestType.Purchase:
        this.modifyPurchaseRequest(payload);
        break;
      case RequestType.NonExpenditurePurchase:
        this.modifyNonPurchaseRequest(payload);
        break;
      default:
        this.toast.error('This request not implement modify');
        this.loadingService.hide();
        this.openModifyModal = false;
        this.isModifyModalBusy = false;
        break;
    }
  }

  private modifyPromotionRequest(payload: RequestModificationDto) {
    this.loadingService.show();
    this.subscriptions.push(
      this.promotionRequestService.modify(this.requestDetails.id, payload).subscribe({
        next: res => {
          this.loadingService.hide();
          this.openModifyModal = false;
          this.navigateModifyRequest(res.id);
          this.isModifyModalBusy = false;
        },
        error: err => {
          this.loadingService.hide();
          this.isModifyModalBusy = false;
          console.error('Error occurred:', err);
        },
      }),
    );
  }

  private modifyNonPurchaseRequest(payload: RequestModificationDto) {
    this.loadingService.show();
    this.subscriptions.push(
      this.purchaseNonRequestService.modify(this.requestDetails.id, payload).subscribe({
        next: res => {
          this.loadingService.hide();
          this.openModifyModal = false;
          this.navigateModifyRequest(res.id);
          this.isModifyModalBusy = false;
        },
        error: err => {
          this.loadingService.hide();
          this.isModifyModalBusy = false;
          console.error('Error occurred:', err);
        },
      }),
    );
  }

  private modifyBusinessTripRequest(payload: RequestModificationDto) {
    this.loadingService.show();
    this.subscriptions.push(
      this.businessTripRequestService.modify(this.requestDetails.id, payload).subscribe({
        next: res => {
          this.loadingService.hide();
          this.openModifyModal = false;
          this.navigateModifyRequest(res.id);
          this.isModifyModalBusy = false;
        },
        error: err => {
          this.loadingService.hide();
          this.isModifyModalBusy = false;
          console.error('Error occurred:', err);
        },
      }),
    );
  }

  private modifyEntertainmentRequest(payload: RequestModificationDto) {
    this.loadingService.show();
    this.subscriptions.push(
      this.entertainmentRequestService.modify(this.requestDetails.id, payload).subscribe({
        next: res => {
          this.loadingService.hide();
          this.openModifyModal = false;
          this.navigateModifyRequest(res.id);
          this.isModifyModalBusy = false;
        },
        error: err => {
          this.loadingService.hide();
          this.isModifyModalBusy = false;
          console.error('Error occurred:', err);
        },
      }),
    );
  }

  private modifyPurchaseRequest(payload: RequestModificationDto) {
    this.loadingService.show();
    this.subscriptions.push(
      this.purchaseRequestService.modify(this.requestDetails.id, payload).subscribe({
        next: res => {
          this.loadingService.hide();
          this.openModifyModal = false;
          this.navigateModifyRequest(res.id);
          this.isModifyModalBusy = false;
        },
        error: err => {
          this.loadingService.hide();
          this.isModifyModalBusy = false;
          console.error('Error occurred:', err);
        },
      }),
    );
  }

  private navigateModifyRequest(id: string) {
    switch (this.requestType) {
      case RequestType.Promotion:
        this.router.navigate([
          AppRoutes.AF_SERVICES.BASE,
          AppRoutes.REQUESTS.BASE,
          AppRoutes.REQUESTS.PROMOTION.BASE,
          AppRoutes.EDIT,
          AppRoutes.DETAILS_WITH_ID(id),
        ]);
        break;
      case RequestType.BusinessTrip:
        this.router.navigate([
          AppRoutes.AF_SERVICES.BASE,
          AppRoutes.REQUESTS.BASE,
          AppRoutes.REQUESTS.BUSINESS_TRIP.BASE,
          AppRoutes.EDIT,
          AppRoutes.DETAILS_WITH_ID(id),
        ]);
        break;
      case RequestType.Entertainment:
        this.router.navigate([
          AppRoutes.AF_SERVICES.BASE,
          AppRoutes.REQUESTS.BASE,
          AppRoutes.REQUESTS.ENTERTAINMENT.BASE,
          AppRoutes.EDIT,
          AppRoutes.DETAILS_WITH_ID(id),
        ]);
        break;
      case RequestType.Purchase:
        this.router.navigate([
          AppRoutes.AF_SERVICES.BASE,
          AppRoutes.REQUESTS.BASE,
          AppRoutes.REQUESTS.PURCHASE.BASE,
          AppRoutes.EDIT,
          AppRoutes.DETAILS_WITH_ID(id),
        ]);
        break;
      case RequestType.NonExpenditurePurchase:
        this.router.navigate([
          AppRoutes.AF_SERVICES.BASE,
          AppRoutes.REQUESTS.BASE,
          AppRoutes.REQUESTS.NONE_EXPENDITURE_PURCHASE.BASE,
          AppRoutes.EDIT,
          AppRoutes.DETAILS_WITH_ID(id),
        ]);
        break;
      default:
        break;
    }
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}
