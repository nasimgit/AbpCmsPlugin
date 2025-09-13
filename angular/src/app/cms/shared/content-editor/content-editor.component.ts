import { Component, Input, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { Editor, Toolbar, Validators } from 'ngx-editor';

@Component({
  selector: 'app-content-editor',
  templateUrl: './content-editor.component.html',
  styleUrls: ['./content-editor.component.scss'],
  standalone: false
})
export class ContentEditorComponent implements OnInit, OnDestroy {
  @Input() content = '';
  @Input() isMarkdown = false;
  @Input() placeholder = 'Enter content...';
  @Output() contentChange = new EventEmitter<string>();
  @Output() isMarkdownChange = new EventEmitter<boolean>();

  contentControl = new FormControl('');
  private destroy$ = new Subject<void>();
  editor!: Editor;

  // ngx-editor toolbar configuration
  toolbar: Toolbar = [
    ['bold', 'italic', 'underline', 'strike'],
    ['code', 'blockquote'],
    ['ordered_list', 'bullet_list'],
    [{ heading: ['h1', 'h2', 'h3', 'h4', 'h5', 'h6'] }],
    ['link', 'image'],
    ['text_color', 'background_color'],
    ['align_left', 'align_center', 'align_right', 'align_justify'],
    ['horizontal_rule', 'format_clear'],
  ];


  ngOnInit(): void {
    this.contentControl.setValue(this.content);
    
    // Initialize ngx-editor
    this.editor = new Editor();
    
    this.contentControl.valueChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe(value => {
        this.contentChange.emit(value || '');
      });
  }


  ngOnDestroy(): void {
    this.editor.destroy();
    this.destroy$.next();
    this.destroy$.complete();
  }

  toggleEditorType(): void {
    this.isMarkdown = !this.isMarkdown;
    this.isMarkdownChange.emit(this.isMarkdown);
    
    if (this.isMarkdown) {
      // Destroy ngx-editor when switching to Markdown
      if (this.editor) {
        this.editor.destroy();
      }
    } else {
      // Re-initialize ngx-editor when switching to WYSIWYG
      this.editor = new Editor();
    }
  }


  previewContent(): void {
    // This would open a preview modal or navigate to preview
    console.log('Preview content:', this.contentControl.value);
  }

  getCharacterCount(): number {
    const content = this.contentControl.value || '';
    // For HTML content, strip HTML tags to get actual text length
    if (!this.isMarkdown) {
      const tempDiv = document.createElement('div');
      tempDiv.innerHTML = content;
      return tempDiv.textContent?.length || 0;
    }
    return content.length;
  }

  getCharacterCountClass(): string {
    const count = this.getCharacterCount();
    if (count > 8000) {
      return 'text-danger font-weight-bold';
    } else if (count > 7000) {
      return 'text-warning font-weight-bold';
    } else if (count > 6000) {
      return 'text-warning';
    }
    return 'text-muted';
  }

}
