import { Injectable, OnInit } from '@angular/core';
import { DynamicMenuService } from './dynamic-menu.service';

@Injectable({
  providedIn: 'root'
})
export class AppInitializationService implements OnInit {
  private menuLoaded = false;

  constructor(private dynamicMenuService: DynamicMenuService) {}

  ngOnInit(): void {
    // This will be called when the service is first injected
    this.loadMenuIfNeeded();
  }

  async loadMenuIfNeeded(): Promise<void> {
    // if (this.menuLoaded) {
    //   return;
    // }

    try {
      console.log('App initialization service: Loading dynamic menu...');
      await this.dynamicMenuService.loadPublishedPagesInMenu();
      this.menuLoaded = true;
      console.log('App initialization service: Dynamic menu loaded successfully');
    } catch (error) {
      console.error('App initialization service: Error loading dynamic menu:', error);
      // Retry after a delay
      setTimeout(() => {
        this.loadMenuIfNeeded();
      }, 2000);
    }
  }

  refreshMenu(): void {
    this.menuLoaded = false;
    this.loadMenuIfNeeded();
  }
}

