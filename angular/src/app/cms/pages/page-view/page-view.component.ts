import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { marked } from 'marked';
import { PageDto, PublicPageService } from 'src/app/proxy/cms/core';

@Component({
  selector: 'app-page-view',
  templateUrl: './page-view.component.html',
  styleUrls: ['./page-view.component.scss'],
  standalone: false
})
export class PageViewComponent implements OnInit {
  page: PageDto | null = null;
  loading = false;
  error: string | null = null;
  renderedContent = '';

  constructor(
    private route: ActivatedRoute,
    private publicPageService: PublicPageService
  ) {}

  ngOnInit(): void {
    const slug = this.route.snapshot.paramMap.get('slug');
    if (slug) {
      this.loadPage(slug);
    } else {
      this.error = 'Page slug not provided';
    }
  }

  loadPage(slug: string): void {
    this.loading = true;
    this.error = null;

    this.publicPageService.getBySlug(slug).subscribe({
      next: async (page) => {
        this.page = page;
        await this.renderContent();
        this.loading = false;
      },
      error: (error) => {
        this.error = error.error?.error?.message || 'Page not found';
        this.loading = false;
      }
    });
  }

  private async renderContent(): Promise<void> {
    if (!this.page) return;

    if (this.page.isMarkdown) {
      // Render Markdown to HTML
      this.renderedContent = await marked(this.page.content);
    } else {
      // Use HTML content directly
      this.renderedContent = this.page.content;
    }
  }

  getPageTitle(): string {
    return this.page?.metaTitle || this.page?.title || 'Page';
  }

  getPageDescription(): string {
    return this.page?.metaDescription || '';
  }

  getPageKeywords(): string {
    return this.page?.metaKeywords || '';
  }
}
