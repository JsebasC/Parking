import { Component, OnInit } from '@angular/core';

declare const $: any;
declare interface RouteInfo {
    path: string;
    title: string;
    icon: string;
    class: string;
}
export const ROUTES: RouteInfo[] = [
  { path: '/dashboard', title: 'Dashboard', icon: 'dashboard', class: '' },
    { path: '/parking', title: 'Parking',  icon:'garage', class: '' },    
    { path: '/garage', title: 'Garaje',  icon:'warehouse', class: '' },
    
];

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  menuItems: any[];
  dateTime = '';
  intervalId: any;
  
  constructor() { }

  ngOnInit() {
    this.menuItems = ROUTES.filter(menuItem => menuItem);
    this.start();  
  }
  
  ngOnDestroy(): void {
    this.stop();
  }


  isMobileMenu() {
      if ($(window).width() > 991) {
          return false;
      }
      return true;
  };

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

}
