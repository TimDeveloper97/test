import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { DataDistributionService } from '../services/data-distribution.service';
import { MessageService, ComboboxService, Constants } from 'src/app/shared';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DataDistributionFileCreateUpdateComponent } from '../data-distribution-file-create-update/data-distribution-file-create-update.component';

@Component({
  selector: 'app-data-distribution-file-choose',
  templateUrl: './data-distribution-file-choose.component.html',
  styleUrls: ['./data-distribution-file-choose.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class DataDistributionFileChooseComponent implements OnInit {

  constructor(
    private dataDistributionService: DataDistributionService,
    private messageService: MessageService,
    private modalService: NgbModal,
    private comboboxService: ComboboxService,
    public constant: Constants,
    private activeModal: NgbActiveModal,
  ) { }

  listAllFile: any = [];
  isAction: boolean = false;
  dataDistributionFileSearchModel: any = {
    ListSelectId: [],
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
    this.dataDistributionFileSearchModel.ListSelectId = this.listIdSelected;
    this.dataDistributionService.searchDataDistributionFile(this.dataDistributionFileSearchModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.listAllFile = data.ListResult;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  addRow() {
    this.listAllFile.forEach(element => {
      if (element.Checked) {
        this.listSelect.push(element);
      }
    });
    this.listSelect.forEach(element => {
      var index = this.listAllFile.indexOf(element);
      if (index > -1) {
        this.listAllFile.splice(index, 1);
      }
    });
  }

  removeRow() {
    this.listSelect.forEach(element => {
      if (element.Checked) {
        this.listAllFile.push(element);
      }
    });
    this.listAllFile.forEach(element => {
      var index = this.listSelect.indexOf(element);
      if (index > -1) {
        this.listSelect.splice(index, 1);
      }
    });
  }

  choose() {
    this.activeModal.close(this.listSelect);
  }

  clear() {
    this.dataDistributionFileSearchModel = {
      ListSelectId: [],
      Name: ''
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
