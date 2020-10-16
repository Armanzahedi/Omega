import { Router } from '@angular/router';
import { ServicesGalleryModalComponent } from './../services-gallery-modal/services-gallery-modal.component';
import { AlertifyService } from './../../../_services/alertify.service';
import { ServicesService } from './../../../_services/services.service';
import { Service } from '../../../_models/Service';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-services-list',
  templateUrl: './services-list.component.html',
  styleUrls: ['./services-list.component.css'],
})
export class ServicesListComponent implements OnInit {
  services: Service[];
  loading: boolean = true;
  breakpoint: number = 4;
  constructor(
    private servicesService: ServicesService,
    private alertify: AlertifyService,
    public dialog: MatDialog,
    private router: Router
  ) {}

  ngOnInit() {
    this.gridSystem();
    this.loadServices();
  }
  onResize(event) {
    this.gridSystem();
  }
  gridSystem() {
    if (window.innerWidth <= 1300) {
      this.breakpoint = 3;
    }
    if (window.innerWidth <= 1103) {
      this.breakpoint = 2;
    }
    if (window.innerWidth <= 735) {
      this.breakpoint = 1;
    }
    if (window.innerWidth >= 1300) {
      this.breakpoint = 4;
    }
  }
  loadServices() {
    this.servicesService.getAllServices().subscribe(
      (res: Array<Service>) => {
        this.services = res;
      },
      (error) => {
        this.alertify.error(error);
      },
      () => {
        this.loading = false;
      }
    );
  }
  openGallery(serviceId: number) {
    this.dialog.open(ServicesGalleryModalComponent, {
      data: {
        serviceId: serviceId,
      },
    });
  }
  goToDetails(serviceId: number) {
    this.router.navigate(['services/' + serviceId]);
  }
}
