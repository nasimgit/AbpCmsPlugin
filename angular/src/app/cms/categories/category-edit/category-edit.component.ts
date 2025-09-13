import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CreatePageCategoryDto, PageCategoryDto, PageCategoryService, UpdatePageCategoryDto } from 'src/app/proxy/cms/core';

@Component({
  selector: 'app-category-edit',
  templateUrl: './category-edit.component.html',
  styleUrls: ['./category-edit.component.scss'],
  standalone: false
})
export class CategoryEditComponent implements OnInit {
  categoryForm: FormGroup;
  isEdit = false;
  categoryId: string | null = null;
  loading = false;
  saving = false;
  currentCategory: PageCategoryDto | null = null;
  availableCategories: PageCategoryDto[] = [];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private pageCategoryService: PageCategoryService
  ) {
    this.categoryForm = this.createForm();
  }

  ngOnInit(): void {
    this.categoryId = this.route.snapshot.paramMap.get('id');
    this.isEdit = !!this.categoryId;

    this.loadAvailableCategories();

    if (this.isEdit && this.categoryId) {
      this.loadCategory();
    }
  }

  private createForm(): FormGroup {
    return this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      slug: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', [Validators.maxLength(500)]],
      color: ['', [Validators.maxLength(20)]],
      icon: ['', [Validators.maxLength(50)]],
      sortOrder: [0, [Validators.min(0)]],
      parentId: [null]
    });
  }

  private loadAvailableCategories(): void {
    this.pageCategoryService.getList({
      skipCount: 0,
      maxResultCount: 1000,
      sorting: 'name asc'
    }).subscribe({
      next: (result) => {
        // Filter out current category if editing
        this.availableCategories = result.items.filter(cat => 
          !this.isEdit || cat.id !== this.categoryId
        );
      },
      error: (error) => {
        console.error('Error loading categories:', error);
      }
    });
  }

  private loadCategory(): void {
    if (!this.categoryId) return;

    this.loading = true;
    this.pageCategoryService.get(this.categoryId).subscribe({
      next: (category) => {
        this.currentCategory = category;
        this.categoryForm.patchValue({
          name: category.name,
          slug: category.slug,
          description: category.description || '',
          color: category.color || '',
          icon: category.icon || '',
          sortOrder: category.sortOrder,
          parentId: category.parentId || null
        });
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading category:', error);
        this.loading = false;
      }
    });
  }

  generateSlug(): void {
    const name = this.categoryForm.get('name')?.value;
    if (name) {
      const slug = name
        .toLowerCase()
        .replace(/[^a-z0-9\s-]/g, '')
        .replace(/\s+/g, '-')
        .replace(/-+/g, '-')
        .trim();
      this.categoryForm.patchValue({ slug });
    }
  }

  onSubmit(): void {
    if (this.categoryForm.valid && !this.saving) {
      this.saving = true;
      const formValue = this.categoryForm.value;

      if (this.isEdit && this.categoryId) {
        const updateDto: UpdatePageCategoryDto = {
          name: formValue.name,
          slug: formValue.slug,
          description: formValue.description || undefined,
          color: formValue.color || undefined,
          icon: formValue.icon || undefined,
          sortOrder: formValue.sortOrder,
          parentId: formValue.parentId || undefined
        };

        this.pageCategoryService.update(this.categoryId, updateDto).subscribe({
          next: (category) => {
            this.saving = false;
            this.router.navigate(['/cms/categories']);
          },
          error: (error) => {
            console.error('Error updating category:', error);
            this.saving = false;
          }
        });
      } else {
        const createDto: CreatePageCategoryDto = {
          name: formValue.name,
          slug: formValue.slug,
          description: formValue.description || undefined,
          color: formValue.color || undefined,
          icon: formValue.icon || undefined,
          sortOrder: formValue.sortOrder,
          parentId: formValue.parentId || undefined
        };

        this.pageCategoryService.create(createDto).subscribe({
          next: (category) => {
            this.saving = false;
            this.router.navigate(['/cms/categories']);
          },
          error: (error) => {
            console.error('Error creating category:', error);
            this.saving = false;
          }
        });
      }
    }
  }

  onCancel(): void {
    this.router.navigate(['/cms/categories']);
  }

  getFieldError(fieldName: string): string {
    const field = this.categoryForm.get(fieldName);
    if (field?.errors && field.touched) {
      if (field.errors['required']) {
        return `${fieldName} is required`;
      }
      if (field.errors['maxlength']) {
        return `${fieldName} is too long`;
      }
      if (field.errors['min']) {
        return `${fieldName} must be greater than or equal to ${field.errors['min'].min}`;
      }
    }
    return '';
  }
}
