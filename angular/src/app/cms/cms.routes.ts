import { Routes } from '@angular/router';
import { PageListComponent } from './pages/page-list/page-list.component';
import { PageEditComponent } from './pages/page-edit/page-edit.component';
import { PageViewComponent } from './pages/page-view/page-view.component';
import { CategoryListComponent } from './categories/category-list/category-list.component';
import { CategoryEditComponent } from './categories/category-edit/category-edit.component';

export const cmsRoutes: Routes = [
  {
    path: 'cms',
    children: [
      {
        path: 'pages',
        component: PageListComponent,
        data: { breadcrumb: 'Pages' }
      },
      {
        path: 'pages/new',
        component: PageEditComponent,
        data: { breadcrumb: 'New Page' }
      },
      {
        path: 'pages/edit/:id',
        component: PageEditComponent,
        data: { breadcrumb: 'Edit Page' }
      },
      {
        path: 'pages/view/:slug',
        component: PageViewComponent,
        data: { breadcrumb: 'View Page' }
      },
      {
        path: 'categories',
        component: CategoryListComponent,
        data: { breadcrumb: 'Categories' }
      },
      {
        path: 'categories/new',
        component: CategoryEditComponent,
        data: { breadcrumb: 'New Category' }
      },
      {
        path: 'categories/edit/:id',
        component: CategoryEditComponent,
        data: { breadcrumb: 'Edit Category' }
      }
    ]
  }
];
