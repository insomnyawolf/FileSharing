import { Component, OnInit } from '@angular/core';
import { FileService } from '../../services/file.service';

@Component({
  selector: 'app-upload-file',
  templateUrl: './upload-file.component.html',
  styleUrls: ['./upload-file.component.css']
})
export class UploadFileComponent implements OnInit {
  private fileService: FileService;
  private selectedFiles: FileList;
  fileNames: string;
  constructor(fileService: FileService) {
    this.fileService = fileService;
  }

  ngOnInit() {
  }

  handleFileInput(files: FileList) {
    if (files === undefined || files === null || files.length < 1) {
      // Do nothing as there are no files to upload
      return;
    }

    this.selectedFiles = files;

    this.fileNames = '';

    for (let i = 0; i < this.selectedFiles.length; i++) {

      const file = this.selectedFiles.item(i);

      this.fileNames += ` ${file.name}`;
    }
  }

  upload() {
    const formData: FormData = new FormData();

    for (let i = 0; i < this.selectedFiles.length; i++) {

      const file = this.selectedFiles.item(i);

      formData.append('fileKey', file, file.name);
    }

    this.fileService.uploadFiles(formData).subscribe(
      (response: any) => {
        // Executed on sucess
        alert("Upload Sucess!");
      }
    );
  }
}
