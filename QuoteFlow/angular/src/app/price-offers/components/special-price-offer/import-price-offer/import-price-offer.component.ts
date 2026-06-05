import { CoreModule } from '@abp/ng.core';
import { Component, inject, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { AutoResizeDirective } from '@app/shared/directives/autoResize.directive';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { TokenClaimsService } from '@app/shared/services/token-claims/token-claims.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { LookupService } from '@proxy/general-lookups';
import { KeyAccountService } from '@proxy/key-accounts';
import { PriceOfferDto } from '@proxy/price-offers';
import { SalesAssignmentDto, SalesAssignmentService } from '@proxy/sales-assignments';
import { KeyAccountLookupDto, LookupDto } from '@proxy/shared';
import {
  BehaviorSubject,
  combineLatest,
  debounceTime,
  distinctUntilChanged,
  map,
  startWith,
  Subject,
  takeUntil,
} from 'rxjs';
import { ImportPriceOfferInformation, ImportPriceOfferType, PriceOfferTypes } from '../price-offer.types';

// Form filter criteria interface
interface FormFilterCriteria {
  materialType?: string;
  locationId?: string;
  buyerId?: string;
  buyerTypeId?: string;
}

@Component({
  selector: 'app-import-price-offer',
  templateUrl: './import-price-offer.component.html',
  styleUrls: ['./import-price-offer.component.scss'],
  providers: [LookupService, KeyAccountService, LoadingService, SalesAssignmentService, TokenClaimsService],
  imports: [FormsModule, NgbModule, NgSelectModule, MatCheckboxModule, CoreModule, AutoResizeDirective],
})
export class ImportPriceOfferComponent implements OnInit, OnChanges, OnDestroy {
  protected proxyLookupService = inject(LookupService);
  protected lookupService = inject(LookupService);
  protected readonly loadingService = inject(LoadingService);
  protected salesAssignmentService = inject(SalesAssignmentService);
  protected tokenClaimsService = inject(TokenClaimsService);
  protected PriceOfferTypes = PriceOfferTypes;

  @Input() importMode: ImportPriceOfferType | undefined;
  @Input() priceOffer?: PriceOfferDto;
  public fileImport: File | null = null;
  importForm: FormGroup = new FormGroup({});
  importPriceOfferType = ImportPriceOfferType;
  importInformation: ImportPriceOfferInformation = {
    file: null,
    saleName: '',
    buyerId: '',
    locationId: '',
    closeDate: null,
    keyAccountId: '',
    keyAccountTypeId: '',
    keyAccountClassId: '',
    autoGetOfferPrice: false,
    note: '',
    materialTypeId: '',
    projectName: '',
    buyerTypeId: '',
  };

  // Reactive dropdown options
  buyerOptions: LookupDto<string>[] = [];
  buyerTypeOptions: LookupDto<string>[] = [];
  locationOptions: LookupDto<string>[] = [];
  keyAccountOptions: KeyAccountLookupDto<string>[] = [];
  materialTypeOptions: LookupDto<string>[] = [];

  // Separate options for NoBuyer mode (loaded from normal lookup endpoints)
  noBuyerLocationOptions: LookupDto<string>[] = [];
  noBuyerMaterialTypeOptions: LookupDto<string>[] = [];

  // Reactive data streams
  private salesAssignments$ = new BehaviorSubject<SalesAssignmentDto[]>([]);
  private destroy$ = new Subject<void>();

  // Track previous values to detect when a field is cleared
  private previousValues: FormFilterCriteria = {};

  // Pre-selection configuration
  private enablePreSelection = true;
  private preSelectionOrder = ['materialType', 'saleLocation', 'buyerType', 'buyer'] as const;
  private preSelectionCompleted = false;

  constructor(private fb: FormBuilder) {
    this.initializeForm();
  }

  ngOnInit(): void {
    this.loadCurrentUser();
    this.updateValidators();
    this.setupReactiveFiltering();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['importMode']) {
      this.updateValidators();
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  get f() {
    return this.importForm.controls;
  }

  get priceOfferType(): string | undefined {
    return this.priceOffer?.priceOfferCode.substring(0, 2);
  }

  private initializeForm(): void {
    this.importForm = this.fb.group({
      file: [null],
      saleName: [null],
      buyer: ['none'],
      buyerType: [null],
      saleLocation: [null],
      closeDate: [null],
      keyAccount: [{ value: '', disabled: true }],
      priceOfferDesc: [null],
      keyAccountType: [{ value: '', disabled: true }],
      keyAccountClass: [{ value: '', disabled: true }],
      keyAccountTypeName: [{ value: '', disabled: true }],
      keyAccountClassName: [{ value: '', disabled: true }],
      materialType: [null],
      autoGetOfferPrice: [false],
      note: [null],
      projectName: [null],
      salePIC: [null],
      location: [null],
      commentAddMoreItems: [null],
    });
  }
  private setupReactiveFiltering(): void {
    // Watch form value changes and debounce to avoid excessive filtering
    const formCriteria$ = this.importForm.valueChanges.pipe(
      startWith(this.importForm.value),
      debounceTime(100),
      distinctUntilChanged(),
      map(
        (formValue): FormFilterCriteria => ({
          materialType: formValue.materialType,
          locationId: formValue.saleLocation,
          buyerId: formValue.buyer && formValue.buyer !== 'none' ? formValue.buyer : undefined,
          buyerTypeId: formValue.buyerType,
        }),
      ),
      takeUntil(this.destroy$),
    );

    // Combine sales assignments with form criteria to get filtered assignments
    const filteredAssignments$ = combineLatest([this.salesAssignments$, formCriteria$]).pipe(
      map(([assignments, criteria]) => {
        // Check if any field was cleared - if so, clear all fields
        if (this.isAnyFieldCleared(criteria)) {
          this.clearAllFields();
          // Return all assignments when fields are cleared
          return assignments;
        }
        return this.filterAssignments(assignments, criteria);
      }),
      takeUntil(this.destroy$),
    );

    // Update dropdown options based on filtered assignments
    filteredAssignments$.subscribe(filteredAssignments => {
      this.updateDropdownOptions(filteredAssignments);
      this.clearInvalidSelections();
    });

    // on auto price offer change
    this.importForm.get('autoGetOfferPrice')?.valueChanges.subscribe(value => {
      this.importInformation.autoGetOfferPrice = value;
    });
  }

  private filterAssignments(assignments: SalesAssignmentDto[], criteria: FormFilterCriteria): SalesAssignmentDto[] {
    return assignments.filter(assignment => {
      // Filter by material type
      if (criteria.materialType && assignment.materialType !== criteria.materialType) {
        return false;
      }

      // Filter by location
      if (criteria.locationId && assignment.locationId !== criteria.locationId) {
        return false;
      }

      // Filter by buyer
      if (criteria.buyerId && assignment.buyerId !== criteria.buyerId) {
        return false;
      }

      // Filter by buyer type
      if (criteria.buyerTypeId && assignment.buyerTypeId !== criteria.buyerTypeId) {
        return false;
      }

      return true;
    });
  }
  private updateDropdownOptions(filteredAssignments: SalesAssignmentDto[]): void {
    const currentValues = this.importForm.value;

    // If no fields are selected, show all options
    const hasAnySelection =
      currentValues.materialType ||
      currentValues.saleLocation ||
      (currentValues.buyer && currentValues.buyer !== 'none') ||
      currentValues.buyerType;

    if (!hasAnySelection) {
      // Reset to all available options
      this.initializeAllDropdownOptions(this.salesAssignments$.value);
      return;
    }

    // Excel-like filtering: Only update options for UNSELECTED fields
    // Selected fields keep their current options unchanged

    // Update material type options only if NOT currently selected
    if (!currentValues.materialType) {
      const materialTypes = [...new Set(filteredAssignments.map(sa => sa.materialType).filter(mt => mt))];
      if (materialTypes.length > 0) {
        this.materialTypeOptions = materialTypes.map(mt => ({ displayName: mt, id: mt }));
      }
    }

    // Update location options only if NOT currently selected
    if (!currentValues.saleLocation) {
      const uniqueLocations = filteredAssignments
        .filter(sa => sa.location)
        .map(sa => sa.location)
        .filter((location, index, self) => self.findIndex(l => l.id === location.id) === index);

      this.locationOptions = this.getDistinctById(
        uniqueLocations.map(
          location =>
            ({
              id: location.id,
              displayName: location.description || location.code || location.id,
            }) as LookupDto<string>,
        ),
      );
    }

    // Update buyer options only if NOT currently selected
    if (!currentValues.buyer || currentValues.buyer === 'none') {
      const uniqueBuyers = filteredAssignments
        .filter(sa => sa.buyer)
        .map(sa => sa.buyer)
        .filter((buyer, index, self) => self.findIndex(b => b.id === buyer.id) === index);

      const buyerOptionsWithoutDefault = this.getDistinctById(
        uniqueBuyers.map(
          buyer =>
            ({
              id: buyer.id,
              displayName: buyer.shortName || buyer.id,
            }) as LookupDto<string>,
        ),
      );

      // Add default "none" option at the beginning
      this.buyerOptions = [
        { id: 'none', displayName: '-- Please select buyer --' } as LookupDto<string>,
        ...buyerOptionsWithoutDefault,
      ];
    }

    // Update buyer type options only if NOT currently selected
    if (!currentValues.buyerType) {
      const uniqueBuyerTypes = filteredAssignments
        .filter(sa => sa.buyerType)
        .map(sa => sa.buyerType)
        .filter((buyerType, index, self) => self.findIndex(bt => bt.id === buyerType.id) === index);

      this.buyerTypeOptions = this.getDistinctById(
        uniqueBuyerTypes.map(
          buyerType =>
            ({
              id: buyerType.id,
              displayName: buyerType.description || buyerType.code || buyerType.id,
            }) as LookupDto<string>,
        ),
      );
    }
  }
  private clearInvalidSelections(): void {
    // With the "clear all" approach, we don't need complex invalid selection logic
    // The reactive streams handle everything automatically
  }

  // Distinct helper function
  private getDistinctById<T extends { id: string }>(items: T[]): T[] {
    const seen = new Set<string>();
    return items.filter(item => {
      if (seen.has(item.id)) {
        return false;
      }
      seen.add(item.id);
      return true;
    });
  }
  private updateValidators() {
    this.resetValidators();

    switch (this.importMode) {
      case ImportPriceOfferType.ProjectPriceOffer:
        this.importForm.get('closeDate')?.setValidators(Validators.required);
        this.importForm.get('buyer')?.setValidators([Validators.required, this.validateBuyerSelection]);
        this.importForm.get('buyerType')?.setValidators(Validators.required);
        this.importForm.get('saleLocation')?.setValidators(Validators.required);
        this.importForm.get('materialType')?.setValidators(Validators.required);
        this.importForm.get('projectName')?.setValidators(Validators.required);
        this.importForm.get('note')?.setValidators(Validators.required);
        break;
      case ImportPriceOfferType.BuyerStockPriceOffer:
        this.importForm.get('note')?.setValidators(Validators.required);
        this.importForm.get('buyer')?.setValidators([Validators.required, this.validateBuyerSelection]);
        this.importForm.get('buyerType')?.setValidators(Validators.required);
        this.importForm.get('saleLocation')?.setValidators(Validators.required);
        this.importForm.get('materialType')?.setValidators(Validators.required);
        this.importForm.get('projectName')?.setValidators(Validators.required);
        this.importForm.get('keyAccount')?.setValidators(Validators.required);
        break;
      case ImportPriceOfferType.KeyAccountPriceOffer:
        this.importForm.get('buyer')?.setValidators([Validators.required, this.validateBuyerSelection]);
        this.importForm.get('buyerType')?.setValidators(Validators.required);
        this.importForm.get('saleLocation')?.setValidators(Validators.required);
        this.importForm.get('materialType')?.setValidators(Validators.required);
        this.importForm.get('projectName')?.setValidators(Validators.required);
        this.importForm.get('keyAccount')?.setValidators(Validators.required);
        break;
      case ImportPriceOfferType.NoBuyerPriceOffer:
        this.importForm.get('closeDate')?.setValidators(Validators.required);
        this.importForm.get('saleLocation')?.setValidators(Validators.required);
        this.importForm.get('materialType')?.setValidators(Validators.required);
        this.importForm.get('projectName')?.setValidators(Validators.required);
        this.importForm.get('note')?.setValidators(Validators.required);
        break;
      case ImportPriceOfferType.AddMoreItems:
      case ImportPriceOfferType.UpdateItemProperties:
        // For AddMoreItems and UpdateItemProperties, only file validation is required (handled in verifyData)
        break;

      default:
        this.importForm.get('buyer')?.setValidators([Validators.required, this.validateBuyerSelection]);
        break;
    }

    this.importForm.updateValueAndValidity();
  }

  /**
   * Custom validator to ensure buyer is not 'none'
   */
  private validateBuyerSelection = (control: any) => {
    const value = control.value;
    if (!value || value === 'none') {
      return { buyerRequired: true };
    }
    return null;
  };

  private resetValidators() {
    Object.keys(this.importForm.controls).forEach(key => {
      this.importForm.get(key)?.clearValidators();
      this.importForm.get(key)?.updateValueAndValidity();
    });
  }

  onFileChange(event: any): void {
    const files = event.target.files;
    if (files.length > 0) {
      this.fileImport = files[0];
    } else {
      this.fileImport = null;
    }
    this.importInformation.file = this.fileImport;
  }

  /**
   * Handle Material Type change
   * Cascade: Reset Location, Buyer Type, Buyer, Key Account
   */
  onMaterialTypeChange(event: LookupDto<string>): void {
    const materialTypeId = event?.id;

    if (!materialTypeId) {
      // Clear material type - reset everything
      this.importInformation.materialTypeId = '';
      this.clearAllFields();
      return;
    }

    // Set material type
    this.importInformation.materialTypeId = materialTypeId;

    // Reset dependent fields
    this.importForm.patchValue(
      {
        saleLocation: null,
        buyerType: null,
        buyer: 'none',
        keyAccount: null,
        keyAccountTypeName: null,
        keyAccountClassName: null,
      },
      { emitEvent: false },
    );

    // Clear dependent import information
    this.importInformation.locationId = '';
    this.importInformation.locationName = '';
    this.importInformation.buyerTypeId = '';
    this.importInformation.buyerTypeName = '';
    this.importInformation.buyerId = '';
    this.importInformation.buyerName = '';
    this.importInformation.keyAccountId = '';
    this.importInformation.keyAccountName = '';
    this.importInformation.keyAccountTypeId = '';
    this.importInformation.keyAccountTypeName = '';
    this.importInformation.keyAccountClassId = '';
    this.importInformation.keyAccountClassName = '';

    // Clear key account options
    this.keyAccountOptions = [];
    this.importForm.get('keyAccount')?.disable();

    // Update previousValues to avoid "clear all" trigger
    this.previousValues = {
      materialType: materialTypeId,
      locationId: undefined,
      buyerTypeId: undefined,
      buyerId: undefined,
    };

    if (this.importMode === ImportPriceOfferType.KeyAccountPriceOffer) {
      const currentBuyerId = this.importForm.get('buyer')?.value;
      if (currentBuyerId && currentBuyerId !== 'none') {
        this.getKeyAccountClassLookup(currentBuyerId, materialTypeId);
      }
    }

    // Enable and trigger reactive filtering
    this.importForm.get('materialType')?.enable();
    this.importForm.patchValue({ materialType: materialTypeId }, { emitEvent: true });
  }

  /**
   * Handle Location change
   * Cascade: Reset Buyer Type, Buyer, Key Account
   */
  onLocationChange(event: any): void {
    const locationId = event?.id;

    if (!locationId) {
      // Clear location - reset dependent fields
      this.importInformation.locationId = '';
      this.importInformation.locationName = '';

      this.importForm.patchValue(
        {
          buyerType: null,
          buyer: 'none',
          keyAccount: null,
          keyAccountTypeName: null,
          keyAccountClassName: null,
        },
        { emitEvent: false },
      );

      this.importInformation.buyerTypeId = '';
      this.importInformation.buyerTypeName = '';
      this.importInformation.buyerId = '';
      this.importInformation.buyerName = '';
      this.importInformation.keyAccountId = '';
      this.importInformation.keyAccountName = '';
      this.importInformation.keyAccountTypeId = '';
      this.importInformation.keyAccountTypeName = '';
      this.importInformation.keyAccountClassId = '';
      this.importInformation.keyAccountClassName = '';

      // Clear key account options
      this.keyAccountOptions = [];
      this.importForm.get('keyAccount')?.disable();

      // Update previousValues
      this.previousValues = {
        ...this.previousValues,
        locationId: undefined,
        buyerTypeId: undefined,
        buyerId: undefined,
      };

      // Trigger reactive filtering
      this.importForm.patchValue(
        {
          saleLocation: null,
        },
        { emitEvent: true },
      );

      return;
    }

    // Set location
    this.importInformation.locationId = event.displayName;
    this.importInformation.locationName = event.displayName;

    // Reset dependent fields
    this.importForm.patchValue(
      {
        buyerType: null,
        buyer: 'none',
        keyAccount: null,
        keyAccountTypeName: null,
        keyAccountClassName: null,
      },
      { emitEvent: false },
    );

    this.importInformation.buyerTypeId = '';
    this.importInformation.buyerTypeName = '';
    this.importInformation.buyerId = '';
    this.importInformation.buyerName = '';
    this.importInformation.keyAccountId = '';
    this.importInformation.keyAccountName = '';
    this.importInformation.keyAccountTypeId = '';
    this.importInformation.keyAccountTypeName = '';
    this.importInformation.keyAccountClassId = '';
    this.importInformation.keyAccountClassName = '';

    // Clear key account options
    this.keyAccountOptions = [];
    this.importForm.get('keyAccount')?.disable();

    // Update previousValues
    this.previousValues = {
      ...this.previousValues,
      locationId: locationId,
      buyerTypeId: undefined,
      buyerId: undefined,
    };

    // Trigger reactive filtering
    this.importForm.patchValue(
      {
        saleLocation: locationId,
      },
      { emitEvent: true },
    );
  }

  /**
   * Handle Buyer Type change
   * Cascade: Reset Buyer, Key Account
   */
  onBuyerTypeChange(event: LookupDto<string>): void {
    const buyerTypeId = event?.id;

    if (!buyerTypeId) {
      // Clear buyer type - reset dependent fields
      this.importInformation.buyerTypeId = '';
      this.importInformation.buyerTypeName = '';

      this.importForm.patchValue(
        {
          buyer: 'none',
          keyAccount: null,
          keyAccountTypeName: null,
          keyAccountClassName: null,
        },
        { emitEvent: false },
      );

      this.importInformation.buyerId = '';
      this.importInformation.buyerName = '';
      this.importInformation.keyAccountId = '';
      this.importInformation.keyAccountName = '';
      this.importInformation.keyAccountTypeId = '';
      this.importInformation.keyAccountTypeName = '';
      this.importInformation.keyAccountClassId = '';
      this.importInformation.keyAccountClassName = '';

      // Clear key account options
      this.keyAccountOptions = [];
      this.importForm.get('keyAccount')?.disable();

      // Update previousValues
      this.previousValues = {
        ...this.previousValues,
        buyerTypeId: undefined,
        buyerId: undefined,
      };

      // Trigger reactive filtering
      this.importForm.patchValue(
        {
          buyerType: null,
        },
        { emitEvent: true },
      );

      return;
    }

    // Set buyer type
    this.importInformation.buyerTypeId = event.id;
    this.importInformation.buyerTypeName = event.displayName;

    // Reset dependent fields
    this.importForm.patchValue(
      {
        buyer: 'none',
        keyAccount: null,
        keyAccountTypeName: null,
        keyAccountClassName: null,
      },
      { emitEvent: false },
    );

    this.importInformation.buyerTypeId = '';
    this.importInformation.buyerTypeName = '';
    this.importInformation.buyerId = '';
    this.importInformation.buyerName = '';
    this.importInformation.keyAccountId = '';
    this.importInformation.keyAccountName = '';
    this.importInformation.keyAccountTypeId = '';
    this.importInformation.keyAccountTypeName = '';
    this.importInformation.keyAccountClassId = '';
    this.importInformation.keyAccountClassName = '';

    // Clear key account options
    this.keyAccountOptions = [];
    this.importForm.get('keyAccount')?.disable();

    // Update previousValues
    this.previousValues = {
      ...this.previousValues,
      buyerTypeId: buyerTypeId,
      buyerId: undefined,
    };

    // Enable and trigger reactive filtering
    this.importForm.get('buyerType')?.enable();
    this.importForm.patchValue(
      {
        buyerType: buyerTypeId,
      },
      { emitEvent: true },
    );
  }

  /**
   * Handle Buyer change
   * Cascade: Reset Key Account (if applicable)
   */
  onBuyerChange(event: LookupDto<string>): void {
    const buyerId = event?.id;

    if (!buyerId || buyerId === 'none') {
      // Clear buyer - reset dependent fields
      this.importInformation.buyerId = '';
      this.importInformation.buyerName = '';

      // Clear key account data
      this.keyAccountOptions = [];
      this.importForm.patchValue(
        {
          keyAccount: null,
          keyAccountType: '',
          keyAccountClass: '',
          keyAccountTypeName: '',
          keyAccountClassName: '',
        },
        { emitEvent: false },
      );

      this.importInformation.keyAccountId = '';
      this.importInformation.keyAccountName = '';
      this.importInformation.keyAccountTypeId = '';
      this.importInformation.keyAccountTypeName = '';
      this.importInformation.keyAccountClassId = '';
      this.importInformation.keyAccountClassName = '';

      this.importForm.get('keyAccount')?.disable();

      // Update previousValues
      this.previousValues = {
        ...this.previousValues,
        buyerId: undefined,
      };

      return;
    }

    // Set buyer
    this.importInformation.buyerId = event.displayName;
    this.importInformation.buyerName = event.displayName;

    // Update previousValues
    this.previousValues = {
      ...this.previousValues,
      buyerId: buyerId,
    };

    // Reset key account data first
    this.keyAccountOptions = [];
    this.importForm.patchValue(
      {
        keyAccount: null,
        keyAccountType: '',
        keyAccountClass: '',
        keyAccountTypeName: '',
        keyAccountClassName: '',
      },
      { emitEvent: false },
    );

    this.importInformation.keyAccountId = '';
    this.importInformation.keyAccountName = '';
    this.importInformation.keyAccountTypeId = '';
    this.importInformation.keyAccountTypeName = '';
    this.importInformation.keyAccountClassId = '';
    this.importInformation.keyAccountClassName = '';

    this.importForm.get('keyAccount')?.disable();

    // Load key account for KeyAccountPriceOffer mode
    if (this.importMode === ImportPriceOfferType.KeyAccountPriceOffer) {
      const currentMaterialType = this.importForm.get('materialType')?.value ?? undefined;
      this.getKeyAccountClassLookup(buyerId, currentMaterialType);
    }
  }

  // onBuyerTypeChange(event: LookupDto<string>): void {
  //   const buyerTypeId = event?.id;

  //   if (buyerTypeId) {
  //     this.importInformation.buyerTypeId = event.id;
  //     this.importInformation.buyerTypeName = event.displayName;
  //     this.importForm.get('buyerType')?.setValue(buyerTypeId);
  //     this.importForm.get('buyerType')?.enable();
  //   }
  // }

  // onBuyerChange(event: LookupDto<string>): void {
  //   const buyerId = event?.id;
  //   if (buyerId && buyerId !== 'none') {
  //     this.importInformation.buyerId = event.displayName;
  //     this.importInformation.buyerName = event.displayName;
  //   } else {
  //     // Handle "none" selection or clearing
  //     this.importInformation.buyerId = '';
  //     this.importInformation.buyerName = '';
  //   }

  //   if (this.importMode === ImportPriceOfferType.KeyAccountPriceOffer) {
  //     // Clear key account data
  //     this.keyAccountOptions = [];
  //     this.importForm.get('keyAccount')?.disable();
  //     this.importForm.patchValue(
  //       {
  //         keyAccount: null,
  //         keyAccountType: '',
  //         keyAccountClass: '',
  //       },
  //       { emitEvent: false },
  //     );

  //     if (buyerId && buyerId !== 'none') {
  //       this.getKeyAccountClassLookup(buyerId);
  //     }
  //   }
  // }

  // onLocationChange(event: any): void {
  //   if (event?.displayName) {
  //     this.importInformation.locationId = event.displayName;
  //     this.importInformation.locationName = event.displayName;
  //   }
  // }

  onKeyAccountChange(event: KeyAccountLookupDto<string>): void {
    const selectedKeyAccount = event;
    this.importInformation.keyAccountId = selectedKeyAccount.id || '';
    this.importInformation.keyAccountName = selectedKeyAccount.displayName || '';
    if (selectedKeyAccount) {
      this.importForm.patchValue({
        keyAccountType: selectedKeyAccount?.keyAccountTypeId || '',
        keyAccountClass: selectedKeyAccount?.keyAccountClassId || '',
        keyAccountTypeName: selectedKeyAccount?.keyAccountTypeName || '',
        keyAccountClassName: selectedKeyAccount?.keyAccountClassName || '',
      });
      this.importInformation.keyAccountTypeId = selectedKeyAccount?.keyAccountTypeId || '';
      this.importInformation.keyAccountTypeName = selectedKeyAccount?.keyAccountTypeName || '';
      this.importInformation.keyAccountClassId = selectedKeyAccount?.keyAccountClassId || '';
      this.importInformation.keyAccountClassName = selectedKeyAccount?.keyAccountClassName || '';
    }
  }

  private loadCurrentUser(): void {
    try {
      const currentUser = this.tokenClaimsService.getUserInfo();
      if (currentUser && currentUser.userName) {
        // Set the sale name in the form
        this.importForm.patchValue({
          saleName: currentUser.fullName,
          salePIC: currentUser.fullName,
        });
        this.importInformation.saleName = currentUser.fullName;

        // Load sales assignments for the current user
        this.loadSalesAssignments(currentUser.userName);
      }
    } catch (error) {
      console.error('Error loading current user:', error);
    }
  }

  private loadSalesAssignments(userName: string): void {
    this.loadingService.show();

    const input = {
      saleUserName: userName,
      maxResultCount: 1000, // Load all assignments for this user
    };
    this.salesAssignmentService.getList(input).subscribe({
      next: result => {
        // Update the BehaviorSubject with new data
        this.salesAssignments$.next(result.items);

        // Initialize all dropdown options on first load
        this.initializeAllDropdownOptions(result.items);

        this.loadingService.hide();
      },
      error: error => {
        console.error('Error fetching sales assignments:', error);
        this.loadingService.hide();
      },
    });

    // Load NoBuyer options from normal lookup endpoints
    this.loadNoBuyerOptions();
  }

  /**
   * Load location and material type options from normal lookup endpoints for NoBuyer mode
   */
  private loadNoBuyerOptions(): void {
    // Load locations from normal lookup endpoint
    this.lookupService.getLocationLookup().subscribe({
      next: result => {
        this.noBuyerLocationOptions = result.items || [];
      },
      error: error => {
        console.error('Error loading NoBuyer locations:', error);
      },
    });

    // Load material types from normal lookup endpoint
    this.lookupService.getMaterialTypeLookup().subscribe({
      next: result => {
        this.noBuyerMaterialTypeOptions =
          result.items.map(item => ({
            id: item.displayCode || '',
            displayName: item.displayName || '',
            displayCode: item.displayCode || '',
          })) || [];
      },
      error: error => {
        console.error('Error loading NoBuyer material types:', error);
      },
    });
  }

  /**
   * Initialize all dropdown options on first load - show ALL available options
   * This is like having no filters applied initially in Excel
   */
  private initializeAllDropdownOptions(assignments: SalesAssignmentDto[]): void {
    // Extract all unique material types
    // const materialTypes = [...new Set(assignments.map(sa => sa.materialType).filter(mt => mt))];
    // this.materialTypeOptions = materialTypes.map(mt => ({ displayName: mt, id: mt }));
    const uniqueMaterialTypes = assignments
      .filter(sa => sa.materialType)
      .map(sa => sa.materialType)
      .filter((materialType, index, self) => self.indexOf(materialType) === index);

    this.materialTypeOptions = uniqueMaterialTypes.map(mt => ({ displayName: mt, id: mt }));

    // Extract all unique locations
    const uniqueLocations = assignments
      .filter(sa => sa.location)
      .map(sa => sa.location)
      .filter((location, index, self) => self.findIndex(l => l.id === location.id) === index);

    this.locationOptions = this.getDistinctById(
      uniqueLocations.map(
        location =>
          ({
            id: location.id,
            displayName: location.description || location.code || location.id,
          }) as LookupDto<string>,
      ),
    );

    // Extract all unique buyers
    const uniqueBuyers = assignments
      .filter(sa => sa.buyer)
      .map(sa => sa.buyer)
      .filter((buyer, index, self) => self.findIndex(b => b.id === buyer.id) === index);

    const buyerOptionsWithoutDefault = this.getDistinctById(
      uniqueBuyers.map(
        buyer =>
          ({
            id: buyer.id,
            displayName: buyer.shortName || buyer.id,
          }) as LookupDto<string>,
      ),
    );

    // Add default "none" option at the beginning
    this.buyerOptions = [
      { id: 'none', displayName: '-- Please select buyer --' } as LookupDto<string>,
      ...buyerOptionsWithoutDefault,
    ];

    // Extract all unique buyer types
    const uniqueBuyerTypes = assignments
      .filter(sa => sa.buyerType)
      .map(sa => sa.buyerType)
      .filter((buyerType, index, self) => self.findIndex(bt => bt.id === buyerType.id) === index);

    this.buyerTypeOptions = this.getDistinctById(
      uniqueBuyerTypes.map(
        buyerType =>
          ({
            id: buyerType.id,
            displayName: buyerType.description || buyerType.code || buyerType.id,
          }) as LookupDto<string>,
      ),
    );

    // After initializing all options, trigger pre-selection if enabled
    if (this.enablePreSelection && !this.preSelectionCompleted) {
      // Use setTimeout to ensure options are fully loaded before pre-selection
      setTimeout(() => {
        this.performPreSelection();
      }, 100);
    }
  }

  private getKeyAccountClassLookup(buyerId: string, materialType?: string) {
    this.loadingService.show();
    this.lookupService.getKeyAccountLookup(buyerId, materialType).subscribe({
      next: result => {
        this.keyAccountOptions = result.items;
        this.importForm.get('keyAccount')?.enable();
        this.loadingService.hide();
      },
      error: error => {
        this.loadingService.hide();
        console.error('Error fetching key account class lookup:', error);
      },
    });
  }

  /**
   * Check if any field was cleared (changed from having a value to null)
   */
  private isAnyFieldCleared(currentCriteria: FormFilterCriteria): boolean {
    const wasCleared =
      (this.previousValues.materialType && !currentCriteria.materialType) ||
      (this.previousValues.locationId && !currentCriteria.locationId) ||
      (this.previousValues.buyerId && !currentCriteria.buyerId) ||
      (this.previousValues.buyerTypeId && !currentCriteria.buyerTypeId);

    // Update previous values for next comparison
    this.previousValues = { ...currentCriteria };

    return wasCleared;
  }

  /**
   * Clear all interdependent fields when any field is cleared
   */
  private clearAllFields(): void {
    this.importForm.patchValue({
      buyer: 'none',
      buyerType: null,
      saleLocation: null,
      location: null,
      keyAccount: null,
      keyAccountType: null,
      keyAccountClass: null,
      materialType: null,
    });

    // Also clear the lookup display names
    this.importForm.patchValue({
      keyAccountTypeName: null,
      keyAccountClassName: null,
    });

    // Reset all dropdown options to show all available data
    const allAssignments = this.salesAssignments$.value;
    this.initializeAllDropdownOptions(allAssignments);
  }

  /**
   * Clear the selected file and reset file input
   */
  clearSelectedFile(): void {
    this.fileImport = null;
    this.importForm.patchValue({ file: null });
    this.importInformation.file = null;

    // Reset the file input element based on current import mode
    let fileInputId: string;
    switch (this.importMode) {
      case ImportPriceOfferType.AddMoreItems:
        fileInputId = 'fileAddMoreItems';
        break;
      case ImportPriceOfferType.UpdateItemProperties:
        fileInputId = 'fileUpdateItemProperties';
        break;
      case ImportPriceOfferType.ProjectPriceOffer:
        fileInputId = 'fileProjectPriceOffer';
        break;
      case ImportPriceOfferType.KeyAccountPriceOffer:
        fileInputId = 'fileKeyAccountPriceOffer';
        break;
      case ImportPriceOfferType.BuyerStockPriceOffer:
        fileInputId = 'fileBuyerStockPriceOffer';
        break;
      case ImportPriceOfferType.NoBuyerPriceOffer:
        fileInputId = 'fileNoBuyerPriceOffer';
        break;
      default:
        // Fallback to the old behavior for any unknown import mode
        fileInputId = '';
        break;
    }

    if (fileInputId) {
      const fileInput = document.getElementById(fileInputId) as HTMLInputElement;
      if (fileInput) {
        fileInput.value = '';
      }
    } else {
      // Fallback: clear the first file input found
      const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
      if (fileInput) {
        fileInput.value = '';
      }
    }
  }

  /**
   * Perform automatic pre-selection in the specified order
   */
  private performPreSelection(): void {
    if (this.preSelectionCompleted) {
      return;
    }

    // Start the chain of pre-selections
    this.preSelectMaterialType();
  }

  /**
   * Pre-select the first available material type
   */
  private preSelectMaterialType(): void {
    if (this.materialTypeOptions.length > 0) {
      const firstMaterialType = this.materialTypeOptions[0];
      this.importForm.patchValue({ materialType: firstMaterialType.id });
      this.onMaterialTypeChange(firstMaterialType);

      // Wait for reactive filtering to complete, then proceed to next selection
      setTimeout(() => {
        this.preSelectLocation();
      }, 150);
    } else {
      this.preSelectionCompleted = true;
    }
  }

  /**
   * Pre-select the first available location
   */
  private preSelectLocation(): void {
    if (this.locationOptions.length > 0) {
      const firstLocation = this.locationOptions[0];
      this.importForm.patchValue({ saleLocation: firstLocation.id });
      this.onLocationChange(firstLocation);

      // Wait for reactive filtering to complete, then proceed to next selection
      setTimeout(() => {
        this.preSelectBuyerType();
      }, 150);
    } else {
      this.preSelectionCompleted = true;
    }
  }

  /**
   * Pre-select the first available buyer type
   */
  private preSelectBuyerType(): void {
    if (this.buyerTypeOptions.length > 0) {
      const firstBuyerType = this.buyerTypeOptions[0];
      this.importForm.patchValue({ buyerType: firstBuyerType.id });
      this.onBuyerTypeChange(firstBuyerType);

      // Wait for reactive filtering to complete, then proceed to next selection
      setTimeout(() => {
        this.preSelectBuyer();
      }, 150);
    } else {
      this.preSelectionCompleted = true;
    }
  }

  /**
   * Pre-select the first available buyer (skip 'none' option)
   */
  private preSelectBuyer(): void {
    // Find the first real buyer (not the 'none' option)
    // const realBuyers = this.buyerOptions.filter(b => b.id !== 'none');
    // if (realBuyers.length > 0) {
    const firstBuyer = this.buyerOptions[0];
    this.importForm.patchValue({ buyer: firstBuyer.id });
    this.onBuyerChange(firstBuyer);
    // }

    // Mark pre-selection as completed
    this.preSelectionCompleted = true;
  }

  /**
   * Reset pre-selection state (useful if you want to re-trigger pre-selection)
   */
  public resetPreSelection(): void {
    this.preSelectionCompleted = false;
  }

  /**
   * Enable or disable pre-selection feature
   */
  public setPreSelectionEnabled(enabled: boolean): void {
    this.enablePreSelection = enabled;
  }

  /**
   * Format currency value for display
   */
  formatCurrency(value: number | null | undefined): string {
    if (value == null) return '$0';
    return new Intl.NumberFormat('en-US', {
      minimumFractionDigits: 0,
      maximumFractionDigits: 0,
    }).format(value);
  }
}
