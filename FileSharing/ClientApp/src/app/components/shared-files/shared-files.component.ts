import { DOCUMENT } from '@angular/common';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Component, OnInit, AfterViewInit, Inject } from '@angular/core';
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

  previewEnabled: boolean;
  files: SharedFile[];

  private searchQueryFilename: string;
  private searchQueryPage: number;
  constructor(fileService: FileService, @Inject(DOCUMENT) document: Document) {
    this.fileService = fileService;
    this.document = document;
    this.previewEnabled = true;
    this.searchQueryFilename = '';
    this.searchQueryPage = 1;
  }

  ngOnInit() {
    this.refresh();
  }

  // Need to guess the kind of this event
  changeSearch(targetElement) {
    const newValue = targetElement.target.value;

    if (this.searchQueryFilename === newValue) {
      // If nothing changed don't call
      return;
    }

    this.searchQueryFilename = newValue;
    this.searchQueryPage = 1;
    this.refresh();
  }

  // Need to guess the kind of this event
  togglePreview(targetElement) {
    this.previewEnabled = targetElement.target.checked;
  }

  // Doesn't work yet
  ngAfterViewInit() {
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


  scrollTop() {
    this.document.documentElement.scrollTop = 0;
  }

  refresh() {
    this.fileService.getFileInfo(this.searchQueryFilename, this.searchQueryPage).subscribe(
      (response: PagedResult<SharedFile>) => {
        // Executed on sucess
        this.files = response.content;
        console.table(this.files);
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
