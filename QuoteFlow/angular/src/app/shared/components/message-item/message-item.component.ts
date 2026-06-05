import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, inject } from '@angular/core';
import { TokenClaimsService } from '@app/shared/services/token-claims/token-claims.service';
import { MessageDto } from '../../../proxy/messages/models';

@Component({
  selector: 'app-message-item',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div
      class="message"
      [class.message-outgoing]="isCurrentUserMessage"
      [class.message-incoming]="!isCurrentUserMessage">
      <div class="message-header">
        <span class="sender">{{ messageSender }}</span>
        <span class="timestamp">{{ message.creationTime | date: 'short' }}</span>
        <button
          *ngIf="!isCurrentUserMessage && showReplyButton"
          type="button"
          class="btn btn-sm btn-link reply-button"
          (click)="onReplyToSender()">
          <i class="bi bi-reply-fill"></i>
        </button>
      </div>
      <div class="message-content">
        {{ message.comment }}
        <span *ngIf="isPending" class="message-pending">
          <span class="spinner-border spinner-border-sm" role="status"></span>
        </span>
      </div>
      <div *ngIf="message.sendTo" class="message-recipients">
        <div class="recipients-header" (click)="toggleRecipientsExpansion()">
          <small class="text-muted recipients-text">
            <span class="recipients-to">To: </span>
            <span class="recipients-content" [attr.data-expanded]="isRecipientsExpanded">
              {{ message.sendTo }}
            </span>
          </small>
          <i
            class="bi toggle-icon"
            [class.bi-chevron-down]="!isRecipientsExpanded"
            [class.bi-chevron-up]="isRecipientsExpanded"></i>
        </div>
      </div>
    </div>
  `,
  styleUrls: ['./message-item.component.scss'],
})
export class MessageItemComponent {
  @Input() message!: MessageDto;
  @Input() showReplyButton = true;
  @Output() replyToSender = new EventEmitter<string>();

  isRecipientsExpanded = false;

  private readonly tokenClaimsService = inject(TokenClaimsService);

  get isCurrentUserMessage(): boolean {
    const currentUser = this.tokenClaimsService.getUserInfo();
    return this.message.userName === currentUser.userName;
  }

  get messageSender(): string {
    return this.message.fullName || this.message.userName || 'Unknown';
  }

  get isPending(): boolean {
    return (this.message as any).isPending;
  }

  toggleRecipientsExpansion(): void {
    this.isRecipientsExpanded = !this.isRecipientsExpanded;
  }

  onReplyToSender(): void {
    if (!this.isCurrentUserMessage) {
      // Extract sender's email or username
      const senderEmail = this.extractSenderEmail();
      if (senderEmail) {
        this.replyToSender.emit(senderEmail);
      }
    }
  }

  private extractSenderEmail(): string {
    // If the message has userName in email format, use it
    if (this.message.userName && this.message.userName.includes('@')) {
      return this.message.userName;
    }

    // Otherwise, try to construct email with @mevn.com.vn suffix
    if (this.message.userName) {
      return `${this.message.userName}@mevn.com.vn`;
    }

    return '';
  }
}
