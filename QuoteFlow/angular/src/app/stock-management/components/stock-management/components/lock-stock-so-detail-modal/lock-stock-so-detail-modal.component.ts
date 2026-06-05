import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  AppAdvancedDataTableComponent,
  AppTableColumnDirective,
  AppTableColumnGroupDirective,
} from '@app/shared/components/advanced-data-table';
import { NgSelectModule } from '@ng-select/ng-select';
import { catchError, EMPTY, finalize, tap } from 'rxjs';
import { StockManagementListDto, StockManagementService } from '@proxy/stock-managements';
import { UsernamePipe } from '@app/shared/pipes/username.pipe';
import { ExpandablePanelComponent } from '@app/shared/components/expandable-panel/expandable-panel.component';
import { EscCloseModalDirective } from '@app/shared/esc-close-modal/esc-close-modal.directive';

@Component({
  selector: 'app-lock-stock-so-detail-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ThemeSharedModule,
    NgSelectModule,
    AppAdvancedDataTableComponent,
    AppTableColumnDirective,
    AppTableColumnGroupDirective,
    ExpandablePanelComponent,
    EscCloseModalDirective,
  ],
  templateUrl: './lock-stock-so-detail-modal.component.html',
  styleUrls: ['./lock-stock-so-detail-modal.component.scss'],
})
export class LockStockSODetailModalComponent implements OnInit {
  private fb = inject(FormBuilder);
  private readonly service = inject(StockManagementService);
  private readonly usernamePipe = new UsernamePipe();

  @Input() visible: boolean = false;
  @Input() materialItem: StockManagementListDto = undefined;

  @Output() visibleChange = new EventEmitter<boolean>();

  basicInfoForm: FormGroup;
  loading = false;
  lockStockSOData: any = [];
  tableLoading = false;

  ngOnInit(): void {
    if (this.visible) {
      this.buildForm();
      this.loadLockStockSOData();
      this.disableAllForms();
    }
  }

  onCancel(): void {
    this.closeModal();
  }

  closeModal(): void {
    this.visible = false;
    this.visibleChange.emit(false);
  }

  buildForm() {
    this.basicInfoForm = this.fb.group({
      golfaCode: [this.materialItem?.golfaCode, []],
      sapCode: [this.materialItem?.sap_Code, []],
      model: [this.materialItem?.model, []],
      materialGroup: [this.materialItem?.material_Group, []],
    });
  }

  disableAllForms(disable: boolean = true): void {
    const disableFormGroup = (form: FormGroup) => {
      if (!form) return;

      Object.keys(form.controls).forEach(controlName => {
        const control = form.get(controlName);
        if (disable) {
          control.disable();
        } else {
          control.enable();
        }
      });
    };

    disableFormGroup(this.basicInfoForm);
  }

  private loadLockStockSOData(): void {
    if (!this.materialItem) {
      return;
    }
    this.tableLoading = true;
    this.service
      .getStockOfSO(this.materialItem?.golfaCode, this.materialItem?.stockCategoryId)
      .pipe(
        tap(result => {
          this.lockStockSOData = result || [];
        }),
        catchError(error => {
          console.error('Error loading lock stock data:', error);
          this.lockStockSOData = [];
          return EMPTY;
        }),
        finalize(() => {
          this.tableLoading = false;
        }),
      )
      .subscribe();
  }

  // Formatters for table columns
  formatQuantity = (val: number): string => {
    return val ? new Intl.NumberFormat('en-US').format(val) : '0';
  };

  formatDate = (val: string): string => {
    return val ? new Date(val).toLocaleDateString('en-GB') : '';
  };

  formatModifiedDate = (val: string): string => {
    return val
      ? new Date(val).toLocaleDateString('en-GB') +
          ' ' +
          new Date(val).toLocaleTimeString('en-GB', { hour: '2-digit', minute: '2-digit' })
      : '';
  };

  formatUsername = (val: string): string => {
    return val ? this.usernamePipe.transform(val) : '';
  };

  onClose(): void {
    this.closeModal();
  }
}
