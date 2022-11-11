import { Component, OnInit, Input, ElementRef, ViewChild, AfterViewInit } from '@angular/core';
import { AppSetting, Constants, FileProcess, MessageService } from 'src/app/shared';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { EmployeeServiceService } from '../../service/employee-service.service';
//import { ShowSelectSkillEmployeeComponents } from '../show-select-skill-employee/show-select-skill-employee.component';
//import { ShowSelectSkillEmployeeComponent } from 'src/app/employee/worktype/show-select-skill-employee/show-select-skill-employee.component';
import { ShowSelectSkillEmployeeComponents } from '../../Employee/show-select-skill-employee/show-select-skill-employee.component';

@Component({
  selector: 'app-show-employee-skill-details',
  templateUrl: './show-employee-skill-details.component.html',
  styleUrls: ['./show-employee-skill-details.component.scss']
})
export class ShowEmployeeSkillDetailsComponent implements OnInit {

  @Input() Id: string;
  @Input() EmployeeName: string;
  @Input() EmployeeCode: string;
  @Input() WorkType : string;
  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    private modalService: NgbModal,
    public appset: AppSetting,
    public constant: Constants,
    private serviceEmployeeSkillDetails: EmployeeServiceService
  ) { }
  @ViewChild('scrollPracticeMaterial',{static:false}) scrollPracticeMaterial: ElementRef;
  @ViewChild('scrollPracticeMaterialHeader',{static:false}) scrollPracticeMaterialHeader: ElementRef;
  
  isValid: boolean = false;
  editField1: string;
  editField2: string;
  contenteditable: boolean = false;
  listData: any[] = [];
  modelEmployeeSkillDetails: any = {
    page: 1,
    PageSize: 10,
    TotalItem: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    EmployeeId: '',
    LevelRate: '',
    RateDate: ''
  }
  model: any = {
    page: 1,
    PageSize: 10,
    TotalItem: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    EmployeeId: '',
    WorkSkillId: '',
  }
  ngOnInit() {
    this.modelEmployeeSkillDetails.EmployeeId = this.Id;
    this.model.EmployeeId = this.Id;
    this.getSkillEmployeeInfo();
  }

  ngAfterViewInit(){
    this.scrollPracticeMaterial.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollPracticeMaterialHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollPracticeMaterial.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  getSkillEmployeeInfo() {
    this.model.EmployeeId = this.Id;
    this.serviceEmployeeSkillDetails.searchSkillEmployee(this.model).subscribe(data => {
      this.model = data;
      this.listBase = data.ListResult;
    });
  }

  listBase = [];
  
  showClick() {
    let activeModal = this.modalService.open(ShowSelectSkillEmployeeComponents, { container: 'body', windowClass: 'select-skill-employee-model', backdrop: 'static' });
    //var ListIdSelectRequest = [];
    var ListIdSelect = [];
    this.listBase.forEach(element => {
      ListIdSelect.push(element.Id);
    });
    activeModal.componentInstance.model.EmployeeId = this.Id;
    activeModal.componentInstance.model.WorkTypeId = this.WorkType;
    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listBase.push(element);
        });
      }
    }, (reason) => {

    });
  }

  showConfirmDeleteSkill(row) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá kỹ năng này không?").then(
      data => {
        this.removeRowSkill(row);
      },
      error => {
        
      }
    );
  }

  removeRowSkill(row) {
    var index = this.listBase.indexOf(row);
    if (index > -1) {
      this.listBase.splice(index, 1);
    }
  }

  addDegree() {
    this.listBase.forEach(element => {
      if (element.Mark < 0) {
        this.messageService.showMessage('Điểm thấp nhất bằng 0!');
        return;
      }
      if (element.Grade < 0) {
        this.messageService.showMessage('Điểm thấp nhất bằng 0!');
        return;
      }
    })
    this.model.EmployeeId = this.Id;
    this.model.ListResult = this.listBase;
    this.serviceEmployeeSkillDetails.addSkillEmployee(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Thêm mới kĩ năng thành công!');
        this.getSkillEmployeeInfo();
      },
      error => {
        this.messageService.showError(error);
      });
  }

  Save() {
    this.addDegree()
  }

}
