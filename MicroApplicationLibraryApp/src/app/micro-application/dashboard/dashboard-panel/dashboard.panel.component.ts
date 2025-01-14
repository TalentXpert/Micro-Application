import { Component, Input, OnInit, QueryList, ViewChildren } from '@angular/core';
import { Subscription } from 'rxjs';
import { DashboardChart } from '../dashboard.container/dashboard.model';
import { DashboardService } from '../../services/dashboard.service';
import { GoogleChartComponent } from 'angular-google-charts';
import { DashboardViewModel } from '../dashboard.container/dashboard.viewmodel';
import { MicroApplicationGlobalService } from '../../services/micro.application.global.service';
import { DashboardPanel } from '../../dashboard-builder/dashboard.builder.model';


@Component({
  selector: 'dashboard-panel',
  templateUrl: './dashboard.panel.component.html',
  styles: [],
  standalone: false,
})
export class DashboardPanelComponent implements OnInit {
  observableSubscription: Subscription;
  @Input() dashboardPanel: DashboardPanel;
  @Input() panelId: string;
  dashboardChartData: DashboardChart;

  pageId="41C55263-D9E0-CD72-96AC-08D90656C39A";
   
  @ViewChildren('cmp') components:QueryList<GoogleChartComponent>;
  dashboardViewModel:DashboardViewModel;

  

  constructor(private dashboardService: DashboardService, private microApplicationService: MicroApplicationGlobalService) {
    this.dashboardChartData = new DashboardChart();
    this.dashboardViewModel = microApplicationService.dashboardViewModel;
    
  }

  


  ngOnInit() {
    if(this.dashboardPanel.ContentType =='Graph') this.getChartData(this.dashboardPanel.ContentId);
    if(this.dashboardPanel.ContentType =='Summary') this.getChartData(this.dashboardPanel.ContentId);

  }

  


  getChartData(panelId) {
    this.observableSubscription = this.dashboardService.getDashboardChart(panelId).subscribe(response => {
      this.dashboardChartData = response;
      this.dashboardViewModel.saveDashboardPanelData(panelId, response);
    },
      error => {
        throw error;
      });
  }

  hasData(array) {
    if (!array) return false;
    if (array.length > 0) return true;
    return false;
  }

  ngOnDestroy() {
    if (this.observableSubscription) {
      this.observableSubscription.unsubscribe();
    }
  }
  
 

}
