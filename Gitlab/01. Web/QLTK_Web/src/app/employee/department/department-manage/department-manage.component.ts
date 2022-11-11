import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { DepartmentService } from '../../service/department.service';
import { DepartmentCreateComponent } from '../department-create/department-create.component';

@Component({
  selector: 'app-department-manage',
  templateUrl: './department-manage.component.html',
  styleUrls: ['./department-manage.component.scss']
})
export class DepartmentManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    private departmentservice: DepartmentService,
    public constant: Constants
  ) { }

  startIndex = 0;
  startIndexWorkType = 1;
  startIndexWork = 1;


  listData: any[] = [];
  listManager: any[] = [];
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    Name: '',
    Code: '',
    ManagerId: '',
    ManagerName: '',
    Status: ''
  }
  selectIndex =-1;
  selectIndexWorkType =-1;
  Results : any =[];
  listWorkType: any[] = [];

  listTask: any[] = [];
  modelSelect :any ={
    DepartmentId :'',
    WorkLocation :''
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã phòng ban',
    Items: [
      {
        Name: 'Tên phòng ban',
        FieldName: 'Name',
        Placeholder: 'Nhập tên phòng ban',
        Type: 'text'
      },
      {
        Name: 'Tên trưởng phòng',
        FieldName: 'ManagerId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Manager,
        Columns: [{ Name: 'Code', Title: 'Mã trưởng phòng' }, { Name: 'Name', Title: 'Tên trưởng phòng' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        Placeholder: 'Chọn trưởng phòng'
      },
      {
        Name: 'Tình trạng',
        FieldName: 'Status',
        Placeholder: 'Tình trạng',
        Type: 'select',
        Data: this.constant.DeparymentStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };
  ngOnInit() {
    this.appSetting.PageTitle = "Quản lý phòng ban";
    this.searchDepartment();
    this.getCbManager();
    this.getAll();
  }

  searchDepartment() {
    this.departmentservice.searchDepartment(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
        this.listData.forEach(element =>{
          element.LessThanOneExperiencePercen =(element.LessThanOneExperience/element.TotalEmployee)*100;
          element.OneToThreeExperiencePercen =(element.OneToThreeExperience/element.TotalEmployee)*100;
          element.FourToSevenExperiencePercen =(element.FourToSevenExperience/element.TotalEmployee)*100;
          element.EightToTwelveExperiencePercen =(element.EightToTwelveExperience/element.TotalEmployee)*100;
          element.ThirteenToEighteenExperiencePercen =(element.ThirteenToEighteenExperience/element.TotalEmployee)*100;
          element.NineteenToTwentyFiveExperiencePercen =(element.NineteenToTwentyFiveExperience/element.TotalEmployee)*100;
          element.GreaterThanTwentyFiveExperiencePercen =(element.GreaterThanTwentyFiveExperience/element.TotalEmployee)*100;
        });
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getCbManager() {
    this.departmentservice.getCbbManager().subscribe(
      data => {
        this.listManager = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,
      Id: '',
      Name: '',
      Code: '',
      ManagerName: '',
      Status: ''
    }
    this.searchDepartment();
  }

  showConfirmDeleteDepartment(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá phòng ban này không?").then(
      data => {
        this.deleteDepartment(Id);
      },
      error => {
        
      }
    );
  }

  deleteDepartment(Id: string) {
    this.departmentservice.deleteDepartment({ Id: Id }).subscribe(
      data => {
        this.searchDepartment();
        this.messageService.showSuccess('Xóa phòng ban thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(DepartmentCreateComponent, { container: 'body', windowClass: 'department-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchDepartment();
      }
    }, (reason) => {
    });
  }

  selectDepartment(i){
    if (this.selectIndex != i) {
      this.selectIndex = i;
      this.modelSelect.DepartmentId =this.listData[i].Id;
      this.departmentservice.SearchWorkByDepartment(this.modelSelect).subscribe(
        data => {
          this.Results = data;
          this.listWorkType =data.WorkTypes;
          this.listTask =data.works;
          this.listWorkType.forEach(element =>{
            element.LessThanOneExperiencePercen =(element.LessThanOneExperience/element.TotalEmployee)*100;
            element.OneToThreeExperiencePercen =(element.OneToThreeExperience/element.TotalEmployee)*100;
            element.FourToSevenExperiencePercen =(element.FourToSevenExperience/element.TotalEmployee)*100;
            element.EightToTwelveExperiencePercen =(element.EightToTwelveExperience/element.TotalEmployee)*100;
            element.ThirteenToEighteenExperiencePercen =(element.ThirteenToEighteenExperience/element.TotalEmployee)*100;
            element.NineteenToTwentyFiveExperiencePercen =(element.NineteenToTwentyFiveExperience/element.TotalEmployee)*100;
            element.GreaterThanTwentyFiveExperiencePercen =(element.GreaterThanTwentyFiveExperience/element.TotalEmployee)*100;
          });

        },
        error => {
          this.messageService.showError(error);
        }
      );
    }
    else {
      this.selectIndex = -1;
      this.modelSelect.DepartmentId ='';
      this.getAll();

    }
  }
  selectWorkType(i){
    if (this.selectIndexWorkType != i) {
      this.selectIndexWorkType = i;
      this.modelSelect.WorkTypeId =this.listWorkType[i].Id;
      this.departmentservice.SearchWorkByDepartment(this.modelSelect).subscribe(
        data => {
          this.Results = data;
          this.listWorkType =data.WorkTypes;
          this.listTask =data.works;
          this.listWorkType.forEach(element =>{
            element.LessThanOneExperiencePercen =(element.LessThanOneExperience/element.TotalEmployee)*100;
            element.OneToThreeExperiencePercen =(element.OneToThreeExperience/element.TotalEmployee)*100;
            element.FourToSevenExperiencePercen =(element.FourToSevenExperience/element.TotalEmployee)*100;
            element.EightToTwelveExperiencePercen =(element.EightToTwelveExperience/element.TotalEmployee)*100;
            element.ThirteenToEighteenExperiencePercen =(element.ThirteenToEighteenExperience/element.TotalEmployee)*100;
            element.NineteenToTwentyFiveExperiencePercen =(element.NineteenToTwentyFiveExperience/element.TotalEmployee)*100;
            element.GreaterThanTwentyFiveExperiencePercen =(element.GreaterThanTwentyFiveExperience/element.TotalEmployee)*100;
          });
        },
        error => {
          this.messageService.showError(error);
        }
      );
    }
    else {
      this.selectIndexWorkType = -1;
      this.modelSelect.WorkTypeId='';
      this.getAll();
    }
  }
  getAll(){
    this.departmentservice.SearchWorkByDepartment(this.modelSelect).subscribe(
      data => {
        this.Results = data;
        this.listWorkType =data.WorkTypes;
        this.listTask =data.works;
        this.listWorkType.forEach(element =>{
          element.LessThanOneExperiencePercen =(element.LessThanOneExperience/element.TotalEmployee)*100;
          element.OneToThreeExperiencePercen =(element.OneToThreeExperience/element.TotalEmployee)*100;
          element.FourToSevenExperiencePercen =(element.FourToSevenExperience/element.TotalEmployee)*100;
          element.EightToTwelveExperiencePercen =(element.EightToTwelveExperience/element.TotalEmployee)*100;
          element.ThirteenToEighteenExperiencePercen =(element.ThirteenToEighteenExperience/element.TotalEmployee)*100;
          element.NineteenToTwentyFiveExperiencePercen =(element.NineteenToTwentyFiveExperience/element.TotalEmployee)*100;
          element.GreaterThanTwentyFiveExperiencePercen =(element.GreaterThanTwentyFiveExperience/element.TotalEmployee)*100;
        });
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
}
