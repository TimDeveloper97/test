import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { DataDistributionService } from '../services/data-distribution.service';
import { MessageService, ComboboxService, Constants, PermissionService } from 'src/app/shared';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DataDistributionFileCreateUpdateComponent } from '../data-distribution-file-create-update/data-distribution-file-create-update.component';

@Component({
  selector: 'app-data-distribution-file-manage-modal',
  templateUrl: './data-distribution-file-manage-modal.component.html',
  styleUrls: ['./data-distribution-file-manage-modal.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class DataDistributionFileManageModalComponent implements OnInit {

  constructor(
    private dataDistributionService: DataDistributionService,
    private messageService: MessageService,
    private modalService: NgbModal,
    private comboboxService: ComboboxService,
    public constant: Constants,
    private activeModal: NgbActiveModal,
    public permissionService: PermissionService
  ) { }

  listAllFile: any = [];
  isAction: boolean = false;
  dataDistributionFileSearchModel: any = {
    Name: '',
    Type: ''
  }

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên file ...',
    Items: [
      {
        Name: 'Loại thiết kế',
        FieldName: 'Type',
        Placeholder: 'Loại thiết kế',
        Type: 'select',
        Data: this.constant.DesignTypes,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  listIdSelected: any = [];
  listSelect: any = [];

  ngOnInit() {
    this.searchDataDistributionFile();
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  searchDataDistributionFile() {
    this.dataDistributionService.searchDataDistributionFile(this.dataDistributionFileSearchModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.listAllFile = data.ListResult;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  clear() {
    this.dataDistributionFileSearchModel = {
      Name: '',
      Type: ''
    }
    this.searchDataDistributionFile();
  }

  showCreateUpdateFile(id: string) {
    let activeModal = this.modalService.open(DataDistributionFileCreateUpdateComponent, { container: 'body', windowClass: 'datadistributionfilecreateupdatecomponent-model', backdrop: 'static' });
    activeModal.componentInstance.idUpdate = id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchDataDistributionFile();
      }
    }, (reason) => {

    });
  }

  deleteDataDistributionFile(Id: string) {
    this.dataDistributionService.deleteDataDistributionFile({ Id: Id }).subscribe(
      data => {
        this.searchDataDistributionFile();
        this.messageService.showSuccess('Xóa file thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
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

}
