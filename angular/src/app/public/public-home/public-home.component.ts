import { Component, OnInit } from '@angular/core';
import { PageDto, PublicPageService } from 'src/app/proxy/cms/core';
import { AppInitializationService } from 'src/app/shared/services/app-initialization.service';

@Component({
  selector: 'app-public-home',
  templateUrl: './public-home.component.html',
  styleUrls: ['./public-home.component.scss'],
  standalone: false
})
export class PublicHomeComponent implements OnInit {
  homePage: PageDto | null = null;
  recentPages: PageDto[] = [];
  loading = true;

  constructor(
    private publicPageService: PublicPageService,
    private appInitializationService: AppInitializationService
  ) {}

  ngOnInit(): void {
    this.loadHomePage();
    this.loadRecentPages();
    this.loadMenuItems();
  }

  private loadHomePage(): void {
    this.publicPageService.getHomePage().subscribe({
      next: (page) => {
        this.homePage = page;
      },
      error: (error) => {
        console.error('Error loading home page:', error);
      }
    });
  }

  private loadRecentPages(): void {
    this.publicPageService.getPublishedPages({
      maxResultCount: 5,
      skipCount: 0,
      sorting: 'creationTime desc'
    }).subscribe({
      next: (result) => {
        this.recentPages = result.items;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading recent pages:', error);
        this.loading = false;
      }
    });
  }

  private loadMenuItems(): void {
    this.appInitializationService.loadMenuIfNeeded();
  }
}
