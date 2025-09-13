import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { PageDto, PageCategoryDto, PageService, PageCategoryService } from 'src/app/proxy/cms/core';
    
@Component({
  selector: 'app-navigation-menu',
  templateUrl: './navigation-menu.component.html',
  styleUrls: ['./navigation-menu.component.scss']
})
export class NavigationMenuComponent implements OnInit {
  @Input() showHomePage = true;
  @Input() showCategories = true;
  @Input() showPages = true;
  @Input() maxItems = 10;
  @Output() menuItemClick = new EventEmitter<{type: string, item: any}>();

  homePage: PageDto | null = null;
  publishedPages: PageDto[] = [];
  categories: PageCategoryDto[] = [];
  loading = false;

  constructor(
    private cmsService: PageService,
    private pageCategoryService: PageCategoryService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadNavigationData();
  }

  loadNavigationData(): void {
    this.loading = true;

    // Load home page
    if (this.showHomePage) {
      this.cmsService.getHomePage().subscribe({
        next: (homePage) => {
          //this.homePage = homePage;
        },
        error: (error) => {
          console.error('Error loading home page:', error);
        }
      });
    }

    // Load published pages
    if (this.showPages) {
      this.cmsService.getPublishedPages({
        skipCount: 0,
        maxResultCount: this.maxItems,
        sorting: 'sortOrder asc, creationTime desc'
      }).subscribe({
        next: (result) => {
          this.publishedPages = result.items.filter(page => !page.isHomePage);
        },
        error: (error) => {
          console.error('Error loading published pages:', error);
        }
      });
    }

    // Load categories
    if (this.showCategories) {
      this.pageCategoryService.getActiveCategories({
        skipCount: 0,
        maxResultCount: this.maxItems,
        sorting: 'sortOrder asc, name asc'
      }).subscribe({
        next: (result) => {
          this.categories = result.items;
          this.loading = false;
        },
        error: (error) => {
          console.error('Error loading categories:', error);
          this.loading = false;
        }
      });
    } else {
      this.loading = false;
    }
  }

  onMenuItemClick(type: string, item: any): void {
    this.menuItemClick.emit({ type, item });

    switch (type) {
      case 'home':
        this.router.navigate(['/']);
        break;
      case 'page':
        this.router.navigate(['/cms/pages/view', item.slug]);
        break;
      case 'category':
        this.router.navigate(['/cms/categories/view', item.slug]);
        break;
    }
  }

  getPageUrl(page: PageDto): string {
    return `/cms/pages/view/${page.slug}`;
  }

  getCategoryUrl(category: PageCategoryDto): string {
    return `/cms/categories/view/${category.slug}`;
  }
}
