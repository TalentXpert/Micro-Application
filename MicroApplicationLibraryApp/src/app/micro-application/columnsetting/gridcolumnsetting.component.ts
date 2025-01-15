import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { AlertModalService } from '../alert/alert.modal.service';
import { UserGridHeaderVM, SmartGridConfigurationVM, SmartPage, UserGridHeadersVM } from '../application-page-container/application.page.model';
import { ApplicationPageService } from '../services/application.page.service';


@Component({
  selector: 'app-gridcolumnsetting',
  templateUrl: './gridcolumnsetting.component.html',
  styleUrls: ['./gridcolumnsetting.component.css'],
  standalone: false,
})
export class GridColumnSettingComponent implements OnInit {
  observableSubscription: Subscription;
  userGridHeadersVM: UserGridHeadersVM;
  smartPage: SmartPage;
  @Output() sendResponse: EventEmitter<boolean> = new EventEmitter();

  constructor(private ngbmodal: NgbActiveModal, private alertModalService: AlertModalService, private modalService: NgbModal,
    private applicationPageService: ApplicationPageService) {

  }

  ngOnInit() {
    this.getUserHeaders();
  }

  getUserHeaders() {
    let smartGridConfigurationVM = new SmartGridConfigurationVM(this.smartPage.PageId);
    this.observableSubscription = this.applicationPageService.getUserHeaders(smartGridConfigurationVM)
      .subscribe((data) => {
        this.userGridHeadersVM = data;
      }, (error) => {
        throw error;
      });
  }

  drop(event: CdkDragDrop<any[]>) {
    moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
  }

  onclickedCheckbox(column: UserGridHeaderVM) {
    column.IsVisible = !column.IsVisible;
    let index = this.userGridHeadersVM.Headers.findIndex(a => a.Position == column.Position);
    if (index > -1) this.userGridHeadersVM.Headers[index].IsVisible = column.IsVisible;
  }

  apply() {
    let setting = [...this.userGridHeadersVM.Headers]
    for (var i = 0; i < setting.length; i++) {
      setting[i].Position = i + 1;
    }
    if (this.checkMinimunColumnSelcted(setting)) {
      let userGridHeadersVM = new UserGridHeadersVM(this.smartPage.PageId, setting);
      this.saveColumnSetting(userGridHeadersVM);
    }
    else this.alertModalService.setInformaitonModalTemplate("Please select atleast 2 columns.")
  }

  checkMinimunColumnSelcted(setting) {
    let selectedColumn = setting.filter(cl => cl.IsVisible == true);
    return selectedColumn.length >= 2;
  }

  saveColumnSetting(userGridHeadersVM) {
    this.observableSubscription = this.applicationPageService.saveUserHeaders(userGridHeadersVM)
      .subscribe((columSettingSaved) => {
        if (columSettingSaved) {
          this.sendResponse.emit(columSettingSaved);
          this.close();
        }
      }, (error) => {
        throw error;
      });
  }

  close() {
    this.ngbmodal.close();
  }

  ngOnDestroy() {
    if (this.observableSubscription) {
      this.observableSubscription.unsubscribe();
    }
  }
}