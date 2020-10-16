import { ServicesService } from './../../../_services/services.service';
import { Component, NgModule, OnInit } from '@angular/core';
import { Service } from 'src/app/_models/Service';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ServicesGalleryModalComponent } from '../services-gallery-modal/services-gallery-modal.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-service-details',
  templateUrl: './service-details.component.html',
  styleUrls: ['./service-details.component.css'],
})
export class ServiceDetailsComponent implements OnInit {
  serviceId: number;
  service: Service;
  loading: boolean = true;
  constructor(
    private servicesService: ServicesService,
    private activateRoute: ActivatedRoute,
    private router: Router,
    public dialog: MatDialog
  ) {}

  ngOnInit() {
    this.getServiceId();
    this.loadService(this.serviceId);
  }
  getServiceId() {
    this.activateRoute.params.subscribe((params: Params) => {
      this.serviceId = params['id'];
    });
  }
  loadService(id: number) {
    this.servicesService.getService(id).subscribe(
      (res: Service) => {
        this.service = res;
      },
      (error) => {
        this.router.navigate(['']);
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
  toPrice(num, sep) {
    var number = typeof num === 'number' ? num.toString() : num,
      separator = typeof sep === 'undefined' ? ',' : sep;
    return number.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, '$1' + separator);
  }
}
