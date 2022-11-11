import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { DataDistributionService } from '../services/data-distribution.service';
import { MessageService, ComboboxService, Constants, AppSetting, PermissionService } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DataDistributionCreateFolderComponent } from '../data-distribution-create-folder/data-distribution-create-folder.component';
import { DataDistributionFileChooseComponent } from '../data-distribution-file-choose/data-distribution-file-choose.component';
import { DataDistributionFileManageModalComponent } from '../data-distribution-file-manage-modal/data-distribution-file-manage-modal.component';

@Component({
  selector: 'app-data-distribution',
  templateUrl: './data-distribution.component.html',
  styleUrls: ['./data-distribution.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class DataDistributionComponent implements OnInit {

  constructor(
    private dataDistributionService: DataDistributionService,
    private messageService: MessageService,
    private modalService: NgbModal,
    private comboboxService: ComboboxService,
    public constant: Constants,
    public appSetting: AppSetting,
    public permissionService: PermissionService
  ) { }

  listDataDistribution: any = [];
  listDataDistributionId: any = [];
  modelDataDistribution: any = {
    Id: '',
    DepartmentId: '',
    Name: '',
    ParentId: '',
    Description: '',
    Type: '',
    Path: '',
    IsCreateUpdate: false,
    ListFile: []
  }
  columnNameSBU: any[] = [{ Name: 'Code', Title: 'Mã SBU' }, { Name: 'Name', Title: 'Tên SBU' }];
  columnNameDepartment: any[] = [{ Name: 'Code', Title: 'Mã Phòng ban' }, { Name: 'Name', Title: 'Tên Phòng ban' }];
  sbuId: any;
  listSBU: any = [];
  listDepartment: any = [];
  height = 0;
  listFile: any = [];
  dataDistributionIdSelected: any;

  ngOnInit() {
    this.appSetting.PageTitle = "Cấu trúc phân bổ dữ liệu";
    this.height = window.innerHeight - 200;
    this.getCBBSBU();
  }

  searchDataDistribution() {
    let dataDistribution = Object.assign({}, this.modelDataDistribution);
    if (dataDistribution.DepartmentId) {
      this.dataDistributionService.searchDataDistribution(this.modelDataDistribution).subscribe((data: any) => {
        if (data.ListResult) {
          this.listDataDistribution = data.ListResult;
        }
      },
        error => {
          this.messageService.showError(error);
        });
    }
  }

  showCreateUpdateFolder(id: string, isUpdate: boolean) {
    let activeModal = this.modalService.open(DataDistributionCreateFolderComponent, { container: 'body', windowClass: 'datadistributioncreatefolder-model', backdrop: 'static' })
    activeModal.componentInstance.isUpdate = isUpdate;
    activeModal.componentInstance.idUpdate = id;
    activeModal.componentInstance.departmentId = this.modelDataDistribution.DepartmentId;

    activeModal.result.then((result) => {
      if (result) {
        this.searchDataDistribution();
      }
    }, (reason) => {
    });
  }

  getCBBSBU() {
    this.comboboxService.getCbbSBU().subscribe((data: any) => {
      if (data) {
        this.listSBU = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getCBBDepartment() {
    this.comboboxService.getCbbDepartmentBySBU(this.sbuId).subscribe((data: any) => {
      if (data) {
        this.listDepartment = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDeleteDataDistribution(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá thư mục này không?").then(
      data => {
        this.deleteDataDistribution(Id);
      },
      error => {
        
      }
    );
  }

  deleteDataDistribution(Id: string) {
    this.dataDistributionService.deleteDataDistribution({ Id: Id }).subscribe(
      data => {
        this.searchDataDistribution();
        this.messageService.showSuccess('Xóa thư mục thành công!');
        this.listFile = [];
      },
      error => {
        this.messageService.showError(error);
      });
  }

  searchDataDistributionFile(dataDistributionId: any) {
    if (dataDistributionId) {
      this.dataDistributionService.getDataDistributionFile({ Id: dataDistributionId }).subscribe((data: any) => {
        if (data.ListResult) {
          this.listFile = data.ListResult;
        }
      },
        error => {
          this.messageService.showError(error);
        });
    }
  }

  onSelectionChanged(e) {
    // this.searchDataDistributionFile(e.selectedRowKeys[0]);
    // this.dataDistributionIdSelected = e.selectedRowKeys[0];

    if(e.selectedRowKeys[0] != null  && e.selectedRowKeys[0] != this.dataDistributionIdSelected)
    {
      this.dataDistributionIdSelected = e.selectedRowKeys[0];
      this.searchDataDistributionFile(e.selectedRowKeys[0]);
    }
  }

  showConfirmDeleteFile(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá file này không?").then(
      data => {
        this.deleteDataDistributionFile(Id);
      },
      error => {
        
      }
    );
  }

  deleteDataDistributionFile(Id: string) {
    this.dataDistributionService.deleteDataDistributionFileLink({ Id: Id }).subscribe(
      data => {
        this.searchDataDistributionFile(this.dataDistributionIdSelected);
        this.messageService.showSuccess('Xóa file thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showChooseFile() {
    let activeModal = this.modalService.open(DataDistributionFileChooseComponent, { container: 'body', windowClass: 'datadistributionfilechoosecomponent-model', backdrop: 'static' });
    var ListIdSelect = [];
    this.listFile.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.listIdSelected = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.modelDataDistribution.ListFile.push(element);
        });
        this.addFDataDistributionFileLink();
      }
    }, (reason) => {

    });
  }

  showFielManage() {
    let activeModal = this.modalService.open(DataDistributionFileManageModalComponent, { container: 'body', windowClass: 'data-distribution-file-manage-modal', backdrop: 'static' });
    activeModal.result.then((result) => {

    }, (reason) => {

    });
  }

  addFDataDistributionFileLink() {
    this.modelDataDistribution.Id = this.dataDistributionIdSelected;
    this.dataDistributionService.createDataDistributionFileLink(this.modelDataDistribution).subscribe(
      data => {
        this.messageService.showSuccess('Thêm mới file thành công!');
        this.searchDataDistributionFile(this.dataDistributionIdSelected);
        this.modelDataDistribution.ListFile = [];
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
}
