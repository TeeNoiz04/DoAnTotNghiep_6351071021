import { Rest, RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { CargoExcelInput, CargoImportDto, CargoService } from '@proxy/cargos';
import { ExcelValidationResult } from '@proxy/shared/excels';

@Injectable({
  providedIn: 'root',
})
export class CargoExtendedService extends CargoService {
  constructor(private myRestService: RestService) {
    super(myRestService);
  }
  validateAndParseCargoManual(file: File, input: CargoExcelInput, config?: Partial<Rest.Config>) {
    const formData = new FormData();

    formData.append('file', file);

    formData.append('materialType', input.materialType ?? '');
    formData.append('supplierCode', input.supplierCode ?? '');
    formData.append('note', input.note ?? '');

    return this.myRestService.request<any, ExcelValidationResult<CargoImportDto>>(
      {
        method: 'POST',
        url: '/api/app/cargos/validate-and-parse',
        body: formData,
      },
      { apiName: this.apiName, ...config },
    );
  }
}
