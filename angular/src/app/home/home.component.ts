import { AuthService } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AppInitializationService } from '../shared/services/app-initialization.service';
import { PageDto, PublicPageService } from '../proxy/cms/core';

@Component({
  standalone: false,
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  homePage: PageDto | null = null;
  recentPages: PageDto[] = [];
  loading = true;

  get hasLoggedIn(): boolean {
    return this.authService.isAuthenticated
  }

  constructor(
    private authService: AuthService,
    private publicPageService: PublicPageService,
    private appInitializationService: AppInitializationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadHomePage();
    this.loadRecentPages();
    this.loadMenuItems();
  }

  login() {
    this.authService.navigateToLogin();
  }

  private loadHomePage(): void {
    this.publicPageService.getHomePage().subscribe({
      next: (page) => {
        this.homePage = page;
        // If no home page is set, redirect to all pages
        if (!page) {
          this.router.navigate(['/public/pages']);
        }
      },
      error: (error) => {
        console.error('Error loading home page:', error);
        // If error loading home page, redirect to all pages
        this.router.navigate(['/public/pages']);
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
