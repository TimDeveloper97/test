import { Component, OnInit, ViewEncapsulation, ViewChild, ElementRef } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, AppSetting, Constants, PermissionService } from 'src/app/shared';
import { Title } from '@angular/platform-browser';
import { ModuleGroupService } from '../../services/module-group-service';
import { ModuleGroupChooseProductStandardComponent } from '../module-group-choose-product-standard/module-group-choose-product-standard.component';
import { ModuleGroupChooseStageComponent } from '../module-group-choose-stage/module-group-choose-stage.component';
import { ModuleProjectTestCriteiaComponent } from '../../Module/module-project-test-criteia/module-project-test-criteia.component';
import { ModuleChooseTestCriteiaComponent } from '../../Module/module-choose-test-criteia/module-choose-test-criteia.component';

@Component({
  selector: 'app-module-group-create',
  templateUrl: './module-group-create.component.html',
  styleUrls: ['./module-group-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ModuleGroupCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private appSetting: AppSetting,
    private moduleGroupService: ModuleGroupService,
    private modalService: NgbModal,
    public constant: Constants,
    public permissionService: PermissionService

  ) { }

  ModalInfo = {
    Title: 'Thêm mới nhóm module',
    SaveText: 'Lưu',

  };
  isAction: boolean = false;
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
    ParentId: '',
    Description: '',
    Note: '',
    ListProductStandard: [],
    ListStage: [],
    ListTestCriteri: []
  }
  idUpdate: string;
  parentId: string;
  listGroupModule: any[] = [];
  listGroupModuleId: any[] = [];
  listData: any = [];
  listStage: any = [];
  listTestCriteri: any = [];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];

  tabActiveId = '';
  @ViewChild('scrollProductStandard', { static: false}) scrollProductStandard: ElementRef;
  @ViewChild('scrollProductStandardHeader', { static: false}) scrollProductStandardHeader: ElementRef;
  @ViewChild('scrollTestCriteria', { static: false}) scrollTestCriteria: ElementRef;
  @ViewChild('scrollTestCriteriaHeader', { static: false}) scrollTestCriteriaHeader: ElementRef;

  ngOnInit() {

    if (this.idUpdate) {
      this.getCbbModuleGroupForUpdate();
      this.ModalInfo.Title = 'Chỉnh sửa nhóm module';
      this.ModalInfo.SaveText = 'Lưu';
      this.getModuleGroupInfo();
    }
    else {
      this.getCbbModuleGroup();
      this.ModalInfo.Title = "Thêm mới nhóm module";
      this.model.ParentId = this.parentId; // gán PanrenId 
    }


  }

  ngOnDestroy() {
    this.scrollProductStandard.nativeElement.removeEventListener('ps-scroll-x', null);
    this.scrollTestCriteria.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  public beforeChange($event) {
    this.tabActiveId = $event.nextId;

    if (this.tabActiveId == 'tab-product-standard') {
      this.scrollProductStandard.nativeElement.removeEventListener('ps-scroll-x', null);
      this.scrollProductStandard.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
        this.scrollProductStandardHeader.nativeElement.scrollLeft = event.target.scrollLeft;
      }, true);
    }
    else if (this.tabActiveId == 'tab-critera') {
      this.scrollTestCriteria.nativeElement.removeEventListener('ps-scroll-x', null);
      this.scrollTestCriteria.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
        this.scrollTestCriteriaHeader.nativeElement.scrollLeft = event.target.scrollLeft;
      }, true);
    }
  }

  //lấy info update
  getModuleGroupInfo() {
    this.moduleGroupService.getModuleGroupInfo({ Id: this.idUpdate }).subscribe(data => {
      this.model = data;
      this.ModalInfo.Title = "Chỉnh sửa nhóm module - " + this.model.Code + " - " + this.model.Name;
      this.listData = data.ListProductStandard;
      this.listStage = data.ListStage;
      this.listTestCriteri = data.ListTestCriteri;
    }, error => {
      this.messageService.showError(error);
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  save(isContinue: boolean) {
    this.model.ListProductStandard = this.listData;
    this.model.ListStage = this.listStage;
    this.model.ListTestCriteri = this.listTestCriteri;
    if (this.idUpdate) {
      this.updateModuleGroup();
    }
    else {
      this.model.ListProductStandard = this.listData;
      this.createModuleGroup(isContinue);
    }
  }
  saveAndContinue() {
    this.save(true);
  }

  getCbbModuleGroup() {
    this.moduleGroupService.getCbbModuleGroup().subscribe((data: any) => {
      this.listGroupModule = data.ListResult;
      // lấy list id expandedRowKeys 
      for (var item of this.listGroupModule) {
        this.listGroupModuleId.push(item.Id);
        if (this.parentId == item.Id) {
          this.model.Code = item.Code;
        }
      }
    });
  }

  getCbbModuleGroupForUpdate() {
    this.moduleGroupService.getCbbModuleGroupForUpdate({ Id: this.idUpdate }).subscribe((data: any) => {
      this.listGroupModule = data;
      // lấy list id expandedRowKeys 
      for (var item of this.listGroupModule) {
        this.listGroupModuleId.push(item.Id);
      }
    });
  }

  //Thêm mới
  createModuleGroup(isContinue) {
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
      this.moduleGroupService.createModuleGroup(this.model).subscribe(
        data => {
          if (isContinue) {
            this.isAction = true;
            this.model = {};
            this.messageService.showSuccess('Thêm mới nhóm module thành công!');
          }
          else {
            this.messageService.showSuccess('Thêm mới nhóm module thành công!');
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
          this.moduleGroupService.createModuleGroup(this.model).subscribe(
            data => {
              if (isContinue) {
                this.isAction = true;
                this.model = {};
                this.messageService.showSuccess('Thêm mới nhóm module thành công!');
              }
              else {
                this.messageService.showSuccess('Thêm mới nhóm module thành công!');
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

  //cập nhật nhóm module
  updateModuleGroup() {
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
      this.moduleGroupService.updateMmoduleGroup(this.model).subscribe(
        () => {
          this.activeModal.close(true);
          this.messageService.showSuccess('Cập nhật nhóm module thành công!');
        },
        error => {
          this.messageService.showError(error);
        }
      );
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
          this.moduleGroupService.updateMmoduleGroup(this.model).subscribe(
            () => {
              this.activeModal.close(true);
              this.messageService.showSuccess('Cập nhật nhóm module thành công!');
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

  //Hiển thì popup chọn Tiêu chuẩn ProductStandard
  showSelectProductStandard() {
    let activeModal = this.modalService.open(ModuleGroupChooseProductStandardComponent, { container: 'body', windowClass: 'modulegroupchooseproductstandard-model', backdrop: 'static' });
    var ListIdSelect = [];
    this.listData.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listData.push(element);
        });
      }
    }, (reason) => {

    });
  }

  // chọn công đoạn

  showSelectStage() {
    let activeModal = this.modalService.open(ModuleGroupChooseStageComponent, { container: 'body', windowClass: 'app-module-group-choose-stage-model', backdrop: 'static' });
    var ListIdSelect = [];
    this.listStage.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          element.IsDelete = false;
          this.listStage.push(element);
        });
      }
    }, (reason) => {

    });
  }

  // chọn tiêu chí

  showTestCriteria() {
    let activeModal = this.modalService.open(ModuleChooseTestCriteiaComponent, { container: 'body', windowClass: 'module-choose-testcriteia-model', backdrop: 'static' });
    var ListIdSelect = [];
    this.listTestCriteri.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.listIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listTestCriteri.push(element);
        });
      }
    }, (reason) => {

    });
  }

  showConfirmDelete(index) {
    this.messageService.showConfirm("Bạn có chắc muốn xóa tiêu chuẩn này không?").then(
      data => {
        this.listData.splice(index, 1);
        this.messageService.showSuccess("Xóa tiêu chuẩn thành công!");
      },
      error => {
        
      }
    );
  }

  showConfirmDeleteTestCriteia(index) {
    this.messageService.showConfirm("Bạn có chắc muốn xóa tiêu chí này không?").then(
      data => {
        this.listTestCriteri.splice(index, 1);
        this.messageService.showSuccess("Xóa tiêu chí thành công!");
      },
      error => {
        
      }
    );
  }

  deleteCheckProductStandard(model: any) {
    this.moduleGroupService.deleteProductStandard(model).subscribe(
      data => {
      },
      error => {
        this.messageService.showError(error);
      });
  }

  deleteProductStandard(model: any) {
    var index = this.listData.indexOf(model);
    if (index > -1) {
      this.listData.splice(index, 1);
    }
  }

  showConfirmDeleteStage(index) {
    this.messageService.showConfirm("Bạn có chắc muốn xóa công đoạn này không?").then(
      data => {

        this.listStage.splice(index, 1);
        this.messageService.showSuccess("Xóa công đoạn thành công!");
      },
      error => {
        
      }
    );
  }



  //Combobox đa cấp
  treeBoxValue: string;
  isDropDownBoxOpened = false;
  selectedRowKeys: any[] = [];
  rowFocusIndex = -1;
  syncTreeViewSelection() {
    if (!this.treeBoxValue) {
      this.selectedRowKeys = [];
    } else {
      this.selectedRowKeys = [this.treeBoxValue];
    }
  }

  treeView_itemSelectionChanged(e) {
    this.treeBoxValue = e.selectedRowKeys[0];
    this.model.ParentId = e.selectedRowKeys[0];
    this.closeDropDownBox();
  }
  closeDropDownBox() {
    this.isDropDownBoxOpened = false;
  }

}
