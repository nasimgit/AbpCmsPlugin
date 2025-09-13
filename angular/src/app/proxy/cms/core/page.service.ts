import type { CreatePageDto, GetPageListDto, PageDto, UpdatePageDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PageService {
  apiName = 'Default';
  

  create = (input: CreatePageDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PageDto>({
      method: 'POST',
      url: '/api/CMS/page',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/CMS/page/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PageDto>({
      method: 'GET',
      url: `/api/CMS/page/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getBySlug = (slug: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PageDto>({
      method: 'GET',
      url: '/api/CMS/page/by-slug',
      params: { slug },
    },
    { apiName: this.apiName,...config });
  

  getHomePage = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, PageDto>({
      method: 'GET',
      url: '/api/CMS/page/home-page',
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetPageListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PageDto>>({
      method: 'GET',
      url: '/api/CMS/page',
      params: { filter: input.filter, isPublished: input.isPublished, isMarkdown: input.isMarkdown, author: input.author, tags: input.tags, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getPublishedPages = (input: GetPageListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PageDto>>({
      method: 'GET',
      url: '/api/CMS/page/published-pages',
      params: { filter: input.filter, isPublished: input.isPublished, isMarkdown: input.isMarkdown, author: input.author, tags: input.tags, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  publish = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PageDto>({
      method: 'POST',
      url: `/api/CMS/page/${id}/publish`,
    },
    { apiName: this.apiName,...config });
  

  setAsHomePage = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PageDto>({
      method: 'POST',
      url: `/api/CMS/page/${id}/set-as-home-page`,
    },
    { apiName: this.apiName,...config });
  

  unpublish = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PageDto>({
      method: 'POST',
      url: `/api/CMS/page/${id}/unpublish`,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdatePageDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PageDto>({
      method: 'PUT',
      url: `/api/CMS/page/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
