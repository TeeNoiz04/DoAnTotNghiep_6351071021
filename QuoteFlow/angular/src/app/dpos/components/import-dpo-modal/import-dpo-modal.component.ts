import { CoreModule } from '@abp/ng.core';
import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { LookupService } from '@app/proxy/general-lookups';
import type { LookupDto } from '@app/proxy/shared/models';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';

interface ImportDPOInformation {
  file: File | null;
  materialTypeId: string;
  buyerTypeId: string;
  buyerId: string;
  confirmDate: Date | null;
  note: string;
  securityExportControlChecked: boolean;
}

@Component({
  selector: 'app-import-dpo-modal',
  standalone: true,
  templateUrl: './import-dpo-modal.component.html',
  styleUrls: ['./import-dpo-modal.component.scss'],
  providers: [LookupService, LoadingService],
  imports: [FormsModule, ReactiveFormsModule, NgbModule, NgSelectModule, MatCheckboxModule, CoreModule],
})
export class ImportDPOModalComponent implements OnInit {
  protected readonly lookupService = inject(LookupService);
  protected readonly loadingService = inject(LoadingService);
  protected readonly fb = inject(FormBuilder);

  @ViewChild('fileInput') fileInput: any;
  importForm: FormGroup = new FormGroup({});
  public fileImport: File | null = null;

  // Dropdown options
  materialTypeOptions: LookupDto<string>[] = [];
  buyerTypeOptions: LookupDto<string>[] = [];
  buyerOptions: LookupDto<string>[] = [];

  // Pre-selection configuration
  private enablePreSelection = true;
  private preSelectionCompleted = false;

  ngOnInit(): void {
    this.initializeForm();
    this.loadDropdownData();
  }

  get f() {
    return this.importForm.controls;
  }

  private initializeForm(): void {
    this.importForm = this.fb.group({
      file: [null, [Validators.required]],
      materialType: ['', [Validators.required]],
      buyerTypeId: ['', [Validators.required]],
      buyerId: ['none', [this.validateBuyerSelection]],
      confirmDate: [null, [Validators.required]],
      note: [''],
      securityExportControlChecked: [false, [Validators.requiredTrue]],
    });
  }

  private async loadDropdownData(): Promise<void> {
    try {
      this.loadingService.show();

      // Load material types
      const materialTypes = await this.lookupService.getMaterialTypeLookup().toPromise();
      this.materialTypeOptions = materialTypes?.items || [];

      // Load buyer types
      const buyerTypes = await this.lookupService.getBuyerTypeLookup({}).toPromise();
      this.buyerTypeOptions = buyerTypes?.items || [];

      // Initialize buyers with default 'none' option only
      this.initializeBuyerOptions();

      // After loading all options, trigger pre-selection if enabled
      if (this.enablePreSelection && !this.preSelectionCompleted) {
        // Use setTimeout to ensure options are fully loaded before pre-selection
        setTimeout(() => {
          this.performPreSelection();
        }, 100);
      }
    } catch (error) {
      console.error('Error loading dropdown data:', error);
    } finally {
      this.loadingService.hide();
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
      this.importForm.patchValue({ materialType: firstMaterialType.displayCode });
      this.onMaterialTypeChange(firstMaterialType);

      // Proceed to next selection
      setTimeout(() => {
        this.preSelectBuyerType();
      }, 50);
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
      this.importForm.patchValue({ buyerTypeId: firstBuyerType.id });
      this.onBuyerTypeChange(firstBuyerType);

      // Proceed to next selection
      setTimeout(() => {
        this.preSelectBuyer();
      }, 50);
    } else {
      this.preSelectionCompleted = true;
    }
  }

  /**
   * Pre-select the first available buyer (skip 'none' option)
   */
  private preSelectBuyer(): void {
    // Find the first real buyer (skip the 'none' option at index 0)
    const realBuyers = this.buyerOptions;
    if (realBuyers.length > 0) {
      const firstBuyer = realBuyers[0];
      this.importForm.patchValue({ buyerId: firstBuyer.id });
      this.onBuyerChange(firstBuyer);
    }

    // Mark pre-selection as completed
    this.preSelectionCompleted = true;
  }

  onFileChange(event: any): void {
    const file = event.target.files?.[0];
    if (file) {
      this.fileImport = file;
      this.importForm.patchValue({ file });
    }
  }

  onMaterialTypeChange(event: LookupDto<string>): void {
    // Material type change handling - updates are handled by reactive streams
  }

  onBuyerTypeChange(event: LookupDto<string>): void {
    const buyerTypeId = event?.id;

    // Clear current buyer selection
    this.importForm.patchValue({ buyerId: 'none' });

    if (buyerTypeId) {
      // Load buyers filtered by buyer type
      this.loadBuyersByType(buyerTypeId);
    } else {
      // Reset to default 'none' option only
      this.initializeBuyerOptions();
    }
  }

  onBuyerChange(event: LookupDto<string>): void {
    const buyerId = event?.id;

    if (buyerId && buyerId !== 'none') {
      // Update form with selected buyer
    } else {
      // Handle 'none' selection or clearing
    }
  }

  onConfirmDateChange(date: any): void {}

  onNoteChange(note: string): void {}

  validateForm(): boolean {
    if (this.importForm.valid) {
      return true;
    } else {
      // Mark all fields as touched to show validation errors
      Object.keys(this.importForm.controls).forEach(key => {
        this.importForm.get(key)?.markAsTouched();
      });
      return false;
    }
  }

  resetPreSelection(): void {
    this.preSelectionCompleted = false;
    this.importForm.reset();
    this.fileImport = null;
    // Re-trigger pre-selection if enabled
    if (this.enablePreSelection) {
      setTimeout(() => {
        this.performPreSelection();
      }, 100);
    }
  }

  /**
   * Enable or disable pre-selection feature
   */
  public setPreSelectionEnabled(enabled: boolean): void {
    this.enablePreSelection = enabled;
  }

  /**
   * Custom validator for buyer selection
   */
  private validateBuyerSelection = (control: any) => {
    const value = control.value;
    if (!value || value === 'none') {
      return { buyerRequired: true };
    }
    return null;
  };

  /**
   * Initialize buyer options with default 'none' option
   */
  private initializeBuyerOptions(): void {
    this.buyerOptions = [
      {
        id: 'none',
        displayCode: '-- Please select buyer --',
        displayName: '-- Please select buyer --',
      } as LookupDto<string>,
    ];
  }

  /**
   * Load buyers filtered by buyer type
   */
  private async loadBuyersByType(buyerTypeId: string): Promise<void> {
    try {
      this.loadingService.show();

      // Load buyers filtered by buyer type
      const buyers = await this.lookupService.getBuyerLookupByBuyerType(buyerTypeId).toPromise();
      const buyerItems = buyers?.items || [];

      // Add default "none" option at the beginning
      this.buyerOptions = [
        {
          id: 'none',
          displayCode: '-- Please select buyer --',
          displayName: '-- Please select buyer --',
        } as LookupDto<string>,
        ...buyerItems,
      ];
    } catch (error) {
      console.error('Error loading buyers by type:', error);
      // Fallback to default option only
      this.initializeBuyerOptions();
    } finally {
      this.loadingService.hide();
    }
  }

  resetFile() {
    if (this.fileInput) {
      this.fileInput.nativeElement.value = '';
      this.fileImport = null;
      this.importForm.patchValue({ file: null });
    }
  }
}
