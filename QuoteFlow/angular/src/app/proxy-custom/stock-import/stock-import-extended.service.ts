import { Rest, RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import {
  InvoiceImportInput,
  ResultValidateInvoiceImport,
  StockImportService,
} from '@proxy/stock-imports';

@Injectable({
  providedIn: 'root',
})
export class StockImportExtendedService extends StockImportService {
  constructor(private myRestService: RestService) {
    super(myRestService);
  }
  validateAndParseStockManual(
    file: File,
    input: InvoiceImportInput,
    config?: Partial<Rest.Config>,
  ) {
    const formData = new FormData();

    formData.append('file', file);

    formData.append('supplierId', input.supplierId ?? '');
    formData.append('supplierCode', input.supplierCode ?? '');
    formData.append('invoiceNo', input.invoiceNo ?? '');
    formData.append('invoiceType', input.invoiceType ?? '');
    formData.append('invoiceDate', input.invoiceDate ?? '');
    formData.append('shipmentMethod', input.shipmentMethod ?? '');
    formData.append('etd', input.etd ?? '');
    formData.append('eta', input.eta ?? '');
    formData.append('note', input.note ?? '');
    formData.append('deliveryTerm', input.deliveryTerm ?? '');
    formData.append('stockDate', input.stockDate ?? '');

    return this.myRestService.request<any, ResultValidateInvoiceImport>(
      {
        method: 'POST',
        url: '/api/app/stock-imports/validate-and-parse',
        body: formData,
      },
      { apiName: this.apiName, ...config },
    );
  }
}
