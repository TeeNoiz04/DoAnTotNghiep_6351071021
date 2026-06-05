import { CommonModule } from '@angular/common';
import {
  AfterViewInit,
  Component,
  ElementRef,
  EventEmitter,
  HostListener,
  Input,
  OnDestroy,
  Output,
  ViewChild,
  inject,
  ChangeDetectorRef,
  NgZone,
} from '@angular/core';
import { RequestStatusEnum } from '@app/shared/status/components/status-label.component';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { animate, style, transition, trigger } from '@angular/animations';

export interface StatusTab {
  id: string;
  label: string;
  status: RequestStatusEnum | null;
  count: number;
  icon?: string;
}

@Component({
  selector: 'app-status-nav-bar',
  standalone: true,
  imports: [CommonModule, NgbNavModule],
  animations: [
    trigger('tabAnimation', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(-5px)' }),
        animate('200ms ease-out', style({ opacity: 1, transform: 'translateY(0)' })),
      ]),
    ]),
    trigger('countAnimation', [
      transition(':increment', [
        style({ transform: 'scale(1.3)', color: '#dc3545' }),
        animate('300ms ease-out', style({ transform: 'scale(1)', color: '*' })),
      ]),
    ]),
    trigger('fadeInOut', [
      transition(':enter', [style({ opacity: 0 }), animate('200ms ease-out', style({ opacity: 1 }))]),
      transition(':leave', [animate('200ms ease-in', style({ opacity: 0 }))]),
    ]),
  ],
  template: `
    <div class="tabs-nav-container">
      <button
        *ngIf="canScrollPrev"
        class="scroll-nav-button prev-button"
        (click)="scrollPrev()"
        [@fadeInOut]
        aria-label="Scroll previous">
        <i class="bi bi-chevron-left"></i>
      </button>

      <div class="bordered-tabs-container" #scrollContainer (scroll)="onScroll()">
        <ul ngbNav #nav="ngbNav" [activeId]="activeTab" class="bordered-tabs" (activeIdChange)="onTabChange($event)">
          <li [ngbNavItem]="tab.id" *ngFor="let tab of tabs" [class.active]="activeTab === tab.id">
            <button ngbNavLink class="bordered-tab-link" [attr.data-status]="tab.id">
              <div class="tab-content-wrapper">
                <div class="icon-wrapper" [ngClass]="getStatusClass(tab.status)">
                  <i [ngClass]="getStatusIcon(tab.status, tab.icon)"></i>
                </div>
                <span class="tab-label">{{ tab.label }}</span>
                <span
                  *ngIf="tab.count > 0 && tab.id !== 'all'"
                  class="count-badge"
                  [ngClass]="getCountClass(tab.status, tab.count)"
                  [@countAnimation]="tab.count">
                  {{ tab.count > 999 ? '999+' : tab.count }}
                </span>
              </div>
            </button>
          </li>
        </ul>
      </div>

      <button
        *ngIf="canScrollNext"
        class="scroll-nav-button next-button"
        (click)="scrollNext()"
        [@fadeInOut]
        aria-label="Scroll next">
        <i class="bi bi-chevron-right"></i>
      </button>
    </div>
  `,
  styleUrls: ['./status-nav-bar.component.scss'],
})
export class StatusNavBarComponent implements AfterViewInit, OnDestroy {
  @Input() tabs: StatusTab[] = [];
  @Input() activeTab: string = 'all';
  @Output() tabChange = new EventEmitter<string>();
  @ViewChild('scrollContainer') scrollContainer!: ElementRef<HTMLDivElement>;

  RequestStatusEnum = RequestStatusEnum;

  canScrollPrev = false;
  canScrollNext = false;
  private scrollAmount = 300;
  private resizeObserver: ResizeObserver | null = null;
  private scrollAnimationId: number | null = null;

  private cdr = inject(ChangeDetectorRef);
  private ngZone = inject(NgZone);
  private elementRef = inject(ElementRef);

  ngAfterViewInit(): void {
    setTimeout(() => {
      this.checkScrollability();
      this.scrollToActiveTab();
    }, 0);
    this.resizeObserver = new ResizeObserver(() => {
      this.checkScrollability();
    });

    if (this.scrollContainer?.nativeElement) {
      this.resizeObserver.observe(this.scrollContainer.nativeElement);
    }
  }

  ngOnDestroy(): void {
    if (this.resizeObserver) {
      this.resizeObserver.disconnect();
      this.resizeObserver = null;
    }
    if (this.scrollAnimationId) {
      cancelAnimationFrame(this.scrollAnimationId);
      this.scrollAnimationId = null;
    }
  }

  onTabChange(tabId: string) {
    this.tabChange.emit(tabId);
    setTimeout(() => {
      this.scrollToActiveTab();
    }, 0);
  }

  @HostListener('window:resize')
  onResize(): void {
    this.checkScrollability();
  }

  onScroll(): void {
    this.checkScrollability();
  }

  scrollPrev(): void {
    if (!this.scrollContainer) return;

    const container = this.scrollContainer.nativeElement;
    const scrollLeft = Math.max(0, container.scrollLeft - this.scrollAmount);

    this.smoothScrollTo(scrollLeft);
  }

  scrollNext(): void {
    if (!this.scrollContainer) return;

    const container = this.scrollContainer.nativeElement;
    const maxScrollLeft = container.scrollWidth - container.clientWidth;
    const scrollLeft = Math.min(maxScrollLeft, container.scrollLeft + this.scrollAmount);

    this.smoothScrollTo(scrollLeft);
  }

