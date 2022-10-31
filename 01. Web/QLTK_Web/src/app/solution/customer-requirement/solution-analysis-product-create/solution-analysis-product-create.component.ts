import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, FileProcess, Constants, AppSetting, ComboboxService } from 'src/app/shared';
import { SolutionAnalysisProductService } from '../../customer-requirement/service/solution-analysis-product.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';

@Component({
  selector: 'app-solution-analysis-product-create',
  templateUrl: './solution-analysis-product-create.component.html',
  styleUrls: ['./solution-analysis-product-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SolutionAnalysisProductCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public constants: Constants,
    public appset: AppSetting,
    private solutionAnalysisProductService: SolutionAnalysisProductService,
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
  CustomerRequirementId: string;
  listSolutionProduct: any[] = []

  model: any = {
    Id: null,
    ObjectId: null,
    Specification: '',
    Type: 1,
    Name : '',
    Code : '',
  }
  listObjects: any[] = [];
  objectId = null;

  ngOnInit() {
    this.model.customerRequirementId = this.CustomerRequirementId;
    this.getObjects();

      this.ModalInfo.Title = "Thêm mới sản phẩm";
  }

  getSolutionProductInfo() {
    this.solutionAnalysisProductService.getSolutionProduct(this.Id).subscribe(data => {
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


  addSolutionProduct(isContinue) {

        if (isContinue) {
          this.isAction = true;
          this.model = {
            Id: null,
            CustomerRequirementId: '',
            ObjectId: null,
            Type: this.model.Type,
          };
          this.objectId = null;
          this.model.customerRequirementId = this.CustomerRequirementId;
          this.messageService.showSuccess('Thêm mới sản phẩm thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới sản phẩm thành công!');
          this.closeModal(true);
        }
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.saveSolutionProduct();
    }
    else {
      this.addSolutionProduct(isContinue);
    }
  }

  saveSolutionProduct() {
    this.solutionAnalysisProductService.updateSolutionProduct(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật sản phẩm thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  

  changePriceProduct() {
    if (this.model.ObjectId != this.objectId) {
      this.model.ObjectId = this.objectId;
      for(let i =0; i< this.listObjects.length;i++){
        if(this.objectId == this.listObjects[i].Id){
          this.model.Code = this.listObjects[i].Code;
          this.model.Name = this.listObjects[i].Name;
        }
      }
      // this.solutionAnalysisProductService.getObjectInfo({ ObjectId: this.model.ObjectId, Type: this.model.Type }).subscribe(data => {
      //   if (data) {
      //     this.model.Price = data.Price;
      //     this.model.Code = data.Code;
      //     this.model.Name = data.Name;
      //   }
      // });
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
    if (this.model.Type == 1) {
      this.comboboxService.getListModule().subscribe(data => {
        this.listObjects = data;
      });
    }
    else if (this.model.Type == 2) {
      this.comboboxService.getListProduct().subscribe(
        data => {
          this.listObjects = data;
        }
      );
    }
  }
}
