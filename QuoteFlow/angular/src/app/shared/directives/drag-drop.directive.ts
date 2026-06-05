import { Directive, EventEmitter, HostListener, Output } from '@angular/core';

@Directive({
  selector: '[appDragDrop]',
  standalone: true,
})
export class DragDropDirective {
  @Output() filesDropped = new EventEmitter<FileList>();
  @Output() filesHovered = new EventEmitter<boolean>();
  @Output() fileInputClick = new EventEmitter<void>();

  @HostListener('dragover', ['$event'])
  onDragOver(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.filesHovered.emit(true);
  }

  @HostListener('dragleave', ['$event'])
  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.filesHovered.emit(false);
  }

  @HostListener('drop', ['$event'])
  onDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.filesHovered.emit(false);
    if (event.dataTransfer && event.dataTransfer.files.length > 0) {
      this.filesDropped.emit(event.dataTransfer.files);
    }
  }

  @HostListener('click', ['$event'])
  onClick(event: MouseEvent): void {
    this.fileInputClick.emit();
  }
}
