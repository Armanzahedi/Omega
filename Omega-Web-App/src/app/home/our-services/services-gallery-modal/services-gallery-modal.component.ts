import { AlertifyService } from './../../../_services/alertify.service';
import { ServicesService } from './../../../_services/services.service';
import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA ,MatDialog, MatDialogRef} from '@angular/material/dialog';

@Component({
  selector: 'app-services-gallery-modal',
  templateUrl: './services-gallery-modal.component.html',
  styleUrls: ['./services-gallery-modal.component.css']
})
export class ServicesGalleryModalComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: any,private servicesService: ServicesService,private dialogRef:MatDialogRef<ServicesGalleryModalComponent>) { }
  images: string[]
  ngOnInit() {
    this.loadImages();
  }
  loadImages(){
    this.servicesService
    .getServiceImages(this.data.serviceId)
    .subscribe((res: Array<string>) => {
      this.images = res;
  },error => {
    this.dialogRef.close();
  });
  }
  showImage(imgs) {
    var expandImg: any = document.getElementById("expandedImg");
    expandImg.src = imgs;
    expandImg.parentElement.style.display = "block";
  }
}
