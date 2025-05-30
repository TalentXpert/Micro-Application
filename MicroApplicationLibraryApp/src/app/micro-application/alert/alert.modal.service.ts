import { Injectable } from '@angular/core';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { AlertModalComponent } from './alert.modal.component';

export class AlertModalVM {
    public Type: string="";
    public ModalTitle: string="";
    public ModalBody: string="";
    public Note:string="";
    public UnorderedList:string[]=[];
}

@Injectable()
export class AlertModalService {

    public alertModalVM: AlertModalVM = new AlertModalVM();
    public modalShow: boolean = false;
    
    // Modal custom configuration
  ngbModalOptions: NgbModalOptions = {
    backdrop: 'static',
    keyboard: false,
    windowClass: "AlertMsgModal",
  };
  
    constructor(private modalService: NgbModal) { }

    //alert model
    setAlertModalTemplate(alertMessage:string) {
        this.alertModalVM.Type = 'alert';
        this.alertModalVM.ModalBody = alertMessage;
        this.alertModalVM.Note = '';
        const alertModalRef = this.modalService.open(AlertModalComponent, this.ngbModalOptions);
        alertModalRef.componentInstance.alertModalVM = this.alertModalVM;
    }

    //error model
    setErrorAlertModalTemplate(errorMessage:string) {
        this.alertModalVM.Type = 'error';
        this.alertModalVM.ModalBody = errorMessage;
        this.alertModalVM.Note = '';
        if (this.modalShow == false) {
            this.modalShow = true;
            const alertModalRef = this.modalService.open(AlertModalComponent, this.ngbModalOptions);
            alertModalRef.componentInstance.alertModalVM = this.alertModalVM;
            alertModalRef.componentInstance.sendResponse.subscribe(
                (modalResponse:boolean) => {
                    if (modalResponse) {
                        alertModalRef.close();
                        this.modalShow = false;
                    }
                }
            )
        }
    }

    //success model
    setSuccessAlertModalTemplate(successMessage:string) {
        this.alertModalVM.Type = 'success';
        this.alertModalVM.ModalBody = successMessage;
        this.alertModalVM.Note = '';
        const alertModalRef = this.modalService.open(AlertModalComponent, this.ngbModalOptions);
        alertModalRef.componentInstance.alertModalVM = this.alertModalVM;
    }

    //warning model
    setWarningModalTemplate(successMessage:string) {
        this.alertModalVM.Type = 'warning';
        this.alertModalVM.ModalBody = successMessage;
        this.alertModalVM.Note = '';
        const alertModalRef = this.modalService.open(AlertModalComponent, this.ngbModalOptions);
        alertModalRef.componentInstance.alertModalVM = this.alertModalVM;
    }

    //information model
    setInformaitonModalTemplate(successMessage:string) {
        this.alertModalVM.Type = 'information';
        this.alertModalVM.ModalBody = successMessage;
        this.alertModalVM.Note = '';
        const alertModalRef = this.modalService.open(AlertModalComponent, this.ngbModalOptions);
        alertModalRef.componentInstance.alertModalVM = this.alertModalVM;
    }

    //success alert model with note
    setSuccessAlertModalTemplateWithNote(successMessage:string, note:any) {
        this.alertModalVM.Type = 'success';
        this.alertModalVM.ModalBody = successMessage;
        this.alertModalVM.Note=note;
        const alertModalRef = this.modalService.open(AlertModalComponent, this.ngbModalOptions);
        alertModalRef.componentInstance.alertModalVM = this.alertModalVM;
    }
    //success alert model 
    setFormErrorListTemplate(formDataMessage:string[],modalTitle:string) {
        this.alertModalVM.Type = 'alert';
        this.alertModalVM.ModalTitle = modalTitle;
        this.alertModalVM.UnorderedList = [...formDataMessage];
        const alertModalRef = this.modalService.open(AlertModalComponent, this.ngbModalOptions);
        alertModalRef.componentInstance.alertModalVM = this.alertModalVM;
        alertModalRef.componentInstance.sendResponse.subscribe((IsResponse: boolean) => {
            if (IsResponse) {
              this.alertModalVM.UnorderedList=[];
            }
          });
    }
}
