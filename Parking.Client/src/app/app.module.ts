import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';


import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';

//Material desing
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatNativeDateModule } from '@angular/material/core';
//Alertar material desing
import { MatSnackBarModule } from '@angular/material/snack-bar';
//iconos
import { MatIconModule } from '@angular/material/icon';
//modales
import { MatDialogModule } from '@angular/material/dialog';
//Grillas
import { MatGridListModule } from '@angular/material/grid-list';
//Darle formato a las fechas

import { AppComponent } from './app.component';
import { ParkingService } from './Services/parking.service';
import { AddParkingComponent } from './Dialog/add-parking/add-parking.component';
import { ExitParkingComponent } from './Dialog/exit-parking/exit-parking.component';
import { NavbarComponent } from './NavBar/navbar/navbar.component';

@NgModule({
  declarations: [    
    AppComponent, AddParkingComponent, ExitParkingComponent, NavbarComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatPaginatorModule,
    MatTableModule,
    MatSnackBarModule,
    MatIconModule,
    MatDialogModule,
    MatGridListModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatNativeDateModule
  ],
  providers: [ParkingService],
  bootstrap: [AppComponent,NavbarComponent]
})
export class AppModule { }
