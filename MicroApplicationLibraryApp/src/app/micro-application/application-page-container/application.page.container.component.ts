import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ApplicationPageViewModel } from './application.page.viewModel';
import { ControlFilter, ControlValue, GridRequestVM, SmartAction, UIControl, SmartGrid, SmartGridConfigurationVM, SmartPage } from './application.page.model';
import { FilterComponent } from '../filters/filter-form/filter.form.component';
import { SmartFormComponent } from '../smart-form/smart.form.component';
import { GridColumnSettingComponent } from '../columnsetting/gridcolumnsetting.component';
import { MicroApplicationEvent, MicroApplicationEventData, MicroApplicationOperation } from '../micro-application.event';
import { ActivatedRoute, Router } from '@angular/router';
import { ApplicationPageService } from '../services/application.page.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AlertModalService } from '../alert/alert.modal.service';
import { ViewPageContentComponent } from '../view-page-content/view.page.content.component';
import { ImportExcelComponent } from '../import-excel/import.excel.component';
import { SearchDataModel } from '../filters/search-filter/search.data.model';
import { UtilityService } from '../library/utility.service';
import { PageContentView } from '../view-page-content/view.page.content.model';
import { UIFormClientVM } from '../UI-form/UI.form.model';
import { FormTypes } from '../library/constant';
import { InputSelectFromListForm } from '../select-from-list-form/select.from.list.form.model';
import { SpecialFormComponent } from '../special-form/special.form.component';



@Component({
  selector: 'application-page',
  templateUrl: './application.page.container.component.html',
  styleUrls: ['./application.page.component.css'],
  standalone: false
})

export class ApplicationPageContainerComponent implements OnInit, OnDestroy {
  subscription: any = {};
  observableSubscription: Subscription;
  applicationPageViewModel: ApplicationPageViewModel;
  PageIdentifier: string = "";
  searchText: string = '';
  smartFilters: UIControl[] = [];
  pageSizeList: number[] = [100, 200, 500, 1000];
  filters: ControlFilter[] = [];
  globalControlFB: FormGroup;
  globalControls: UIControl[] = [];
  showFilters: boolean = false;
  smartPage: SmartPage;
  viewMode: string = "Grid View";
  searchDataModel: SearchDataModel;
  pageContentView: PageContentView;
  @Input() dashboardPageId: string;

  constructor(private utilsService: UtilityService, private applicationPageService: ApplicationPageService, private applicationPageEvent: MicroApplicationEvent,
    private modalService: NgbModal, private _Activatedroute: ActivatedRoute, private router: Router, private alertModalService: AlertModalService) {
    router.events.subscribe((val) => {
      this.loadPageState();
    });

    this.applicationPageViewModel = new ApplicationPageViewModel(applicationPageEvent);
    this.searchDataModel = new SearchDataModel();
  }

  ngOnInit() {
    if (this.dashboardPageId) {
      this.PageIdentifier = this.dashboardPageId;
      this.getPageState();
    }
  }

  loadPageState() {
    let pageId = this._Activatedroute.snapshot.queryParams['pageId'];
    if (pageId && this.PageIdentifier !== pageId) {
      this.clearAll();
      this.PageIdentifier = pageId;
      this.utilsService.setLoaderVisibility(true);
      this.getPageState();
      this.utilsService.setLoaderVisibility(false);
    }

  }


  getPageState() {
    if (this.PageIdentifier) {
      this.observableSubscription = this.applicationPageService.getSmartPage(this.PageIdentifier).subscribe(smartPage => {
        this.applicationPageEvent.publish(new MicroApplicationEventData(MicroApplicationOperation.LoadSmartPageState, smartPage));
        this.smartPage = smartPage;
        if (this.smartPage.PageId) {
          if (smartPage.GlobalControls.length > 0) {
            this.globalControls = smartPage.GlobalControls;
            this.showFilters = false;
            this.addValidators();
          }
          else {

            this.smartGridConfiguration();
          }

        }

      },
        error => {
          throw error;
        });
    }
  }

