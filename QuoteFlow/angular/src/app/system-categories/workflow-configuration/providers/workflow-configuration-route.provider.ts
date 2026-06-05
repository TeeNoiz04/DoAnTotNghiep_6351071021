import { inject, provideAppInitializer } from '@angular/core';
import { ABP, RoutesService } from '@abp/ng.core';
import { WORKFLOW_CONFIGURATION_BASE_ROUTES } from './workflow-configuration-base.routes';

export const WORKFLOW_CONFIGURATION_ROUTE_PROVIDER = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

function configureRoutes() {
  const routesService = inject(RoutesService);
  const routes: ABP.Route[] = [...WORKFLOW_CONFIGURATION_BASE_ROUTES];
  routesService.add(routes);
  routesService.remove(['File Templates']);
}
