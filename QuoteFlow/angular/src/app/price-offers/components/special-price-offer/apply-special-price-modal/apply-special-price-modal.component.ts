import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { NgSelectModule } from '@ng-select/ng-select';
import { LookupService } from '@proxy/general-lookups';
import { PriceOfferService } from '@proxy/price-offers';
import { AssignSpecialInputPriceDto } from '@proxy/price-offers/models';
import { SpecialInputPriceLookupDto } from '@proxy/special-input-prices/models';

export interface ApplySpecialPriceData {
  selectedSpecialInputPriceId: string | null;
  note: string;
}

export interface ApplySpecialPriceRequest {
  specialInputPriceId: string;
  note: string;
  priceOfferId: string;
}

@Component({
  selector: 'app-apply-special-price-modal',
  standalone: true,
  imports: [CommonModule, CoreModule, ThemeSharedModule, FormsModule, NgSelectModule],
  templateUrl: './apply-special-price-modal.component.html',
  styleUrls: ['./apply-special-price-modal.component.scss'],
})
export class ApplySpecialPriceModalComponent implements OnChanges {
  private readonly loadingService = inject(LoadingService);
  private readonly toasterService = inject(ToasterService);
  private readonly lookupService = inject(LookupService);
  private readonly priceOfferService = inject(PriceOfferService);
  @Input() visible = false;
  @Input() priceOfferId: string | null = null;
  @Input() concurrencyStamp: string | null = null;
  @Input() materialType: string | null = null;
  @Output() visibleChange = new EventEmitter<boolean>();
  @Output() applyConfirmed = new EventEmitter<ApplySpecialPriceRequest>();
  private hasLoadedOptions = false;

  public applySpecialPriceData: ApplySpecialPriceData = {
    selectedSpecialInputPriceId: null,
    note: '',
  };

  public specialInputPriceOptions: SpecialInputPriceLookupDto<string>[] = [];
  public isLoadingOptions = false;
  public busy = false;

  ngOnChanges(changes: SimpleChanges) {
    const shouldLoad = this.visible && this.materialType && !this.hasLoadedOptions;

    if (shouldLoad) {
      this.loadSpecialInputPriceOptions();
    }
  }

  private loadSpecialInputPriceOptions() {
    this.isLoadingOptions = true;
    this.lookupService.getSpecialInputPriceLookup(this.materialType).subscribe({
      next: response => {
        this.specialInputPriceOptions = response.items || [];
        this.isLoadingOptions = false;
      },
      error: () => {
        this.toasterService.error('Failed to load special input price options.');
        this.isLoadingOptions = false;
      },
    });
  }
  onModalVisibilityChange(visible: boolean) {
    this.visible = visible;
    this.visibleChange.emit(visible);

    if (!visible) {
      this.resetForm();
    }
  }

  isFormValid(): boolean {
    // Check if special input price is selected and note is provided
    return this.applySpecialPriceData.selectedSpecialInputPriceId !== null;
  }

  onCancel() {
    this.onModalVisibilityChange(false);
  }

  onApply() {
    if (!this.isFormValid()) {
      this.toasterService.error('Please select a special input price and provide a note.');
      return;
    }
    if (!this.priceOfferId) {
      this.toasterService.error('Price Offer ID is required.');
      return;
    }

    if (!this.concurrencyStamp) {
      this.toasterService.error('Concurrency stamp is required.');
      return;
    }

    const assignData: AssignSpecialInputPriceDto = {
      specialInputPriceId: this.applySpecialPriceData.selectedSpecialInputPriceId!,
      note: this.applySpecialPriceData.note.trim(),
      concurrencyStamp: this.concurrencyStamp,
    };

    const applyData: ApplySpecialPriceRequest = {
      specialInputPriceId: this.applySpecialPriceData.selectedSpecialInputPriceId!,
      note: this.applySpecialPriceData.note.trim(),
      priceOfferId: this.priceOfferId,
    };

    this.busy = true;

    this.priceOfferService.assignSpecialInputPriceByIdAndInput(this.priceOfferId, assignData).subscribe({
      next: () => {
        this.busy = false;
        this.toasterService.success('Special price applied successfully.');
        this.applyConfirmed.emit(applyData);
        this.onModalVisibilityChange(false);
      },
      error: () => {
        this.busy = false;
        this.toasterService.error('Failed to apply special price. Please try again.');
      },
    });
  }

  private resetForm() {
    this.applySpecialPriceData = {
      selectedSpecialInputPriceId: null,
      note: '',
    };
  }
}
