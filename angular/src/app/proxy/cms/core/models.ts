import type { FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CreatePageCategoryDto {
  name: string;
  slug: string;
  description?: string;
  color?: string;
  icon?: string;
  sortOrder: number;
  parentId?: string;
}

export interface CreatePageDto {
  title: string;
  slug: string;
  content: string;
  metaTitle?: string;
  metaDescription?: string;
  metaKeywords?: string;
  isMarkdown: boolean;
  isHomePage: boolean;
  sortOrder: number;
  tags?: string;
  author?: string;
}

export interface FileDeleteResultDto {
  success: boolean;
  message?: string;
}

export interface FileInfoDto {
  fileName?: string;
  url?: string;
  size: number;
  created?: string;
}

export interface FileUploadResultDto {
  success: boolean;
  url?: string;
  fileName?: string;
  originalName?: string;
  size: number;
}

export interface GetPageCategoryListDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  isActive?: boolean;
  parentId?: string;
}

export interface GetPageListDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  isPublished?: boolean;
  isMarkdown?: boolean;
  author?: string;
  tags?: string;
}

export interface PageCategoryDto extends FullAuditedEntityDto<string> {
  name?: string;
  slug?: string;
  description?: string;
  color?: string;
  icon?: string;
  sortOrder: number;
  isActive: boolean;
  parentId?: string;
}

export interface PageDto extends FullAuditedEntityDto<string> {
  title?: string;
  slug?: string;
  content?: string;
  metaTitle?: string;
  metaDescription?: string;
  metaKeywords?: string;
  isPublished: boolean;
  isMarkdown: boolean;
  isHomePage: boolean;
  publishedAt?: string;
  sortOrder: number;
  tags?: string;
  author?: string;
}

export interface UpdatePageCategoryDto {
  name: string;
  slug: string;
  description?: string;
  color?: string;
  icon?: string;
  sortOrder: number;
  parentId?: string;
}

export interface UpdatePageDto {
  title: string;
  slug: string;
  content: string;
  metaTitle?: string;
  metaDescription?: string;
  metaKeywords?: string;
  isMarkdown: boolean;
  isHomePage: boolean;
  sortOrder: number;
  tags?: string;
  author?: string;
}
