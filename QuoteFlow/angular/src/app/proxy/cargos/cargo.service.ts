import type { CargoDataDto, GetCargoDataInput } from './cargo-datas/models';
import type {
  CargoCreateDto,
  CargoDto,
  CargoExcelInput,
  CargoImportDto,
  CargoReportDto,
  CargoUpdateDto,
  GetCargoReportsInput,
  GetCargosInput,
  ImportCargoRequestDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { ExcelValidationResult } from '../shared/excels/models';

@Injectable({
  providedIn: 'root',
})
export class CargoService {
  apiName = 'Default';

  create = (input: CargoCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CargoDto>(
      {
        method: 'POST',
        url: '/api/app/cargos',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  deleteCargoData = (cargoDataId: string, cargoId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/cargos/${cargoDataId}/${cargoId}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CargoDto>(
      {
        method: 'GET',
        url: `/api/app/cargos/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getDetail = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CargoDto>(
      {
        method: 'GET',
        url: `/api/app/cargos/detail/${id}/cargo-data`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetCargosInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CargoDto>>(
      {
        method: 'GET',
        url: '/api/app/cargos',
        params: {
          filterText: input.filterText,
          fileName: input.fileName,
          note: input.note,
          supplierCode: input.supplierCode,
          materialType: input.materialType,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListAsExcel = (input: GetCargoReportsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/cargos/cargo-report/export',
        params: {
          poNo: input.poNo,
          materialCode: input.materialCode,
          modelName: input.modelName,
          from: input.from,
          to: input.to,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListCargoData = (input: GetCargoDataInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CargoDataDto>>(
      {
        method: 'GET',
        url: '/api/app/cargos/data',
        params: {
          filterText: input.filterText,
          cargoId: input.cargoId,
          materialCode: input.materialCode,
          model: input.model,
          machineNumber: input.machineNumber,
          po: input.po,
          eu: input.eu,
          fileName: input.fileName,
          materialType: input.materialType,
          supplier: input.supplier,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListCargoReport = (input: GetCargoReportsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CargoReportDto>>(
      {
        method: 'GET',
        url: '/api/app/cargos/cargo-report',
        params: {
          poNo: input.poNo,
          materialCode: input.materialCode,
          modelName: input.modelName,
          from: input.from,
          to: input.to,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListDetail = (input: GetCargoDataInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CargoDataDto>>(
      {
        method: 'GET',
        url: '/api/app/cargos/detail',
        params: {
          filterText: input.filterText,
          cargoId: input.cargoId,
          materialCode: input.materialCode,
          model: input.model,
          machineNumber: input.machineNumber,
          po: input.po,
          eu: input.eu,
          fileName: input.fileName,
          materialType: input.materialType,
          supplier: input.supplier,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  importCargo = (request: ImportCargoRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CargoDto>(
      {
        method: 'POST',
        url: '/api/app/cargos/import',
        body: request,
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: CargoUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CargoDto>(
      {
        method: 'PUT',
        url: `/api/app/cargos/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseCargo = (file: FormData, input: CargoExcelInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExcelValidationResult<CargoImportDto>>(
      {
        method: 'POST',
        url: '/api/app/cargos/validate-and-parse',
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
