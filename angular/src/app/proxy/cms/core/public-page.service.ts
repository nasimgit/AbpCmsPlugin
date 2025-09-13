import type { GetPageCategoryListDto, GetPageListDto, PageCategoryDto, PageDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PublicPageService {
  apiName = 'Default';
  

  getActiveCategories = (input: GetPageCategoryListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PageCategoryDto>>({
      method: 'GET',
      url: '/api/CMS/public-page/active-categories',
      params: { filter: input.filter, isActive: input.isActive, parentId: input.parentId, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getBySlug = (slug: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PageDto>({
      method: 'GET',
      url: '/api/CMS/public-page/by-slug',
      params: { slug },
    },
    { apiName: this.apiName,...config });
  

  getCategoryBySlug = (slug: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PageCategoryDto>({
      method: 'GET',
      url: '/api/CMS/public-page/category-by-slug',
      params: { slug },
    },
    { apiName: this.apiName,...config });
  

  getHomePage = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, PageDto>({
      method: 'GET',
      url: '/api/CMS/public-page/home-page',
    },
    { apiName: this.apiName,...config });
  

  getPublishedPages = (input: GetPageListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PageDto>>({
      method: 'GET',
      url: '/api/CMS/public-page/published-pages',
      params: { filter: input.filter, isPublished: input.isPublished, isMarkdown: input.isMarkdown, author: input.author, tags: input.tags, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
