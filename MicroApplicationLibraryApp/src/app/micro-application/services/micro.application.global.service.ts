import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { MenuViewModel } from "../menu/menu.model";
import { DashboardViewModel } from "../dashboard/dashboard.container/dashboard.viewmodel";
import { MicroApplicationEvent } from "../micro-application.event";

@Injectable({
    providedIn: 'root'
})
export class MicroApplicationGlobalService {
  menuViewModel: MenuViewModel;
  dashboardViewModel:DashboardViewModel;

  constructor(private router: Router, private applicationPageEvent: MicroApplicationEvent) {
    this.menuViewModel = new MenuViewModel(applicationPageEvent);
    this.dashboardViewModel = new DashboardViewModel(applicationPageEvent);
  }

}



