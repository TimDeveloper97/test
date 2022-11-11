import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService } from 'src/app/shared';
import { MaterialGroupTPAService } from '../../services/materialgrouptpa-service';

@Component({
  selector: 'app-materialgrouptpa-create',
  templateUrl: './materialgrouptpa-create.component.html',
  styleUrls: ['./materialgrouptpa-create.component.scss']
})
export class MaterialgrouptpaCreateComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private materialGroupTPAService: MaterialGroupTPAService) { }

  ModalInfo = {
    Title: 'Thêm mới nhóm vật tư',
    SaveText: 'Lưu',

  };

  isAction: boolean = false;
  Id: string;
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
  }

  ngOnInit() {
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa nhóm vật tư';
      this.ModalInfo.SaveText = 'Lưu';
      this.getMaterialGroupTPAInfo();
    }
    else {
      this.ModalInfo.Title = "Thêm nhóm vật tư";
    }
  }

  getMaterialGroupTPAInfo() {
    this.materialGroupTPAService.getMaterialGroupTPAInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
    });
  }

  createMaterialGroupTPA(isContinue) {
    //kiểm tra ký tự đặc việt trong Mã
    var index1 = this.model.Code.indexOf("*");
    var index2 = this.model.Code.indexOf("{");
    var index3 = this.model.Code.indexOf("}");
    var index4 = this.model.Code.indexOf("!");
    var index5 = this.model.Code.indexOf("^");
    var index6 = this.model.Code.indexOf("<");
    var index7 = this.model.Code.indexOf(">");
    var index8 = this.model.Code.indexOf("?");
    var index9 = this.model.Code.indexOf("|");
    var index10 = this.model.Code.indexOf(",");
    var index11 = this.model.Code.indexOf("_");
    var index12 = this.model.Code.indexOf(" ");

    var validCode = true;
    if (index1 != -1 || index2 != -1 || index3 != -1 || index4 != -1 || index5 != -1 || index6 != -1 || index7 != -1 || index8 != -1 || index9 != -1
      || index10 != -1 || index11 != -1 || index12 != -1) {
      validCode = false;
    }

    if (validCode) {
      this.model.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
      this.materialGroupTPAService.createMaterialGroupTPA(this.model).subscribe(
        data => {
          if (isContinue) {
            this.isAction = true;
            this.model = {};
            this.messageService.showSuccess('Thêm mới nhóm vật tư TPA thành công!');
          }
          else {
            this.messageService.showSuccess('Thêm mới nhóm vật tư TPA thành công!');
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
          this.materialGroupTPAService.createMaterialGroupTPA(this.model).subscribe(
            data => {
              if (isContinue) {
                this.isAction = true;
                this.model = {};
                this.messageService.showSuccess('Thêm mới nhóm vật tư TPA thành công!');
              }
              else {
                this.messageService.showSuccess('Thêm mới nhóm vật tư TPA thành công!');
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

  updateMaterialGroupTPA() {
    //kiểm tra ký tự đặc việt trong Mã
    var index1 = this.model.Code.indexOf("*");
    var index2 = this.model.Code.indexOf("{");
    var index3 = this.model.Code.indexOf("}");
    var index4 = this.model.Code.indexOf("!");
    var index5 = this.model.Code.indexOf("^");
    var index6 = this.model.Code.indexOf("<");
    var index7 = this.model.Code.indexOf(">");
    var index8 = this.model.Code.indexOf("?");
    var index9 = this.model.Code.indexOf("|");
    var index10 = this.model.Code.indexOf(",");
    var index11 = this.model.Code.indexOf("_");
    var index12 = this.model.Code.indexOf(" ");

    var validCode = true;
    if (index1 != -1 || index2 != -1 || index3 != -1 || index4 != -1 || index5 != -1 || index6 != -1 || index7 != -1 || index8 != -1 || index9 != -1
      || index10 != -1 || index11 != -1 || index12 != -1) {
      validCode = false;
    }
    if (validCode) {
      this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
      this.materialGroupTPAService.updateMaterialGroupTPA(this.model).subscribe(
        () => {
          this.activeModal.close(true);
          this.messageService.showSuccess('Cập nhật nhóm vật tư TPA thành công!');
        },
        error => {
          this.messageService.showError(error);
        }
      );
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
          this.materialGroupTPAService.updateMaterialGroupTPA(this.model).subscribe(
            () => {
              this.activeModal.close(true);
              this.messageService.showSuccess('Cập nhật nhóm vật tư TPA thành công!');
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
    if (this.Id) {
      this.updateMaterialGroupTPA();
    }
    else {
      this.createMaterialGroupTPA(isContinue);
    }
  }
  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
