import { Injectable } from '@angular/core';
import { RoutesService } from '@abp/ng.core';
import { PageDto, PublicPageService } from 'src/app/proxy/cms/core';

@Injectable({
  providedIn: 'root'
})
export class DynamicMenuService {
  private menuItemsLoaded = false;

  constructor(
    private routesService: RoutesService,
    private publicPageService: PublicPageService
  ) {}

  async loadPublishedPagesInMenu(): Promise<void> {
    // if (this.menuItemsLoaded) {
    //   return; // Already loaded
    // }

    try {
      // Get all published pages
      const result = await this.publicPageService.getPublishedPages({
        maxResultCount: 100, // Adjust as needed
        skipCount: 0,
        sorting: 'sortOrder asc, creationTime desc'
      }).toPromise();

      // Always add "All Pages" as the first item (no parent - top level)
      const allPagesRoute = {
        path: 'public/pages',
        name: 'All Pages',
        iconClass: 'fas fa-list',
        order: 100 // This will appear first among public pages
      };

      const routesToAdd = [allPagesRoute];

      if (result && result.items && result.items.length > 0) {
        // Create flat menu structure - all pages as individual top-level menu items
        const pageRoutes = result.items.map((page: PageDto, index: number) => ({
          path: `public/page/${page.slug}`,
          name: page.title,
          iconClass: 'fas fa-file-alt',
          order: 101 + index // Start from 101, after "All Pages"
        }));

        routesToAdd.push(...pageRoutes);
      }
      
      // Add all routes including "All Pages" and individual pages
      this.routesService.add(routesToAdd);
      this.menuItemsLoaded = true;
      
    } catch (error) {
      console.error('Error loading published pages for menu:', error);
      // Don't set menuItemsLoaded to true on error, so it can retry
    }
  }

  refreshMenu(): void {
    this.menuItemsLoaded = false;
    this.loadPublishedPagesInMenu();
  }
}
