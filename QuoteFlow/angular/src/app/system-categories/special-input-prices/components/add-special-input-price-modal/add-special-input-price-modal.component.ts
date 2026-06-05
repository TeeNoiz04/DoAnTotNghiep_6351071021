import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
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
import { SpecialInputPriceLookupDto } from '@proxy/special-input-prices/models';
import { BehaviorSubject, combineLatest, Observable, of } from 'rxjs';
import { distinctUntilChanged, map, switchMap, tap } from 'rxjs/operators';

export interface AddSpecialInputPriceData {
  accountNo: string;
  customerName: string;
  projectName: string;
  materialType?: string;
  supplierId?: string;
  supplierBUId?: string;
  currency?: string;
  validFrom?: string;
  validTo?: string;
  note: string;
  status?: string;
}

@Component({
  selector: 'app-add-special-input-price-modal',
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
  templateUrl: './add-special-input-price-modal.component.html',
  styleUrls: ['./add-special-input-price-modal.component.scss'],
})
export class AddSpecialInputPriceModalComponent implements OnInit {
  private readonly loadingService = inject(LoadingService);
  private readonly toasterService = inject(ToasterService);
  private readonly lookupService = inject(LookupService);
  private readonly specialInputPriceService = inject(SpecialInputPriceService);
  private readonly formBuilder = inject(FormBuilder);

  @Input() visible = false;
  @Output() visibleChange = new EventEmitter<boolean>();
  @Output() addConfirmed = new EventEmitter<AddSpecialInputPriceData>();

  public addForm: FormGroup;
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
    this.addForm = this.formBuilder.group({
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
    this.addForm.addValidators(this.dateRangeValidator.bind(this));

    // Initialize observables for cascading dropdowns
    this.initializeDropdowns();
  }

  ngOnInit() {
    this.loadSpecialInputPriceOptions();
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

  isFormValid(): boolean {
    return this.addForm.valid;
  }

  onCancel() {
    this.visible = false;
    this.visibleChange.emit(false);
    this.resetForm();
  }

  onAdd() {
    if (!this.isFormValid()) {
      this.addForm.markAllAsTouched();
      this.toasterService.error('Please fill in all required fields correctly.');
      return;
    }

    const formValue = this.addForm.getRawValue();
    const createDto = {
      accountNo: formValue.accountNo.trim(),
      accountName: formValue.customerName.trim(),
      projectName: formValue.projectName.trim() || '',
      materialType: formValue.materialType || '',
      supplierId: formValue.supplierId || undefined,
      supplierBUId: formValue.supplierBUId || undefined,
      currency: formValue.currency || '',
      note: formValue.note.trim() || '',
      validFrom: formValue.validFrom || undefined,
      validTo: formValue.validTo || undefined,
      status: formValue.status || 'Active',
    };

    this.busy = true;

    this.specialInputPriceService.create(createDto).subscribe({
      next: result => {
        this.busy = false;
        // Map API result to interface format
        const addData: AddSpecialInputPriceData = {
          accountNo: result.accountNo || '',
          customerName: result.accountName || '',
          projectName: result.projectName || '',
          materialType: result.materialType || '',
          supplierId: result.supplierId || '',
          supplierBUId: result.supplierBUId || '',
          currency: result.currency || '',
          note: result.note || '',
          validFrom: result.validFrom,
          validTo: result.validTo,
          status: result.status,
        };
        this.addConfirmed.emit(addData);
        this.visible = false;
        this.visibleChange.emit(false);
        this.resetForm();
      },
      error: error => {
        this.busy = false;
        this.toasterService.error('Failed to add special input price. Please try again.');
      },
    });
  }

  private resetForm() {
    this.addForm.reset();
    this.addForm.patchValue({
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

  private initializeDropdowns() {
    // Load material types
    this.materialTypes$ = this.lookupService.getMaterialTypeLookup().pipe(map(response => response.items || []));

    // Load suppliers based on material type
    this.filteredSuppliers$ = this.materialTypeSubject.pipe(
      distinctUntilChanged(),
      switchMap(materialType =>
        materialType
          ? this.lookupService.getSupplierByMaterialTypeLookup(materialType).pipe(map(response => response.items || []))
          : of([]),
      ),
    );

    // Load supplier BUs based on supplier and material type
    this.filteredSupplierBUs$ = combineLatest([this.supplierSubject, this.materialTypeSubject]).pipe(
      distinctUntilChanged(),
      switchMap(([supplierId, materialType]) =>
        supplierId && materialType
          ? this.lookupService
              .getSupplierBUBySupplierAndMaterialTypeLookup(supplierId, materialType)
              .pipe(map(response => response.items || []))
          : of([]),
      ),
    );
  }

  onMaterialTypeChange(materialType: any) {
    // Extract the actual value from the event
    const value = typeof materialType === 'string' ? materialType : materialType?.displayCode || materialType || null;

    this.materialTypeSubject.next(value);
    // Reset dependent fields
    this.addForm.patchValue({
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
    this.addForm.patchValue({
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
              this.addForm.patchValue({ currency: selectedBU.currency });
            }
          }),
        )
        .subscribe();
    } else {
      this.addForm.patchValue({ currency: '' });
    }
  }

  get form() {
    return this.addForm;
  }

  get ctrls() {
    return this.addForm.controls;
  }

  // Helper methods for template
  getFieldError(fieldName: string): string {
    const field = this.addForm.get(fieldName);
    if (field?.errors && field.touched) {
      if (field.errors['required']) {
        return `${this.getFieldDisplayName(fieldName)} is required.`;
      }
      if (field.errors['accountNoExists']) {
        return 'This account number already exists.';
      }
    }

    // Check for form-level date range validation
    if (fieldName === 'validTo' && this.addForm.errors?.['dateRangeInvalid']) {
      return 'Valid To must be after Valid From.';
    }

    if (field?.errors && field.touched) {
      return Object.values(field.errors)
        .map(error => {
          if (typeof error === 'string') {
            return error;
          }
          return error.message || 'Invalid value';
        })
        .join(', ');
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
    const field = this.addForm.get(fieldName);
    return !!(field?.errors && field.touched);
  }
}
