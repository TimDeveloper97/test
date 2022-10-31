import { Component, OnInit } from '@angular/core';
import { AppSetting, MessageService, ComboboxService, Constants, Configuration } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ProjectPhaseService } from '../../service/project-phase.service';
import { ProjectPhaseCreateComponent } from '../project-phase-create/project-phase-create.component';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-project-phase-manage',
  templateUrl: './project-phase-manage.component.html',
  styleUrls: ['./project-phase-manage.component.scss']
})
export class ProjectPhaseManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    public constant: Constants,
    private config :  Configuration,
    private projectPhaseService : ProjectPhaseService,
    private route: ActivatedRoute
  ) { }

  startIndex = 1;
  listData: any[] = [];
  selectedProjectPhasesId = '';
  listProjectPhases: any[] = [];
  listProjectPhasesId = [];
  projectPhasesId: '';

  model: any = {
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    Code: '',
    Name: '',
    Description:'',
    SBUId: '',
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Mã giai đoạn dự án',
    Items: [
      {
        Name: 'Tên giai đoạn dự án',
        FieldName: 'Name',
        Placeholder: 'Nhập tên giai đoạn',
        Type: 'text'
      },
      {
        Name: 'SBU',
        FieldName: 'SBUId',
        Placeholder: 'SBU',
        Type: 'select',
        DataType: this.constant.SearchDataType.SBU,
        DisplayName: 'Name',
        ValueName: 'Id',
        IsRelation: true,
        Permission: ['F060005'],
        RelationIndexTo: 2
      },
    ]
  };
  ngOnInit() {
    this.appSetting.PageTitle = "Quản lý giai đoạn dự án";
    this.searchProjectPhases();
    this.selectedProjectPhasesId = localStorage.getItem("selectedProjectPhasesId");
    localStorage.removeItem("selectedProjectPhasesId");
  }

  onSelectionChanged(e) {
    if (e.selectedRowKeys[0] != null && e.selectedRowKeys[0] != this.selectedProjectPhasesId) {
      this.selectedProjectPhasesId = e.selectedRowKeys[0];
      this.projectPhasesId = e.selectedRowKeys[0];
    }
  }

  itemClick(e) {
    if (this.projectPhasesId == '' || this.projectPhasesId == null) {
      this.messageService.showMessage("Đây không phải chủng loại cuộc họp!")
    } else {
      if (e.itemData.Id == 1) {
        this.showCreateUpdate(this.projectPhasesId);
      }
      else if (e.itemData.Id == 2) {
        this.showConfirmDelete(this.projectPhasesId);
      }
    }
  }

  searchProjectPhases() {
    this.projectPhaseService.searchProjectPhase(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.totalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.model = {
      page: 1,
      PageSize: 10,
      totalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      Id: '',
      Name: '',
      Code: '',
    }
    this.searchProjectPhases();
  }

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá giai đoạn dự án này không này không?").then(
      data => {
        this.delete(Id);
      },
      error => {
        
      }
    );
  }

  delete(Id: string) {
    this.projectPhaseService.deleteProjectPhase({ Id: Id }).subscribe(
      data => {
        this.searchProjectPhases();
        this.messageService.showSuccess('Xóa ngành thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(ProjectPhaseCreateComponent, { container: 'body', windowClass: 'project-phase-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchProjectPhases();
      }
    }, (reason) => {
    });
  }
}
