import { Component, OnInit, OnDestroy, EventEmitter, Output, } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ApplicationPageService } from '../services/application.page.service';
import { ControlValue, ExcelImportRequest, ExcelImportTmplateRequest, UIControl } from '../application-page-container/application.page.model';
import { AlertModalService } from '../alert/alert.modal.service';
import { UtilityService } from '../library/utility.service';
import { ValidationMessage } from '../utilities/validaton.messages';

@Component({
    selector: 'app-import-excel',
    templateUrl: './import.excel.component.html',
    standalone: false
})

export class ImportExcelComponent implements OnInit, OnDestroy {
      observableSubscription: Subscription | null = null;;
    formId: string;
    pageTitle: string;
    importExcelFB: FormGroup;
    globalControls: UIControl[] = [];
    formData: FormData;
    uplaodedFile: any;
    formBuilder: any;
    validationMessage: ValidationMessage;
    fileLabel: string = "Choose file...";
    @Output() sendResponse: EventEmitter<boolean> = new EventEmitter();

    constructor(public ngbActiveModal: NgbActiveModal, private applicationPageService: ApplicationPageService, private modalService: NgbModal,
        public utilsService: UtilityService, private alertModalService: AlertModalService) {
        this.formData = new FormData();
        this.validationMessage = new ValidationMessage();

    }

    ngOnInit() {
        this.createForm();
    }


    createForm() {
        this.importExcelFB = new FormGroup({}) //<--create the formGroup
        for (let formModule of this.globalControls) {
            this.importExcelFB.addControl(formModule.DisplayLabel, new FormControl(formModule.Value ? formModule.Value : "", formModule.Validators))
        }
        this.importExcelFB.addControl("File", new FormControl());
        this.importExcelFB.addControl("Rows", new FormControl("", Validators.required));
        
    };

    downloadTemplate() {
        let excelImportTmplateRequest = new ExcelImportTmplateRequest();
        excelImportTmplateRequest.FormId = this.formId;
        excelImportTmplateRequest.Rows = this.importExcelFB.controls["Rows"].value;
        if (this.globalControls.length > 0) {
            let globalControlsList :ControlValue []=[];
            let formValues = this.importExcelFB.value;
            this.globalControls.forEach(element => {
                let value = formValues[element.DisplayLabel];
                globalControlsList.push(new ControlValue(element.ControlId, element.ControlIdentifier, value, null))
            });
            excelImportTmplateRequest.GlobalControls = globalControlsList;
        }

        this.observableSubscription = this.applicationPageService.getImportTemplateRequest(excelImportTmplateRequest).subscribe(
            (res) => {
                const blob = new Blob([res.body], { type: res.body.type });
                var fileURL = URL.createObjectURL(blob);
                var a = document.createElement('a');
                document.body.appendChild(a);
                a.setAttribute('style', 'display: none');
                a.href = fileURL;
                a.download = this.pageTitle + ".xlsx";
                a.click();
            },
            (error) => {
                this.alertModalService.setErrorAlertModalTemplate(error.error);
            }
        );
    }

    checkUplaodedFile(file) {
        if (file && file instanceof FileList) {
            if (file.length > 0) return true;
        }
        return false;
    }

    uploadTemplate(event: any) {
        let file = event.target.files;
        this.uplaodedFile = [];
        this.uplaodedFile = file;
        this.formData.delete("File");
        if (this.uplaodedFile.length > 0) {
            if (this.uplaodedFile.length > 1) {
                this.uplaodedFile = [];
                this.alertModalService.setErrorAlertModalTemplate("You can upload only 1 file.");
                return;
            }
            else {
                let extension = this.uplaodedFile[0].name.split('.').pop();
                if (extension != 'xlsx') {
                    this.uplaodedFile = [];
                    this.alertModalService.setErrorAlertModalTemplate("Only .xlsx format files are supported.");
                    return;
                }
            }
        }
    }

    importExcel() {
        this.formData.delete("File");
        if (this.uplaodedFile.length > 0) {
            this.fileLabel = this.uplaodedFile.item(0).name;
            this.formData.append("File", this.uplaodedFile.item(0));
        }
        let importDataVM = new ExcelImportRequest();
        importDataVM.FormId = this.formId;
        this.formData = this.utilsService.createFormData(importDataVM, this.formData);

        this.observableSubscription = this.applicationPageService.importDataFromExcel(this.formData).subscribe(response => {
            this.uplaodedFile = [];
            this.importExcelFB.controls["File"].reset();
            this.sendResponse.emit(response);
            this.close();
        }, error => {
            throw error;
        });
    }
    

    close() {
        this.ngbActiveModal.close();
    }


    ngOnDestroy() {
        if (this.observableSubscription)
            this.observableSubscription.unsubscribe();
    }
}

