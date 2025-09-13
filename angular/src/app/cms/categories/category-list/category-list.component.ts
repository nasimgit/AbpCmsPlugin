import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ConfirmationService } from '@abp/ng.theme.shared';
import { PageCategoryDto, PageCategoryService, GetPageCategoryListDto } from 'src/app/proxy/cms/core';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.scss'],
  standalone: false
})
export class CategoryListComponent implements OnInit {
  categories: PageCategoryDto[] = [];
  totalCount = 0;
  loading = false;
  filter = '';
  parentId: string | null = null;
  currentPage = 1;
  pageSize = 10;

  constructor(
    private pageCategoryService: PageCategoryService,
    private router: Router,
    private confirmationService: ConfirmationService
  ) {}

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.loading = true;
    const input: GetPageCategoryListDto = {
      filter: this.filter,
      parentId: this.parentId || undefined,
      skipCount: (this.currentPage - 1) * this.pageSize,
      maxResultCount: this.pageSize,
      sorting: 'sortOrder asc, name asc'
    };

    this.pageCategoryService.getList(input).subscribe({
      next: (result) => {
        this.categories = result.items;
        this.totalCount = result.totalCount;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading categories:', error);
        this.loading = false;
      }
    });
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadCategories();
  }

  onFilterChange(): void {
    this.currentPage = 1;
    this.loadCategories();
  }

  clearFilters(): void {
    this.filter = '';
    this.parentId = null;
    this.onFilterChange();
  }

  createCategory(): void {
    this.router.navigate(['/cms/categories/new']);
  }

  editCategory(category: PageCategoryDto): void {
    this.router.navigate(['/cms/categories/edit', category.id]);
  }

  deleteCategory(category: PageCategoryDto): void {
    this.confirmationService
      .warn('::AreYouSureToDelete', '::AreYouSure')
      .subscribe(status => {
        if (status === 'confirm') {
          this.pageCategoryService.delete(category.id).subscribe({
            next: () => {
              this.loadCategories();
            },
            error: (error) => {
              console.error('Error deleting category:', error);
            }
          });
        }
      });
  }

  toggleActive(category: PageCategoryDto): void {
    if (category.isActive) {
      this.pageCategoryService.deactivate(category.id).subscribe({
        next: () => {
          this.loadCategories();
        },
        error: (error) => {
          console.error('Error deactivating category:', error);
        }
      });
    } else {
      this.pageCategoryService.activate(category.id).subscribe({
        next: () => {
          this.loadCategories();
        },
        error: (error) => {
          console.error('Error activating category:', error);
        }
      });
    }
  }

  getStatusBadgeClass(isActive: boolean): string {
    return isActive ? 'badge-success' : 'badge-secondary';
  }

  getStatusText(isActive: boolean): string {
    return isActive ? 'Active' : 'Inactive';
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
