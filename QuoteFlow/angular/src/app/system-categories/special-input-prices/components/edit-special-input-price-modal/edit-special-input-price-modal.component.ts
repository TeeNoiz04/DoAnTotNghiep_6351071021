import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnChanges, OnInit, Output } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { EscCloseModalDirective } from '@app/shared/esc-close-modal/esc-close-modal.directive';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { LookupService } from '@proxy/general-lookups';
import { LookupDto, SupplierBULookupDto } from '@proxy/shared';
import { SpecialInputPriceService } from '@proxy/special-input-prices';
import { SpecialInputPriceLookupDto, SpecialInputPriceUpdateDto } from '@proxy/special-input-prices/models';
import { BehaviorSubject, combineLatest, Observable, of } from 'rxjs';
import { distinctUntilChanged, map, switchMap, tap } from 'rxjs/operators';

export interface EditSpecialInputPriceData {
  id: string;
  accountNo: string;
  customerName: string;
  projectName: string;
  materialType?: string;
  supplierId?: string;
  supplierBUId?: string;
  currency?: string;
  note: string;
  validFrom?: string;
  validTo?: string;
  status?: string;
  concurrencyStamp?: string;
}

@Component({
  selector: 'app-edit-special-input-price-modal',
  standalone: true,
  imports: [
    CommonModule,
    CoreModule,
    ThemeSharedModule,
    ReactiveFormsModule,
    NgSelectModule,
    NgbDatepickerModule,
    EscCloseModalDirective,
  ],
  templateUrl: './edit-special-input-price-modal.component.html',
  styleUrls: ['./edit-special-input-price-modal.component.scss'],
})
export class EditSpecialInputPriceModalComponent implements OnInit, OnChanges {
  // TODO: can implement FE temporary cache for material, supplier, and supplier BU lookups to avoid
  // multiple API calls, also improve BE to include the list of supplier BUs for a given supplier
  private readonly loadingService = inject(LoadingService);
  private readonly toasterService = inject(ToasterService);
  private readonly lookupService = inject(LookupService);
  private readonly specialInputPriceService = inject(SpecialInputPriceService);
  private readonly formBuilder = inject(FormBuilder);

  @Input() visible = false;
  @Input() editData: EditSpecialInputPriceData | null = null;
  @Output() visibleChange = new EventEmitter<boolean>();
  @Output() saveConfirmed = new EventEmitter<EditSpecialInputPriceData>();

  public editForm: FormGroup;
  public specialInputPriceOptions: SpecialInputPriceLookupDto<string>[] = [];
  public isLoadingOptions = false;
  public busy = false;

  // New properties for cascading dropdowns
  public materialTypes$: Observable<LookupDto<string>[]>;
  public filteredSuppliers$: Observable<LookupDto<string>[]>;
  public filteredSupplierBUs$: Observable<SupplierBULookupDto<string>[]>;

  private materialTypeSubject = new BehaviorSubject<string | null>(null);
  private supplierSubject = new BehaviorSubject<string | null>(null);

  constructor() {
    this.editForm = this.formBuilder.group({
      accountNo: ['', [Validators.required, this.accountNoValidator.bind(this)]],
      customerName: ['', Validators.required],
      projectName: ['', Validators.required],
      materialType: ['', Validators.required],
      supplierId: ['', Validators.required],
      supplierBUId: ['', Validators.required],
      currency: [''],
      note: [''],
      validFrom: ['', Validators.required],
      validTo: ['', Validators.required],
      status: [''],
    });

    // Add custom validator for date range
    this.editForm.addValidators(this.dateRangeValidator.bind(this));

    // Initialize observables for cascading dropdowns
    this.initializeDropdowns();
  }

  ngOnInit() {
    this.loadSpecialInputPriceOptions();
  }

  ngOnChanges() {
    if (this.editData) {
      this.populateForm();
    }
  }

