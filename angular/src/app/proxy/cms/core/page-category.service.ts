import type { CreatePageCategoryDto, GetPageCategoryListDto, PageCategoryDto, UpdatePageCategoryDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PageCategoryService {
  apiName = 'Default';
  

  activate = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PageCategoryDto>({
      method: 'POST',
      url: `/api/CMS/page-category/${id}/activate`,
    },
    { apiName: this.apiName,...config });
  

  create = (input: CreatePageCategoryDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PageCategoryDto>({
      method: 'POST',
      url: '/api/CMS/page-category',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  deactivate = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PageCategoryDto>({
      method: 'POST',
      url: `/api/CMS/page-category/${id}/deactivate`,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/CMS/page-category/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PageCategoryDto>({
      method: 'GET',
      url: `/api/CMS/page-category/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getActiveCategories = (input: GetPageCategoryListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PageCategoryDto>>({
      method: 'GET',
      url: '/api/CMS/page-category/active-categories',
      params: { filter: input.filter, isActive: input.isActive, parentId: input.parentId, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getBySlug = (slug: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PageCategoryDto>({
      method: 'GET',
      url: '/api/CMS/page-category/by-slug',
      params: { slug },
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetPageCategoryListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PageCategoryDto>>({
      method: 'GET',
      url: '/api/CMS/page-category',
      params: { filter: input.filter, isActive: input.isActive, parentId: input.parentId, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdatePageCategoryDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PageCategoryDto>({
      method: 'PUT',
      url: `/api/CMS/page-category/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
