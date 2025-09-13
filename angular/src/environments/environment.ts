 import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

const oAuthConfig = {
  issuer: 'https://localhost:44377/',
  redirectUri: baseUrl,
  clientId: 'AbpCMS_App',
  responseType: 'code',
  scope: 'offline_access AbpCMS',
  requireHttps: true,
};

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'AbpCMS',
  },
  oAuthConfig,
  apis: {
    default: {
      url: 'https://localhost:44377',
      rootNamespace: 'AbpCMS',
    },
    AbpAccountPublic: {
      url: oAuthConfig.issuer,
      rootNamespace: 'AbpAccountPublic',
    },
  },
} as Environment;
