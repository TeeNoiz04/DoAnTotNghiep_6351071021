import { ABP, RoutesService } from '@abp/ng.core';
import { inject, provideAppInitializer } from '@angular/core';
import { STOCK_TRACING_BASE_ROUTES } from './stock-tracing-base.routes';

export const STOCK_TRACINGS_STOCK_TRACING_ROUTE_PROVIDER = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

function configureRoutes() {
  const routesService = inject(RoutesService);
  const routes: ABP.Route[] = [...STOCK_TRACING_BASE_ROUTES];
  routesService.add(routes);
}