  private loadSpecialInputPriceOptions() {
    this.isLoadingOptions = true;
    this.lookupService.getSpecialInputPriceLookup(null).subscribe({
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

  private accountNoValidator(control: any) {
    if (!control.value) {
      return null;
    }

    // Skip validation if the account number is the same as the original (no change)
    if (this.editData && control.value === this.editData.accountNo) {
      return null;
    }

    const accountNoExists = this.specialInputPriceOptions.some(option => option.accountNo === control.value);

    return accountNoExists ? { accountNoExists: true } : null;
  }

  private dateRangeValidator(control: AbstractControl): ValidationErrors | null {
    const validFrom = control.get('validFrom')?.value;
    const validTo = control.get('validTo')?.value;

    if (validFrom && validTo && validFrom >= validTo) {
      return { dateRangeInvalid: true };
    }
    return null;
  }

  private populateForm() {
    if (this.editData) {
      // Update subjects FIRST to trigger cascading dropdowns before form patching
      if (this.editData.materialType) {
        this.materialTypeSubject.next(this.editData.materialType);
      }
      if (this.editData.supplierId) {
        this.supplierSubject.next(this.editData.supplierId);
      }

      // Use setTimeout to ensure dropdown data is loaded before patching form values
      setTimeout(() => {
        this.editForm.patchValue({
          accountNo: this.editData.accountNo,
          customerName: this.editData.customerName,
          projectName: this.editData.projectName,
          materialType: this.editData.materialType || '',
          supplierId: this.editData.supplierId || '',
          supplierBUId: this.editData.supplierBUId || '',
          currency: this.editData.currency,
        });

        this.editForm.patchValue({
          accountNo: this.editData.accountNo,
          customerName: this.editData.customerName,
          projectName: this.editData.projectName,
          materialType: this.editData.materialType || '',
          supplierId: this.editData.supplierId || '',
          supplierBUId: this.editData.supplierBUId || '',
          currency: this.editData.currency || '',
          note: this.editData.note,
          validFrom: this.editData.validFrom || '',
          validTo: this.editData.validTo || '',
          status: this.editData.status || '',
        });
      }, 100);
    }
  }

  isFormValid(): boolean {
    return this.editForm.valid;
  }

  onCancel() {
    this.visible = false;
    this.visibleChange.emit(false);
    this.resetForm();
  }

  onSave() {
    if (!this.isFormValid()) {
      this.editForm.markAllAsTouched();
      this.toasterService.error('Please fill in all required fields correctly.');
      return;
    }

    if (!this.editData) {
      this.toasterService.error('No data to save.');
      return;
    }

    const formValue = this.editForm.getRawValue();
    const updateDto: SpecialInputPriceUpdateDto = {
      accountNo: formValue.accountNo?.trim(),
      accountName: formValue.customerName?.trim(),
      projectName: formValue.projectName?.trim() || '',
      materialType: formValue.materialType || '',
      supplierId: formValue.supplierId || undefined,
      supplierBUId: formValue.supplierBUId || undefined,
      currency: formValue.currency || '',
      note: formValue.note?.trim() || '',
      validFrom: formValue.validFrom || undefined,
      validTo: formValue.validTo || undefined,
      status: formValue.status || 'Active',
      concurrencyStamp: this.editData.concurrencyStamp,
    };

    this.busy = true;

    this.specialInputPriceService.update(this.editData.id, updateDto).subscribe({
      next: result => {
        this.busy = false;
        // Map API result to interface format
        const saveData: EditSpecialInputPriceData = {
          id: result.id || this.editData.id,
          accountNo: result.accountNo.trim() || '',
          customerName: result.accountName.trim() || '',
          projectName: result.projectName.trim() || '',
          materialType: result.materialType || '',
          supplierId: result.supplierId || '',
          supplierBUId: result.supplierBUId || '',
          currency: result.currency || '',
          note: result.note.trim() || '',
          validFrom: result.validFrom,
          validTo: result.validTo,
          status: result.status,
          concurrencyStamp: result.concurrencyStamp,
        };
        this.saveConfirmed.emit(saveData);
        this.visible = false;
        this.visibleChange.emit(false);
        this.resetForm();
      },
      error: error => {
        this.busy = false;
        this.toasterService.error('Failed to update special input price. Please try again.');
      },
    });
  }

  private resetForm() {
    this.editForm.reset();
    this.editForm.patchValue({
      accountNo: '',
      customerName: '',
      projectName: '',
      materialType: '',
      supplierId: '',
      supplierBUId: '',
      currency: '',
      note: '',
      validFrom: '',
      validTo: '',
      status: '',
    });
    // Reset subjects
    this.materialTypeSubject.next(null);
    this.supplierSubject.next(null);
  }

  // Helper methods for template
  getFieldError(fieldName: string): string {
    const field = this.editForm.get(fieldName);
    if (field?.errors && field.touched) {
      if (field.errors['required']) {
        return `${this.getFieldDisplayName(fieldName)} is required.`;
      }
      if (field.errors['accountNoExists']) {
        return 'This account number already exists.';
      }
    }

    // Check for form-level date range validation
    if (fieldName === 'validTo' && this.editForm.errors?.['dateRangeInvalid']) {
      return 'Valid To must be after Valid From.';
    }

    if (field?.errors && field.touched) {
      // Return all error messages as a comma-separated string
      const errorMessages = Object.values(field.errors)
        .map(error => {
          if (typeof error === 'string') {
            return error;
          }
          return error.message || 'Invalid value';
        })
        .join(', ');
      return errorMessages;
    }
    return '';
  }

  private getFieldDisplayName(fieldName: string): string {
    const fieldNames: { [key: string]: string } = {
      accountNo: 'Account No',
      customerName: 'Customer Name',
      projectName: 'Project Name',
      materialType: 'Material Type',
      supplierId: 'Supplier',
      supplierBUId: 'Supplier BU',
      currency: 'Currency',
      note: 'Note',
      validFrom: 'Valid From',
      validTo: 'Valid To',
    };
    return fieldNames[fieldName] || fieldName;
  }

  hasFieldError(fieldName: string): boolean {
    const field = this.editForm.get(fieldName);
    return !!(field?.errors && field.touched);
  }

  private initializeDropdowns() {
    // Load material types
    this.materialTypes$ = this.lookupService.getMaterialTypeLookup().pipe(
      map(response => {
        return response.items || [];
      }),
    );

    // Load suppliers based on material type
    this.filteredSuppliers$ = this.materialTypeSubject.pipe(
      distinctUntilChanged(),
      switchMap(materialType =>
        materialType
          ? this.lookupService.getSupplierByMaterialTypeLookup(materialType).pipe(
              map(response => {
                return response.items || [];
              }),
            )
          : of([]),
      ),
    );

    // Load supplier BUs based on supplier and material type
    this.filteredSupplierBUs$ = combineLatest([this.supplierSubject, this.materialTypeSubject]).pipe(
      distinctUntilChanged(),
      switchMap(([supplierId, materialType]) =>
        supplierId && materialType
          ? this.lookupService.getSupplierBUBySupplierAndMaterialTypeLookup(supplierId, materialType).pipe(
              map(response => {
                return response.items || [];
              }),
            )
          : of([]),
      ),
    );
  }

  onMaterialTypeChange(materialType: any) {
    // Extract the actual value from the event
    const value = typeof materialType === 'string' ? materialType : materialType?.displayCode || materialType || null;

    this.materialTypeSubject.next(value);
    // Reset dependent fields
    this.editForm.patchValue({
      supplierId: '',
      supplierBUId: '',
      currency: '',
    });
    this.supplierSubject.next(null);
  }

  onSupplierChange(supplierId: any) {
    // Extract the actual value from the event
    const value = typeof supplierId === 'string' ? supplierId : supplierId?.id || supplierId || null;

    this.supplierSubject.next(value);
    // Reset dependent fields
    this.editForm.patchValue({
      supplierBUId: '',
      currency: '',
    });
  }

  onSupplierBUChange(supplierBUId: any) {
    // Extract the actual value from the event
    const value = typeof supplierBUId === 'string' ? supplierBUId : supplierBUId?.id || supplierBUId || null;

    if (value) {
      // Find the selected supplier BU and set currency
      this.filteredSupplierBUs$
        .pipe(
          map(supplierBUs => supplierBUs.find(bu => bu.id === value)),
          tap(selectedBU => {
            if (selectedBU?.currency) {
              this.editForm.patchValue({ currency: selectedBU.currency });
            }
          }),
        )
        .subscribe();
    } else {
      this.editForm.patchValue({ currency: '' });
    }
  }

  get form() {
    return this.editForm;
  }

  get ctrls() {
    return this.editForm.controls;
  }
}