  scrollToActiveTab(): void {
    if (!this.scrollContainer || !this.activeTab) return;

    const container = this.scrollContainer.nativeElement;
    const activeTabElement = this.elementRef.nativeElement.querySelector(`[data-status="${this.activeTab}"]`);

    if (!activeTabElement) return;
    const tabRect = activeTabElement.getBoundingClientRect();
    const containerRect = container.getBoundingClientRect();
    const tabOffsetLeft = activeTabElement.offsetLeft;
    const tabWidth = tabRect.width;
    const containerWidth = containerRect.width;
    const scrollLeft = tabOffsetLeft - containerWidth / 2 + tabWidth / 2;

    this.smoothScrollTo(scrollLeft);
  }

  private checkScrollability(): void {
    if (!this.scrollContainer) return;

    const container = this.scrollContainer.nativeElement;
    const newCanScrollPrev = container.scrollLeft > 0;
    const newCanScrollNext = container.scrollLeft < container.scrollWidth - container.clientWidth - 2;
    if (this.canScrollPrev !== newCanScrollPrev || this.canScrollNext !== newCanScrollNext) {
      this.canScrollPrev = newCanScrollPrev;
      this.canScrollNext = newCanScrollNext;
      this.cdr.detectChanges();
    }
  }

  private smoothScrollTo(scrollLeft: number): void {
    if (!this.scrollContainer) return;

    const container = this.scrollContainer.nativeElement;
    try {
      container.scrollTo({
        left: scrollLeft,
        behavior: 'smooth',
      });
      setTimeout(() => {
        this.checkScrollability();
      }, 300);
    } catch (e) {
      this.animateScroll(container.scrollLeft, scrollLeft);
    }
  }

  private animateScroll(fromPosition: number, toPosition: number): void {
    if (this.scrollAnimationId) {
      cancelAnimationFrame(this.scrollAnimationId);
    }

    const container = this.scrollContainer.nativeElement;
    const distance = toPosition - fromPosition;
    const duration = 300;
    const startTime = performance.now();

    const doScroll = (currentTime: number) => {
      const elapsedTime = currentTime - startTime;
      const progress = Math.min(elapsedTime / duration, 1);
      const easeProgress = this.easeInOutQuad(progress);
      const scrollPos = fromPosition + distance * easeProgress;

      container.scrollLeft = scrollPos;

      if (progress < 1) {
        this.scrollAnimationId = requestAnimationFrame(doScroll);
      } else {
        this.scrollAnimationId = null;
        this.checkScrollability();
      }
    };

    this.ngZone.runOutsideAngular(() => {
      this.scrollAnimationId = requestAnimationFrame(doScroll);
    });
  }

  private easeInOutQuad(t: number): number {
    return t < 0.5 ? 2 * t * t : 1 - Math.pow(-2 * t + 2, 2) / 2;
  }

  getStatusClass(status: RequestStatusEnum | null): string {
    if (!status) return 'status-all';

    switch (status) {
      case RequestStatusEnum.SUBMITTED:
        return 'status-submitted';
      case RequestStatusEnum.IN_PROGRESS:
        return 'status-inprogress';
      case RequestStatusEnum.APPROVED:
        return 'status-approved';
      case RequestStatusEnum.CONFIRMED:
        return 'status-confirmed';
      case RequestStatusEnum.CLOSED:
        return 'status-closed';
      case RequestStatusEnum.REJECTED:
        return 'status-rejected';
      case RequestStatusEnum.CANCELLED:
        return 'status-cancelled';
      case RequestStatusEnum.LOCKED:
        return 'status-locked';
      case RequestStatusEnum.LOCKED_STOCK:
        return 'status-locked-stock';
      default:
        return '';
    }
  }

  getStatusIcon(status: RequestStatusEnum | null, customIcon?: string): string {
    if (customIcon) return customIcon;

    if (!status) return 'bi bi-list-ul';

    switch (status) {
      case RequestStatusEnum.SUBMITTED:
        return 'bi bi-file-earmark-arrow-up';
      case RequestStatusEnum.IN_PROGRESS:
        return 'bi bi-hourglass-split';
      case RequestStatusEnum.APPROVED:
        return 'bi bi-check-circle';
      case RequestStatusEnum.CONFIRMED:
        return 'bi bi-shield-check';
      case RequestStatusEnum.CLOSED:
        return 'bi bi-folder-check';
      case RequestStatusEnum.REJECTED:
        return 'bi bi-x-circle';
      case RequestStatusEnum.CANCELLED:
        return 'bi bi-slash-circle';
      case RequestStatusEnum.LOCKED:
        return 'bi bi-lock';
      case RequestStatusEnum.LOCKED_STOCK:
        return 'bi bi-lock-fill';
      default:
        return 'bi bi-circle';
    }
  }

  getCountClass(status: RequestStatusEnum | null, count: number): string {
    let baseClass = '';

    if (count > 100) {
      baseClass += ' count-high';
    }

    if (!status) return baseClass;

    switch (status) {
      case RequestStatusEnum.SUBMITTED:
        return `${baseClass} count-submitted`;
      case RequestStatusEnum.IN_PROGRESS:
        return `${baseClass} count-inprogress`;
      case RequestStatusEnum.APPROVED:
        return `${baseClass} count-approved`;
      case RequestStatusEnum.CONFIRMED:
        return `${baseClass} count-confirmed`;
      case RequestStatusEnum.CLOSED:
        return `${baseClass} count-closed`;
      case RequestStatusEnum.REJECTED:
        return `${baseClass} count-rejected`;
      case RequestStatusEnum.CANCELLED:
        return `${baseClass} count-cancelled`;
      case RequestStatusEnum.LOCKED:
      case RequestStatusEnum.LOCKED_STOCK:
        return `${baseClass} count-locked`;
      default:
        return baseClass;
    }
  }
}
