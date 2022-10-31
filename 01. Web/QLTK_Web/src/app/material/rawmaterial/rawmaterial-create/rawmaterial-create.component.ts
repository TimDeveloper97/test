import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, ComboboxService } from 'src/app/shared';
import { RawMaterialService } from '../../services/rawmaterial-service';

@Component({
  selector: 'app-rawmaterial-create',
  templateUrl: './rawmaterial-create.component.html',
  styleUrls: ['./rawmaterial-create.component.scss']
})
export class RawmaterialCreateComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private comboboxService: ComboboxService,
    private rawMaterialService: RawMaterialService) { }

  ModalInfo = {
    Title: 'Thêm mới đơn vị',
    SaveText: 'Lưu',

  };

  isAction: boolean = false;
  Id: string;
  listRawMaterial: any[] = []
  model: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Index',
    OrderType: true,
    Id: '',
    Name: '',
    Code: '',
    Price: 0,
    Index: '',
    Note: '',
    MaterialId: '',
  }
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
  listMaterial: any[] = [];
  ngOnInit() {
    this.getCbbRawMaterial();
    this.getMaterials();

    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa vật liệu';
      this.ModalInfo.SaveText = 'Lưu';
      this.getRawMaterialInfo();
    }
    else {
      this.ModalInfo.Title = "Thêm vật liệu";
    }
  }

  ///combobox Material
  getMaterials() {
    this.comboboxService.getListMaterial().subscribe(data => {
      this.listMaterial = data;
    });
  }

  getRawMaterialInfo() {
    this.rawMaterialService.getRawMaterialInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
    });
  }

  getCbbRawMaterial() {
    this.rawMaterialService.getCbbRawMaterial().subscribe((data: any) => {
      this.listRawMaterial = data;
      if (this.Id == null || this.Id == '') {
        this.model.Index = data[data.length - 1].Exten;
      } else {
        this.listRawMaterial.splice(this.listRawMaterial.length - 1, 1);
      }

    });
  }

  createRawMaterial(isContinue) {
    //kiểm tra ký tự đặc việt trong Mã
    // var index1 = this.model.Code.indexOf("*");
    // var index2 = this.model.Code.indexOf("{");
    // var index3 = this.model.Code.indexOf("}");
    // var index4 = this.model.Code.indexOf("!");
    // var index5 = this.model.Code.indexOf("^");
    // var index6 = this.model.Code.indexOf("<");
    // var index7 = this.model.Code.indexOf(">");
    // var index8 = this.model.Code.indexOf("?");
    // var index9 = this.model.Code.indexOf("|");
    // var index10 = this.model.Code.indexOf(",");
    // var index11 = this.model.Code.indexOf("_");
    // var index12 = this.model.Code.indexOf(" ");

    var validCode = true;
    // if (index1 != -1 || index2 != -1 || index3 != -1 || index4 != -1 || index5 != -1 || index6 != -1 || index7 != -1 || index8 != -1 || index9 != -1
    //   || index10 != -1 || index11 != -1 || index12 != -1) {
    //   validCode = false;
    // }

    if (validCode) {
      this.model.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
      this.rawMaterialService.createRawMaterial(this.model).subscribe(
        data => {
          if (isContinue) {
            this.isAction = true;
            this.model = {};
            this.messageService.showSuccess('Thêm mới vật liệu thành công!');
          }
          else {
            this.messageService.showSuccess('Thêm mới vật liệu thành công!');
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
          this.rawMaterialService.createRawMaterial(this.model).subscribe(
            data => {
              if (isContinue) {
                this.isAction = true;
                this.model = {};
                this.messageService.showSuccess('Thêm mới vật liệu thành công!');
              }
              else {
                this.messageService.showSuccess('Thêm mới vật liệu thành công!');
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



  updateRawMaterial() {
    //kiểm tra ký tự đặc việt trong Mã
    // var index1 = this.model.Code.indexOf("*");
    // var index2 = this.model.Code.indexOf("{");
    // var index3 = this.model.Code.indexOf("}");
    // var index4 = this.model.Code.indexOf("!");
    // var index5 = this.model.Code.indexOf("^");
    // var index6 = this.model.Code.indexOf("<");
    // var index7 = this.model.Code.indexOf(">");
    // var index8 = this.model.Code.indexOf("?");
    // var index9 = this.model.Code.indexOf("|");
    // var index10 = this.model.Code.indexOf(",");
    // var index11 = this.model.Code.indexOf("_");
    // var index12 = this.model.Code.indexOf(" ");
    var validCode = true;
    // if (index1 != -1 || index2 != -1 || index3 != -1 || index4 != -1 || index5 != -1 || index6 != -1 || index7 != -1 || index8 != -1 || index9 != -1
    //   || index10 != -1 || index11 != -1 || index12 != -1) {
    //   validCode = false;
    // }


    if (validCode) {
      this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
      this.rawMaterialService.updateRawMaterial(this.model).subscribe(
        () => {
          this.activeModal.close(true);
          this.messageService.showSuccess('Cập nhật vật liệu thành công!');
        },
        error => {
          this.messageService.showError(error);
        }
      );
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
          this.rawMaterialService.updateRawMaterial(this.model).subscribe(
            () => {
              this.activeModal.close(true);
              this.messageService.showSuccess('Cập nhật vật liệu thành công!');
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

  save(isContinue: boolean) {
    if (isNaN(this.model.Price)) {
      this.messageService.showMessage('Giá vật liệu là số');
      return;
    }
    if (this.Id) {
      this.updateRawMaterial();
    }
    else {
      this.createRawMaterial(isContinue);
    }
  }
  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
