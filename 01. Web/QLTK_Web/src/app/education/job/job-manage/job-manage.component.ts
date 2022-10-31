import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DxTreeListComponent } from 'devextreme-angular';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

import { AppSetting, Configuration, MessageService, Constants, ComboboxService } from 'src/app/shared';
import { JobServiceService } from '../../service/job-service.service';
import { JobGroupService } from '../../service/job-group.service';
import { JobgroupCreateComponent } from '../../jobgroup/jobgroup-create/jobgroup-create.component';

@Component({
  selector: 'app-job-manage',
  templateUrl: './job-manage.component.html',
  styleUrls: ['./job-manage.component.scss']
})
export class JobManageComponent implements OnInit {
  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    public appSetting: AppSetting,
    private router: Router,
    private config: Configuration,
    private messageService: MessageService,
    private modalService: NgbModal,
    private jobService: JobServiceService,
    private comboboxService: ComboboxService,
    public constant: Constants,
    private jobGroupServiec: JobGroupService,
  ) {  this.pagination = Object.assign({}, appSetting.Pagination);
    this.items = [
    //{ Id: 1, text: 'Thêm mới loại phòng học', icon: 'fas fa-plus' },
    { Id: 2, text: 'Chỉnh sửa nhóm nghề', icon: 'fa fa-edit' },
    { Id: 3, text: 'Xóa', icon: 'fas fa-times' }
  ];
}
items: any;
  startIndex = 0;
  pagination;
  lstpageSize = [5, 10, 15, 20, 25, 30];
  listJob: any[] = [];
  listGroupJob = [];
  listDegree = [];
  ListJobSubject = [];
  SubjectName: string;
  logUserId: string;
  ListJobGroup: any[] = [];
  ListJobGroupId = [];
  selectedJobGroupId = '';
  JobGroupId = '';
  GroupId: string;
  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };
  modelGroupJob: any = {
    Id: '',
    Name: '',
  }
  modelDegree: any = {
    Id: '',
    Name: '',
  }
  model: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    Name: '',
    Code: '',
    Description: '',
    DegreeId: '',
    JobGroupId: '',
    SubjectId: '',
    SubjectName: '',

  }

  modelJopGroup: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    Name: '',
    Code: '',
    Description: '',
  }

  modelAll: any = {
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Cdoe',
    OrderType: true,

    Id: '',
    Name: 'Tất cả',
    Code: '',
    Description: '',
  }
  height = 0;

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã nghề',
    Items: [
      {
        Name: 'Tên nghề',
        FieldName: 'Name',
        Placeholder: 'Nhập tên nghề',
        Type: 'text'
      },
      {
        Name: 'Trình độ',
        FieldName: 'DegreeId',
        Placeholder: 'Trình độ',
        Type: 'select',
        DataType: this.constant.SearchDataType.Degree,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Môn học',
        FieldName: 'SubjectName',
        Placeholder: 'Nhập môn học',
        Type: 'text'
      },
    ]
  };

  ngOnInit() {
    this.height = window.innerHeight - 140;
    this.appSetting.PageTitle = "Quản lý nghề";
    this.searchJobGroups();
    this.searchJob("");
    this.getListGroupJob();
    this.getListDegree();
    this.selectedJobGroupId = localStorage.getItem("selectedJobGroupId");
    localStorage.removeItem("selectedJobGroupId");
  }

  onSelectionChanged(e) {
    if(e.selectedRowKeys[0] != null  && e.selectedRowKeys[0] != this.selectedJobGroupId)
    {
      this.selectedJobGroupId = e.selectedRowKeys[0];
      this.searchJob(e.selectedRowKeys[0]);
      this.JobGroupId = e.selectedRowKeys[0];
    }
  }

  searchJobGroups() {
    this.jobGroupServiec.SearchJobGroup(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.ListJobGroup = data.ListResult;
        this.ListJobGroup.unshift(this.modelAll);
        this.modelJopGroup.totalItems = data.TotalItem;
        if (this.selectedJobGroupId == null) {
          this.selectedJobGroupId = this.ListJobGroup[0].Id;
        }

        this.treeView.selectedRowKeys = [this.selectedJobGroupId];
        for (var item of this.ListJobGroup) {
          this.ListJobGroupId.push(item.Id);
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchJob(JobGroupId: string) {
    this.model.JobGroupId = JobGroupId;
    this.jobService.SearchJob(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listJob = data.ListResult;
        this.model.totalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getListGroupJob() {
    this.comboboxService.getCBBListGroupJob().subscribe(
      data => {
        this.listGroupJob = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getListDegree() {
    this.comboboxService.getListDegree().subscribe(
      data => {
        this.listDegree = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  clear(JobGroupId:string) {
    this.model = {
      PageSize: 10,
      totalItems: 0,
      TotalItemExten: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      Id: '',
      Name: '',
      Code: '',
      Description: '',
      DegreeId: '',
      JobGroupId: '',
    }
    this.searchJob(JobGroupId);
  }

  ExportExcel() {
    this.jobService.ExPort(this.model).subscribe(d => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerApi + d;
      link.download = 'Download.docx';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, e => {
      this.messageService.showError(e);
    });
  }

  ShowCreate() {
    this.GroupId = this.JobGroupId;
    if(this.GroupId == null){
      this.messageService.showMessage("Bạn phải chọn nhóm nghề trước!");
    }
    localStorage.setItem("selectedJobGroupId", this.selectedJobGroupId);
    this.router.navigate(['phong-hoc/quan-ly-nghe/them-moi/', this.GroupId]);
  }

  ShowUpdate(Id: string) {
    localStorage.setItem("selectedJobGroupId", this.selectedJobGroupId);
    this.router.navigate(['phong-hoc/quan-ly-nghe/chinh-sua/', Id]);
  }

  ShowCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(JobgroupCreateComponent, { container: 'body', windowClass: 'job-group-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchJobGroups();
        this.searchJob(Id);
      }
    }, (reason) => {
    });
  }

  showConfirmDeleteJobGroup(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhóm nghề này không?").then(
      data => {
        this.deleteJobGroup(Id);
      },
      error => {
        
      }
    );
  }

  deleteJobGroup(Id: string) {
    this.jobGroupServiec.DeleteJobGroup({ Id: Id }).subscribe(
      data => {
        this.searchJobGroups();
        this.searchJob("");
        this.messageService.showSuccess('Xóa nhóm nghề thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nghề này không?").then(
      data => {
        this.delete(Id);
      },
      error => {
        
      }
    );
  }

  delete(Id: string) {
    this.jobService.deleteJob({ Id: Id }).subscribe(
      data => {
        this.searchJob("");
        this.messageService.showSuccess('Xóa nghề thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  typeId: number;
  /// Skien click chuột phải
  itemClick(e) {
    if (e.itemData.Id == 1) {
      this.ShowCreateUpdate('');
    } else if (e.itemData.Id == 2) {
      this.ShowCreateUpdate(this.JobGroupId);
    } else if (e.itemData.Id == 3) {
      this.showConfirmDeleteJobGroup(this.JobGroupId);
    }
  }
}
