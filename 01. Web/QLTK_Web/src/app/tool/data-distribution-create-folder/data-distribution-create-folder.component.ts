import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { DataDistributionService } from '../services/data-distribution.service';
import { MessageService, PermissionService } from 'src/app/shared';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-data-distribution-create-folder',
  templateUrl: './data-distribution-create-folder.component.html',
  styleUrls: ['./data-distribution-create-folder.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class DataDistributionCreateFolderComponent implements OnInit {

  constructor(private dataDistributionService: DataDistributionService,
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
    public permissionService: PermissionService, ) { }
  modalInfo = {
    Title: 'Thêm mới thư mục',
    SaveText: 'Lưu',
  };
  modelDataDistribution = {
    Id: '',
    Name: '',
    Description: '',
    Type: '',
    ParentId: '',
    DepartmentId: '',
    IsCreateUpdate: true,
    IsExportMaterial: false
  }
  isDropDownBoxOpened = false;
  dataDistributionParent: any = [];
  isAction: boolean = false;
  idUpdate: any;
  departmentId: any;
  columnName: any[] = [{ Name: 'Name', Title: 'Tên thư mục' }];
  isUpdate: boolean;
  ngOnInit() {
    this.modelDataDistribution.DepartmentId = this.departmentId;

    if (this.isUpdate) {
      this.modalInfo = {
        Title: 'Sửa thư mục',
        SaveText: 'Lưu',
      };
      this.modelDataDistribution.Id = this.idUpdate;
      this.getDataDistributionInfo();
    } else {
      this.modelDataDistribution.ParentId = this.idUpdate;
    }
    this.searchDataDistribution();
  }

  closeDropDownBox() {
    this.isDropDownBoxOpened = false;
  }

  searchDataDistribution() {
    this.dataDistributionService.searchDataDistribution(this.modelDataDistribution).subscribe((data: any) => {
      if (data.ListResult) {
        this.dataDistributionParent = data.ListResult;

      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  save(isContinue: boolean) {
    if (this.idUpdate && this.isUpdate) {
      this.updateDataDistribution();
    }
    else {
      this.createDataDistribution(isContinue);
    }
  }

  createDataDistribution(isContinue) {
    this.dataDistributionService.createDataDistribution(this.modelDataDistribution).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.modelDataDistribution = {
            Id: '',
            Name: '',
            Description: '',
            Type: '',
            ParentId: '',
            DepartmentId: '',
            IsCreateUpdate: true,
            IsExportMaterial: false
          };
          this.modelDataDistribution.ParentId = this.idUpdate;
          this.modelDataDistribution.DepartmentId = this.departmentId;
        }
        else {
          this.messageService.showSuccess('Thêm mới thư mục thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );

  }

  getDataDistributionInfo() {
    this.searchDataDistribution();
    this.dataDistributionService.getDataDistributionInfo({ Id: this.idUpdate }).subscribe(data => {
      this.modelDataDistribution = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  updateDataDistribution() {
    this.dataDistributionService.updateDataDistribution(this.modelDataDistribution).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật thư mục thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  saveAndContinue() {
    this.save(true);
  }
}
