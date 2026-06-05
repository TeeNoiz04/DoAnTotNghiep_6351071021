import { inject, provideAppInitializer } from '@angular/core';
import { ABP, RoutesService } from '@abp/ng.core';
import { REPORT_BASE_ROUTES } from './report-base.routes';
import { ReportMenuService } from '../services/report-menu.service';

export const REPORT_ROUTE_PROVIDER = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

function configureRoutes() {
  const routesService = inject(RoutesService);
  const reportMenuService = inject(ReportMenuService);
  const routes: ABP.Route[] = [...REPORT_BASE_ROUTES];
  routesService.add(routes);
}
