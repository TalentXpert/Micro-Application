import { Component, OnDestroy, OnInit, Input, EventEmitter, Output, ChangeDetectorRef } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AlertModalVM,  } from './alert.modal.service';

@Component({
    templateUrl: './alert.modal.component.html',
    standalone:false
})
export class AlertModalComponent implements OnInit, OnDestroy {
    alertModal:AlertModalVM;
    @Input() public alertModalVM: AlertModalVM = new AlertModalVM();
    @Output () sendResponse: EventEmitter<any> = new EventEmitter();


    constructor(public ngbActiveModal: NgbActiveModal,private cdr: ChangeDetectorRef) {} 

    ngOnInit() { }

    ngAfterViewInit() {
        this.alertModal=new AlertModalVM();
        this.alertModal=this.alertModalVM;
        this.cdr.detectChanges();
      }

    onClickOk(response) {
        if(response==="You haven't logged into system yet."){
            var data={flag:true}
        }else{
            this.alertModal.UnorderedList=[];
            this.sendResponse.emit(true);
        }       
    }

    hasData(array) {
        if (!array) return false;
        if (array.length > 0) return true;
        return false;
      }
    
    getClassforMessage(){
        if(this.alertModalVM.ModalBody){
            if(this.alertModalVM.ModalBody.length > 53){
                return'text-justify mb-0 alert-message';
            }
        }
        return'text-center mb-0 alert-message';        
    }
    getClassforNote(){
        if(this.alertModalVM.Note.length > 53){
            return'text-justify mb-1';
        }
        return'text-center mb-1';
    }
    ngOnDestroy() {}
 }