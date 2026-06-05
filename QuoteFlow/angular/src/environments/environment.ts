import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

const oAuthConfig = {
  issuer: 'https://localhost:44365/',
  redirectUri: baseUrl,
  clientId: 'QuoteFlow_App',
  responseType: 'code',
  scope: 'offline_access QuoteFlow',
  requireHttps: false,
};

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'QuoteFlow',
  },
  oAuthConfig,
  apis: {
    default: {
      url: 'https://localhost:44365',
      rootNamespace: 'QuoteFlow',
    },
    AbpAccountPublic: {
      url: 'https://localhost:44365/',
      rootNamespace: 'AbpAccountPublic',
    },
  },
  features: {
    hideMenuItemNames: [], // Array of menu item IDs to hide
  },
} as Environment;
