import { ABP, RoutesService } from '@abp/ng.core';
import { inject, provideAppInitializer } from '@angular/core';
import { MATERIAL_BASE_ROUTES } from './material-base.routes';

export const MATERIALS_MATERIAL_ROUTE_PROVIDER = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

function configureRoutes() {
  const routesService = inject(RoutesService);
  const routes: ABP.Route[] = [...MATERIAL_BASE_ROUTES];
  routesService.add(routes);
}
