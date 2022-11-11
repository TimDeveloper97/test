import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { SupplierService } from '../../services/supplier-service';
import { MessageService, Constants, DateUtils, ComboboxService, FileProcess } from 'src/app/shared';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-supplier-contract-create',
  templateUrl: './supplier-contract-create.component.html',
  styleUrls: ['./supplier-contract-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SupplierContractCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private combobox: ComboboxService,
    public fileProcess: FileProcess,
    private messageService: MessageService,
    private supplierService: SupplierService,
    private fileService: UploadfileService,
    public dateUtils: DateUtils,
  ) { }

  modalInfo = {
    Title: 'Thêm mới hợp đồng',
    SaveText: 'Lưu',

  };

  supplierId = '';
  model: any = {
    Name: '',
    Code: '',
    LaborContractId: '',
    LaborContractName: '',
    SupplierId: '',
    Path: '',
    FileName: '',
    FileSize: '',
    SignDate: null,
    DueDate: null,
    CreateByName: '',
    CreateDate: '',
  };
  signDate: any = null;
  dueDate: any = null;

  fileModel: any = {
    FileName: '',
    Path: '',
    FileSize: null,
    CreateBy: null,
    CreateDate: null
  };
  user: any;
  documentFilesUpload :any;
  laborContracts: any[] = [];
  SupplierContractId: '';


  ngOnInit(): void {
    let userLocal = localStorage.getItem('qltkcurrentUser');
    if (userLocal) {
      this.user = JSON.parse(userLocal);
    }
    this.getCbbData();
    if (this.SupplierContractId) {
      this.modalInfo.Title = 'Chỉnh sửa hợp đồng';
      this.modalInfo.SaveText = 'Lưu';
      this.getSupplierContractInfo();
    }
    else {
      this.modalInfo.Title = "Thêm mới hợp đồng";
     // this.getSupplierCode();
    }
  }

  getSupplierContractInfo() {
    this.supplierService.getSupplierContractInfo({ Id: this.SupplierContractId }).subscribe(data => {
      this.model = data;
      if (data.SignDate != null && data.SignDate != '') {
        this.model.SignDate = this.dateUtils.convertDateToObject(data.SignDate)
      }
      if (data.DueDate != null && data.DueDate != '') {
        this.model.DueDate = this.dateUtils.convertDateToObject(data.DueDate)
      }
      this.modalInfo.Title = "Chỉnh sửa hợp đồng - " + this.model.Name;
    }, error => {
      this.messageService.showError(error);
    }
    );
  }

  getCbbData() {
    let cbbLaborContract = this.combobox.getLabroContractSupplier();
    forkJoin([cbbLaborContract]).subscribe(results => {
      this.laborContracts = results[0];
    });
  }
  fileToUpload: File = null;

  uploadFileClick($event) {
     this.fileProcess.onAFileChange($event);
      this.fileService.uploadFile(this.fileProcess.FileDataBase, 'SupplierContracts/').subscribe(
        data => {
          if (data != null) {
            this.model.Path = data.FileUrl;
            this.model.FileName = data.FileName;
            this.model.FileSize = data.FileSize;
          }
        }
      );
  }

  showConfirmDeleteFile() {
    this.messageService.showConfirm("Bạn có chắc muốn xoá file này không?").then(
      data => {
        this.model.Path = '';
        this.model.FileName = '';
        this.model.FileSize = '';
        this.messageService.showSuccess('Xóa file thành công!');
      },
      error => {

      }
    );
  }

  save() {
    if (this.model.SignDate != null && this.model.SignDate != '') {
      this.model.SignDate = this.dateUtils.convertObjectToDate(this.model.SignDate)
    }
    if (this.model.DueDate != null && this.model.DueDate != '') {
      this.model.DueDate = this.dateUtils.convertObjectToDate(this.model.DueDate)
    }
    this.model.SupplierId = this.supplierId;
    if(this.model.Id)
    {
      this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
      this.supplierService.updateSupplierContract(this.model).subscribe(
        () => {
          this.clear();
          this.messageService.showSuccess('Cập nhật nhà cung cấp thành công!');
          this.activeModal.close(true);
        },
        error => {
          this.messageService.showError(error);
        }
      )
    }
    else{
      this.supplierService.CreateSupplierContract(this.model).subscribe(
        data => {
              this.clear();
              this.messageService.showSuccess('Thêm mới nhân viên thành công!');
              this.activeModal.close(true);
        },
        error => {
          this.messageService.showError(error);
        }
      );
    }
    
  }

  clear(){
    this.model = {
      Name: '',
      Code: '',
      LaborContractId: '',
      LaborContractName: '',
      SupplierId: '',
      Path: '',
      FileName: '',
      FileSize: '',
      SignDate: null,
      DueDate: null,
      CreateByName: '',
      CreateDate: '',
    }
    this.fileModel = {
      FileName: '',
      Path: '',
      FileSize: null,
      CreateBy: null,
      CreateDate: null
    };
  }

  closeModal() {
      this.activeModal.close();
  }
}
