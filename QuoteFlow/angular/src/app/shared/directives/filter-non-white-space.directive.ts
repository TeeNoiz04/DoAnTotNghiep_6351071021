import { Directive, ElementRef, HostListener } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
  selector: '[appFilterNonWhiteSpace]',
  standalone: true,
})
export class FilterNonWhiteSpaceDirective {
  constructor(
    private el: ElementRef,
    private control: NgControl,
  ) {}
  @HostListener('blur', ['$event'])
  onBlur(event: any) {
    const value = this.el.nativeElement.value;
    if (value) {
      const trimmedValue = value.trim();

      // Update the input element value
      this.el.nativeElement.value = trimmedValue;

      // If the input is part of a form control, update the model
      if (this.control && this.control.control) {
        this.control.control.setValue(trimmedValue, { emitEvent: false });
      }
    }
  }
}
