import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { FileDeleteResultDto, FileInfoDto, FileUploadResultDto } from '../core/models';

@Injectable({
  providedIn: 'root',
})
export class FileUploadService {
  apiName = 'Default';
  

  deleteImageByFileName = (fileName: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, FileDeleteResultDto>({
      method: 'DELETE',
      url: `/api/upload/image/${fileName}`,
    },
    { apiName: this.apiName,...config });
  

  getUploadedImages = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, FileInfoDto[]>({
      method: 'GET',
      url: '/api/upload/images',
    },
    { apiName: this.apiName,...config });
  

  uploadImage = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, FileUploadResultDto>({
      method: 'POST',
      url: '/api/upload/image',
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
