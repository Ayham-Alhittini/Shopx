import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class EditComponent {

  title: string;
  description: string;
  initialUrl: string = '';
  
  // file: File;
  fileModel;
  name: string;
  isDone = false;
  imageSrc = '';

  constructor(@Inject(MAT_DIALOG_DATA) data) {
    this.title = data.title;
    this.description = data.description;
    this.initialUrl = data.initialUrl;
    if(this.initialUrl) {
      this.imageSrc = this.initialUrl;
    }
  }

  upload(file: File) {
    if(!file) {
      this.fileModel = {}
    }
    
    this.name = file.name;
    this.isDone = true;
    this.imageSrc = URL.createObjectURL(file)
    
    this.fileModel = {
      file: file,
      name: this.name,
      url: this.imageSrc,
    }
    
  }

}
