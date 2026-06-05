import { CommonModule } from '@angular/common';
import { Component, EventEmitter, forwardRef, Input, OnInit, Output } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

export interface ThreeStateButtonOption {
  value: any;
  label: string;
  class?: string;
}

@Component({
  selector: 'app-three-state-button',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="btn-group three-state-button" role="group" [attr.aria-label]="ariaLabel">
      @for (option of displayOptions; track option.value) {
        <button
          type="button"
          class="btn"
          [class]="getButtonClass(option)"
          [class.active]="isActive(option)"
          (click)="selectOption(option)"
          [disabled]="disabled">
          {{ option.label }}
        </button>
      }
    </div>
  `,
  styleUrl: './three-state-button.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ThreeStateButtonComponent),
      multi: true,
    },
  ],
})
export class ThreeStateButtonComponent implements ControlValueAccessor, OnInit {
  @Input() options: ThreeStateButtonOption[] = [];
  @Input() twoStateMode = false;
  @Input() buttonClass = 'btn-outline-dark';
  @Input() activeButtonClass = 'btn-dark';
  @Input() disabled = false;
  @Input() ariaLabel = 'Three state button group';

  // Default options for common use cases
  @Input() defaultType: 'yes-no-both' | 'active-inactive-both' | 'custom' = 'yes-no-both';

  @Output() selectionChange = new EventEmitter<any>();

  private _value: any = null;
  private onChange = (value: any) => {};
  private onTouched = () => {};

  displayOptions: ThreeStateButtonOption[] = [];

  ngOnInit() {
    this.setupOptions();
  }

  private setupOptions() {
    if (this.options.length > 0) {
      this.displayOptions = [...this.options];
    } else {
      // Use smart defaults based on type
      switch (this.defaultType) {
        case 'yes-no-both':
          this.displayOptions = [
            { value: true, label: 'Yes' },
            { value: false, label: 'No' },
            { value: null, label: 'Both' },
          ];
          break;
        case 'active-inactive-both':
          this.displayOptions = [
            { value: true, label: 'Active' },
            { value: false, label: 'Inactive' },
            { value: null, label: 'Both' },
          ];
          break;
        default:
          this.displayOptions = [
            { value: 'first', label: 'First' },
            { value: 'second', label: 'Second' },
            { value: null, label: 'Both' },
          ];
      }
    }

    // In two-button mode, only show first two options, but keep 3 states
    if (this.twoStateMode && this.displayOptions.length >= 2) {
      this.displayOptions = this.displayOptions.slice(0, 2);
    }
  }

  selectOption(option: ThreeStateButtonOption) {
    if (this.disabled) return;

    if (this.twoStateMode) {
      this.handleTwoStateSelection(option);
    } else {
      this.handleThreeStateSelection(option);
    }
  }

  private handleThreeStateSelection(option: ThreeStateButtonOption) {
    this._value = option.value;
    this.onChange(this._value);
    this.onTouched();
    this.selectionChange.emit(this._value);
  }

  private handleTwoStateSelection(option: ThreeStateButtonOption) {
    // In two-button mode: cycle through states regardless of which button is clicked
    // Cycle: first → second → both → first → ...
    const firstOption = this.displayOptions[0];
    const secondOption = this.displayOptions[1];

    // Cycle through states regardless of which button is clicked
    if (this._value === firstOption.value) {
      // Currently first, go to second
      this._value = secondOption.value;
    } else if (this._value === secondOption.value) {
      // Currently second, go to both
      this._value = null;
    } else {
      // Currently both (or initial state), go to first
      this._value = firstOption.value;
    }

    this.onChange(this._value);
    this.onTouched();
    this.selectionChange.emit(this._value);
  }

  isActive(option: ThreeStateButtonOption): boolean {
    if (this.twoStateMode) {
      // In two-button mode: both buttons active when value is null, otherwise only the matching one
      return this._value === null || this._value === option.value;
    } else {
      // Three-button mode: only the exact match is active
      return this._value === option.value;
    }
  }

  getButtonClass(option: ThreeStateButtonOption): string {
    const baseClass = option.class || this.buttonClass;
    const activeClass = this.activeButtonClass;

    if (this.isActive(option)) {
      return `${baseClass.replace('btn-outline-', 'btn-')} ${activeClass}`;
    }

    return baseClass;
  }

  // ControlValueAccessor implementation
  writeValue(value: any): void {
    this._value = value;
  }

  registerOnChange(fn: (value: any) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  get value(): any {
    return this._value;
  }
}
