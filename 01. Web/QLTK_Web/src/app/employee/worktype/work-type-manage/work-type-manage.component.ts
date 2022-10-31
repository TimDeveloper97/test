import { Component, OnInit } from '@angular/core';
import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';
import { EmployeeWorkTypeService } from '../../service/employee-work-type.service';
import { WorkTypeCreateComponent } from '../work-type-create/work-type-create.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-work-type-manage',
  templateUrl: './work-type-manage.component.html',
  styleUrls: ['./work-type-manage.component.scss'],

})
export class WorkTypeManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private workTypeService: EmployeeWorkTypeService,
    private titleservice: Title,
    public constant: Constants,
    private router: Router,
  ) { }

  StartIndex = 0;
  listData: any[] = [];

  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true,

    Name: null,
    SBUId: null,
    DepartmentId: null,
    FlowStageId: null

  }
  ngOnInit() {
    this.appSetting.PageTitle = "Quản lý vị trí công việc";
    this.searchWorkType();
  }
  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo vị trí công việc',
    Items: [
      {
        Name: 'SBU',
        FieldName: 'SBUId',
        Placeholder: 'SBU',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.SBU,
        Columns: [{ Name: 'Code', Title: 'Mã SBU' }, { Name: 'Name', Title: 'Tên SBU' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        IsRelation: true,
        RelationIndexTo: 1
      },
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng ban',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.Department,
        Columns: [{ Name: 'Code', Title: 'Mã phòng ban' }, { Name: 'Name', Title: 'Tên phòng ban' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        RelationIndexFrom: 0
      },
      {
        Name: 'Dòng chảy',
        FieldName: 'FlowStageId',
        Placeholder: 'Dòng chảy',
        Type: 'dropdowntree',
        SelectMode: 'single',
        ParentId: 'ParentId',
        DataType: this.constant.SearchDataType.FlowStage,
        Columns: [{ Name: 'Code', Title: 'Mã dòng chảy' }, { Name: 'Name', Title: 'Tên dòng chảy' }],
        DisplayName: 'Name',
        ValueName: 'Id',
      },
    ]
  };
  searchWorkType() {
    this.workTypeService.searchWorkType(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Name',
      OrderType: true,

      Name: null,
      SBUId: null,
      DepartmentId: null,
      FlowStageId: null
    }
    this.searchWorkType();
  }

  // showCreateUpdate(Id: string) {// s
  //   let activeModal = this.modalService.open(WorkTypeCreateComponent, { container: 'body', windowClass: 'WorkSkill-create-model', backdrop: 'static' })
  //   activeModal.componentInstance.Id = Id;
  //   activeModal.result.then((result) => {
  //     if (result) {
  //       this.searchWorkType();
  //     }
  //   }, (reason) => {
  //   });
  // }

  showCreateUpdate(id: string) {
    if (id) {
      this.router.navigate(['nhan-vien/quan-ly-vi-tri-cong-viec/chinh-sua/', id]);
    } else {
      this.router.navigate(['nhan-vien/quan-ly-vi-tri-cong-viec/them-moi']);
    }
  }

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá vị trí công việc này không?").then(
      data => {
        this.delete(Id);
      },
      error => {

      }
    );
  }

  delete(Id: string) {
    this.workTypeService.deleteWorkType({ Id: Id }).subscribe(
      data => {
        this.searchWorkType();
        this.messageService.showSuccess('Xóa vị trí công việc thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }


}
