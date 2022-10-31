import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';

import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DxTreeListComponent } from 'devextreme-angular';
import { SupplierContractCreateComponent } from '../supplier-contract-create/supplier-contract-create.component';
import { MessageService, Constants, ComboboxService, FileProcess } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { SupplierService } from '../../services/supplier-service';
import { ChooseManufactureComponent } from '../choose-manufacture/choose-manufacture.component';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-supplier-create',
  templateUrl: './supplier-create.component.html',
  styleUrls: ['./supplier-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SupplierCreateComponent implements OnInit {

  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    private activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private messageService: MessageService,
    private supplierService: SupplierService,
    public constants: Constants,
    private combobox: ComboboxService,
    public fileProcess: FileProcess,
    private fileService: UploadfileService,
  ) { }

  modalInfo = {
    Title: 'Thêm mới nhà cung cấp',
    SaveText: 'Lưu',

  };

  Id: string;
  isAction: boolean = false;
  listManufacture: any[] = [];
  listSupplierGroup: any[] = [];
  treeBoxValue: string[];
  isDropDownBoxOpened = false;
  selectedRowKeys: any[] = [];
  supplierGroupName: '';
  listCountry: [];
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
    Email: '',
    PhoneNumber: '',
    Address: '',
    Country: null,
    Website: '',
    BankPayment: null,
    TypePayment: null,
    RulesPayment: '',
    RulesDelivery: null,
    DeliveryTime: '',
    Note: '',
    Status: '0',
    ListSupplierContact: [],
    ListSupplierGroupId: [],
    ListManfacture: [],
    lis: [],
    Contracts: [],
    
  }

  manufacturerModel: any = {
    Id: '',
    SupplierId: '',
    Code: '',
    Name: ''
  }

  supplierContact: any = {
    Id: '',
    SupplierId: '',
    Name: '',
    Location: '',
    PhoneNumber: '',
    Email: ''
  }
  supplierGroupId: '';
  supplierGroupCode: '';
  suppliserId = '';
  ngOnInit() {
    let userLocal = localStorage.getItem('qltkcurrentUser');
    if (userLocal) {
      this.user = JSON.parse(userLocal);
    }
    this.getCbbData();
    this.getListCountry();
    this.getListSupplierGroup();
    if (this.Id) {
      this.modalInfo.Title = 'Chỉnh sửa nhà cung cấp';
      this.modalInfo.SaveText = 'Lưu';
      this.getSupplierInfo();
    }
    else {
      this.modalInfo.Title = "Thêm mới nhà cung cấp";
      this.getSupplierCode();
    }
  }

  getListSupplierGroup() {
    this.combobox.getCbbMaterialGroupParent().subscribe((data: any) => {
      if (data) {
        this.listSupplierGroup = data;
      }
      if (this.supplierGroupId != null) {
        this.model.ListSupplierGroupId.push(this.supplierGroupId);
        this.treeBoxValue = this.model.ListSupplierGroupId;
        this.selectedRowKeys = this.treeBoxValue;
      }
      else if (this.supplierGroupCode) {
        this.listSupplierGroup.forEach(item => {
          if (item.Code == this.supplierGroupCode) {
            this.model.ListSupplierGroupId.push(item.Id);
          }
        });

        this.treeBoxValue = this.model.ListSupplierGroupId;
        this.selectedRowKeys = this.treeBoxValue;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchSupplierManufacture() {
    this.listManufacture = this.model.ListManfacture;
    if (this.model.ListManfacture.length > 0) {
      if (this.manufacturerModel.Code) {
        this.model.ListManfacture = this.model.ListManfacture.filter(o => o.Code.includes(this.manufacturerModel.Code));
      }
    }
  }

  getListCountry() {
    this.combobox.getListCountry().subscribe((data: any) => {
      if (data) {
        this.listCountry = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.manufacturerModel = {
      Id: '',
      SupplierId: '',
      Code: '',
      Name: ''
    }
    this.model.SupplierId = this.Id;
    this.model.ListManfacture = this.listManufacture;
  }

  createSupplier(isContinue) {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.model.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
      this.supplierService.createSupplier(this.model).subscribe(
        data => {
          this.suppliserId = data;
          if (isContinue) {
            this.isAction = true;
            this.model = {};
            this.model.ListSupplierContact = [];
            this.supplierContactName = '';
            this.supplierContactPhoneNumber = '';
            this.supplierContactEmail = '';

            this.supplierGroupName = '';
            this.documentFilesUpload = [];
            this.model.Contracts = [];
            this.messageService.showSuccess('Thêm mới nhà cung cấp thành công!');
          }
          else {
            this.messageService.showSuccess('Thêm mới nhà cung cấp thành công!');
            this.closeModal(true);
          }
        },

        error => {
          this.messageService.showError(error);
        }
      );
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.model.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
          this.supplierService.createSupplier(this.model).subscribe(
            data => {
              if (isContinue) {
                this.suppliserId = data;
                this.isAction = true;
                this.model = {};
                this.model.ListSupplierContact = [];
                this.supplierContactName = '';
                this.supplierContactPhoneNumber = '';
                this.supplierContactEmail = '';

                this.supplierGroupName = '';
                this.messageService.showSuccess('Thêm mới nhà cung cấp thành công!');
              }
              else {
                this.messageService.showSuccess('Thêm mới nhà cung cấp thành công!');
                this.closeModal(true);
              }
            },
            error => {
              this.messageService.showError(error);
            }
          );
        },
        error => {

        }
      );
    }
  }


  updateSupplier() {
    var validEmail = true;
    var regex = this.constants.validEmailRegEx;
    for (var item of this.model.ListSupplierContact) {
      if (item.Email != '') {
        if (!regex.test(item.Email)) {
          validEmail = false;
        }
      }
    }

    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      if (validEmail) {
        this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
        this.supplierService.updateSupplier(this.model).subscribe(
          () => {
            this.activeModal.close(true);
            this.messageService.showSuccess('Cập nhật nhà cung cấp thành công!');
          },
          error => {
            this.messageService.showError(error);
          }
        );
      } else {
        this.messageService.showMessage("Email người liên hệ không đúng định dạng!");
      }
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          if (validEmail) {
            this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
            this.supplierService.updateSupplier(this.model).subscribe(
              () => {
                this.activeModal.close(true);
                this.messageService.showSuccess('Cập nhật nhà cung cấp thành công!');
              },
              error => {
                this.messageService.showError(error);
              }
            );
          } else {
            this.messageService.showMessage("Email người liên hệ không đúng định dạng!");
          }
        },
        error => {

        }
      );
    }

  }

  getSupplierInfo() {
    this.supplierService.getSupplierInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
      this.modalInfo.Title = "Chỉnh sửa nhà cung cấp - " + this.model.Code + " - " + this.model.Name;
      this.treeBoxValue = this.model.ListSupplierGroupId;
      this.selectedRowKeys = this.treeBoxValue;
    }, error => {
      this.messageService.showError(error);
    }
    );
  }

  getSupplierCode() {
    this.supplierService.getSupplierCode().subscribe(data => {
      this.model.Code = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  save(isContinue: boolean) {
    var contractCheck = this.model.Contracts.find(a => a.LaborContractId == null || a.LaborContractId == '');

    if (contractCheck != null) {
      this.messageService.showMessage("Vui lòng chọn đủ loại hợp đồng");
      return;
    }

    if (this.documentFilesUpload.length > 0) {
      let files = this.fileService.uploadListFile(this.documentFilesUpload, 'SupplierContracts/');
      forkJoin([files]).subscribe(results => {
        if (results[0].length > 0) {
          results[0].forEach(item => {
            var file = this.model.Contracts.find(a => a.FileName == item.FileName && a.FileSize == item.FileSize);
            file.Path = item.FileUrl;
          });
        }
        if (this.Id) {
          if (this.model.ListSupplierGroupId.length == 1) {
            this.model.ListSupplierGroupId.forEach(element => {
              if (element == "") {
                this.model.ListSupplierGroupId.splice(element, 1)
              }
            });
          }
          if (this.model.ListSupplierGroupId.length == 0) {
            this.messageService.showMessage("Nhóm vật tư không được để trống");
          }
          else {
            this.updateSupplier();
          }
        }
        else {
          if (this.model.ListSupplierGroupId.length == 1) {
            this.model.ListSupplierGroupId.forEach(element => {
              if (element == "") {
                this.model.ListSupplierGroupId.splice(element, 1);
              }
            });
          }
          if (this.model.ListSupplierGroupId.length == 0) {
            this.messageService.showMessage("Nhóm vật tư không được để trống");
          }
          else {
            this.createSupplier(isContinue);
          }
        }
      });
    } else {
      if (this.Id) {
        if (this.model.ListSupplierGroupId.length == 1) {
          this.model.ListSupplierGroupId.forEach(element => {
            if (element == "") {
              this.model.ListSupplierGroupId.splice(element, 1)
            }
          });
        }
        if (this.model.ListSupplierGroupId.length == 0) {
          this.messageService.showMessage("Nhóm vật tư không được để trống");
        }
        else {
          this.updateSupplier();
        }
      }
      else {
        if (this.model.ListSupplierGroupId.length == 1) {
          this.model.ListSupplierGroupId.forEach(element => {
            if (element == "") {
              this.model.ListSupplierGroupId.splice(element, 1);
            }
          });
        }
        if (this.model.ListSupplierGroupId.length == 0) {
          this.messageService.showMessage("Nhóm vật tư không được để trống");
        }
        else {
          this.createSupplier(isContinue);
        }
      }
    }
  }
  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    if (this.supplierGroupCode) {
      this.activeModal.close(this.suppliserId);
    } else {
      this.activeModal.close(isOK ? isOK : this.isAction);
    }
  }

  // Thêm người liên hệ
  Index = 1;
  supplierContactName = '';
  supplierLocation = '';
  supplierContactPhoneNumber = '';
  supplierContactEmail = '';
  addRow() {
    var validEmail = true;
    const validEmailRegEx = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (this.supplierContactEmail != '') {
      if (validEmailRegEx.test(this.supplierContactEmail)) {
        validEmail = true;
      } else {
        validEmail = false;
      }
    }
    if (this.supplierContactName != '') {
      if (validEmail) {
        var addSupplierContact = Object.assign({}, this.supplierContact);
        addSupplierContact.Name = this.supplierContactName;
        addSupplierContact.PhoneNumber = this.supplierContactPhoneNumber;
        addSupplierContact.Email = this.supplierContactEmail;
        addSupplierContact.Location = this.supplierLocation;
        this.model.ListSupplierContact.push(addSupplierContact);

        //refresh dòng trống
        this.supplierContactName = '';
        this.supplierLocation = '';
        this.supplierContactPhoneNumber = '';
        this.supplierContactEmail = '';
      } else {
        this.messageService.showMessage("Email không đúng định dạng!");
      }
    }
    else {
      this.messageService.showMessage("Bạn không được để trống tên người liên hệ!");
    }

  }

  deleteRow(id, index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xoá người liên hệ này không?").then(
      data => {
        this.model.ListSupplierContact.splice(index, 1);
      },
      error => {

      }
    );
  }

  syncTreeViewSelection(e) {
    var component = (e && e.component) || (this.treeView && this.treeView.instance);
  }

  onRowDblClick() {
    this.isDropDownBoxOpened = false;

  }

  treeView_itemSelectionChanged(e) {
    this.treeBoxValue = e.selectedRowKeys;
    this.model.ListSupplierGroupId = e.selectedRowKeys;

  }

  closeDropDownBox() {
    this.isDropDownBoxOpened = false;
  }

  showConfirmDelete(model: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xóa hãng sản xuất này không?").then(
      data => {
        this.deleteManufacture(model);
      },
      error => {

      }

    );
  }

  deleteManufacture(model: any) {
    var index = this.model.ListManfacture.indexOf(model);
    if (index > -1) {
      this.model.ListManfacture.splice(index, 1);
    }
  }

  showChooseManufacture() {
    let activeModal = this.modalService.open(ChooseManufactureComponent, { container: 'body', windowClass: 'choose-manufacture-model', backdrop: 'static' });
    var ListIdSelect = [];
    this.model.ListManfacture.forEach(element => {
      ListIdSelect.push(element.Id);
    });
    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.model.ListManfacture.push(element);
        });
      }
    }, (reason) => {
    });
  }

  fileModel: any = {
    FileName: '',
    Path: '',
    FileSize: null,
    CreateBy: null,
    CreateDate: null
  };
  documentFilesUpload: any[] = [];
  user: any;
  laborContracts: any[] = [];

  getCbbData() {
    let cbbLaborContract = this.combobox.getLabroContractSupplier();
    forkJoin([cbbLaborContract]).subscribe(results => {
      this.laborContracts = results[0];
    });
  }

  uploadFileClick($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var filesExist = "";

    for (var item of fileDataSheet) {
      var file = Object.assign({}, this.fileModel);
      file.FileName = item.name;
      file.FileSize = item.size;
      file.CreateByName = this.user.userfullname;
      file.CreateDate = new Date();

      var fileExist = this.model.Contracts.find(a => a.FileName == item.name && a.FileSize == item.size);
      if (fileExist != null) {
        filesExist = fileExist.FileName + ", " + filesExist;
      } else {
        this.documentFilesUpload.push(item);
        this.model.Contracts.push(file);
      }
    }
    if (filesExist != "") {
      this.messageService.showMessage("File: " + filesExist + " đã tồn tại");
    }

  }


  downloadAFile(row: any) {
    this.fileProcess.downloadFileBlob(row.Path, row.FileName);
  }

  showDelete(Id: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá hợp đồng này không?").then(
      data => {
        this.supplierService.deleteSupplierContract({ Id: Id }).subscribe(data => {
          this.getSupplierInfo();
          this.messageService.showSuccess('Xóa hợp đồng thành công!');
        }, error => {
          this.messageService.showError(error);
        }
        );
      },
      error => {

      }
    );
  }

  ShowCreateUpdate(Id: any){
  let activeModal = this.modalService.open(SupplierContractCreateComponent, { container: 'body', windowClass: 'supplier-contract-create-model', backdrop: 'static' })
    activeModal.componentInstance.supplierId = this.model.Code;
    activeModal.componentInstance.SupplierContractId = Id;
    if (this.supplierGroupId != null) {
      activeModal.componentInstance.supplierGroupId = this.supplierGroupId;
    }
    activeModal.result.then((result) => {
      if (result) {
        this.getSupplierInfo();
        this.getCbbData();
      }
    }, (reason) => {
    });
  }

}
