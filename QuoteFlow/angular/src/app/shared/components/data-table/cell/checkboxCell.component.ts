import { Component, Input, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-checkbox-cell',
  templateUrl: './checkboxCell.component.html',
  standalone: true,
  imports: [CommonModule, FormsModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CheckboxCellComponent),
      multi: true,
    },
  ],
})
export class CheckboxCellComponent implements ControlValueAccessor {
  @Input() value: boolean;
  @Input() entry: any;
  @Input() name: string;
  @Input() disabled: boolean = false;
  @Input() cssClass: string;

  private onChange = (_: any) => {};
  private onTouched = () => {};

  writeValue(value: any): void {
    this.value = value;
  }
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onModelChange(event: any) {
    this.value = event.target.checked;
    this.onChange(this.value);
    this.onTouched();
  }
}
