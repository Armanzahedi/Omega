import { async } from '@angular/core/testing';
import { Component, OnInit } from '@angular/core';
import { Service } from 'src/app/_models/Service';
import { ServicesService } from 'src/app/_services/services.service';

@Component({
  selector: 'app-services-report',
  templateUrl: './services-report.component.html',
  styleUrls: ['./services-report.component.css'],
})
export class ServicesReportComponent implements OnInit {
  services: Service[];
  loading: boolean = true;
  reportParams: any = {};
  displayedColumns: string[] = [
    'number',
    'name',
    'price',
    'unitMeasureName',
    'contractorName',
    'addedDate',
    'modifiedDate',
  ];
  constructor(private servicesService: ServicesService) {}

  ngOnInit() {
    this.LoadPage();
  }
  LoadPage() {
    this.servicesService.updateServicesTable().subscribe(
      () => {},
      () => {},
      () => {
        this.loadServices();
      }
    );
  }
  loadServices() {
    this.loading = true;
    this.servicesService.getReport(this.reportParams).subscribe(
      (res: Array<Service>) => {
        this.services = res;
      },
      (error) => {
        console.log(error);
      },
      () => {
        this.loading = false;
      }
    );
  }
  toPrice(num, sep) {
    var number = typeof num === 'number' ? num.toString() : num,
      separator = typeof sep === 'undefined' ? ',' : sep;
    return number.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, '$1' + separator);
  }
}
