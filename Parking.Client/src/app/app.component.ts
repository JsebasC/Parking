import {
  AfterViewInit,
  Component,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
// import { Observable, Subscription } from 'rxjs';

import { Parking } from './Model/parking';
import { ParkingService } from './Services/parking.service';
import { MatDialog } from '@angular/material/dialog';
import { AddParkingComponent } from './Dialog/add-parking/add-parking.component';
import { ExitParkingComponent } from './Dialog/exit-parking/exit-parking.component';

@Component({
  selector: 'app-app',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements AfterViewInit, OnInit, OnDestroy {
  dateTime = '';
  intervalId: any;
  parking: Parking[] = [];
  displayedColumns: string[] = [
    'vehicle',
    'parkingSpace',
    'entryDate',
    'vehicleTypeName',
    'actions',
  ];
  dataSource = new MatTableDataSource<Parking>();

  constructor(
    private _parkingService: ParkingService,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.start();
    console.log(this.dateTime);
    this.GetVehicleNoExit();
  }

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  AddParking() {
    this.dialog
      .open(AddParkingComponent, {
        disableClose: true,
        width: '350px',
      })
      .afterClosed()
      .subscribe((result) => {        
        this.GetVehicleNoExit();        
      });
  }

  ExitVehicle(data: Parking) {
    this.dialog
      .open(ExitParkingComponent, {
        disableClose: true,
        data: data,
        width: '450px',
      })
      .afterClosed()
      .subscribe((result) => {        
        this.GetVehicleNoExit();
      });
  }

  GetVehicleNoExit() {
    this.dataSource.data = [];
    this._parkingService.getListNoExitVehicle().subscribe(
      (data) => {
        this.dataSource.data = data;
      },
      (e) => {
        console.log(e);
      }
    );
  }

  //#region
  start() {
    this.intervalId = setInterval(() => {
      this.dateTime =
        new Date().toLocaleDateString() + ' ' + new Date().toLocaleTimeString();
    }, 1000);
  }

  stop() {
    clearInterval(this.intervalId);
  }
  //#endregion

  ngOnDestroy(): void {
    this.stop();
  }
}
