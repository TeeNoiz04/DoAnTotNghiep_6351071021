import { CommonModule } from '@angular/common';
import {
  Component,
  EnvironmentInjector,
  Injector,
  Input,
  OnDestroy,
  OnInit,
  Type,
  ViewChild,
  ViewContainerRef,
  createComponent,
  inject,
} from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-dialog',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="modal-header" [ngClass]="customClass">
      <h4 class="modal-title">{{ title }}</h4>
      <button *ngIf="showClose" type="button" class="btn-close" aria-label="Close" (click)="close()"></button>
    </div>
    <div class="modal-body">
      <ng-container #contentContainer></ng-container>
      <p *ngIf="isConfirmation" class="mb-0">{{ message }}</p>
    </div>
    <div class="modal-footer" *ngIf="closeBtnLabel || showConfirmBtn">
      <button type="button" *ngIf="closeBtnLabel" class="btn btn-outline-secondary" (click)="close()">
        {{ closeBtnLabel }}
      </button>
      <button *ngIf="showConfirmBtn" type="button" class="btn btn-primary" (click)="confirm()">
        {{ confirmBtnLabel }}
      </button>
    </div>
  `,
  styles: [
    `
      .modal-header {
        background-color: #f8f9fa;
        border-top-left-radius: 0.25rem;
        border-top-right-radius: 0.25rem;
        padding: 10px 16px;
      }
      .modal-title {
        font-size: 1.125rem;
        font-weight: 500;
        margin: 0;
      }
      .modal-body {
        padding: 10px 16px !important;
      }
    `,
  ],
})
export class DialogComponent implements OnInit, OnDestroy {
  @Input() title: string = '';
  @Input() message: string = '';
  @Input() showClose: boolean = true;
  @Input() closeBtnLabel: string = 'Close';
  @Input() confirmBtnLabel: string = 'Confirm';
  @Input() showConfirmBtn: boolean = false;
  @Input() isConfirmation: boolean = false;
  @Input() customClass: string = '';
  @Input() contentComponent: Type<any> | null = null;
  @Input() contentData: any = {};

  @ViewChild('contentContainer', { read: ViewContainerRef, static: true })
  contentContainer!: ViewContainerRef;

  private environmentInjector = inject(EnvironmentInjector);
  private injector = inject(Injector);
  private activeModal = inject(NgbActiveModal);
  private componentRef: any = null;

  ngOnInit(): void {
    setTimeout(() => this.createContentComponent(), 0);
  }

  private createContentComponent(): void {
    if (this.contentComponent && this.contentContainer) {
      this.contentContainer.clear();
      this.componentRef = createComponent(this.contentComponent, {
        environmentInjector: this.environmentInjector,
        elementInjector: this.injector,
      });

      if (this.contentData) {
        Object.keys(this.contentData).forEach(key => {
          if (this.componentRef.instance && key in this.componentRef.instance) {
            this.componentRef.instance[key] = this.contentData[key];
          }
        });
      }

      this.contentContainer.insert(this.componentRef.hostView);

      // Trigger change detection to ensure data is properly set
      this.componentRef.changeDetectorRef.detectChanges();

      // If component has ngOnChanges, manually trigger it
      if (this.componentRef.instance && typeof this.componentRef.instance.ngOnChanges === 'function') {
        const changes: any = {};
        Object.keys(this.contentData || {}).forEach(key => {
          changes[key] = {
            currentValue: this.contentData[key],
            previousValue: undefined,
            firstChange: true,
          };
        });
        this.componentRef.instance.ngOnChanges(changes);
      }
    }
  }

  ngOnDestroy(): void {
    if (this.componentRef) {
      this.componentRef.destroy();
    }
  }

  close(): void {
    this.activeModal.dismiss('closed');
  }

  confirm(): void {
    this.activeModal.close(true);
  }
}
