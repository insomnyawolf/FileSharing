import { Component, Inject, Input, OnInit } from '@angular/core';
import { SharedFile } from '../../../models/sharedfile';

export enum MultimediaType {
  None,
  Audio,
  Video,
  Image,
}

@Component({
  selector: 'app-file',
  templateUrl: './file.component.html',
  styleUrls: ['./file.component.css']
})
export class FileComponent implements OnInit {
  private apiUrl: string;

  @Input()
  file: SharedFile;

  @Input()
  preview: boolean;

  fileUrl: string;
  previewUrl: string;

  mediaType: MultimediaType;
  multimediaType: typeof MultimediaType = MultimediaType;
  constructor(@Inject('API_URL') baseUrl: string)
  {
    this.apiUrl = baseUrl;
  }

  ngOnInit() {

    const mediatype = this.file.contentType.split('/')[0].toLowerCase();
    switch (mediatype) {
      case 'audio':
        this.mediaType = MultimediaType.Audio;
        break;
      case 'video':
        this.mediaType = MultimediaType.Video;
        break;
      case 'image':
        this.mediaType = MultimediaType.Image;
        break;
      default:
        this.mediaType = MultimediaType.None;
        break;
    }

    this.fileUrl = `${this.apiUrl}/File/Download?filename=${this.file.url}&preview=${false}`;
    this.previewUrl = `${this.apiUrl}/File/Download?filename=${this.file.url}&preview=${true}`;
  }
}


