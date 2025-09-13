import { NgModule } from '@angular/core';
import { PageModule } from '@abp/ng.components/page';
import { SharedModule } from '../shared/shared.module';
import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home.component';
import { AppInitializationService } from '../shared/services/app-initialization.service';
import { PublicPageService } from '../proxy/cms/core';

@NgModule({
  declarations: [HomeComponent],
  imports: [SharedModule, HomeRoutingModule, PageModule],
  providers: [
    PublicPageService,
    AppInitializationService
  ]
})
export class HomeModule {}
