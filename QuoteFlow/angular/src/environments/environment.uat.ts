import { Environment } from '@abp/ng.core';

const baseUrl = 'https://fa-uat.mitsubishi-electric.vn';

const oAuthConfig = {
  issuer: 'https://fa-uat.mitsubishi-electric.vn/',
  redirectUri: baseUrl,
  clientId: 'QuoteFlow_App',
  responseType: 'code',
  scope: 'offline_access QuoteFlow',
  requireHttps: true,
  impersonation: {
    userImpersonation: true,
  },
};

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'QuoteFlow',
  },
  oAuthConfig,
  apis: {
    default: {
      url: '',
      rootNamespace: 'QuoteFlow',
    },
    AbpAccountPublic: {
      url: '',
      rootNamespace: 'AbpAccountPublic',
    },
  },
  features: {
    hideMenuItemNames: [],
  },
} as Environment;
