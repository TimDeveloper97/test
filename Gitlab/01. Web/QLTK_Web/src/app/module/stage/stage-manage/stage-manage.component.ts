import { Component, OnInit } from '@angular/core';
import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { StageService } from '../../services/stage.service';
import { StageCreateComponent } from '../stage-create/stage-create.component';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-stage-manage',
  templateUrl: './stage-manage.component.html',
  styleUrls: ['./stage-manage.component.scss']
})
export class StageManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    public constant: Constants,
    private service: StageService
  ) { }
  startIndex = 0;
  listData: any[] = [];

  sbuid = '';
  departmentId = '';

  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true,
    Id: '',
    Name: '',
    DepartmentId: this.departmentId,
    SBUId: this.sbuid,
  }

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo mã/tên công đoạn',
    Items: [
      {
        Name: 'SBU',
        FieldName: 'SBUId',
        Placeholder: 'SBU',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.SBU,
        DisplayName: 'Name',
        ValueName: 'Id',
        Columns: [{ Name: 'Code', Title: 'Mã SBU' }, { Name: 'Name', Title: 'Tên SBU' }],
        IsRelation: true,
        RelationIndexTo: 1,
        Permission: ['F110307'],
      },
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng ban',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Department,
        DisplayName: 'Name',
        Columns: [{ Name: 'Code', Title: 'Mã phòng ban' }, { Name: 'Name', Title: 'Tên phòng ban' }],
        ValueName: 'Id',
        RelationIndexFrom: 0,
        Permission: ['F110307'],
      },
    ]
  };


  ngOnInit() {
    this.appSetting.PageTitle = "Quản lý công đoạn";

    let userStore = localStorage.getItem('qltkcurrentUser');
    if (userStore) {
      this.sbuid = JSON.parse(userStore).sbuId;
      this.departmentId = JSON.parse(userStore).departmentId;
    }
    this.search();
  }

  search() {
    this.service.search(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        //this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
      }
    }, error => {
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
      Id: '',
      Name: '',
      DepartmentId: '',
      SBUId: '',
    }
    this.search();
  }

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá công đoạn này không?").then(
      data => {
        this.delete(Id);
      },
      error => {
        
      }
    );
  }

  delete(Id: string) {
    this.service.delete({ Id: Id }).subscribe(
      data => {
        this.search();
        this.messageService.showSuccess('Xóa công đoạn thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(StageCreateComponent, { container: 'body', windowClass: 'stage-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.search();
      }
    }, (reason) => {
    });
  }

  onDrop(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.listData, event.previousIndex, event.currentIndex);

    this.modelStageModule.ListStage = [];
    this.save();
  }

  modelStageModule: any = {
    Id: '',
    ListStage: [],
  }
  save(){
    this.listData.forEach(element => {
      if (element) {
        this.modelStageModule.ListStage.push(element);
      }
    });
    this.service.createIndex(this.modelStageModule).subscribe(
      data => {
          this.messageService.showSuccess('Thêm mới công đoạn thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
}
