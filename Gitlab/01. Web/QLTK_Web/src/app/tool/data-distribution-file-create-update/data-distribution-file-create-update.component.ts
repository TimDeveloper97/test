import { Component, OnInit } from '@angular/core';
import { DataDistributionService } from '../services/data-distribution.service';
import { MessageService, PermissionService } from 'src/app/shared';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-data-distribution-file-create-update',
  templateUrl: './data-distribution-file-create-update.component.html',
  styleUrls: ['./data-distribution-file-create-update.component.scss']
})
export class DataDistributionFileCreateUpdateComponent implements OnInit {

  constructor(private dataDistributionService: DataDistributionService,
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
    public permissionService: PermissionService ) { }

  modalInfo = {
    Title: 'Thêm mới file',
    SaveText: 'Lưu',
  };

  dataDistributionFileModel: any = {
    GetType: '',
    Name: '',
    Type: '',
    FolderContain: '',
    Extension: '',
    FilterThongSo: '',
    FilterDonVi: '',
    TEM: false,
    MAT: false,
    FilterMaVatLieu: false,
    Description: '',
    IsFolderMaterial: false,
    FilterManuafacturer: ''
  }

  isAction: boolean = false;
  idUpdate: any;
  ngOnInit() {
    if (this.idUpdate) {
      this.modalInfo = {
        Title: 'Sửa file',
        SaveText: 'Lưu',
      };
      this.getDataDistributionFileInfo();
    }
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  createDataDistributionFile(isContinue) {
    this.dataDistributionService.createDataDistributionFile(this.dataDistributionFileModel).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.dataDistributionFileModel = {
            GetType: '',
            Name: '',
            Type: '',
            FolderContain: '',
            Extension: '',
            FilterThongSo: '',
            FilterDonVi: '',
            TEM: false,
            MAT: false,
            FilterMaVatLieu: false,
            Description: '',
            IsFolderMaterial: false,
            FilterManuafacturer: ''
          };
        }
        else {
          this.messageService.showSuccess('Thêm mới file thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );

  }

  updateDataDistributionFile() {
    this.dataDistributionService.updateDataDistributionFile(this.dataDistributionFileModel).subscribe(
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

  save(isContinue: boolean) {
    if (this.idUpdate) {
      this.updateDataDistributionFile();
    }
    else {
      this.createDataDistributionFile(isContinue);
    }
  }

  getDataDistributionFileInfo() {
    this.dataDistributionService.getDataDistributionFileInfo({ Id: this.idUpdate }).subscribe(data => {
      this.dataDistributionFileModel = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

}
