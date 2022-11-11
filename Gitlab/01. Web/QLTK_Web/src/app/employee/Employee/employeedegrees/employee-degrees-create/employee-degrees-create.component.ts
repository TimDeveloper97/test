import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService, ComboboxService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { EmployeeDegreeServiceService } from '../../../service/employee-degree-service.service';


@Component({
  selector: 'app-employee-degrees-create',
  templateUrl: './employee-degrees-create.component.html',
  styleUrls: ['./employee-degrees-create.component.scss']
})
export class EmployeeDegreesCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private comboboxService: ComboboxService,
    private employeeDegressService: EmployeeDegreeServiceService,
    private checkSpecialCharacter: CheckSpecialCharacter
  ) { }

  ModalInfo = {
    Title: 'Thêm mới bằng cấp',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;
  IdDegrees: string;

  model: any ={
    Id:'',
    QualificationId:'',
    EmployeeId:'',
    ClassIficationId:'',
    Name:'',
    Code:'',
    Year:'',
    School:'',
    Rank:'', 
  }
  listClassIfication: any =[];
  listQualification : any =[];
  listCBBYear : any=[];
  ngOnInit() {
    this.model.EmployeeId = this.Id;
    this.getYear();
    this.getQualification();
    this.GetCBBClassIfication();
    if(this.IdDegrees){
      this.ModalInfo.Title='Chỉnh sửa bằng cấp';
      this.ModalInfo.SaveText = 'Lưu';
      this.get();
      
    }
    else
    {
      this.ModalInfo.Title = 'Thêm mới bằng cấp';
    }
  }

  getQualification(){
    this.comboboxService.getCBBQualification().subscribe((data: any) => {
      this.listQualification = data;

    });
  }
  

  GetCBBClassIfication(){
    this.comboboxService.GetCBBClassIfication().subscribe((data: any) => {
      this.listClassIfication = data;
    });
  }

  get(){
    this.employeeDegressService.GetInfos({Id: this.IdDegrees}).subscribe(data=> {
      this.model = data;
    });
  }

  getYear(){
    var listYear = new Date();
    var list = (listYear.getFullYear()+5 - 1970);
    for(var i=0; i <=list; i++){
      this.listCBBYear.push(1970+i);
    }
    this.listCBBYear.sort(function(a,b){return b-a});
    
  }
   
  createEmployDegree(isContinue){
    //kiểm tra ký tự đặc việt trong Mã
    var  validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.model.CreateBy  = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
      this.model.EmployeeId= this.Id;
      this.employeeDegressService.Add(this.model).subscribe(
        data => {
          if (isContinue) {
            this.isAction = true;
            
            this.model = {};
            this.messageService.showSuccess('Thêm mới bằng cấp thành công!');
          }
          else {
            this.messageService.showSuccess('Thêm mới bằng cấp thành công!');
            this.closeModal(true);
          }
        },
        error => {
          this.messageService.showError(error);
        }
      );
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.model.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
         
          this.employeeDegressService.Add(this.model).subscribe(
            data => {
              if (isContinue) {
                this.isAction = true;
                this.model = {};
                this.messageService.showSuccess('Thêm mới bằng cấp thành công!');
              }
              else {
                this.messageService.showSuccess('Thêm mới bằng cấp thành công!');
                this.closeModal(true);
              }
            },
            error => {
              this.messageService.showError(error);
            }
          );
        },
        error => {
          
        }
      );
    }
  }

  updateEmployDegree(){
    //kiểm tra ký tự đặc việt trong Mã
    var  validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
      this.employeeDegressService.Updates(this.model).subscribe(
        () => {
          this.activeModal.close(true);
          this.messageService.showSuccess('Cập nhật bằng cấp thành công!');
        },
        error => {
          this.messageService.showError(error);
        }
      );
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
          this.employeeDegressService.Updates(this.model).subscribe(
            () => {
              this.activeModal.close(true);
              this.messageService.showSuccess('Cập nhật bằng cấp thành công!');
            },
            error => {
              this.messageService.showError(error);
            }
          );
        },
        error => {
          
        }
      );
    }
  }

  save(isContinue: boolean) {
    if (this.IdDegrees) {
      this.updateEmployDegree();
    }
    else {
      this.createEmployDegree(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
