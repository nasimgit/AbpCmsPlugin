import { Component, OnInit } from '@angular/core';
import { GetPageListDto, PageDto, PublicPageService } from 'src/app/proxy/cms/core';

@Component({
  selector: 'app-public-page-list',
  templateUrl: './public-page-list.component.html',
  styleUrls: ['./public-page-list.component.scss'],
  standalone: false
})
export class PublicPageListComponent implements OnInit {
  pages: PageDto[] = [];
  loading = true;
  totalCount = 0;
  currentPage = 1;
  pageSize = 10;
  searchTerm = '';
  Math = Math;

  constructor(private publicPageService: PublicPageService) {}

  ngOnInit(): void {
    this.loadPages();
  }

  loadPages(): void {
    this.loading = true;
    
    const input: GetPageListDto = {
      maxResultCount: this.pageSize,
      skipCount: (this.currentPage - 1) * this.pageSize,
      sorting: 'sortOrder asc, creationTime desc',
      filter: this.searchTerm || undefined
    };

    this.publicPageService.getPublishedPages(input).subscribe({
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

  onSearch(): void {
    this.currentPage = 1;
    this.loadPages();
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadPages();
  }

  get totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  get pagesArray(): number[] {
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }
}
