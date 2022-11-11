import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, FileProcess, Constants, AppSetting, ComboboxService } from 'src/app/shared';
import { SolutionTabProductService } from '../service/solution-tab-product.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';

@Component({
  selector: 'app-solution-product-create',
  templateUrl: './solution-product-create.component.html',
  styleUrls: ['./solution-product-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SolutionProductCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public constants: Constants,
    public appset: AppSetting,
    private solutionProductService: SolutionTabProductService,
    private comboboxService: ComboboxService,
    private checkSpecialCharacter: CheckSpecialCharacter

  ) { }

  ModalInfo = {
    Title: 'Thêm mới sản phẩm',
    SaveText: 'Lưu',
  };

  columnName: any[] = [{ Name: 'Code', Title: 'Mã ' }, { Name: 'Name', Title: 'Tên ' }];

  isAction: boolean = false;
  Id: string;
  SolutionId: string;
  listSolutionProduct: any[] = []

  model: any = {
    Id: null,
    Name: '',
    Code: '',
    Price: 0,
    Quantity: 0,
    SolutionId: '',
    ObjectId: null,
    Specification: '',
    ObjectType: 1
  }
  listObjects: any[] = [];
  objectId = null;

  ngOnInit() {
    this.model.SolutionId = this.SolutionId;
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa sản phẩm';
      this.ModalInfo.SaveText = 'Lưu';
      this.getSolutionProductInfo();
      this.getObjects();
    }
    else {
      this.ModalInfo.Title = "Thêm mới sản phẩm";
    }
  }

  getSolutionProductInfo() {
    this.solutionProductService.getSolutionProduct(this.Id).subscribe(data => {
      this.model = data;
      this.objectId = this.model.ObjectId;
      this.getObjects();
    }, error => {
      this.messageService.showError(error);
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  saveAndContinue() {
    this.save(true);
  }

  createSolutionProduct(isContinue) {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.addSolutionProduct(isContinue);
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.addSolutionProduct(isContinue);
        },
        error => {

        }
      );
    }
  }

  addSolutionProduct(isContinue) {
    this.solutionProductService.createSolutionProduct(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {
            Id: null,
            Name: '',
            Code: '',
            Price: 0,
            Quantity: 0,
            SolutionId: '',
            ObjectId: null,
            Specification: '',
            ObjectType: this.model.ObjectType
          };
          this.objectId = null;
          this.model.SolutionId = this.SolutionId;
          this.messageService.showSuccess('Thêm mới sản phẩm thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới sản phẩm thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.updateSolutionProduct();
    }
    else {
      this.createSolutionProduct(isContinue);
    }
  }

  saveSolutionProduct() {
    this.solutionProductService.updateSolutionProduct(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật sản phẩm thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateSolutionProduct() {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.saveSolutionProduct();
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
          this.saveSolutionProduct();
        },
        error => {

        }
      );
    }
  }

  changePriceProduct() {
    this.getObjects();
    if (this.model.ObjectId != this.objectId) {
      this.model.ObjectId = this.objectId;
      this.solutionProductService.getObjectInfo({ ObjectId: this.model.ObjectId, ObjectType: this.model.ObjectType }).subscribe(data => {
        if (data) {
          this.model.Price = data.Price;
          this.model.Code = data.Code;
          this.model.Name = data.Name;
        }
      });
    }
  }

  choose() {
    if (this.model.Id == null || this.model.Id == "") {
      this.activeModal.close(this.model);
    }
    else if (this.Id != null || this.Id != "") {
      this.saveSolutionProduct();
    }
  }

  changeObjectType() {
    this.objectId = null;
    this.model.ObjectId = null;
    this.getObjects();
  }

  getObjects() {
    if (this.model.ObjectType == 1) {
      this.comboboxService.getListModule().subscribe(data => {
        this.listObjects = data;
      });
    }
    else if (this.model.ObjectType == 2) {
      this.comboboxService.getListProduct().subscribe(
        data => {
          this.listObjects = data;
        }
      );
    }
  }
}
