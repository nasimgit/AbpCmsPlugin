import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ConfirmationService } from '@abp/ng.theme.shared';
import { PageDto, PageService, GetPageListDto } from 'src/app/proxy/cms/core';
import { AppInitializationService } from 'src/app/shared/services/app-initialization.service';

@Component({
  selector: 'app-page-list',
  templateUrl: './page-list.component.html',
  styleUrls: ['./page-list.component.scss'],
  standalone: false
})
export class PageListComponent implements OnInit {
  pages: PageDto[] = [];
  totalCount = 0;
  loading = false;
  filter = '';
  author = '';
  tags = '';
  isMarkdown: boolean | null = null;
  currentPage = 1;
  pageSize = 10;

  constructor(
    private pageService: PageService,
    private router: Router,
    private confirmationService: ConfirmationService,
    private appInitializationService: AppInitializationService
  ) {}

  ngOnInit(): void {
    this.loadPages();
  }

  loadPages(): void {
    this.loading = true;
    const input: GetPageListDto = {
      filter: this.filter,
      author: this.author,
      tags: this.tags,
      isMarkdown: this.isMarkdown,
      skipCount: (this.currentPage - 1) * this.pageSize,
      maxResultCount: this.pageSize,
      sorting: 'creationTime desc'
    };

    this.pageService.getList(input).subscribe({
      next: (result) => {
        this.pages = result.items;
        this.totalCount = result.totalCount;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading pages:', error);
        this.loading = false;
      }
    });
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadPages();
  }

  onFilterChange(): void {
    this.currentPage = 1;
    this.loadPages();
  }

  clearFilters(): void {
    this.filter = '';
    this.author = '';
    this.tags = '';
    this.isMarkdown = null;
    this.onFilterChange();
  }

  createPage(): void {
    this.router.navigate(['/cms/pages/new']);
  }

  editPage(page: PageDto): void {
    this.router.navigate(['/cms/pages/edit', page.id]);
  }

  viewPage(page: PageDto): void {
    this.router.navigate(['/cms/pages/view', page.slug]);
  }

  deletePage(page: PageDto): void {
    this.confirmationService
      .warn('::AreYouSureToDelete', '::AreYouSure')
      .subscribe(status => {
        if (status === 'confirm') {
          this.pageService.delete(page.id).subscribe({
            next: () => {
              this.loadPages();
            },
            error: (error) => {
              console.error('Error deleting page:', error);
            }
          });
        }
      });
  }

  togglePublish(page: PageDto): void {
    if (page.isPublished) {
      this.pageService.unpublish(page.id).subscribe({
        next: () => {
          this.loadPages();
          this.appInitializationService.refreshMenu(); // Refresh menu when page is unpublished
        },
        error: (error) => {
          console.error('Error unpublishing page:', error);
        }
      });
    } else {
      this.pageService.publish(page.id).subscribe({
        next: () => {
          this.loadPages();
          this.appInitializationService.refreshMenu(); // Refresh menu when page is published
        },
        error: (error) => {
          console.error('Error publishing page:', error);
        }
      });
    }
  }

  setAsHomePage(page: PageDto): void {
    this.confirmationService
      .warn('::SetAsHomePageConfirmation', '::AreYouSure')
      .subscribe(status => {
        if (status === 'confirm') {
          this.pageService.setAsHomePage(page.id).subscribe({
            next: () => {
              this.loadPages();
            },
            error: (error) => {
              console.error('Error setting home page:', error);
            }
          });
        }
      });
  }

  getStatusBadgeClass(isPublished: boolean): string {
    return isPublished ? 'badge-success' : 'badge-warning';
  }

  getStatusText(isPublished: boolean): string {
    return isPublished ? 'Published' : 'Draft';
  }

  getPageNumbers(): number[] {
    const totalPages = this.getTotalPages();
    const pages: number[] = [];
    const startPage = Math.max(1, this.currentPage - 2);
    const endPage = Math.min(totalPages, this.currentPage + 2);

    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }
    return pages;
  }

  getTotalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  Math = Math;
}
