import { Component, forwardRef, Input } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgxMaskDirective, provideNgxMask } from 'ngx-mask';

@Component({
  selector: 'app-input-number',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, NgxMaskDirective],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputNumberComponent),
      multi: true,
    },
    provideNgxMask(),
  ],
  template: `
    <input
      type="text"
      [class]="'form-control text-end ' + valueClass"
      [class.is-invalid]="isInvalid"
      [class.is-valid]="isValid"
      [class.check-view]="readOnly"
      [readOnly]="readOnly"
      [attr.id]="id"
      [attr.placeholder]="placeholder"
      [mask]="getMaskPattern()"
      [thousandSeparator]="thousandSeparator"
      [decimalMarker]="decimalMarker"
      [allowNegativeNumbers]="allowNegative"
      [dropSpecialCharacters]="false"
      [(ngModel)]="value"
      (ngModelChange)="onInput($event)"
      [separatorLimit]="maxValue"
      (blur)="onTouched()"
      autocomplete="off" />
  `,
})
export class InputNumberComponent implements ControlValueAccessor {
  @Input() id?: string;
  @Input() placeholder = '';
  @Input() readOnly = false;
  @Input() allowDecimal = true;
  @Input() allowNegative = false;
  @Input() thousandSeparator = ',';
  @Input() decimalMarker: ',' | '.' | ['.', ','] = '.';
  @Input() isInvalid = false;
  @Input() isValid = false;
  @Input() maxValue = '9999999999999999';
  @Input() valueClass = '';

  private initializing = false;
  private decimalPlaces = 2;
  private _value: number | null = null;

  get value(): number | null {
    return this._value;
  }

  set value(val: number | null) {
    this._value = val;
  }

  onChange: any = () => {};
  onTouched: any = () => {};

  getMaskPattern(): string {
    return this.allowDecimal ? `separator.${this.decimalPlaces}` : 'separator.0';
  }

  writeValue(value: any): void {
    this.initializing = true;
    if (value !== undefined) {
      this._value = value;
    }
    setTimeout(() => (this.initializing = false)); // next tick
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.readOnly = isDisabled;
  }

  onInput(val: any) {
    if (this.initializing) return;
    this._value = val;
    this.onChange(val); // only user typing triggers onChange
  }
}
