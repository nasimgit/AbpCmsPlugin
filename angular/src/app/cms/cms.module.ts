import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

// ABP Modules
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';

// ngx-editor
import { NgxEditorModule } from 'ngx-editor';

// CMS Components
import { PageListComponent } from './pages/page-list/page-list.component';
import { PageEditComponent } from './pages/page-edit/page-edit.component';
import { PageViewComponent } from './pages/page-view/page-view.component';
import { CategoryListComponent } from './categories/category-list/category-list.component';
import { CategoryEditComponent } from './categories/category-edit/category-edit.component';
import { FileUploadComponent } from './shared/file-upload/file-upload.component';
import { ContentEditorComponent } from './shared/content-editor/content-editor.component';

// Generated Proxy Services

// Shared Services
import { AppInitializationService } from '../shared/services/app-initialization.service';

// CMS Routes
import { cmsRoutes } from './cms.routes';
import { PageCategoryService, PageService, PublicPageService } from '../proxy/cms/core';

@NgModule({
  declarations: [
    PageListComponent,
    PageEditComponent,
    PageViewComponent,
    CategoryListComponent,
    CategoryEditComponent,
    FileUploadComponent,
   // NavigationMenuComponent,
    ContentEditorComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    CoreModule,
    ThemeSharedModule,
    NgxEditorModule,
    RouterModule.forChild(cmsRoutes)
  ],
  providers: [
    PageService,
    PageCategoryService,
    PublicPageService,
    AppInitializationService
  ],
  exports: [
  //  NavigationMenuComponent,
    ContentEditorComponent,
    FileUploadComponent
  ]
})
export class CmsModule { }