  addValidators() {
    this.globalControls.forEach(x => {
      if (x.IsMandatory) {
        const validators :any[]=[];
        validators.forEach(v => {
          switch (v.type) {
            case "required":
              validators.push(Validators.required);
              break;
          }
        })
        x.Validators = validators;
      }
    })
    this.createForm();
  }

  createForm() {
    this.globalControlFB = new FormGroup({}) //<--create the formGroup
    for (let formModule of this.globalControls) {
      this.globalControlFB.addControl(formModule.DisplayLabel, new FormControl(formModule.Value ? formModule.Value : "", formModule.Validators))
      if (formModule.IsPageRefreshNeeded) this.loadChildOptions(formModule)
    }
  };

  loadChildOptions(options: UIControl) {
    this.clearGridData();
    let value = this.globalControlFB.controls[options.DisplayLabel].value;
    if (options.IsPageRefreshNeeded && value) this.smartGridConfiguration();
    else if (!options.IsPageRefreshNeeded) this.loadChildrenValue(options);
  }

  loadChildrenValue(control: UIControl) {
    if (control) {
      let childControl = this.globalControls.find(c => c.ParentControlIdentifier == control.ControlIdentifier);
      if (childControl) {
        let formValues = this.globalControlFB.value;
        let parentValue = formValues[control.DisplayLabel];
        this.observableSubscription = this.applicationPageService.getChildren(childControl.ControlId, parentValue).subscribe(options => {
          this.addSmartSelectionOption(options, childControl);

        },
          error => {
            throw error;
          });
      }

    }
  }

  addSmartSelectionOption(Options: any, childControl: any) {
    let index = this.globalControls.findIndex(control => control.ControlId == childControl.ControlId);
    this.globalControls[index].Value = "";
    if (index > -1) {
      this.globalControlFB.controls[childControl.DisplayLabel].patchValue("");
      this.globalControls[index].Options = [...Options];
      this.showFilters = false;
    }
  }


  smartGridConfiguration() {
    let smartGridConfigurationVM = new SmartGridConfigurationVM(this.PageIdentifier);
    if (this.globalControls.length > 0) {
      let globalControlsList : ControlValue[]=[];
      let formValues = this.globalControlFB.value;
      this.globalControls.forEach(element => {
        let value = formValues[element.DisplayLabel];
        globalControlsList.push(new ControlValue(element.ControlId, element.ControlIdentifier, value, null))
      });
      smartGridConfigurationVM.GlobalControlValues = globalControlsList;
    }
    this.observableSubscription = this.applicationPageService.getSmartGridConfiguration(smartGridConfigurationVM).subscribe(smartGrid => {
      this.applicationPageEvent.publish(new MicroApplicationEventData(MicroApplicationOperation.LoadSmartGrid, smartGrid));
      this.smartFilters = smartGrid.Filters;
      this.showFilters = true;
      let index = this.applicationPageViewModel.smartGrid.UserGridFilters.findIndex(filters => filters.Id == this.smartPage.CurrentGridFilterId);
      if (index > -1) this.applicationPageViewModel.filterId = this.smartPage.CurrentGridFilterId?this.smartPage.CurrentGridFilterId:undefined;
      this.processGrid();
    },
      error => {
        throw error;
      });
  }

  processGrid() {
    let gridRequestVM = this.getGridRequestVM();
    this.observableSubscription = this.applicationPageService.processPageDataRequest(gridRequestVM).subscribe(gridModel => {
      this.applicationPageEvent.publish(new MicroApplicationEventData(MicroApplicationOperation.LoadGridModel, gridModel));
      if (this.viewMode == 'List View') this.applicationPageEvent.publish(new MicroApplicationEventData(MicroApplicationOperation.LoadListModel, null));
      this.filters = [];
    },
      error => {
        throw error;
      });
  }

