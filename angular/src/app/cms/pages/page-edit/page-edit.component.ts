import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CreatePageDto, PageDto, PageService, UpdatePageDto } from 'src/app/proxy/cms/core';

@Component({
  selector: 'app-page-edit',
  templateUrl: './page-edit.component.html',
  styleUrls: ['./page-edit.component.scss'],
  standalone: false
})
export class PageEditComponent implements OnInit {
  pageForm: FormGroup;
  isEdit = false;
  pageId: string | null = null;
  loading = false;
  saving = false;
  currentPage: PageDto | null = null;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private pageService: PageService
  ) {
    this.pageForm = this.createForm();
  }

  ngOnInit(): void {
    this.pageId = this.route.snapshot.paramMap.get('id');
    this.isEdit = !!this.pageId;

    if (this.isEdit && this.pageId) {
      this.loadPage();
    }
  }

  private createForm(): FormGroup {
    return this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(200)]],
      slug: ['', [Validators.required, Validators.maxLength(200)]],
      content: ['', [Validators.required, Validators.maxLength(8000)]],
      metaTitle: ['', [Validators.maxLength(200)]],
      metaDescription: ['', [Validators.maxLength(500)]],
      metaKeywords: ['', [Validators.maxLength(200)]],
      isMarkdown: [false],
      isHomePage: [false],
      sortOrder: [0, [Validators.min(0)]],
      tags: ['', [Validators.maxLength(500)]],
      author: ['', [Validators.maxLength(100)]]
    });
  }

  private loadPage(): void {
    if (!this.pageId) return;

    this.loading = true;
    this.pageService.get(this.pageId).subscribe({
      next: (page) => {
        this.currentPage = page;
        this.pageForm.patchValue({
          title: page.title,
          slug: page.slug,
          content: page.content,
          metaTitle: page.metaTitle || '',
          metaDescription: page.metaDescription || '',
          metaKeywords: page.metaKeywords || '',
          isMarkdown: page.isMarkdown,
          isHomePage: page.isHomePage,
          sortOrder: page.sortOrder,
          tags: page.tags || '',
          author: page.author || ''
        });
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading page:', error);
        this.loading = false;
      }
    });
  }

  onContentChange(content: string): void {
    this.pageForm.patchValue({ content });
  }

  onIsMarkdownChange(isMarkdown: boolean): void {
    this.pageForm.patchValue({ isMarkdown });
  }

  generateSlug(): void {
    const title = this.pageForm.get('title')?.value;
    if (title) {
      const slug = title
        .toLowerCase()
        .replace(/[^a-z0-9\s-]/g, '')
        .replace(/\s+/g, '-')
        .replace(/-+/g, '-')
        .trim();
      this.pageForm.patchValue({ slug });
    }
  }

  onSubmit(): void {
    if (this.pageForm.valid && !this.saving) {
      this.saving = true;
      const formValue = this.pageForm.value;

      if (this.isEdit && this.pageId) {
        const updateDto: UpdatePageDto = {
          title: formValue.title,
          slug: formValue.slug,
          content: formValue.content,
          metaTitle: formValue.metaTitle || undefined,
          metaDescription: formValue.metaDescription || undefined,
          metaKeywords: formValue.metaKeywords || undefined,
          isMarkdown: formValue.isMarkdown,
          isHomePage: formValue.isHomePage,
          sortOrder: formValue.sortOrder,
          tags: formValue.tags || undefined,
          author: formValue.author || undefined
        };

        this.pageService.update(this.pageId, updateDto).subscribe({
          next: (page) => {
            this.saving = false;
            this.router.navigate(['/cms/pages']);
          },
          error: (error) => {
            console.error('Error updating page:', error);
            this.saving = false;
          }
        });
      } else {
        const createDto: CreatePageDto = {
          title: formValue.title,
          slug: formValue.slug,
          content: formValue.content,
          metaTitle: formValue.metaTitle || undefined,
          metaDescription: formValue.metaDescription || undefined,
          metaKeywords: formValue.metaKeywords || undefined,
          isMarkdown: formValue.isMarkdown,
          isHomePage: formValue.isHomePage,
          sortOrder: formValue.sortOrder,
          tags: formValue.tags || undefined,
          author: formValue.author || undefined
        };

        this.pageService.create(createDto).subscribe({
          next: (page) => {
            this.saving = false;
            this.router.navigate(['/cms/pages']);
          },
          error: (error) => {
            console.error('Error creating page:', error);
            this.saving = false;
          }
        });
      }
    }
  }

  onCancel(): void {
    this.router.navigate(['/cms/pages']);
  }

  getFieldError(fieldName: string): string {
    const field = this.pageForm.get(fieldName);
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
