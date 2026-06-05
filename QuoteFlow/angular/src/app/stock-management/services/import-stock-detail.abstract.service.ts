import { inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { StockManagementService, StockManagementUploadDto } from '@proxy/stock-managements';

export abstract class AbstractImportStockDetailViewService {
  protected readonly proxyService = inject(StockManagementService);
  private readonly fb = inject(FormBuilder);

  form: FormGroup | undefined;
  isBusy = false;
  selected = {} as StockManagementUploadDto;

  loadDetailsAndImports(id: string) {
    return forkJoin({
      details: this.proxyService.getUploadDetail(id),
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