  openFilters() {

    if (this.applicationPageViewModel.smartGrid.Filters.length > 0) {
      const dialogRef = this.modalService.open(FilterComponent, { backdrop: 'static' });
      dialogRef.componentInstance.SmartGrid = this.applicationPageViewModel.smartGrid;
      if (this.applicationPageViewModel.filterId) dialogRef.componentInstance.SelectedFilterId = this.applicationPageViewModel.filterId;
      if (this.globalControls.length > 0) {
        dialogRef.componentInstance.globalControls = this.getGlobalControlValues();
      }

      dialogRef.componentInstance.sendResponse.subscribe((userGridFilter) => {
        if (userGridFilter) {
          this.applicationPageEvent.publish(new MicroApplicationEventData(MicroApplicationOperation.AddUpdateUserFilter, userGridFilter));
          this.getFilteredData(userGridFilter.Id);
        }
      });
      dialogRef.componentInstance.applyResponse.subscribe((userGridFilter) => {
        if (userGridFilter) {
          this.filters = userGridFilter.Filters;
          this.applicationPageViewModel.filterId = "";
          this.processGrid();

        }
      });
    }
  }

  getFilteredData(currentGridFilterId) {
    if (currentGridFilterId)
      this.applicationPageViewModel.filterId = currentGridFilterId;
    else this.applicationPageViewModel.filterId = "";
    this.processGrid();
  }

  deleteUserFilter() {
    if (this.applicationPageViewModel.filterId) {
      this.observableSubscription = this.applicationPageService.deleteFilter(this.applicationPageViewModel.filterId).subscribe(gridModel => {
        this.applicationPageEvent.publish(new MicroApplicationEventData(MicroApplicationOperation.DeleteUserFilter, this.applicationPageViewModel.filterId));
        this.applicationPageViewModel.filterId = "";
        this.processGrid();
      },
        error => {
          throw error;
        });
    }
  }

  //Column setting start here
  opengridColumnSetting() {
    const dialogRef = this.modalService.open(GridColumnSettingComponent, { backdrop: 'static', windowClass: 'AddOrgnization' });
    dialogRef.componentInstance.smartPage = this.applicationPageViewModel.smartPage;
    dialogRef.componentInstance.sendResponse.subscribe((dataSaved) => {
      if (dataSaved) {
        if (this.applicationPageViewModel.filterId) this.getFilteredData(this.applicationPageViewModel.filterId);
        else this.processGrid();
      }
    });
  }

  getGlobalControlValues(): ControlFilter[] {
    let globalControlsList : ControlFilter[]=[];
    let formValues = this.globalControlFB.value;
    this.globalControls.forEach(element => {
      let value = formValues[element.DisplayLabel];
      globalControlsList.push(new ControlFilter(element.ControlId, element.ControlIdentifier, value, "="))
    });
    return globalControlsList;
  }

  getOptionList(control) {
    let index = this.globalControls.findIndex(c => c.ControlId == control.ControlId);
    return control.Options = [... this.globalControls[index].Options];
  }

  getSearchItems(event, control: UIControl) {
    if (control && event.term) {
      let parentControl = this.globalControls.find(c => c.ControlIdentifier == control.ParentControlIdentifier);
      if (parentControl) {
        this.observableSubscription = this.applicationPageService.getSearchOptions(control.ControlId, parentControl.ControlId, event.term).subscribe(options => {
          this.addSmartSelectionOption(options, parentControl.ControlId);
        },
          error => {
            throw error;
          });
      }
    }
  }

  getClassforAction(menu) {
    if (menu == null) menu = this.getMenuList();
    if (menu && menu.length > 0) return "text-right";
    return "d-none";
  }

  getMenuList() {
    let index = this.applicationPageViewModel.gridHeaders.findIndex(gh => gh.Actions.length > 0);
    if (index > -1) {
      return this.applicationPageViewModel.gridHeaders[index].Actions;
    }
    return null;
  }

  getClassGridView() {
    if (this.globalControls && this.globalControls.length > 0) return "InternalCard pt-3 pb-2 mb-1 ApplicationPageDataGlobalControlsView";
    return "InternalCard pt-3 pb-2 mb-1 ApplicationPageDataView";
  }

