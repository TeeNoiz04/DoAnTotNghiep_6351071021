import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

export interface ConfirmationNoteResult {
  confirmed: boolean;
  note?: string;
}

@Component({
  selector: 'app-confirmation-note-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, ThemeSharedModule],
  templateUrl: './confirmation-note-modal.component.html',
  styleUrls: ['./confirmation-note-modal.component.scss'],
})
export class ConfirmationNoteModalComponent {
  @Input() visible: boolean = false;
  @Input() modalTitle: string = 'Confirmation';
  @Input() requiredNote: boolean = false;
  @Input() submitLabel: string = 'Submit';
  @Input() cancelLabel: string = 'Cancel';
  @Input() noteLabel: string = 'Note';
  @Input() notePlaceholder: string = 'Enter your note here...';
  @Input() noteRows: number = 4;

  @Output() visibleChange = new EventEmitter<boolean>();
  @Output() modalResult = new EventEmitter<ConfirmationNoteResult>();

  note: string = '';

  get isValid(): boolean {
    if (this.requiredNote) {
      return !!(this.note && this.note.trim().length > 0);
    }
    return true;
  }

  onSubmit(): void {
    if (!this.isValid) {
      return;
    }

    const result = {
      confirmed: true,
      note: this.note?.trim() || undefined,
    };

    this.modalResult.emit(result);
    this.closeModal();
  }

  onCancel(): void {
    const result = {
      confirmed: false,
    };

    this.modalResult.emit(result);
    this.closeModal();
  }

  closeModal(): void {
    this.note = ''; // Reset note when closing
    this.visible = false;
    this.visibleChange.emit(false);
  }
}
