import type { FileDeleteResultDto, FileInfoDto, FileUploadResultDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class FileService {
  apiName = 'Default';
  

  deleteImage = (fileName: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, FileDeleteResultDto>({
      method: 'DELETE',
      url: '/api/CMS/file/image',
      params: { fileName },
    },
    { apiName: this.apiName,...config });
  

  getUploadedImages = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, FileInfoDto[]>({
      method: 'GET',
      url: '/api/CMS/file/uploaded-images',
    },
    { apiName: this.apiName,...config });
  

  uploadImage = (fileBytes: number[], fileName: string, contentType: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, FileUploadResultDto>({
      method: 'POST',
      url: '/api/CMS/file/upload-image',
      params: { fileName, contentType },
      body: fileBytes,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
