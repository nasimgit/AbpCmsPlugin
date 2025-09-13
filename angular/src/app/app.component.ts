import { Component, OnInit } from '@angular/core';
import { AppInitializationService } from './shared/services/app-initialization.service';

@Component({
  standalone: false,
  selector: 'app-root',
  template: `
    <abp-loader-bar></abp-loader-bar>
    <abp-dynamic-layout></abp-dynamic-layout>
  `,
})
export class AppComponent implements OnInit {
  constructor(private appInitializationService: AppInitializationService) {}

  ngOnInit(): void {
    // This will trigger the menu loading when the app component initializes
    this.appInitializationService.loadMenuIfNeeded();
  }
}
