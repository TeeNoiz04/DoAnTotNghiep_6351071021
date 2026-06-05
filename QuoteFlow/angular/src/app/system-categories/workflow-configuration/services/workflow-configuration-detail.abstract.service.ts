import { ListService, TrackByService } from '@abp/ng.core';
import { inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { finalize, tap } from 'rxjs/operators';

import { FormMode } from '@app/shared/enums/form-mode';
import {
  WorkflowConfigurationDto,
  WorkflowConfigurationService,
  WorkflowConfigurationUpdateDto,
} from '@proxy/workflow-configurations';
import { BehaviorSubject } from 'rxjs';
import { WorkflowApproverDto } from '@proxy/workflow-approvers';
import { UserLookupDto } from '@proxy/shared';
import { ToasterService } from '@abp/ng.theme.shared';

export abstract class AbstractWorkflowConfigurationDetailViewService {
  public readonly fb = inject(FormBuilder);
  protected readonly track = inject(TrackByService);

  public readonly proxyService = inject(WorkflowConfigurationService);
  public readonly list = inject(ListService);
  protected readonly toast = inject(ToasterService);

  public approverSubject = new BehaviorSubject<WorkflowApproverDto[]>([]);
  approver$ = this.approverSubject.asObservable();
  public newApprover = new BehaviorSubject<WorkflowApproverDto[]>([]);
  newApprover$ = this.newApprover.asObservable();

  hasUnsavedChanges = false;
  initialApprovers: WorkflowApproverDto[] = [];
  userOptions: UserLookupDto[] = [];

  isBusy = false;
  isVisible = false;
  selected = {} as any;
  form: FormGroup | undefined;

  // Checks if the current lists are different from the initial state
  checkForChanges(): void {
    const currentApprovers = this.approverSubject.getValue();
    const newApproverItems = this.newApprover.getValue();

    // Compare current state with initial state
    const approversChanged = this.areListsDifferent(currentApprovers, this.initialApprovers);
    const hasNewItems = newApproverItems.length > 0;

    // Update hasUnsavedChanges flag based on actual differences
    this.hasUnsavedChanges = approversChanged || hasNewItems;
  }

  // Helper to compare two lists of WorkflowApproverDto objects
  private areListsDifferent(list1: WorkflowApproverDto[], list2: WorkflowApproverDto[]): boolean {
    if (list1.length !== list2.length) {
      return true;
    }

    // First, create arrays of IDs for quick comparison
    const ids1 = list1.map(item => item.id).sort();
    const ids2 = list2.map(item => item.id).sort();

    // Compare IDs
    return JSON.stringify(ids1) !== JSON.stringify(ids2);
  }

  addNewApprover() {
    const currentItems = this.newApprover.getValue();
    const newItem: WorkflowApproverDto = {
      approver: '',
    };
    this.newApprover.next([...currentItems, newItem]);
    this.checkForChanges();
  }

  onApproverSelectionChange(item: any, index: number): void {
    if (item.approver) {
      const currentItems = this.newApprover.getValue();
      currentItems[index] = {
        ...item,
      };
      this.newApprover.next([...currentItems]);
    }
  }

  deleteDetailItem(entry: any) {
    const currentApprovers = this.approverSubject.getValue();
    const updatedApprovers = currentApprovers.filter(item => item.id !== entry.id);
    this.approverSubject.next(updatedApprovers);

    this.checkForChanges();
  }

  removeApprover(index: number) {
    const currentItems = this.newApprover.getValue();
    currentItems.splice(index, 1);
    this.newApprover.next([...currentItems]);

    this.checkForChanges();
  }

  protected createRequest() {
    const formValues = {
      ...this.form.getRawValue(),
    };

    const currentApprovers = this.approverSubject.getValue();
    const newApproverItems = this.newApprover.getValue();

    const approverList = [...currentApprovers, ...newApproverItems];

    const approvers = (approverList as any).map(item => {
      return {
        wfId: this.selected.id,
        approver: item.approver,
      };
    });

    const item: WorkflowConfigurationUpdateDto = {
      workflowRole: formValues.workflowRole,
      note: formValues.note,
      approvers: approvers,
      concurrencyStamp: this.selected.concurrencyStamp,
    };

    return this.proxyService.update(this.selected?.id, item);
  }

  buildForm(mode: FormMode = FormMode.New) {
    this.approverSubject.next([]);
    this.newApprover.next([]);
    const { workflowRole, note } = this.selected || {};

    this.form = this.fb.group({
      workflowRole: [workflowRole ?? null, [Validators.required]],
      note: [note ?? null, [Validators.maxLength(4000)]],
    });
  }

  showForm(mode: FormMode = FormMode.New) {
    this.buildForm(mode);
    this.isVisible = true;
  }

  create() {
    this.selected = undefined;
    this.showForm();
  }

  update(record: WorkflowConfigurationDto) {
    this.selected = record;
    this.showForm(FormMode.Edit);
  }

  hideForm() {
    this.isVisible = false;
  }

  validateApprovers(): boolean {
    const items = this.newApprover.getValue();
    if (items.length === 0) return true;
    const invalidItems = items.filter(item => !item.approver);

    if (invalidItems.length > 0) {
      this.toast.error('All new items must have value filled in.', 'Validation Error');
      return false;
    }

    const approvers = items.map(item => item.approver);
    const uniqueApprovers = new Set(approvers);
    if (uniqueApprovers.size !== approvers.length) {
      this.toast.error('Duplicate approver are not allowed in new items.', 'Validation Error');
      return false;
    }

    return true;
  }

  submitForm() {
    if (this.form.invalid) return;

    // Validate new approver items
    if (!this.validateApprovers()) {
      return;
    }

    this.isBusy = true;

    const request = this.createRequest().pipe(
      finalize(() => (this.isBusy = false)),
      tap(() => this.hideForm()),
    );

    request.subscribe(() => {
      this.hasUnsavedChanges = false;
      this.list.get();
    });
  }

  changeVisible($event: boolean) {
    this.isVisible = $event;
  }

  onModalClose() {
    this.isVisible = false;
    this.hasUnsavedChanges = false;
  }
}
