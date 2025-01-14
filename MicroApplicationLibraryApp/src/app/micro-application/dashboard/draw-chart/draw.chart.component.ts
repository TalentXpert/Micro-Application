import { Component, Input, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { DashboardChart } from '../dashboard.container/dashboard.model';



@Component({
  selector: 'draw-chart',
  templateUrl: './draw.chart.component.html',
  styles: [],
  standalone:false
})
export class DrawChartComponent {
  chartSubscription : Subscription= new Subscription();
  showChart: boolean = false;
  @Input() dashboardChart: DashboardChart = new DashboardChart();
  chartData: any;
  title: string ="";
  options: any;
  columnsNames: any;
  data: any[] = [];
  type: string = "";

  constructor() {
    /** line charts*/

  }

  ngOnChanges() {
    if (this.dashboardChart.ChartType == 'PieChart' || this.dashboardChart.ChartType == 'Bar' || this.dashboardChart.ChartType == 'HorizontalLine') {
      if (this.dashboardChart.ChartType == 'HorizontalLine') this.dashboardChart.ChartType = 'Line';
      this.setSettingChart();

    }
  }

  setSettingChart() {
    this.title = this.dashboardChart.Title;
    this.columnsNames = [] = [];
    this.data = [] = [];
    this.type = this.dashboardChart.ChartType;
    this.options = {
      tooltip: { isHtml: true },
      legend: { position: 'right'},
      width:640
    };

    this.dashboardChart.Columns.forEach(element => {
      this.columnsNames.push({ type: element.DataType, label: element.Title })
    })

    if (this.dashboardChart.ChartType == 'PieChart') this.convertDataToNumber()

    this.dashboardChart.SeriesData.forEach(element => {
      this.data.push(element)
    })

    this.showChart = true;
  }

  convertDataToNumber() {
    this.dashboardChart.SeriesData.forEach(element =>
      element[1] = parseInt(element[1]));
  }

  ngOnDestroy() {
    if (this.chartSubscription) {
      this.chartSubscription.unsubscribe();
    }
  }

}
