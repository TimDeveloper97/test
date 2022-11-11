import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ComboboxService, MessageService } from 'src/app/shared';
import { SolutionAnalysisProductService } from '../../service/solution-analysis-product.service';

@Component({
  selector: 'app-product-create',
  templateUrl: './product-create.component.html',
  styleUrls: ['./product-create.component.scss']
})
export class ProductCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private combobox: ComboboxService,
    private solutionAnalysisProductService : SolutionAnalysisProductService
  ) { }

  modalInfo = {
    Title: 'Thêm mới sản phẩm',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;

  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Link: '',
    ManufactureName: '',
    customerRequirementId: '',
  }
  ListProduct : any =[];
  CustomerRequirementId: string;

  ngOnInit() {
    this.model.customerRequirementId = this.CustomerRequirementId;
      this.modalInfo.Title = 'Thêm mới sản phẩm';
      this.modalInfo.SaveText = 'Lưu';
      
  }


  create(isContinue) {
    this.ListProduct.push(this.model);
        if (isContinue) {
          this.messageService.showSuccess('Thêm mới sản phẩm thành công!');
          this.model = {
            Id: '',
            Name: '',
            Code: '',
            Link: '',
            ManufactureName: '',
            customerRequirementId: this.CustomerRequirementId,
          }
        }
        else {
          this.messageService.showSuccess('Thêm mới sản phẩm thành công!');
          this.closeModal(true);
        }
  }


  save(isContinue: boolean) {
    this.create(isContinue);
  }

  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }
}
