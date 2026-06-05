import { inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { SpoBatchRequestDto, SpoBatchRequestService } from '@proxy/spo-batch-requests';
import { forkJoin } from 'rxjs';

export abstract class AbstractImportBatchRequestDetailViewService {
  protected readonly proxyService = inject(SpoBatchRequestService);
  private readonly fb = inject(FormBuilder);

  form: FormGroup | undefined;
  isBusy = false;
  selected = {} as SpoBatchRequestDto;
  loadDetailsAndImports(id: string) {
    return forkJoin({
      details: this.proxyService.get(id),
    });
  }
  buildForm(): void {
    const { fileName, importType, requestNo } = this.selected || {};

    this.form = this.fb.group({
      fileName: [fileName, []],
      requestNo: [requestNo, []],
      importType: [importType, []],
    });
  }
}