  getClassListView() {
    if (this.globalControls && this.globalControls.length > 0) return "pt-2 mb-0 ListViewGlobalControlsCardBody";
    return "pt-2 mb-0  ListViewCardBody";
  }

  getClassForViewDetails() {
    if (this.globalControls && this.globalControls.length > 0) return "InternalCard pt-2 pb-2 ListViewGlobalControlsCardBody";
    return "InternalCard pt-2 pb-2 ListViewCardBody";
  }

  //ngClass start here
  getPreviousBtnClass() {
    if (this.applicationPageViewModel.disablePreviousBtn)
      return 'disablebtnClass';
    else
      return 'CustomPointer';
  }

  getNextBtnClass() {
    if (this.applicationPageViewModel.disableNextBtn)
      return 'disablebtnClass';
    else
      return 'CustomPointer';
  }

  sortBy(headerCell) {
    this.applicationPageViewModel.sortData(headerCell);
  }


  executeRespectiveEvent(action: SmartAction, row) {
    if (action.FormType == FormTypes.DynamicForm) {
      if (action.FormMode == 'Delete') this.deleteRow(action, row);
      else if (action.FormMode == 'View') this.openViewPageContent(action, row);
      else {
        const dialogRef = this.modalService.open(SmartFormComponent, { backdrop: 'static' });
        let uIFormClientVM = new UIFormClientVM();
        uIFormClientVM.SmartAction = action;
        if (row) uIFormClientVM.DataKey = row[0].T;
        let globalControlsList :ControlValue[]=[];
        if (this.globalControls.length > 0) {
          let formValues = this.globalControlFB.value;
          this.globalControls.forEach(element => {
            let value = formValues[element.DisplayLabel];
            globalControlsList.push(new ControlValue(element.ControlId, element.ControlIdentifier, value, null))
          });
        }
        uIFormClientVM.globalControls = globalControlsList;
        dialogRef.componentInstance.uIFormClientVM = uIFormClientVM;
        dialogRef.componentInstance.sendResponse.subscribe((dataSaved) => {
          if (dataSaved) {
            this.processGrid();
          }
        });
      }
    }
    else if (action.FormType == FormTypes.SelectFromListForm) {
      let inputSelectFromListForm = new InputSelectFromListForm();
      if (row) inputSelectFromListForm.EntityId = row[0].T;
      inputSelectFromListForm.FormId = action.FormId;
      inputSelectFromListForm.Text = action.Text;
      const dialogRef = this.modalService.open(SpecialFormComponent, { backdrop: 'static' });
      dialogRef.componentInstance.inputSelectFromListForm = inputSelectFromListForm;
      dialogRef.componentInstance.sendResponse.subscribe((dataSaved) => {
        if (dataSaved) {
          this.processGrid();
        }
      });

    }
  }

  deleteRow(action, row) {
    if (action.FormId) {
      this.observableSubscription = this.applicationPageService.deleteRecord(action.FormId, row[0].T).subscribe(response => {
        if (response) {
          this.alertModalService.setInformaitonModalTemplate("Record is deleted successfully.");
          this.processGrid();
        }
      },
        error => {
          throw error;
        });
    }
  }

  openViewPageContent(action, row) {
    const dialogRef = this.modalService.open(ViewPageContentComponent, { backdrop: 'static' });
    dialogRef.componentInstance.SmartAction = action;
    if (row) dialogRef.componentInstance.DataKey = row[0].T;
  }


  exportExcel() {
    let gridRequestVM = this.getGridRequestVM();
    this.observableSubscription = this.applicationPageService.export(gridRequestVM).subscribe(
      (res) => {
        const blob = new Blob([res.body], { type: res.body.type });
        var fileURL = URL.createObjectURL(blob);
        var a = document.createElement('a');
        document.body.appendChild(a);
        a.setAttribute('style', 'display: none');
        a.href = fileURL;
        a.download = this.applicationPageViewModel.smartPage.PageTitle + ".xlsx";
        a.click();
      },
      (error) => {
        this.alertModalService.setErrorAlertModalTemplate(error.error);
      }
    );
  }

