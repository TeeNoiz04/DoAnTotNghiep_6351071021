import { ABP, RoutesService } from '@abp/ng.core';
import { inject, provideAppInitializer } from '@angular/core';
import { DPO_BASE_ROUTES } from './dpo-base.routes';

export const DPO_ROUTE_PROVIDERS = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

function configureRoutes() {
  const routesService = inject(RoutesService);
  const routes: ABP.Route[] = [...DPO_BASE_ROUTES];
  routesService.add(routes);
}
