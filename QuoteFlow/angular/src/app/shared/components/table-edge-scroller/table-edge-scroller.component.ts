import {
  Component,
  Input,
  ElementRef,
  OnInit,
  OnDestroy,
  AfterViewInit,
  Renderer2,
  HostListener,
  NgZone,
  Output,
  EventEmitter,
  ViewChild,
  ChangeDetectorRef,
  ChangeDetectionStrategy,
} from '@angular/core';
import { CommonModule } from '@angular/common';

export interface EdgeScrollConfig {
  edgeSize?: number;
  maxSpeed?: number;
  containerSelector?: string;
  showNavigationButtons?: boolean;
  alwaysShowButtonsForLongContent?: boolean;
  scrollAmount?: number;
  navButtonClass?: string;
  buttonSize?: number;
  buttonVisibilityTimeout?: number;
}

@Component({
  selector: 'app-table-edge-scroller',
  standalone: true,
  imports: [CommonModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './table-edge-scroller.component.html',
  styleUrls: ['./table-edge-scroller.component.scss'],
})
export class TableEdgeScrollerComponent implements OnInit, AfterViewInit, OnDestroy {
  @Input() config: EdgeScrollConfig = {
    edgeSize: 80,
    maxSpeed: 20,
    showNavigationButtons: true,
    scrollAmount: 400,
    buttonSize: 32,
    alwaysShowButtonsForLongContent: true,
    buttonVisibilityTimeout: 2000,
  };

  @Output() scrolling = new EventEmitter<{ horizontal: number }>();
  @ViewChild('scrollContainer') scrollContainerRef!: ElementRef<HTMLDivElement>;

  canScrollNext = false;
  canScrollPrev = false;

  showPrevButton = false;
  showNextButton = false;

  mouseYPosition = 0;
  containerHeight = 0;

  isLongContent = false;

  private scrollContainer: HTMLElement | null = null;
  private datatableBodyElement: HTMLElement | null = null;
  private containerRect: DOMRect | null = null;
  private isMouseInside = false;
  private scrollAnimationId: number | null = null;
  private horizontalScrollSpeed = 0;
  private initializationAttempts = 0;
  private maxInitializationAttempts = 10;
  private resizeObserver: ResizeObserver | null = null;
  private mouseMoveThrottle: number | null = null;
  private buttonVisibilityTimer: number | null = null;
  private recalculateContainerRectInterval: number | null = null;

  private longPressTimer: number | null = null;
  private continuousScrollTimer: number | null = null;
  private isLongPressing = false;
  private longPressDelay = 500;
  private continuousScrollStep = 20;
  private continuousScrollInterval = 16;

  constructor(
    private elementRef: ElementRef,
    private renderer: Renderer2,
    private ngZone: NgZone,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.validateConfig();
  }

  ngAfterViewInit(): void {
    setTimeout(() => {
      this.initScrollContainer();
    }, 100);

    this.recalculateContainerRectInterval = window.setInterval(() => {
      if (this.isMouseInside && this.scrollContainer) {
        this.containerRect = this.scrollContainer.getBoundingClientRect();
        this.containerHeight = this.containerRect.height;
      }
    }, 1000);
  }

  ngOnDestroy(): void {
    this.stopScrolling();

    this.clearLongPressTimers();
    document.removeEventListener('mouseup', this.handleLongPressEnd);
    document.removeEventListener('touchend', this.handleLongPressEnd);
    document.removeEventListener('mouseleave', this.handleLongPressEnd);

    if (this.mouseMoveThrottle) {
      clearTimeout(this.mouseMoveThrottle);
      this.mouseMoveThrottle = null;
    }

    if (this.buttonVisibilityTimer) {
      clearTimeout(this.buttonVisibilityTimer);
      this.buttonVisibilityTimer = null;
    }

    if (this.resizeObserver) {
      this.resizeObserver.disconnect();
      this.resizeObserver = null;
    }

    if (this.recalculateContainerRectInterval) {
      clearInterval(this.recalculateContainerRectInterval);
      this.recalculateContainerRectInterval = null;
    }
  }

  getButtonYPosition(): number {
    if (this.mouseYPosition > 0) {
      const buttonSize = this.config.buttonSize || 32;
      const halfButtonSize = buttonSize / 2;

      return Math.max(halfButtonSize, Math.min(this.mouseYPosition, this.containerHeight - halfButtonSize));
    }

    return this.containerHeight / 2;
  }

  onWrapperMouseMove(event: MouseEvent): void {
    if (!this.scrollContainer) return;

    if (!this.containerRect) {
      this.containerRect = this.scrollContainer.getBoundingClientRect();
      this.containerHeight = this.containerRect.height;
    }

    const { left, top, right } = this.containerRect;

    this.mouseYPosition = event.clientY - top;

    const distanceFromLeft = event.clientX - left;
    const distanceFromRight = right - event.clientX;

    const edgeSize = this.isLongContent ? this.config.edgeSize * 3 : this.config.edgeSize * 2;

    const shouldShowPrev = this.canScrollPrev && distanceFromLeft <= edgeSize;
    const shouldShowNext = this.canScrollNext && distanceFromRight <= edgeSize;

    if (this.showPrevButton !== shouldShowPrev || this.showNextButton !== shouldShowNext) {
      this.showPrevButton = shouldShowPrev;
      this.showNextButton = shouldShowNext;
      this.cdr.markForCheck();
    }
  }

  onContainerMouseMove(event: MouseEvent): void {
    if (!this.scrollContainer) return;
    this.isMouseInside = true;

    if (!this.containerRect) {
      this.containerRect = this.scrollContainer.getBoundingClientRect();
      this.containerHeight = this.containerRect.height;
    }

    const { left, top, width } = this.containerRect;

    const relativeX = event.clientX - left;
    const relativeY = event.clientY - top;

    const distanceFromLeft = relativeX;
    const distanceFromRight = width - relativeX;

    const edgeSize = this.isLongContent ? this.config.edgeSize * 3 : this.config.edgeSize * 2;

    const shouldShowPrev = this.canScrollPrev && distanceFromLeft <= edgeSize;
    const shouldShowNext = this.canScrollNext && distanceFromRight <= edgeSize;

    if (this.showPrevButton !== shouldShowPrev || this.showNextButton !== shouldShowNext) {
      this.showPrevButton = shouldShowPrev;
      this.showNextButton = shouldShowNext;
      this.cdr.markForCheck();
    }

    this.mouseYPosition = relativeY;

    this.processMousePosition(event);
  }

  scrollPrev(): void {
    if (!this.scrollContainer) return;

    this.showButtonsTemporarily();

    if (this.datatableBodyElement) {
      this.scrollDatatableBody(-1);

      this.startLongPressScroll(-1);
      return;
    }

    const containerWidth = this.scrollContainer.clientWidth;
    const scrollAmount = Math.min(this.config.scrollAmount || 300, containerWidth * 0.8);

    const targetScrollLeft = Math.max(0, this.scrollContainer.scrollLeft - scrollAmount);

    this.scrollContainer.scrollLeft = targetScrollLeft;

    setTimeout(() => {
      this.smoothScroll(targetScrollLeft);
    }, 0);

    this.startLongPressScroll(-1);

    this.updateScrollButtons();
  }

  scrollNext(): void {
    if (!this.scrollContainer) return;

    this.showButtonsTemporarily();

    if (this.datatableBodyElement) {
      this.scrollDatatableBody(1);

      this.startLongPressScroll(1);
      return;
    }

    const containerWidth = this.scrollContainer.clientWidth;
    const scrollAmount = Math.min(this.config.scrollAmount || 300, containerWidth * 0.8);

    const maxScrollLeft = this.scrollContainer.scrollWidth - this.scrollContainer.clientWidth;

    const targetScrollLeft = Math.min(maxScrollLeft, this.scrollContainer.scrollLeft + scrollAmount);

    this.scrollContainer.scrollLeft = targetScrollLeft;

    setTimeout(() => {
      this.smoothScroll(targetScrollLeft);
    }, 0);

    this.startLongPressScroll(1);

    this.updateScrollButtons();
  }

  private startLongPressScroll(direction: number): void {
    this.clearLongPressTimers();

    this.longPressTimer = window.setTimeout(() => {
      this.isLongPressing = true;

      this.startContinuousScroll(direction);

      this.longPressTimer = null;
    }, this.longPressDelay);

    document.addEventListener('mouseup', this.handleLongPressEnd);
    document.addEventListener('touchend', this.handleLongPressEnd);
    document.addEventListener('mouseleave', this.handleLongPressEnd);
  }

  private handleLongPressEnd = (): void => {
    this.clearLongPressTimers();

    document.removeEventListener('mouseup', this.handleLongPressEnd);
    document.removeEventListener('touchend', this.handleLongPressEnd);
    document.removeEventListener('mouseleave', this.handleLongPressEnd);

    this.isLongPressing = false;
  };

  private clearLongPressTimers(): void {
    if (this.longPressTimer) {
      clearTimeout(this.longPressTimer);
      this.longPressTimer = null;
    }

    if (this.continuousScrollTimer) {
      clearTimeout(this.continuousScrollTimer);
      this.continuousScrollTimer = null;
    }
  }

  private startContinuousScroll(direction: number): void {
    if (this.continuousScrollTimer) {
      clearTimeout(this.continuousScrollTimer);
    }

    const element = this.datatableBodyElement || this.scrollContainer;
    if (!element) return;

    const maxScrollLeft = element.scrollWidth - element.clientWidth;

    const continuousScrollStep = () => {
      if (!this.isLongPressing || !element) {
        return;
      }

      const newScrollLeft = element.scrollLeft + direction * this.continuousScrollStep;

      if (direction < 0) {
        element.scrollLeft = Math.max(0, newScrollLeft);

        if (element.scrollLeft <= 0) {
          this.handleLongPressEnd();
          return;
        }
      } else {
        element.scrollLeft = Math.min(maxScrollLeft, newScrollLeft);

        if (element.scrollLeft >= maxScrollLeft) {
          this.handleLongPressEnd();
          return;
        }
      }

      this.updateScrollButtons();

      this.continuousScrollTimer = window.setTimeout(continuousScrollStep, this.continuousScrollInterval);
    };

    continuousScrollStep();
  }

  private showButtonsTemporarily(): void {
    if (this.buttonVisibilityTimer) {
      clearTimeout(this.buttonVisibilityTimer);
      this.buttonVisibilityTimer = null;
    }

    const wasChanged = (this.canScrollPrev && !this.showPrevButton) || (this.canScrollNext && !this.showNextButton);

    if (this.canScrollPrev) this.showPrevButton = true;
    if (this.canScrollNext) this.showNextButton = true;

    if (wasChanged) {
      this.cdr.markForCheck();
    }

    this.buttonVisibilityTimer = window.setTimeout(() => {
      this.showPrevButton = false;
      this.showNextButton = false;
      this.buttonVisibilityTimer = null;
      this.cdr.markForCheck();
    }, this.config.buttonVisibilityTimeout || 2000);
  }

  private scrollDatatableBody(direction: number): void {
    if (!this.datatableBodyElement) return;

    const containerWidth = this.datatableBodyElement.clientWidth;
    const scrollAmount = Math.min(this.config.scrollAmount || 300, containerWidth * 0.8);

    const currentScroll = this.datatableBodyElement.scrollLeft;
    const maxScroll = this.datatableBodyElement.scrollWidth - this.datatableBodyElement.clientWidth;
    const targetScroll =
      direction > 0 ? Math.min(maxScroll, currentScroll + scrollAmount) : Math.max(0, currentScroll - scrollAmount);

    this.datatableBodyElement.scrollLeft = targetScroll;

    this.datatableBodyElement.style.overflow = 'auto';

    try {
      this.datatableBodyElement.scrollTo({
        left: targetScroll,
        behavior: 'auto',
      });
    } catch (e) {
      console.warn('ScrollTo API failed, using direct scroll', e);
    }

    this.updateScrollButtons();

    setTimeout(() => {
      if (Math.abs(this.datatableBodyElement!.scrollLeft - targetScroll) > 5) {
        this.datatableBodyElement!.scrollLeft = targetScroll;
      }
      this.updateScrollButtons();
    }, 10);
  }

  onScroll(): void {
    if (!this.scrollContainer) return;

    this.containerRect = null;
    this.updateScrollButtons();
  }

  @HostListener('mouseenter')
  onMouseEnter(): void {
    this.isMouseInside = true;

    this.containerRect = this.scrollContainer?.getBoundingClientRect() || null;
    if (this.containerRect) {
      this.containerHeight = this.containerRect.height;
    }

    if (this.isLongContent) {
      this.showButtonsTemporarily();
    }

    this.updateScrollButtons();
  }

  @HostListener('mouseleave')
  onMouseLeave(): void {
    this.isMouseInside = false;
    this.stopScrolling();

    if (!this.buttonVisibilityTimer) {
      this.showPrevButton = false;
      this.showNextButton = false;
      this.cdr.markForCheck();
    }
  }

  private validateConfig(): void {
    if (this.config.edgeSize <= 0) {
      console.warn('TableEdgeScroller: edgeSize must be greater than 0, resetting to 50');
      this.config.edgeSize = 50;
    }

    if (this.config.maxSpeed <= 0) {
      console.warn('TableEdgeScroller: maxSpeed must be greater than 0, resetting to 15');
      this.config.maxSpeed = 15;
    }

    if (!this.config.scrollAmount || this.config.scrollAmount <= 0) {
      this.config.scrollAmount = 300;
    }

    if (!this.config.buttonSize || this.config.buttonSize <= 0) {
      this.config.buttonSize = 32;
    }

    if (!this.config.buttonVisibilityTimeout) {
      this.config.buttonVisibilityTimeout = 2000;
    }
  }

  private initScrollContainer(): void {
    const datatable = this.elementRef.nativeElement.querySelector('ngx-datatable');
    if (datatable) {
      const datatableBody = datatable.querySelector('.datatable-body');
      if (datatableBody instanceof HTMLElement) {
        this.scrollContainer = this.scrollContainerRef.nativeElement;
        this.datatableBodyElement = datatableBody;

        this.datatableBodyElement.addEventListener('scroll', () => this.onScroll(), { passive: true });

        this.initContainer();
        return;
      }
    }

    if (this.scrollContainerRef?.nativeElement) {
      this.scrollContainer = this.scrollContainerRef.nativeElement;
      this.initContainer();
      return;
    }

    if (this.config.containerSelector) {
      const customContainer = document.querySelector(this.config.containerSelector);
      if (customContainer instanceof HTMLElement) {
        this.scrollContainer = customContainer;
        this.initContainer();
        return;
      } else {
        console.warn(`TableEdgeScroller: No element found matching selector "${this.config.containerSelector}"`);
      }
    }

    this.initializationAttempts++;
    if (this.initializationAttempts < this.maxInitializationAttempts) {
      setTimeout(() => {
        this.initScrollContainer();
      }, 300);
    } else {
      console.warn('TableEdgeScroller: Could not find scrollable container after multiple attempts');
    }
  }

  private initContainer(): void {
    if (!this.scrollContainer) return;

    this.renderer.addClass(this.scrollContainer, 'edge-scrollable-container');

    this.containerRect = this.scrollContainer.getBoundingClientRect();
    this.containerHeight = this.containerRect.height;

    this.updateIsLongContent();

    if (this.datatableBodyElement) {
      setTimeout(() => {
        this.updateIsLongContent();
        this.updateScrollButtons();

        if (this.isLongContent) {
          this.showButtonsTemporarily();
        }

        this.cdr.markForCheck();
      }, 300);
    }

    this.resizeObserver = new ResizeObserver(() => {
      if (this.scrollContainer) {
        this.containerRect = null;
        this.updateIsLongContent();
        this.updateScrollButtons();
      }
    });
    this.resizeObserver.observe(this.scrollContainer);

    this.updateScrollButtons();

    setTimeout(() => this.onScroll(), 100);
  }

  private updateIsLongContent(): void {
    if (!this.scrollContainer) return;

    const element = this.datatableBodyElement || this.scrollContainer;

    const wasLongContent = this.isLongContent;

    this.isLongContent = element.scrollWidth > element.clientWidth * 1.2;

    if (!wasLongContent && this.isLongContent) {
      this.showButtonsTemporarily();
    }
  }

  private updateScrollButtons(): void {
    if (!this.scrollContainer) return;

    const element = this.datatableBodyElement || this.scrollContainer;

    const maxHorizontalScroll = element.scrollWidth - element.clientWidth;

    const oldCanScrollPrev = this.canScrollPrev;
    const oldCanScrollNext = this.canScrollNext;

    this.canScrollPrev = element.scrollLeft > 2;

    this.canScrollNext = element.scrollLeft < maxHorizontalScroll - 2;

    if (oldCanScrollPrev !== this.canScrollPrev || oldCanScrollNext !== this.canScrollNext) {
      this.cdr.markForCheck();
    }
  }

  private processMousePosition(event: MouseEvent): void {
    if (!this.scrollContainer) return;

    if (!this.containerRect) {
      this.containerRect = this.scrollContainer.getBoundingClientRect();
    }

    const { left, right } = this.containerRect;

    const distanceFromLeft = event.clientX - left;
    const distanceFromRight = right - event.clientX;

    this.horizontalScrollSpeed = 0;

    if (distanceFromLeft < this.config.edgeSize) {
      this.horizontalScrollSpeed = -this.calculateScrollSpeed(distanceFromLeft);
      this.showPrevButton = this.canScrollPrev;
    } else if (distanceFromRight < this.config.edgeSize) {
      this.horizontalScrollSpeed = this.calculateScrollSpeed(distanceFromRight);
      this.showNextButton = this.canScrollNext;
    }

    if (this.horizontalScrollSpeed !== 0) {
      this.startScrolling();
    } else {
      this.stopScrolling();
    }
  }

  private calculateScrollSpeed(distance: number): number {
    const normalizedDistance = Math.max(0, Math.min(distance, this.config.edgeSize));
    const speedPercentage = 1 - normalizedDistance / this.config.edgeSize;
    return Math.round(speedPercentage * this.config.maxSpeed);
  }

  private startScrolling(): void {
    // Implementation removed as it's not needed for current functionality
  }

  private scrollTick(): void {
    if (!this.scrollContainer || !this.isMouseInside) {
      this.stopScrolling();
      return;
    }

    const element = this.datatableBodyElement || this.scrollContainer;

    if (this.horizontalScrollSpeed !== 0) {
      element.scrollLeft += this.horizontalScrollSpeed;
    }

    this.ngZone.run(() => {
      this.updateScrollButtons();
    });

    this.ngZone.run(() => {
      this.scrolling.emit({
        horizontal: this.horizontalScrollSpeed,
      });
    });

    this.scrollAnimationId = window.requestAnimationFrame(() => this.scrollTick());
  }

  private stopScrolling(): void {
    if (this.scrollAnimationId !== null) {
      window.cancelAnimationFrame(this.scrollAnimationId);
      this.scrollAnimationId = null;

      this.horizontalScrollSpeed = 0;

      this.ngZone.run(() => {
        this.scrolling.emit({ horizontal: 0 });
      });
    }
  }

  private smoothScroll(left: number): void {
    if (!this.scrollContainer) return;

    const element = this.datatableBodyElement || this.scrollContainer;

    const scrollHandler = () => {
      this.updateScrollButtons();
    };

    element.addEventListener('scroll', scrollHandler, { passive: true });

    try {
      element.scrollTo({
        left,
        behavior: 'smooth',
      });
    } catch {
      element.scrollLeft = left;
    }

    setTimeout(() => {
      element.removeEventListener('scroll', scrollHandler);
      this.updateScrollButtons();
    }, 800);
  }
}
