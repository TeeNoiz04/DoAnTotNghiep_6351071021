import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { CommonModule } from '@angular/common';
import {
  AfterViewChecked,
  Component,
  ElementRef,
  EventEmitter,
  inject,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatChipInputEvent, MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MessageItemComponent } from '@app/shared/components/message-item';
import { TokenClaimsService } from '@app/shared/services/token-claims/token-claims.service';
import { NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { LookupService } from '@proxy/general-lookups';
import { UserLookupDto } from '@proxy/shared';
import { Observable, of, Subject } from 'rxjs';
import { finalize, map, takeUntil } from 'rxjs/operators';
import { DPOService } from '../../../proxy/dpos/dpo.service';
import { GetDPOMessagesInput } from '../../../proxy/dpos/dpomessages/models';
import { MessageCreateDto, MessageDto } from '../../../proxy/messages/models';

@Component({
  selector: 'app-dpo-discussion-box',
  templateUrl: './dpo-discussion-box.component.html',
  styleUrls: ['./dpo-discussion-box.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatChipsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatTooltipModule,
    MatButtonModule,
    NgbTooltipModule,
    MessageItemComponent,
  ],
})
export class DpoDiscussionBoxComponent implements OnInit, OnChanges, OnDestroy, AfterViewChecked {
  @Input() isOpen = false;
  @Input() dpoId: string;
  @Input() dpoCode: string;
  @Output() closeDiscussion = new EventEmitter<void>();
  @Output() messagesCount = new EventEmitter<number>();
  @ViewChild('messagesContainer') messagesContainer: ElementRef;
  @ViewChild('emailInput') emailInput: ElementRef;

  messageForm: FormGroup;
  messages: MessageDto[] = [];
  isLoading = false;
  isLoadingMore = false;
  hasMoreMessages = true;
  currentPage = 0;
  messagesPerLoad = 20;
  emailChips: string[] = [];
  shouldScrollToBottom = false; // Flag to control auto-scroll
  shouldMaintainScrollPosition = false; // Flag to control scroll position maintenance
  scrollPositionData: { previousScrollHeight: number; previousScrollTop: number } | null = null;
  isLoadingMessages = false; // Flag to prevent concurrent API calls
  filteredUsers: UserLookupDto[] = [];
  showSuggestions = false;
  private inputDebounce?: any;
  // Chip input configuration
  readonly separatorKeysCodes = [ENTER, COMMA] as const;
  private readonly generateLookupService = inject(LookupService);
  private destroy$ = new Subject<void>();

  constructor(
    private fb: FormBuilder,
    private dpoService: DPOService,
    private tokenClaimsService: TokenClaimsService,
  ) {}

  ngOnInit(): void {
    this.initForm();
    // Load messages immediately if discussion box is open and dpoId is available
    if (this.isOpen && this.dpoId) {
      this.loadMessages();
    }
  }

  ngOnChanges(changes: any): void {
    // Auto-load messages when the discussion box opens (and avoid loading if already loading)
    if (this.dpoId && this.messages.length === 0 && !this.isLoadingMessages) {
      this.loadMessages();
    }
    if (changes.isOpen && changes.isOpen.currentValue) {
      this.loadMessages();
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  ngAfterViewChecked(): void {
    if (this.shouldScrollToBottom) {
      this.shouldScrollToBottom = false;
      this.scrollToBottomImmediate();
    }

    if (this.shouldMaintainScrollPosition && this.scrollPositionData) {
      this.shouldMaintainScrollPosition = false;
      this.maintainScrollPositionImmediate();
    }
  }

  private scrollToBottomImmediate(): void {
    if (this.messagesContainer?.nativeElement) {
      const element = this.messagesContainer.nativeElement;
      element.scrollTop = element.scrollHeight;
    }
  }

  private maintainScrollPositionImmediate(): void {
    if (this.messagesContainer?.nativeElement && this.scrollPositionData) {
      const element = this.messagesContainer.nativeElement;
      const { previousScrollHeight, previousScrollTop } = this.scrollPositionData;
      const currentScrollHeight = element.scrollHeight;
      const heightDifference = currentScrollHeight - previousScrollHeight;
      element.scrollTop = previousScrollTop + heightDifference;
      this.scrollPositionData = null; // Clear the data after using it
    }
  }

  initForm(): void {
    this.messageForm = this.fb.group({
      content: ['', [Validators.required, Validators.maxLength(500)]],
    });
  }

  loadMessages(refresh = false): void {
    if (!this.dpoId) return;

    // Prevent concurrent API calls
    if (this.isLoadingMessages) {
      return;
    }

    if (refresh) {
      this.currentPage = 0;
      this.hasMoreMessages = true;
      this.messages = []; // Clear existing messages on refresh
    }

    this.isLoading = refresh || this.currentPage === 0;
    this.isLoadingMore = !refresh && this.currentPage > 0;
    this.isLoadingMessages = true; // Set flag to prevent concurrent calls

    const input: GetDPOMessagesInput = {
      skipCount: this.currentPage * this.messagesPerLoad,
      maxResultCount: this.messagesPerLoad,
    };

    this.dpoService
      .getListMessages(this.dpoId, input)
      .pipe(
        takeUntil(this.destroy$),
        finalize(() => {
          this.isLoading = false;
          this.isLoadingMore = false;
          this.isLoadingMessages = false; // Reset flag when done
        }),
      )
      .subscribe({
        next: result => {
          if (refresh || this.currentPage === 0) {
            // Reverse to show oldest first (chronological order)
            this.messages = result.items;

            // Set flag to auto-scroll to bottom after view is checked
            this.shouldScrollToBottom = true;
          } else {
            // Load more (infinite scroll pagination) - prepend older messages to the top
            // Reverse the new items and prepend them
            this.messages = [...result.items, ...this.messages];
          }

          this.hasMoreMessages = this.messages.length < result.totalCount;
          this.currentPage++;

          this.messagesCount.emit(result.totalCount);
        },
        error: error => {
          console.error('Failed to load messages:', error);
        },
      });
  }

  loadMoreMessages(): void {
    if (!this.isLoadingMore && this.hasMoreMessages) {
      this.loadMessages();
    }
  }

  refreshMessages(): void {
    this.loadMessages(true);
  }

  onScroll(event: Event): void {
    const element = event.target as HTMLElement;
    if (element.scrollTop === 0 && this.hasMoreMessages && !this.isLoadingMore) {
      // Store scroll position data before loading more messages
      this.scrollPositionData = {
        previousScrollHeight: element.scrollHeight,
        previousScrollTop: element.scrollTop,
      };

      this.loadMoreMessages();

      // Set flag to maintain scroll position after view is checked
      this.shouldMaintainScrollPosition = true;
    }
  }

  // Chip methods
  addEmail(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();

    if (value) {
      const processedEmail = this.processEmail(value);

      if (processedEmail) {
        // Check for duplicates
        if (!this.emailChips.includes(processedEmail)) {
          this.emailChips.push(processedEmail);
        }
      }

      // Clear the input regardless
      if (event.chipInput) {
        event.chipInput.clear();
      }
    }
  }

  removeEmail(email: string): void {
    const index = this.emailChips.indexOf(email);
    if (index >= 0) {
      this.emailChips.splice(index, 1);
    }
  }

  private processEmail(email: string): string | null {
    const trimmedEmail = email.trim();
    if (!trimmedEmail) return null;

    // If it's already a valid email, return as is
    if (this.isValidEmail(trimmedEmail)) {
      return trimmedEmail;
    }

    // If it contains @ but is invalid, return null (invalid email)
    if (trimmedEmail.includes('@')) {
      return null;
    }

    // If it doesn't contain @, add @mevn.com.vn suffix
    const emailWithSuffix = `${trimmedEmail}@mevn.com.vn`;

    // Validate the email with suffix
    if (this.isValidEmail(emailWithSuffix)) {
      return emailWithSuffix;
    }

    return null;
  }

  private isValidEmail(email: string): boolean {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  }

  private processEmailInput(): void {
    // Get the current value from the email input field
    if (this.emailInput?.nativeElement) {
      const inputValue = this.emailInput.nativeElement.value?.trim();
      if (inputValue) {
        const processedEmail = this.processEmail(inputValue);

        if (processedEmail) {
          // Check for duplicates
          if (!this.emailChips.includes(processedEmail)) {
            this.emailChips.push(processedEmail);
          }
        }

        // Clear the input field
        this.emailInput.nativeElement.value = '';
      }
    }
  }

  sendMessage(): void {
    if (this.messageForm.invalid || !this.dpoId) return;

    // Auto-process any text in the email input field before sending
    this.processEmailInput();

    const content = this.messageForm.get('content')?.value;
    const sendToEmails = this.emailChips.length > 0 ? [...this.emailChips] : [];

    // Get user info from token claims service
    const userInfo = this.tokenClaimsService.getUserInfo();

    const messageInput: MessageCreateDto = {
      userName: userInfo?.userName || '',
      fullName: userInfo?.fullName || '',
      sendToEmails: sendToEmails,
      comment: content,
    };

    // Add optimistic message to UI
    const optimisticMessage: MessageDto & { isPending?: boolean } = {
      id: `temp-${Date.now()}`,
      userName: userInfo?.userName || 'You',
      fullName: userInfo?.fullName || 'You',
      sendTo: sendToEmails.join(', '),
      comment: content,
      creationTime: new Date().toISOString(),
      isDeleted: false,
      forceDelete: false,
      isPending: true,
    };

    this.messages.push(optimisticMessage as MessageDto);

    this.dpoService
      .sendMessage(this.dpoId, messageInput)
      .pipe(
        takeUntil(this.destroy$),
        finalize(() => {
          // Remove pending message
          const pendingIndex = this.messages.findIndex(m => (m as any).isPending);
          if (pendingIndex >= 0) {
            this.messages.splice(pendingIndex, 1);
          }
        }),
      )
      .subscribe({
        next: newMessage => {
          // Add the real message from server at the end
          this.messages.push(newMessage);
          this.messageForm.patchValue({ content: '' });
          this.messageForm.get('content')?.markAsPristine();
          this.emailChips = [];

          // Set flag to auto-scroll to bottom after view is checked
          this.shouldScrollToBottom = true;

          this.messagesCount.emit(this.messages.length - 1);
        },
        error: error => {
          console.error('Failed to send message:', error);
        },
      });
  }

  close(): void {
    this.closeDiscussion.emit();
  }

  trackByMessageId(index: number, message: MessageDto): string {
    return message.id;
  }

  onReplyToSender(senderEmail: string): void {
    if (senderEmail && !this.emailChips.includes(senderEmail)) {
      this.emailChips.push(senderEmail);
    }
  }

  searchRequesters(filterText: string): Observable<UserLookupDto[]> {
    if (filterText?.length < 2 || !filterText) {
      return of([]);
    }

    return this.generateLookupService.getListUserLookupByName(filterText).pipe(map(resp => resp || []));
  }

  onInputChange(value: string): void {
    clearTimeout(this.inputDebounce);

    if (!value || value.length < 2) {
      this.filteredUsers = [];
      this.showSuggestions = false;
      return;
    }

    this.inputDebounce = setTimeout(() => {
      this.isLoading = true;
      this.searchRequesters(value).subscribe(
        users => {
          this.filteredUsers = users;
          this.showSuggestions = true;
          this.isLoading = false;
        },
        () => {
          this.isLoading = false;
        },
      );
    }, 0);
  }

  selectUser(user: UserLookupDto): void {
    this.addEmail({ value: user.email, chipInput: null } as MatChipInputEvent);
    this.filteredUsers = [];
    this.showSuggestions = false;

    if (this.emailInput) {
      this.emailInput.nativeElement.value = '';
    }
  }

  onInputBlur(): void {
    setTimeout(() => {
      this.showSuggestions = false;
    }, 200); // Delay to allow click event on suggestion
  }
}
