import { AbpWindowService } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import { PriceOfferService } from '@proxy/price-offers';
import { GetPriceOfferReportGeneralsInput } from '@proxy/price-offers/price-offer-report-generals';
import { finalize, Observable, of } from 'rxjs';
import { DialogService } from '../../shared/services/dialog/dialog.service';
import { MenuClickHandlerService } from '../../shared/services/menu-click-handler.service';
import { ReportDialogComponent } from '../components/report-dialog/report-dialog.component';
import { GetPriceOfferReportDetailsInput } from '@proxy/price-offers/price-offer-report-details';

@Injectable({
  providedIn: 'root',
})
export class ReportMenuService {
  private readonly dialogService = inject(DialogService);
  private readonly router = inject(Router);
  private readonly toast = inject(ToasterService);
  private readonly menuClickHandler = inject(MenuClickHandlerService);
  private readonly priceOfferService = inject(PriceOfferService);
  protected readonly abpWindowService = inject(AbpWindowService);

  private isExportToExcelBusy = false;

  // Store current report type for the component to access
  private currentReportType: string = '';

  constructor() {
    // Remove routing-based approach - we'll call this service directly from menu clicks
  }

  openReportDialog(reportType: string): void {
    // Store the report type for the component to access
    this.currentReportType = reportType;
    const reportTitle = this.getReportTitle(reportType);

    this.dialogService
      .open(ReportDialogComponent, {
        title: reportTitle,
        size: 'lg',
        data: {
          reportType,
        },
        confirmBtnLabel: null,
      })
      .subscribe({
        next: result => {
          // Modal closed - result will contain final data if modal was closed normally
        },
        error: err => {
          // Handle dialog errors silently or log to external service
        },
      });
  }

  // Public method for the component to get report generation function
  getReportGenerationFunction(): (data: any) => Observable<any> {
    return (data: any) => {
      // Use the stored currentReportType
      return this.performReportGeneration(this.currentReportType, data);
    };
  }

  // Alternative method that takes reportType as parameter for more flexibility
  getReportGenerationFunctionForType(reportType: string): (data: any) => Observable<any> {
    return (data: any) => {
      return this.performReportGeneration(reportType, data);
    };
  }

  getCurrentReportType(): string {
    return this.currentReportType;
  }

  // Public method to be called directly from menu clicks
  openSaleReportDialog(reportType: string): void {
    this.openReportDialog(reportType);
  }

  private getReportTitle(reportType: string): string {
    switch (reportType) {
      case AppRoutes.SPECIAL_PRICE_OFFERS.GENERAL_REPORT.BASE:
        return 'SPO General Report (R03)';
      case AppRoutes.SPECIAL_PRICE_OFFERS.SPO_DETAILED_REPORT.BASE:
        return 'SPO Detailed Report (R04)';
      default:
        return 'Report';
    }
  }

  private performReportGeneration(reportType: string, reportData: any): Observable<any> {
    if (this.isExportToExcelBusy) {
      this.toast.error('Export is already in progress. Please wait.');
      return of(null);
    }

    this.isExportToExcelBusy = true;

    switch (reportType) {
      case AppRoutes.SPECIAL_PRICE_OFFERS.GENERAL_REPORT.BASE:
        return this.priceOfferGenerateReportObservable(reportData);
      case AppRoutes.SPECIAL_PRICE_OFFERS.SPO_DETAILED_REPORT.BASE:
        return this.priceOfferSPODetailReportObservable(reportData);
      default:
        this.toast.error('Unsupported report type for export.');
        this.isExportToExcelBusy = false;
        return of(null);
    }
  }

  private priceOfferGenerateReportObservable(reportData: any): Observable<any> {
    const payload: GetPriceOfferReportGeneralsInput = {
      to: reportData.toDate,
      from: reportData.fromDate,
      buyer: reportData.buyer,
      customerName: reportData.customerName,
      priceOfferCode: reportData.priceOfferCode,
      priceOfferName: reportData.priceOfferName,
      location: reportData.location,
      status: reportData.status,
      materialType: reportData.materialType,
      orderMin: reportData.orderMin,
      orderMax: reportData.orderMax,
      maxResultCount: 1000,
    };

    return this.priceOfferService.getListGeneralAsExcelFile(payload).pipe(
      finalize(() => {
        this.isExportToExcelBusy = false;
      }),
    );
  }

  private priceOfferSPODetailReportObservable(reportData: any): Observable<any> {
    const payload: GetPriceOfferReportDetailsInput = {
      to: reportData.toDate,
      from: reportData.fromDate,
      buyer: reportData.buyer,
      materialGroup: reportData.materialGroup,
      priceOfferCode: reportData.priceOfferCode,
      priceOfferName: reportData.priceOfferName,
      golfaCode: reportData.golfaCode,
      modelName: reportData.modelName,
      maxResultCount: 1000,
    };

    return this.priceOfferService.getListDetailAsExcelFile(payload).pipe(
      finalize(() => {
        this.isExportToExcelBusy = false;
      }),
    );
  }
}
