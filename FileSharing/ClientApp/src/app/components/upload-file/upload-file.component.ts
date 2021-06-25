import { HttpResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FileService } from '../../services/file.service';

@Component({
  selector: 'app-upload-file',
  templateUrl: './upload-file.component.html',
  styleUrls: ['./upload-file.component.css']
})
export class UploadFileComponent implements OnInit {
  private fileService: FileService;
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

    const formData: FormData = new FormData();

    for (let i = 0; i < files.length; i++) {

      const file = files.item(i);

      formData.append('fileKey', file, file.name);
    }

    this.fileService.uploadFiles(formData).subscribe(
      (response: HttpResponse<any>) => {
        // Executed on sucess
        const data: any = response.body;
        alert("Upload Sucess!");
      }
    )
  }
}
