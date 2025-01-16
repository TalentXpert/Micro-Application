import { Component, Input, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { DashboardService } from '../../services/dashboard.service';
import { ActivatedRoute } from '@angular/router';
import { UtilityService } from '../../library/utility.service';
import { Subscription } from 'rxjs';
import { DashboardChart } from './dashboard.model';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DashboardPanelComponent } from '../dashboard-panel/dashboard.panel.component';
import { DashboardViewModel } from './dashboard.viewmodel';
import { MicroApplicationGlobalService } from '../../services/micro.application.global.service';
import { DashboardSchema } from '../../dashboard-builder/dashboard.builder.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.container.component.html',
  styleUrls: ['./dashboard.container.component.css'],
  standalone: false
})
export class DashboardContainerComponent implements OnInit {
    observableSubscription: Subscription | null = null;;
  dashboard: DashboardSchema;
  @Input() dashboardSchema: DashboardSchema = new DashboardSchema();
  @ViewChild(DashboardPanelComponent) child: DashboardPanelComponent | undefined;
  @ViewChild('SummaryData') templateRef: TemplateRef<any> | undefined;
  dashboardViewModel: DashboardViewModel;
  summaryData: DashboardChart = new DashboardChart();


  constructor(private utilsService: UtilityService, private dashboardService: DashboardService,
    private _Activatedroute: ActivatedRoute, private microApplicationGlobalService: MicroApplicationGlobalService, private modalService: NgbModal,
    public ngbActiveModal: NgbActiveModal) {
    this.dashboardViewModel = microApplicationGlobalService.dashboardViewModel;;
    this.dashboard = new DashboardSchema();
  }

  ngOnInit(): void {
    let pageId = this._Activatedroute.snapshot.queryParams['pageId'];
    if (this.dashboardSchema != undefined && this.dashboardSchema.Rows.length > 0) {
      this.dashboard = this.dashboardSchema;
    }
    else if (pageId && this.dashboardSchema == undefined) {
      this.utilsService.setLoaderVisibility(true);
      this.getDashboard(pageId);
      this.utilsService.setLoaderVisibility(false);
    }
    
  }

  ngOnChanges() {
    this.dashboard = this.dashboardSchema;
  }


  getDashboard(pageId) {
    this.observableSubscription = this.dashboardService.getDashboard(pageId).subscribe(response => {
      this.dashboard = response;
    },
      error => {
        throw error;
      });
  }

  openSummaryDialogue(panel) {
    let data = this.dashboardViewModel.summaryDataList.get(panel.Id);
    if(data){
      this.summaryData = data;
      this.summaryData.SeriesData.splice(0, 1);
      this.modalService.open(this.templateRef, { backdrop: "static" }).result.then((result) => {
      }, (reason) => {
      });
    }
   
    

  }

  

  closeModalDialog() {
    this.summaryData = new DashboardChart();
  }

  ngOnDestroy() {
    if (this.observableSubscription) {
      this.observableSubscription.unsubscribe();
    }
  }
}
