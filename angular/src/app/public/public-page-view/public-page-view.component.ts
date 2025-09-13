import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PageDto, PublicPageService } from 'src/app/proxy/cms/core';

@Component({
  selector: 'app-public-page-view',
  templateUrl: './public-page-view.component.html',
  styleUrls: ['./public-page-view.component.scss'],
  standalone: false
})
export class PublicPageViewComponent implements OnInit {
  page: PageDto | null = null;
  loading = true;
  error: string | null = null;

  constructor(
    private publicPageService: PublicPageService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const slug = params['slug'];
      if (slug) {
        this.loadPage(slug);
      }
    });
  }

  private loadPage(slug: string): void {
    this.loading = true;
    this.error = null;

    this.publicPageService.getBySlug(slug).subscribe({
      next: (page) => {
        this.page = page;
        this.loading = false;
        
        // Update page title
        if (page.metaTitle) {
          document.title = page.metaTitle;
        } else {
          document.title = page.title;
        }
      },
      error: (error) => {
        console.error('Error loading page:', error);
        this.error = 'Page not found or not published.';
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/public']);
  }

  goToPages(): void {
    this.router.navigate(['/public/pages']);
  }
}
