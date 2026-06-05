import { CommonModule } from '@angular/common';
import { Component, ElementRef, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { DragDropDirective } from '@app/shared/directives/drag-drop.directive';
import { FileIconPipe } from '@app/shared/pipes/file-icon.pipe';
import { FileSizePipe } from '@app/shared/pipes/file-size.pipe';

@Component({
  selector: 'app-upload-control',
  standalone: true,
  templateUrl: './upload-control.component.html',
  styleUrls: ['./upload-control.component.scss'],
  imports: [CommonModule, DragDropDirective, FileSizePipe, FileIconPipe],
})
export class UploadControlComponent {
  @ViewChild('fileInput') fileInput: ElementRef<HTMLInputElement>;
  @Input() multiple: boolean = false;
  @Input() accept: string = '';
  @Output() filesUploaded = new EventEmitter<File[]>();
  files: File[] = [];
  isHovered = false;
  isDragging = false;

  onFilesDropped(files: FileList): void {
    this.isDragging = false;
    const acceptedFiles: File[] = [];
    if (this.multiple) {
      for (let i = 0; i < files.length; i++) {
        if (this.isFileAccepted(files.item(i))) {
          acceptedFiles.push(files.item(i));
        }
      }
    } else {
      if (this.isFileAccepted(files.item(0))) {
        acceptedFiles.push(files.item(0));
      }
    }
    this.files = acceptedFiles;
    this.filesUploaded.emit(this.files);
    this.clearFileInput();
  }

  onFilesHovered(isHovered: boolean): void {
    this.isHovered = isHovered;
    this.isDragging = isHovered;
  }

  onFileInputClick(): void {
    if (this.fileInput && this.fileInput.nativeElement) {
      this.fileInput.nativeElement.click();
    } else {
      console.error('File input element not found');
    }
  }

  onFilesSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files) {
      this.onFilesDropped(input.files);
    }
  }

  removeFile(file: File): void {
    this.files = this.files.filter(f => f !== file);
    this.filesUploaded.emit(this.files);
  }

  private isFileAccepted(file: File): boolean {
    if (!this.accept) {
      return true;
    }
    const acceptedTypes = this.accept.split(',').map(type => type.trim());
    return acceptedTypes.some(type => file.type === type || file.name.endsWith(type));
  }

  private clearFileInput(): void {
    if (this.fileInput && this.fileInput.nativeElement) {
      this.fileInput.nativeElement.value = '';
    }
  }
}
