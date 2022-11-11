import { Component, ElementRef, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Configuration, Constants, MessageService } from 'src/app/shared';
import { SaleGroupService } from '../service/sale-group.service';
import { ShowChooseEmployeeComponent } from '../show-choose-employee/show-choose-employee.component';
import { ShowChooseProductComponent } from '../show-choose-product/show-choose-product.component';

@Component({
  selector: 'app-sale-group-create',
  templateUrl: './sale-group-create.component.html',
  styleUrls: ['./sale-group-create.component.scss'],
  encapsulation: ViewEncapsulation.None

})
export class SaleGroupCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public constant: Constants,
    private modalService: NgbModal,
    public saleGroupService: SaleGroupService,
    public config: Configuration
  ) { }

  id: string;
  modalInfo = {
    Title: 'Thêm mới nhóm kinh doanh',
  };

  isAction: boolean = false;

  model: any = {
    Name: '',
    Code: '',
    ListEmployee: [],
    ListGroupProduct: [],
  }
  listEmployee = [];
  listProduct = [];

  ngOnInit() {
    if (this.id) {
      this.modalInfo.Title = 'Chỉnh sửa nhóm kinh doanh';
      this.getInfoById();
    }
    else {
      this.modalInfo.Title = "Thêm mới nhóm kinh doanh";
    }
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  showSelectEmployee() {
    let activeModal = this.modalService.open(ShowChooseEmployeeComponent, { container: 'body', windowClass: 'select-employee-model', backdrop: 'static' });
    var listEmployeeId = [];
    this.listEmployee.forEach(element => {
      listEmployeeId.push(element.Id);
    });

    activeModal.componentInstance.listEmployeeId = listEmployeeId;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listEmployee.push(element);
        });
        var j = 1;
        this.listEmployee.forEach(element => {
          element.index = j ++;
        });
      }
    }, (reason) => {

    });
  }

  showComfrimDeleteEmployee(row: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhân viên này không?").then(
      data => {
        this.deleteEmployee(row);
      },
      error => {
        
      }
    );
  }

  deleteEmployee(row: any) {
    var index = this.listEmployee.indexOf(row);
    if (index > -1) {
      this.listEmployee.splice(index, 1);
    }
  }

  showSelectProduct() {
    let activeModal = this.modalService.open(ShowChooseProductComponent, { container: 'body', windowClass: 'select-product-model', backdrop: 'static' });
    var listProductId = [];
    this.listProduct.forEach(element => {
      listProductId.push(element.Id);
    });

    activeModal.componentInstance.listProductId = listProductId;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listProduct.push(element);
        });
        var i = 1;
        this.listProduct.forEach(element => {
          element.index = i ++;
        });
      }
    }, (reason) => {

    });
  }

  showComfrimDeleteProduct(row: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá sản phẩm này không?").then(
      data => {
        this.deleteProduct(row);
      },
      error => {
        
      }
    );
  }

  deleteProduct(row: any) {
    var index = this.listProduct.indexOf(row);
    if (index > -1) {
      this.listProduct.splice(index, 1);
    }
  }

  getInfoById() {
    this.saleGroupService.getInfoSaleGroup(this.id).subscribe(data => {
      this.model = data;
      this.listProduct = this.model.ListGroupProduct;
      var i = 1, j = 1;
      this.listProduct.forEach(element => {
        element.index = i ++;
      });
      this.listEmployee = this.model.ListEmployee;
      this.listEmployee.forEach(element => {
        element.index = j ++;
      });
    });
  }

  update() {
    if (this.model.Name == '' || this.model.Name == null) {
      this.messageService.showError("Tên nhóm kinh doanh không được bỏ trống!");
    }

    this.model.ListEmployee = this.listEmployee;
    this.model.ListGroupProduct = this.listProduct;
    this.saleGroupService.updateSaleGroup(this.id, this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật nhóm kinh doanh thành công!');
      },
      error => {
        this.messageService.showError(error);
        this.model.Name = '';
      }
    );
  }

  create(isContinue) {
    if (this.model.Name == '' || this.model.Name == null) {
      this.messageService.showError("Tên nhóm kinh doanh không được bỏ trống!");
    }

    this.model.ListEmployee = this.listEmployee;
    this.model.ListGroupProduct = this.listProduct;

    this.saleGroupService.createSaleGroup(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {
            Name: '',
            Code: '',
            ListEmployee: [],
            ListGroupProduct: [],
          };
          this.listProduct = [];
          this.listEmployee = [];
          this.messageService.showSuccess('Thêm mới nhóm kinh doanh thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới nhóm kinh doanh thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
        this.model.Name = '';
      });
  }

  save(isContinue: boolean) {
    if (this.id) {
      this.update();
    }
    else {
      this.create(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

}