  importExcel() {
    const dialogRef = this.modalService.open(ImportExcelComponent, { backdrop: 'static' });
    dialogRef.componentInstance.formId = this.applicationPageViewModel.smartPage.PageActions[0].FormId;
    dialogRef.componentInstance.pageTitle = this.applicationPageViewModel.smartPage.PageTitle;
    if (this.globalControls.length > 0) {
      let formValues = this.globalControlFB.value;
      this.globalControls.forEach(element => {
        let value = formValues[element.DisplayLabel];
        element.Value = value;
      });
    }
    dialogRef.componentInstance.globalControls = this.globalControls;
    dialogRef.componentInstance.sendResponse.subscribe((dataSaved) => {
      if (dataSaved) {
        this.processGrid();
      }
    });

  }

  getGridRequestVM(): GridRequestVM {
    let gridRequestVM = new GridRequestVM();
    gridRequestVM.PageId = this.applicationPageViewModel.smartPage.PageId;
    gridRequestVM.PageNumber = 1;
    gridRequestVM.PageSize = this.applicationPageViewModel.smartPage.PageSize;
    gridRequestVM.Filters = this.filters;
    gridRequestVM.FilterId =this.applicationPageViewModel.filterId? this.applicationPageViewModel.filterId:undefined ;
    if (this.globalControls.length > 0) {
      gridRequestVM.GlobalFilters = this.getGlobalControlValues();
    }
    return gridRequestVM;
  }

  clearAll() {
    this.globalControls = [] = [];
    this.dashboardPageId = "";
    if (this.applicationPageViewModel) this.applicationPageViewModel.smartPage = new SmartPage();
    this.clearGridData();
  }

  getViewPageContents(row) {
    let headerActionsList = this.applicationPageViewModel.gridHeaders.find(h => h.Actions.length > 0);
    if (headerActionsList && headerActionsList.Actions.length > 0) {
      this.observableSubscription = this.applicationPageService.getViewPageContents(headerActionsList.Actions[0].FormId, row[0].T).subscribe(response => {
        this.pageContentView = response;
      });
    }
  }

  clearGridData() {
    this.showFilters = false;
    this.viewMode = 'Grid View';
    this.pageContentView = new PageContentView();
    this.searchDataModel = new SearchDataModel();
    if (this.applicationPageViewModel) {
      this.applicationPageViewModel.smartGrid = new SmartGrid();
      this.applicationPageViewModel.gridHeaders = [] = [];
      this.applicationPageViewModel.rows = [] = [];
      this.applicationPageViewModel.filterId = "";
    }
  }

  searchDataGloballyInTable(event: any) {
    let value= event.target.value;
    this.applicationPageViewModel.rows = [...this.searchDataModel.fliterData(this.applicationPageViewModel.gridModel.Rows, value, 'T')];
    if (this.viewMode == 'List View') this.applicationPageEvent.publish(new MicroApplicationEventData(MicroApplicationOperation.LoadListModel, null));

  }

  changeViewMode() {
    this.pageContentView = new PageContentView()
    this.viewMode = this.viewMode == 'Grid View' ? 'List View' : 'Grid View';
    if (this.viewMode == 'List View') this.applicationPageEvent.publish(new MicroApplicationEventData(MicroApplicationOperation.LoadListModel, null));
  }

  onChangePageSize() {
    this.applicationPageViewModel.onChangePageSize(this.viewMode);
  }

  loadPreviousData() {
    this.applicationPageViewModel.loadPreviousData(this.viewMode);
  }
  loadNextData() {
    this.applicationPageViewModel.loadNextData(this.viewMode);
  }

  ngOnDestroy() {
    if (this.observableSubscription) {
      this.observableSubscription.unsubscribe();
    }
  }
}