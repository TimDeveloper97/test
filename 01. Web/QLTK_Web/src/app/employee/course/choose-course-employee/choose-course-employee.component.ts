import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, FileProcess, Constants, AppSetting } from 'src/app/shared';
import { ClassRoomService } from 'src/app/education/service/class-room.service';

@Component({
  selector: 'app-choose-course-employee',
  templateUrl: './choose-course-employee.component.html',
  styleUrls: ['./choose-course-employee.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChooseCourseEmployeeComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public constants: Constants,
    public appset: AppSetting,
    private classRoomService : ClassRoomService,
  ) { }

  isAction: boolean = false;

  ngOnInit() {
  }

  
  searchEmployee(){

  }

  clear(){
    
  }

  addRow() {
    // this.listBase.forEach(element => {
    //   if (element.Checked) {
    //     this.listSelect.push(element);
    //   }
    // });
    // this.listSelect.forEach(element => {
    //   var index = this.listBase.indexOf(element);
    //   if (index > -1) {
    //     this.listBase.splice(index, 1);
    //   }
    // });
  }

  removeRow() {
    // this.listSelect.forEach(element => {
    //   if (element.Checked) {
    //     this.listBase.push(element);
    //   }
    // });
    // this.listBase.forEach(element => {
    //   var index = this.listSelect.indexOf(element);
    //   if (index > -1) {
    //     this.listSelect.splice(index, 1);
    //   }
    // });
  }

  choose() {
    // this.activeModal.close(this.listSelect);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }


}
