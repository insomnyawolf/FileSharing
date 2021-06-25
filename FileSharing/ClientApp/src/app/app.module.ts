import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { UploadFileComponent } from './components/upload-file/upload-file.component';
import { SharedFilesComponent } from './components/shared-files/shared-files.component';
import { FileComponent } from './components/shared-files/file/file.component';
import { FileService } from './services/file.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    UploadFileComponent,
    SharedFilesComponent,
    FileComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: SharedFilesComponent, pathMatch: 'full' },
      { path: 'Upload', component: UploadFileComponent },
], { relativeLinkResolution: 'legacy' })
  ],
  providers: [
    FileService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
