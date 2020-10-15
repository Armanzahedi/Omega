import { ServicesGalleryModalComponent } from './../services-gallery-modal/services-gallery-modal.component';
import { AlertifyService } from './../../../_services/alertify.service';
import { ServicesService } from './../../../_services/services.service';
import { IService } from './../../../_models/IService';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-services-list',
  templateUrl: './services-list.component.html',
  styleUrls: ['./services-list.component.css']
})
export class ServicesListComponent implements OnInit {
  services: IService[];
  breakpoint: number = 4;
  constructor(private servicesService: ServicesService,private alertify: AlertifyService,public dialog: MatDialog) { }

  ngOnInit() {
    this.gridSystem();
    this.loadServices();
  }
  onResize(event) {
    this.gridSystem();
  }
  gridSystem(){
    if(window.innerWidth <= 1300){
      this.breakpoint = 3;
    }
    if(window.innerWidth <= 1103){
      this.breakpoint = 2;
    }
    if(window.innerWidth <= 735){
      this.breakpoint = 1;
    } 
    if(window.innerWidth >= 1300){
      this.breakpoint = 4;
    }
  }
  loadServices(){
    this.servicesService
    .getServices()
    .subscribe((res: Array<IService>) => {
      this.services = res;
      console.log(this.services);
  }, error => {
    this.alertify.error(error);
  });
  }
  openModal(serviceId: number){
    this.dialog.open(ServicesGalleryModalComponent,{
      data: {
        serviceId: serviceId
      }
    });
  }
}
