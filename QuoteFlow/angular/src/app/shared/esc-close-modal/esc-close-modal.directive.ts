import { Directive, HostListener, Input, Output, EventEmitter } from '@angular/core';

@Directive({
  selector: '[appEscCloseModal]',
})
export class EscCloseModalDirective {
  @Input() visible = false;
  @Output() visibleChange = new EventEmitter<boolean>();

  @HostListener('document:keydown.escape', ['$event'])
  onEscPressed(event: KeyboardEvent) {
    if (this.visible) {
      this.visibleChange.emit(false);
    }
  }
}
