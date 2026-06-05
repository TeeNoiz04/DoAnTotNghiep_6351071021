import { Rest, RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { GetPSIImportsInput, PSIImportDto, PSIService } from '@proxy/psis';
import { ExcelValidationResult } from '@proxy/shared/excels';

@Injectable({
  providedIn: 'root',
})
export class PSIExtendedService extends PSIService {
  constructor(private myRestService: RestService) {
    super(myRestService);
  }

  private buildFormData(file: File, input: GetPSIImportsInput): FormData {
    const formData = new FormData();

    formData.append('file', file);
    formData.append('fy', input?.fy ? input?.fy?.toString() : '');
    formData.append('materialType', input?.materialType ? input?.materialType?.toString() : '');
    formData.append('note', input.note ?? '');

    return formData;
  }

  validateAndParseFAManual(file: File, input: GetPSIImportsInput, config?: Partial<Rest.Config>) {
    const formData = this.buildFormData(file, input);

    return this.myRestService.request<any, ExcelValidationResult<PSIImportDto>>(
      {
        method: 'POST',
        url: '/api/app/psi/validate-psi-fa',
        body: formData,
      },
      { apiName: this.apiName, ...config },
    );
  }

  validateAndParseLVSManual(file: File, input: GetPSIImportsInput, config?: Partial<Rest.Config>) {
    const formData = this.buildFormData(file, input);

    return this.myRestService.request<any, ExcelValidationResult<PSIImportDto>>(
      {
        method: 'POST',
        url: '/api/app/psi/validate-psi-lvs',
        body: formData,
      },
      { apiName: this.apiName, ...config },
    );
  }
}
