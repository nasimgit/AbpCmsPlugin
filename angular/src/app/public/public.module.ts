import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

// ABP Modules
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';

// Public Components
import { PublicPageListComponent } from './public-page-list/public-page-list.component';
import { PublicPageViewComponent } from './public-page-view/public-page-view.component';
import { PublicHomeComponent } from './public-home/public-home.component';

// Generated Proxy Services
import { PublicPageService } from 'src/app/proxy/cms/core';

// Shared Services
import { AppInitializationService } from '../shared/services/app-initialization.service';

// Public Routes
import { publicRoutes } from './public.routes';

@NgModule({
  declarations: [
    PublicPageListComponent,
    PublicPageViewComponent,
    PublicHomeComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild(publicRoutes),
    CoreModule,
    ThemeSharedModule
  ],
  providers: [
    PublicPageService,
    AppInitializationService
  ]
})
export class PublicModule { }
