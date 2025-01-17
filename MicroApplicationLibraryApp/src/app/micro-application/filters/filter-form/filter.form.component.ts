import { Component, OnInit, OnDestroy, Output, EventEmitter, } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ControlFilter, UIControl, SmartGrid, UserGridFilter } from '../../application-page-container/application.page.model';
import { ApplicationPageService } from '../../services/application.page.service';


@Component({
    selector: 'app-filter',
    templateUrl: './filter.form.component.html',
    standalone: false
})

export class FilterComponent implements OnInit, OnDestroy {
    observableSubscription: Subscription | null = null;;
    SmartGrid: SmartGrid = new SmartGrid();
    SelectedFilterId: string = "";
    filterForm: FormGroup = new FormGroup({});;
    filterTitle: string = "";
    showValidationMsg: boolean = false;
    Filters: UIControl[] = [];
    selectedFilter: UserGridFilter = new UserGridFilter("", "");
    globalControls: ControlFilter[] = [];
    stringOperators: string[] = ['=', '*', '%', '$'];
    @Output() sendResponse: EventEmitter<UserGridFilter> = new EventEmitter();
    @Output() applyResponse: EventEmitter<UserGridFilter> = new EventEmitter();


    get formControls() { return this.filterForm.controls; }
    get FilterFormArray() { return this.formControls['filters'] as FormArray; }
    get FilterFormControlsArray() {
        return (this.filterForm.controls["filters"] as FormArray).controls as FormGroup[];
    }



    constructor(public ngbActiveModal: NgbActiveModal, private formBuilder: FormBuilder, private applicationPageService: ApplicationPageService,
        private modalService: NgbModal,) {

    }

    ngOnInit() {
        this.Filters = this.SmartGrid.Filters;
        this.createForm();
        if (this.SelectedFilterId) {
            this.patchFormValues();
        }
    }

    createForm() {
        this.filterForm = this.formBuilder.group({
            filters: this.formBuilder.array([]),
        });
        this.addOperators();
    };

    addOperators() {
        for (var i = 0; i < this.Filters.length; i++) {
            this.addOperator(this.Filters[i]);
        }
    }

    addOperator(filter:UIControl) {
        this.FilterFormArray.push(this.formBuilder.group({
            Operator: ['='],
            Value: [],
            ControlId: [filter.ControlId],
            ControlIdentifier: [filter.ControlIdentifier],
        }))
    }

    patchFormValues() {
        var filterId = this.SmartGrid.UserGridFilters.find(f => f.Id == this.SelectedFilterId);
        if (filterId) {
            this.selectedFilter = filterId;
            this.filterTitle = this.selectedFilter.FilterName;
            this.setFormArray(this.selectedFilter.Filters);
        }

    }

    setFormArray(filters: ControlFilter[]) {
        this.FilterFormArray.clear();
        for (var i = 0; i < filters.length; i++) {
            this.FilterFormArray.push(this.formBuilder.group({
                Option: [filters[i]],
            }))
        }
        // let filterFormArray = this.setIdentfier(filters);
        // this.filterForm.setControl('filters', filterFormArray);
    }

    // setIdentfier(data: any[]): FormArray {
    //     let fa ;
    //     data.forEach(d => {
    //         fa.push(this.formBuilder.group({
    //             Operator: [d.Operator],
    //             Value: [d.Value],
    //             ControlId: [d.ControlId],
    //             ControlIdentifier: [d.ControlIdentifier],
    //         }));
    //     });
    //     return fa;
    // }


    checkControlTypeString(i:number) {
        let filter = this.Filters[i];
        if (filter && (filter.ControlType == 'TextBox' || filter.ControlType == 'TextArea')) return true;
        else return false;
    }

    checkControlTypeDropdown(i: number) {
        let filter = this.Filters[i];
        if (filter && (filter.ControlType == 'Dropdown')) return true;
        else return false;
    }

    checkOptionsForDropdown(i:number) {
        let filter = this.Filters[i];
        return filter.Options;
    }

    saveAndApply() {
        let formValues = this.filterForm.value;
        let userGridFilter = new UserGridFilter(this.SmartGrid.PageId, this.filterTitle, formValues.filters);
        if (this.globalControls.length > 0) {
            this.globalControls.forEach(element => {
                userGridFilter.Filters.push(element);
            })
        }
        if (this.SelectedFilterId) userGridFilter.Id = this.SelectedFilterId;
        this.observableSubscription = this.applicationPageService.saveFilter(userGridFilter).subscribe(userGridFilterResponse => {
            this.sendResponse.emit(userGridFilterResponse);
            this.modalService.dismissAll();
        },
            error => {
                throw error;
            });
    }

    apply() {
        let formValues = this.filterForm.value;
        let userGridFilter = new UserGridFilter(this.SmartGrid.PageId, this.filterTitle, formValues.filters);
        this.applyResponse.emit(userGridFilter);
        this.modalService.dismissAll();
    }

    openSaveFilterModel(sectionModal: any) {
        this.showValidationMsg = false;
        if (this.SelectedFilterId == null) this.filterTitle = "";
        this.modalService.open(sectionModal, { backdrop: "static" }).result.then((result) => {
        }, (reason) => {
        });
    }

    checkFilterTitle() {
        if (this.filterTitle == "") this.showValidationMsg = true;
        else this.showValidationMsg = false;
    }


    close() {
        this.ngbActiveModal.close();
    }

    reset() {
        this.FilterFormArray.clear();
        this.addOperators();
        this.SelectedFilterId == null;
        this.filterTitle = "";
    }

    ngOnDestroy() {
        if (this.observableSubscription)
            this.observableSubscription.unsubscribe();
    }
}

