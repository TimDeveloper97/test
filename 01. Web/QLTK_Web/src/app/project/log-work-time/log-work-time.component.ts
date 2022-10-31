import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ComboboxService, Constants,DateUtils, MessageService } from 'src/app/shared';
import { NgbActiveModal, NgbTimeAdapter, NgbTimepickerConfig } from '@ng-bootstrap/ng-bootstrap';

import { PlanService } from '../service/plan.service';
import { NtsTimeStringAdapter } from 'src/app/shared/common/nts-time-string-adapter';


@Component({
  selector: 'app-log-work-time',
  templateUrl: './log-work-time.component.html',
  styleUrls: ['./log-work-time.component.scss'],
  providers: [{ provide: NgbTimeAdapter, useClass: NtsTimeStringAdapter }],
  encapsulation: ViewEncapsulation.None

})
export class LogWorkTimeComponent implements OnInit {

  constructor(     
    public constant: Constants,
    private comboboxService: ComboboxService,
    private planService : PlanService,
    private dateUtils: DateUtils,
    private ngbTimepickerConfig: NgbTimepickerConfig,
    private messageService: MessageService,

    ) {
      ngbTimepickerConfig.spinners = false;
     }
  model ={
    EmployeeName :'',
    EmployeeId:'',
    NowDate :'',
    Monthly : {
      TimeRange:'',
      DataMonthlys:[]
    },
    ListWorkTime: [],
    taskAssigns :[]
  };
  ListEmployee :any[]=[];
  
  ListTask =[
    {
      TaskName :'huan',
    }
  ]
  ModelUpdate=[{
    TaskName :'',
    WorkDate : null,
    NumberTime:0,
    TimeStart : null,
    TimeEnd: null,
    Note :''
  }]
  month =0;
  year =0;

  UserLogin ='';
  selectIndex =-1;
  dateSelect = '';
    
  ngOnInit(): void {
    var today = new Date();
    this.month =today.getMonth() +1;
    this.year =today.getFullYear();
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser) {
      this.UserLogin = currentUser.employeeId;
      this.model.EmployeeId = currentUser.employeeId;
      this.getEmployeeInfor(this.model.EmployeeId,this.dateSelect);
    }
    this.getEmployee();
  }

  getEmployee(){
    this.comboboxService.getCbbEmployee().subscribe(
      data =>{
        this.ListEmployee =data;
    });
  }
  getEmployeeInfor(EmployeeId,dateSelect){
    if(dateSelect ==''){
      this.selectIndex =-1;
      this.dateSelect='';
    }
    this.planService.getEmployeeInfor(EmployeeId,this.month,this.year).subscribe(
      data =>{
        this.model =data;
        if(this.selectIndex != -1 && this.dateSelect)
        this.model.Monthly.DataMonthlys.forEach(element =>{
          if(element.DateTime == this.dateSelect){
            this.model.Monthly.DataMonthlys[this.selectIndex].IsSelect  =true;
          }
        });
        this.model.ListWorkTime.forEach(element =>{
          element.TaskWorkTime.forEach(element1 =>{
            if(element1.DateTime ==this.dateSelect){
              element1.IsSelect =true;
            }
          })
        });
        this.ModelUpdate = this.model.taskAssigns
        if(dateSelect !=''){
          this.planService.GetWorkEmployeeByDate(this.model.EmployeeId,dateSelect).subscribe(
            data =>{
              this.ModelUpdate =data;
            }, error => {
              this.messageService.showError(error);
            });
        }
      });
  }

  rightStep(EmployeeId){
    this.month =this.month+1;
    if(this.month >12){
      this.month=1;
      this.year =this.year+1;
    }
    this.selectIndex =-1;
    this.dateSelect='';
    this.getEmployeeInfor(EmployeeId,'');
  }

  leftStep(EmployeeId){
    this.month =this.month-1;
    if(this.month <1){
      this.month=12;
      this.year =this.year-1;
    }
    this.selectIndex =-1;
    this.dateSelect='';
    this.getEmployeeInfor(EmployeeId,'');
  }
  choosenDate(date,index){
    if(this.selectIndex == -1 && this.dateSelect == ''){
      this.selectIndex = index;
      this.dateSelect =date;
      this.model.Monthly.DataMonthlys.forEach(element =>{
        if(element.DateTime == this.dateSelect){
          this.model.Monthly.DataMonthlys[index].IsSelect  =true;
        }
      });
      this.model.ListWorkTime.forEach(element =>{
        element.TaskWorkTime.forEach(element1 =>{
          if(element1.DateTime ==date){
            element1.IsSelect =true;
          }
        })
      });
      var ojDate =  this.dateUtils.convertDateToObject(date);
      this.ModelUpdate.forEach( (element,i) =>{
        element.WorkDate =ojDate;
      });
    }else{
      this.model.Monthly.DataMonthlys[this.selectIndex].IsSelect  =false;
      this.model.ListWorkTime.forEach(element =>{
        element.TaskWorkTime.forEach(element1 =>{
          if(element1.DateTime ==this.dateSelect){
            element1.IsSelect =false;
          }
        })
      });

      this.selectIndex = index;
      this.dateSelect =date;
      this.model.Monthly.DataMonthlys[index].IsSelect  =true;
      this.model.ListWorkTime.forEach(element =>{
        element.TaskWorkTime.forEach(element1 =>{
          if(element1.DateTime ==date){
            element1.IsSelect =true;
          }
        })
      });
      var ojDate =  this.dateUtils.convertDateToObject(date);
      this.ModelUpdate.forEach( (element,i) =>{
        element.WorkDate =ojDate;
      });
    }

    this.planService.GetWorkEmployeeByDate(this.model.EmployeeId,date).subscribe(
      data =>{
        this.ModelUpdate =data;
      }, error => {
        this.messageService.showError(error);
      });
    
  }

  UpdateTime(){
    this.ModelUpdate.forEach((element,i) =>{
      element.WorkDate = this.dateSelect;
    });
    var model ={
      employeeId : this.model.EmployeeId,
      TaskAssign : this.ModelUpdate
    }

    this.planService.updateWorkTime(model).subscribe(
      data =>{
        this.getEmployeeInfor(this.model.EmployeeId,this.dateSelect); 
        this.messageService.showSuccess('Cập nhật thời gian thành công!');
      }, error => {
        this.messageService.showError(error);
      });
  }

}
