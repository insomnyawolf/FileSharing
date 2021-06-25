import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-upload-file',
  templateUrl: './upload-file.component.html',
  styleUrls: ['./upload-file.component.css']
})
export class UploadFileComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  handleFileInput(files: FileList) {
    if (files === undefined || files === null || files.length < 1) {
      // Do nothing as there are no files to upload
      return;
    }
    const endpoint = 'your-destination-url';
    const formData: FormData = new FormData();

    for (var i = 0; i < files.length; i++) {

      // get item
      const file = files.item(i);

      formData.append('fileKey', file, file.name);
    }

    
    
    return this.httpClient
      .post(endpoint, formData, { headers: yourHeadersConfig })
      .map(() => { return true; })
      .catch((e) => this.handleError(e));
  }
}
