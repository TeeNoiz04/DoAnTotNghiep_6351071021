import { CoreModule } from '@abp/ng.core';
import { DateAdapter, ThemeSharedModule, TimeAdapter } from '@abp/ng.theme.shared';
import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import {
  NgbDateAdapter,
  NgbDatepickerModule,
  NgbNavModule,
  NgbTimeAdapter,
  NgbTimepickerModule,
} from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { BuyerDto } from '@proxy/buyers';
import { LookupService } from '@proxy/general-lookups';
import { SalesAssignmentService } from '@proxy/sales-assignments';
import { UserLookupDto } from '@proxy/shared';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { catchError, debounceTime, distinctUntilChanged, map, Observable, of, Subject, switchMap, tap } from 'rxjs';
import { SaleBuyerDetailViewService } from '../services/sale-buyer-detail.service';
import { EscCloseModalDirective } from '@app/shared/esc-close-modal/esc-close-modal.directive';

@Component({
  selector: 'app-sale-buyer-detail-modal',
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    ReactiveFormsModule,
    NgbDatepickerModule,
    NgbTimepickerModule,
    NgbNavModule,
    MatSlideToggleModule,
    NgSelectModule,
    EscCloseModalDirective,
  ],
  providers: [
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './sale-buyer-detail.component.html',
  styleUrls: ['./sale-buyer-detail.component.scss'],
})
export class SaleBuyerDetailModalComponent implements OnInit {
  public readonly service = inject(SaleBuyerDetailViewService);
  public readonly proxyService = inject(SalesAssignmentService);
  requesterList$: Observable<UserLookupDto[]>;
  inputRequester$ = new Subject<string>();
  loading = false;
  protected proxyLookupService = inject(LookupService);

  locationOptions: { value: string; label: string }[] = [];
  buyerOptions: { value: string; label: string }[] = [];
  buyerTypeOptions: { value: string; label: string }[] = [];
  userOptions: { id: string; fullName: string; userName: string; email: string; phoneNumber: string }[] = [];
  dataBuyer: BuyerDto[] = [];
  ngOnInit(): void {
    this.getLocation();
    if (this.service.form.get('buyerTypeId')?.value != null) {
      this.getBuyerByBuyerType(this.service.form.get('buyerTypeId')?.value);
    }
    // this.getBuyer();
    this.getBuyerType();
    this.requesterList$ = this.inputRequester$.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      tap(() => (this.loading = true)),
      switchMap(term =>
        this.searchRequesters(term).pipe(
          catchError(() => of([])), // Return empty array on error
          tap(() => (this.loading = false)),
        ),
      ),
    );
  }
  onChangeBuyerType(event: any) {
    this.buyerOptions = [];
    this.service.form.get('buyerId')?.reset();
    if (event.value) {
      this.getBuyerByBuyerType(event.value);
    }
  }
  getBuyerByBuyerType(buyerTypeId: string) {
    this.proxyLookupService.getBuyerLookupByBuyerType(buyerTypeId).subscribe(result => {
      this.buyerOptions = result.items.map(item => ({
        value: item.id,
        label: item.displayCode,
      }));
      const buyerId = this.service.form.get('buyerId')?.value;
      if (buyerId) {
        const found = this.buyerOptions.find(x => x.value === buyerId);

        if (found) {
          this.service.form.get('buyerId')?.setValue(found.value, { emitEvent: false });
          this.service.buyerShortName = found.label;
        }
      }
    });
  }

  onBuyerChange(selectedValue: any) {
    this.service.buyerShortName = selectedValue.label || '';
  }

  get f() {
    return this.service.form.controls;
  }

  searchRequesters(filterText: string): Observable<UserLookupDto[]> {
    if (filterText?.length < 2 || !filterText) {
      this.loading = false;
      return of([]);
    }

    return this.proxyService
      .getListUserLookupByName(filterText)
      .pipe(map(response => (Array.isArray(response) ? response : [])));
  }

  getLocation() {
    this.proxyLookupService.getLocationLookup().subscribe(result => {
      this.locationOptions = result.items.map(item => ({
        value: item.id,
        label: item.displayName,
      }));
    });
  }
  getBuyer() {
    this.proxyLookupService.getBuyerLookup(false).subscribe(result => {
      this.buyerOptions = result.items.map(item => ({
        value: item.id,
        label: item.displayCode,
      }));
    });
  }

  getBuyerType() {
    this.proxyLookupService.getBuyerTypeLookup({}).subscribe(result => {
      this.buyerTypeOptions = result.items.map(item => ({
        value: item.id,
        label: item.displayCode,
      }));
    });
  }

  getUser() {
    this.proxyLookupService.getUserLookup().subscribe(result => {
      this.userOptions = result.map((item: UserLookupDto) => ({
        id: item.id,
        fullName: item.fullName,
        userName: item.userName,
        email: item.email,
        phoneNumber: item.phoneNumber,
      }));
    });
  }
}
