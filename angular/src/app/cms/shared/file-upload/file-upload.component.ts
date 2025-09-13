import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { RestService } from '@abp/ng.core';
import { FileUploadResultDto, FileInfoDto } from 'src/app/proxy/cms/core';


@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.scss'],
  standalone: false
})
export class FileUploadComponent implements OnInit {
  @Input() multiple = false;
  @Input() accept = 'image/*';
  @Input() maxSize = 5 * 1024 * 1024; // 5MB
  @Output() fileUploaded = new EventEmitter<FileUploadResultDto>();
  @Output() filesUploaded = new EventEmitter<FileUploadResultDto[]>();

  uploadedFiles: FileInfoDto[] = [];
  uploading = false;
  dragOver = false;
  errorMessage = '';

  constructor(private restService: RestService) {}

  ngOnInit(): void {
    this.loadUploadedFiles();
  }

  isImageFile(fileName: string): boolean {
    return /\.(jpg|jpeg|png|gif|webp)$/i.test(fileName);
  }

  loadUploadedFiles(): void {
    this.restService.request({
      method: 'GET',
      url: '/api/upload/images'
    }).subscribe({
      next: (files: FileInfoDto[]) => {
        this.uploadedFiles = files;
      },
      error: (error) => {
        console.error('Error loading uploaded files:', error);
      }
    });
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    this.dragOver = true;
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    this.dragOver = false;
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    this.dragOver = false;
    
    const files = event.dataTransfer?.files;
    if (files && files.length > 0) {
      this.handleFiles(files);
    }
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.handleFiles(input.files);
    }
  }

  private handleFiles(files: FileList): void {
    this.errorMessage = '';
    const fileArray = Array.from(files);
    
    // Validate files
    for (const file of fileArray) {
      if (!this.validateFile(file)) {
        return;
      }
    }

    if (this.multiple) {
      this.uploadMultipleFiles(fileArray);
    } else {
      this.uploadSingleFile(fileArray[0]);
    }
  }

  private validateFile(file: File): boolean {
    // Check file size
    if (file.size > this.maxSize) {
      this.errorMessage = `File "${file.name}" is too large. Maximum size is ${this.maxSize / (1024 * 1024)}MB.`;
      return false;
    }

    // Check file type
    if (this.accept && !this.isFileTypeAccepted(file)) {
      this.errorMessage = `File "${file.name}" is not an accepted file type.`;
      return false;
    }

    return true;
  }

  private isFileTypeAccepted(file: File): boolean {
    const acceptedTypes = this.accept.split(',').map(type => type.trim());
    return acceptedTypes.some(type => {
      if (type.endsWith('/*')) {
        const category = type.slice(0, -2);
        return file.type.startsWith(category);
      }
      return file.type === type;
    });
  }

  private uploadSingleFile(file: File): void {
    this.uploading = true;
    const formData = new FormData();
    formData.append('file', file);

    this.restService.request({
      method: 'POST',
      url: '/api/upload/image',
      body: formData
    }).subscribe({
      next: (result: FileUploadResultDto) => {
        this.uploading = false;
        this.fileUploaded.emit(result);
        this.loadUploadedFiles();
      },
      error: (error) => {
        this.uploading = false;
        this.errorMessage = error.error?.error?.message || 'Upload failed';
        console.error('Upload error:', error);
      }
    });
  }

  private uploadMultipleFiles(files: File[]): void {
    this.uploading = true;
    const uploadPromises = files.map(file => {
      const formData = new FormData();
      formData.append('file', file);
      return this.restService.request({
        method: 'POST',
        url: '/api/upload/image',
        body: formData
      }).toPromise();
    });

    Promise.all(uploadPromises).then(results => {
      this.uploading = false;
      this.filesUploaded.emit(results.filter(r => r !== undefined) as FileUploadResultDto[]);
      this.loadUploadedFiles();
    }).catch(error => {
      this.uploading = false;
      this.errorMessage = 'Some files failed to upload';
      console.error('Upload error:', error);
    });
  }

  deleteFile(file: FileInfoDto): void {
    this.restService.request({
      method: 'DELETE',
      url: `/api/upload/image/${file.fileName}`
    }).subscribe({
      next: () => {
        this.loadUploadedFiles();
      },
      error: (error) => {
        console.error('Error deleting file:', error);
      }
    });
  }

  selectFile(file: FileInfoDto): void {
    this.fileUploaded.emit({
      success: true,
      url: file.url,
      fileName: file.fileName,
      originalName: file.fileName,
      size: file.size
    });
  }

  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  }
}
