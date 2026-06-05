import { AbpWindowService } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { PSIByProductExportInput, PSIService } from '@proxy/psis';
import { finalize, Observable, of } from 'rxjs';
import { DialogService } from '../../shared/services/dialog/dialog.service';
import { PSIReportDialogComponent } from '../components/psi-report-dialog/psi-report-dialog.component';

interface PSIReportFilter {
  materialType: string;
  fiscalYear: number;
}

@Injectable({
  providedIn: 'root',
})
export class PSIReportDialogService {
  private readonly dialogService = inject(DialogService);
  private readonly router = inject(Router);
  private readonly toast = inject(ToasterService);
  private readonly service = inject(PSIService);
  protected readonly abpWindowService = inject(AbpWindowService);

  private isExportToExcelBusy = false;

  // openPSIReportDialog(): void {
  //   this.dialogService
  //     .open(PSIReportDialogComponent, {
  //       title: 'PSI Export',
  //       size: 'lg',
  //       confirmBtnLabel: null,
  //     })
  //     .subscribe({
  //       next: () => {
  //         // Modal closed - result will contain final data if modal was closed normally
  //       },
  //       error: () => {
  //         // Handle dialog errors silently or log to external service
  //       },
  //     });
  // }

  getReportGenerationFunction(): (filterData: PSIReportFilter) => Observable<Blob> {
    return (filterData: PSIReportFilter) => this.performPSIReportGeneration(filterData);
  }

  private performPSIReportGeneration(filterData: PSIReportFilter): Observable<Blob> {
    return this.generatePSIReport(filterData);
  }

  private generatePSIReport(filterData: PSIReportFilter): Observable<Blob> {
    if (this.isExportToExcelBusy) {
      this.toast.error('Export is already in progress. Please wait.');
      return of(null);
    }

    this.isExportToExcelBusy = true;
    const payload = {
      materialType: filterData.materialType,
      fiscalYear: filterData.fiscalYear.toString(),
    } as PSIByProductExportInput;
    return this.service.getPSIByProductExport(payload).pipe(
      finalize(() => {
        this.isExportToExcelBusy = false;
      }),
    );
  }
}
