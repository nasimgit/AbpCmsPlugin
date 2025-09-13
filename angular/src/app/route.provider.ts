import { RoutesService, eLayoutType } from '@abp/ng.core';
import { inject, provideAppInitializer } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

function configureRoutes() {
  const routes = inject(RoutesService);
  routes.add([
      {
        path: '/',
        name: '::Menu:Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
      },
      {
        path: '/cms',
        name: 'CMS',
        iconClass: 'fas fa-cogs',
        order: 2,
        layout: eLayoutType.application,
        requiredPolicy: 'CMS.Pages', // Add permission requirement for CMS menu
      },
  ]);
  
  routes.add([
    { 
      path: 'cms/pages', 
      name: 'Pages', 
      parentName: 'CMS', 
      iconClass: 'fas fa-file-alt', 
      order: 1,
      requiredPolicy: 'CMS.Pages' // Add permission requirement for Pages
    },
   // { path: 'cms/categories', name: 'Categories', parentName: 'CMS', iconClass: 'fas fa-folder', order: 2}
  ]);
}

