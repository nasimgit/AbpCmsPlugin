import { Routes } from '@angular/router';
import { PublicPageListComponent } from './public-page-list/public-page-list.component';
import { PublicPageViewComponent } from './public-page-view/public-page-view.component';
import { PublicHomeComponent } from './public-home/public-home.component';

export const publicRoutes: Routes = [
  {
    path: '',
    component: PublicHomeComponent,
    data: { breadcrumb: 'Home' }
  },
  {
    path: 'pages',
    component: PublicPageListComponent,
    data: { breadcrumb: 'Pages' }
  },
  {
    path: 'page/:slug',
    component: PublicPageViewComponent,
    data: { breadcrumb: 'Page' }
  }
];
