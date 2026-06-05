import { inject, provideAppInitializer } from '@angular/core';
import { ABP, RoutesService } from '@abp/ng.core';
import { FA_ADMIN_BASE_ROUTES } from './fa-admin-base.routes';

export const FA_ADMIN_ROUTE_PROVIDER = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

function configureRoutes() {
  const routesService = inject(RoutesService);
  const routes: ABP.Route[] = [...FA_ADMIN_BASE_ROUTES];
  routesService.add(routes);
  routesService.remove(['File Templates']);
}
