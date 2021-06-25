import { DOCUMENT } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, AfterViewInit, Inject, HostListener } from '@angular/core';
import { PagedResult } from '../../models/PagedResult';
import { SharedFile } from '../../models/sharedfile';
import { FileService } from '../../services/file.service';

@Component({
  selector: 'app-shared-files',
  templateUrl: './shared-files.component.html',
  styleUrls: ['./shared-files.component.css']
})
export class SharedFilesComponent implements OnInit, AfterViewInit {
  private fileService: FileService;
  private document: Document;

  private loadingNextPage: boolean;
  private furtestScroll: number;
  private noMoreResultsAvailable: boolean;

  previewEnabled: boolean;
  files: SharedFile[];

  private searchQueryFilename: string;
  private searchQueryPage: number;
  constructor(fileService: FileService, @Inject(DOCUMENT) document: Document) {
    this.fileService = fileService;
    this.document = document;

    this.searchQueryFilename = '';
    this.searchQueryPage = 1;

    let previewEnabled = localStorage.getItem('previewEnabled');
    if (previewEnabled === null) {
      previewEnabled = 'true';
      localStorage.setItem('previewEnabled', previewEnabled);
    }

    this.previewEnabled = previewEnabled === 'true';
  }

  ngOnInit(): void {
    this.refresh();
  }

  // Need to guess the kind of this event
  changeSearch(targetElement): void {
    const newValue = targetElement.target.value;

    if (this.searchQueryFilename === newValue) {
      // If nothing changed don't call
      return;
    }

    this.searchQueryFilename = newValue;
    this.refresh();
  }

  // Need to guess the kind of this event
  togglePreview(targetElement): void {
    this.previewEnabled = targetElement.target.checked;
    localStorage.setItem('previewEnabled', `${this.previewEnabled}`);
  }

  // Doesn't work yet
  ngAfterViewInit(): void {
    const videoElements = document.getElementsByTagName('video');

    for (let i = 0; i < videoElements.length; i++) {
      const video = videoElements.item(i);
      video.volume = 0.5;
    }

    const audioElements = document.getElementsByTagName('audio');

    for (let i = 0; i < audioElements.length; i++) {
      const audio = audioElements.item(i);
      audio.volume = 0.5;
    }

    console.log('Volument Seteado');
  }

  refresh(): void {
    this.searchQueryPage = 1;
    this.loadingNextPage = false;
    this.furtestScroll = 0;
    this.noMoreResultsAvailable = false;

    this.fileService.getFileInfo(this.searchQueryFilename, this.searchQueryPage).subscribe(
      (response: PagedResult<SharedFile>) => {
        // Executed on sucess
        this.files = response.content;
      },
      (error: HttpErrorResponse) => {
        // Executed on error
      },
      () => {
        // Always Executed
      }
    );
  }

  scrollTop(): void {
    this.document.documentElement.scrollTop = 0;
  }

  @HostListener('window:scroll')
  onWindowScroll(): void {
    const currentScrollTopPosition = this.document.documentElement.scrollTop;
    const maxScrollHeight = this.document.documentElement.scrollHeight;

    if (this.furtestScroll > currentScrollTopPosition && this.furtestScroll !== maxScrollHeight) {
      // Do nothing
      return;
    }

    if (this.noMoreResultsAvailable) {
      // Do nothing
      return;
    }

    if (this.loadingNextPage) {
      // Do nothing
      return;
    }

    this.furtestScroll = currentScrollTopPosition;

    const viewSize = this.document.documentElement.clientHeight;

    const loadNewPagePosition = maxScrollHeight - (viewSize * 1.3);
    if (currentScrollTopPosition > loadNewPagePosition) {
      this.loadingNextPage = true;
      this.loadNextPage();
    }
  }

  loadNextPage(): void {
    this.searchQueryPage++;
    this.fileService.getFileInfo(this.searchQueryFilename, this.searchQueryPage).subscribe(
      (response: PagedResult<SharedFile>) => {
        // Executed on sucess
        if (response.totalPages === this.searchQueryPage) {
          this.noMoreResultsAvailable = true;
        }
        this.files = this.files.concat(response.content);
        this.loadingNextPage = false;
      },
      (error: HttpErrorResponse) => {
        // Executed on error
      },
      () => {
        // Always Executed
      }
    );
  }
}
