import { Component, OnInit, Input, ViewChild, ElementRef,  AfterContentInit, AfterViewInit } from '@angular/core';
import { Constants, MessageService, ComponentService, Configuration } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ProductChooseModuleComponent } from '../product-choose-module/product-choose-module.component';
import { ChoosePraciteComponent } from 'src/app/practice/skills/choose-pracite/choose-pracite.component';
import { ProductService } from '../services/product.service';
import { Router } from '@angular/router';
import { PopupPracticeCreateComponent } from '../popup-practice-create/popup-practice-create.component';
import { SkillService } from 'src/app/practice/skills/service/skill.service';
import { HistoryVersionComponent } from 'src/app/shared/component/history-version/history-version.component';
import { HistoryVersionService } from 'src/app/shared/services/history-version.service';
import { ProductaccessorieschooseComponent } from '../productaccessorieschoose/productaccessorieschoose.component';
import { ProductAccessoriesService } from '../services/product-accessories.service';


@Component({
  selector: 'app-product-sketches',
  templateUrl: './product-sketches.component.html',
  styleUrls: ['./product-sketches.component.scss']
})
export class ProductSketchesComponent implements OnInit,  AfterContentInit, AfterViewInit {

  constructor(
    public constant: Constants,
    private modalService: NgbModal,
    private productService: ProductService,
    private messageService: MessageService,
    private router: Router,
    private service: SkillService,
    private componentService: ComponentService,
    private config: Configuration,
    private serviceHistory: HistoryVersionService,
    private serviceAccessories: ProductAccessoriesService,
  ) { }
  @Input() Id: string;
  fileTemplate = this.config.ServerApi + 'Template/ImportModule_Template.xls';
  listModule: any = [];
  listPractice: any = [];
  module: any = {
    Name: '',
    Specification: '',
    Note: '',
    ModuleName: '',
    Qty: 0,
    IsCheck: false,
    ModuleId: '',
    Index: '',
    Code: '',
    Price: 0,
    LeadTime: 0
  }

  model: any = {
    ListDatashet: []
  }

  productSketchesModel: any = {
    Id: '',
    ListPractice: [],
    ListModuleProduct: [],

  }

  totalAmount = 0;
  @ViewChild('scrollPracticeHeader',{static:false}) scrollPracticeHeader: ElementRef;
  @ViewChild('scrollPractice',{static:false}) scrollPractice: ElementRef;
  @ViewChild('scrollModuleHeader',{static:false}) scrollModuleHeader: ElementRef;
  @ViewChild('scrollModule',{static:false}) scrollModule: ElementRef;
  height = 500;
  ListHeader = [];
  ListGuides = [];
  ListPracticeFile = [];
  MaxLeadTimeModule = 0;
  TotalModuleNoPrice = 0;
  TotalModuleNoLeadTime = 0;
  TotalAllPricePractice = 0;
  ngOnDestroy() {
    this.scrollPractice.nativeElement.removeEventListener('ps-scroll-x', null);
    this.scrollModule.nativeElement.removeEventListener('ps-scroll-x', null);
    this.scrollAccessories.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  ngOnInit() {
    this.height = window.innerHeight - 290;
    
    this.productSketchesModel.Id = this.Id;
    this.modelProductAccessories.ProductId = this.Id;
    this.getInfoSketches();
    this.getProductaccessoriesInfo();
  }

  ngAfterContentInit() {
   
  }

  ngAfterViewInit(){
    this.scrollPractice.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollPracticeHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.scrollModule.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollModuleHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.scrollAccessories.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollAccessoriesHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  showImportModule() {
    this.componentService.showImportExcel(this.fileTemplate, false).subscribe(data => {
      if (data) {
        this.productService.importProductModule(data, this.Id).subscribe(
          data => {
            var listModuleNewId = [];
            data.forEach(element => {
              var group = this.listModule.find(t => t.ModuleId == element.Id);
              if (group == null) {
                var newModule = Object.assign({}, this.module);
                newModule.ModuleId = element.Id;
                newModule.ModuleName = element.Name;
                newModule.Specification = element.Specification;
                newModule.Note = element.Note;
                newModule.Qty = element.Quantity;
                newModule.Code = element.Code;
                newModule.Price = element.Pricing;
                newModule.LeadTime = element.LeadTime;
                this.listModule.push(newModule);
                for (var item of this.listPractice) {
                  item.ListModuleInPractice.push(Object.assign({}, newModule));
                }
                this.changeMaxLeadTimeModule();
                listModuleNewId.push(newModule.ModuleId);
              }
              else{
                group.Qty = group.Qty + element.Quantity;
                for (var item of this.listPractice) {
                  var ite = item.ListModuleInPractice.find(t => t.ModuleId == element.Id);
                  if(ite != null)
                  {
                    ite.Qty = group.Qty;
                  }
                }
              }
            });
            this.getModulePrice(listModuleNewId);
            if (this.listPractice.length > 0) {
              this.ListHeader = this.listPractice[0].ListModuleInPractice;
            }
            this.getMaxQuantityModuleByPractice();
            // this.changeMaxLeadTimeModule();
            this.calculateTotalAmount();
            this.messageService.showSuccess('Import danh sách module thành công!');
          },
          error => {
            this.messageService.showError(error);
          });
      }
    });
  }

  //Hiển thì popup chọn Module
  showSelectModule() {
    let activeModal = this.modalService.open(ProductChooseModuleComponent, { container: 'body', windowClass: 'productchoosemodulecomponent-model', backdrop: 'static' });
    var ListIdSelect = [];
    this.listModule.forEach(element => {
      ListIdSelect.push(element.ModuleId);
    });

    activeModal.componentInstance.listIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        var listModuleNewId = [];
        result.forEach(element => {
          var newModule = Object.assign({}, this.module);
          newModule.ModuleId = element.Id;
          newModule.ModuleName = element.Name;
          newModule.Specification = element.Specification;
          newModule.Note = element.Note;
          newModule.Qty = element.Qty;
          newModule.Code = element.Code;
          newModule.Price = element.Pricing;
          newModule.LeadTime = element.LeadTime;
          this.listModule.push(newModule);
          for (var item of this.listPractice) {
            item.ListModuleInPractice.push(Object.assign({}, newModule));
          }
          this.changeMaxLeadTimeModule();
          listModuleNewId.push(newModule.ModuleId);
        });
        this.getModulePrice(listModuleNewId);
        if (this.listPractice.length > 0) {
          this.ListHeader = this.listPractice[0].ListModuleInPractice;
        }
        this.getMaxQuantityModuleByPractice();
        // this.changeMaxLeadTimeModule();
        this.calculateTotalAmount();
      }
    }, (reason) => {

    });
  }

  changeMaxLeadTimeModule() {
    if (this.listModule.length > 0) {
      this.MaxLeadTimeModule = Math.max.apply(Math, this.listModule.map(function (o) { return o.LeadTime; }));
    } else {
      this.MaxLeadTimeModule = 0;
    }

    this.TotalModuleNoPrice = this.listModule.filter(d => d.IsNoPrice).length;
    this.TotalModuleNoLeadTime = this.listModule.filter(d => d.LeadTime == 0 || !d.LeadTime).length;
  }

  listModuleName: any = [];
  showSlectPractice() {
    let activeModal = this.modalService.open(ChoosePraciteComponent, { container: 'body', windowClass: 'ChoosePracite-model', backdrop: 'static' });
    var ListIdSelect = [];
    this.listPractice.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      this.setPractice(result);
    }, (reason) => {
    });
  }

  setPractice(practices) {
    if (!practices || practices.length == 0) {
      return;
    }

    var listModuleA: any[] = [];
    if (this.listPractice.length > 0) {
      listModuleA = JSON.parse(JSON.stringify(this.listPractice[0].ListModuleInPractice));
    } else {
      listModuleA = JSON.parse(JSON.stringify(this.listModule));
      this.ListHeader = this.listModule;
    }

    for (let i = 0; i < listModuleA.length; i++) {
      listModuleA[i].Qty = 0;
    }

    var listModuleNewId: string[] = [];
    practices.forEach(practice => {
      let isChecked = true;
      for (var item of listModuleA) {
        item.Qty = 0;
        for (var itemB of practice.ListModuleInPractice) {
          if (item.ModuleId == itemB.ModuleId) {
            item.Qty = itemB.Qty;
          }
        }
      }

      for (var itemB of practice.ListModuleInPractice) {
        isChecked = false;

        for (var item of listModuleA) {
          if (item.ModuleId == itemB.ModuleId) {
            isChecked = true;
          }
        }

        if (!isChecked) {
          listModuleA.push(itemB);
          for (var itemP of this.listPractice) {

            var itemNew = Object.assign({}, itemB);
            itemNew.Qty = 0;
            itemP.ListModuleInPractice.push(itemNew);
          }
        }

        var newModule = Object.assign({}, this.module);
        newModule.ModuleId = itemB.ModuleId;
        newModule.Name = itemB.ModuleName;
        newModule.Specification = itemB.Specification;
        newModule.Note = itemB.Note;
        newModule.Qty = itemB.Qty;
        newModule.Code = itemB.Code;
        var isNewModule = true;
        if (this.listModule.length > 0) {
          this.listModule.forEach(element => {
            if (element.ModuleId == newModule.ModuleId) {
              if (newModule.Qty > element.Qty) {
                element.Qty = newModule.Qty;
              }

              isNewModule = false;
            }
          });
          if (isNewModule) {
            listModuleNewId.push(newModule.ModuleId);
            this.listModule.push(newModule);

          }
        } else {
          listModuleNewId.push(newModule.ModuleId);
          this.listModule.push(newModule);

        }

        this.module = {
          ModuleId: '',
          Name: '',
          Specification: '',
          Note: '',
          Qty: 0,
          IsCheck: false,
          Code: '',
          Price: 0
        }
      }
      practice.ListModuleInPractice = JSON.parse(JSON.stringify(listModuleA));
      this.listPractice.push(practice);
    });

    this.getModulePrice(listModuleNewId);
  }

  getModulePrice(listModuleNewId: any[]) {
    this.productService.getModulePrice(listModuleNewId).subscribe(
      data => {
        if (data && data.length > 0) {
          this.listModule.forEach(mod => {
            data.forEach(element => {
              if (element.ModuleId == mod.ModuleId) {
                mod.Price = element.Price;
                mod.IsNoPrice = element.IsNoPrice;
              }
            });
          });

          this.calculateTotalAmount();
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  calculateTotalAmount() {
    this.totalAmount = 0;
    this.listModule.forEach(mod => {
      this.totalAmount += mod.Price * mod.Qty;
    });
  }

  changeQuantityModule(newModule) {
    var newTotalQuantity = 0;
    let listQuanlity = [];
    this.listPractice.forEach(practice => {
      practice.ListModuleInPractice.forEach(itemModule => {
        if (itemModule.ModuleId == newModule.ModuleId) {
          // newTotalQuantity = newTotalQuantity + parseInt(itemModule.Qty);
          listQuanlity.push(itemModule.Qty);
        }

      });
    });

    let maxQuanlity = Math.max.apply(Math, listQuanlity.map(function (o) { return o; }))

    this.listModule.forEach(itemModule => {
      if (itemModule.ModuleId == newModule.ModuleId) {
        // itemModule.Qty = newTotalQuantity;
        itemModule.Qty = maxQuanlity;
      }
    });

    this.calculateTotalAmount();
  }

  async save(model: any) {
    var isQuantity = true;
    for (var itemModule of this.listModule) {
      if (itemModule.Qty == 0 || itemModule.Qty == undefined || itemModule.Qty == '') {
        isQuantity = false;
        break;
      }
    }

    if (isQuantity == false) {
      this.messageService.showMessage("Bạn chưa điền đủ số lượng module");
      return;
    } else {
      this.productSketchesModel.ListPractice = this.listPractice;
      this.productSketchesModel.ListModuleProduct = this.listModule;
      this.productSketchesModel.ListSelect = this.listData;
      this.productService.createProductSketches(this.productSketchesModel).subscribe(
        data => {
          this.messageService.showSuccess('Lưu phác thảo thiết kế thành công!');
          if (model) {
            this.updateVersion(model);
          }
        },
        error => {
          this.messageService.showError(error);
        }
      );
    }
  }

  getInfoSketches() {
    this.productService.getInfoSketches(this.productSketchesModel).subscribe(data => {
      this.productSketchesModel = data;
      this.listPractice = data.ListPractice;
      if (this.listPractice.length > 0) {
        this.ListPracticeFile = this.listPractice[0].ListPracticeFile;
        this.ListHeader = this.listPractice[0].ListModuleInPractice;
      }
      let listModuleId = [];
      this.listModule = data.ListModuleProduct;
      if (this.listModule.length > 0) {
        this.listModule.forEach(element => {
          listModuleId.push(element.ModuleId);
        });
        this.getModulePrice(listModuleId);
      }
      this.getMaxQuantityModuleByPractice();
      // var isNewModule = true;
      // this.listPractice.forEach(pratice => {
      //   pratice.ListModuleInPractice.forEach(itemModule => {
      //     var newModule = Object.assign({}, this.module);
      //     newModule.ModuleId = itemModule.ModuleId;
      //     newModule.Name = itemModule.ModuleName;
      //     newModule.Specification = itemModule.Specification;
      //     newModule.Note = itemModule.Note;
      //     newModule.Qty = itemModule.Qty;
      //     newModule.Code = itemModule.Code;
      //     newModule.Price = itemModule.Pricing;
      //     newModule.LeadTime = itemModule.LeadTime;
      //     if (this.listModule.length > 0) {
      //       this.listModule.forEach(element => {
      //         if (element.ModuleId == newModule.ModuleId) {
      //           // element.Qty = element.Qty + newModule.Qty;
      //           isNewModule = false;
      //           if (element.Qty < newModule.Qty) {
      //             element.Qty = newModule.Qty;
      //           }
      //         }
      //       });
      //       if (isNewModule) {
      //         this.listModule.push(newModule);
      //       }
      //     } else {
      //       this.listModule.push(newModule);
      //     }

      //     this.module = {
      //       Id: '',
      //       Name: '',
      //       Specification: '',
      //       Note: '',
      //       Qty: 0,
      //       Price: 0,
      //       IsCheck: false,
      //     }
      //   });
      // });
      this.changeMaxLeadTimeModule();
      this.calculateTotalAmount();

    },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showConfirmDeletePractice(row: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá bài thực hành này không?").then(
      data => {
        this.listPractice.splice(this.listPractice.indexOf(row), 1);
        if (this.listPractice.length == 0) {
          this.totalAmount = 0;
          this.MaxLeadTimeModule = 0;
          this.TotalModuleNoLeadTime = 0;
          this.TotalModuleNoPrice = 0;
        } else {
          this.listModule.forEach(mod => {
            mod.Qty = 0;
            this.listPractice.forEach(practice => {
              practice.ListModuleInPractice.forEach(itemModule => {
                if (itemModule.ModuleId == mod.ModuleId && itemModule.Qty > mod.Qty) {
                  mod.Qty = itemModule.Qty
                }
              });
            });
          });
          this.calculateTotalAmount();
        }
      },
      error => {
        
      }
    );
  }

  showConfirmDeleteModule(row: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá module này không?").then(
      data => {
        for (var item of this.listModule) {
          if (item.ModuleId == row.ModuleId) {
            this.listModule.splice(this.listModule.indexOf(item), 1);
          }
        }

        this.listPractice.forEach(practice => {
          practice.ListModuleInPractice.forEach(itemModule => {
            if (itemModule.ModuleId == row.ModuleId) {
              practice.ListModuleInPractice.splice(practice.ListModuleInPractice.indexOf(itemModule), 1);
            }
          });
        });

        this.calculateTotalAmount();
      },
      error => {
        
      }
    );
  }

  showCreatePractice() {
    let activeModal = this.modalService.open(PopupPracticeCreateComponent, { container: 'body', windowClass: 'popup-practice-create-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      this.searchPractice(result);
    }, (reason) => {
    });
  }


  SearchModelPractice: any = {
    page: 1,
    PageSize: 10,
    TotalItem: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    Name: '',
    Code: '',
    PracticeGroupName: '',
    CurentVersion: '',
    TrainingTime: '',
    Note: '',
    listSelect: [],
    listBase: [],
    ListIdSelect: [],
    ListIdChecked: [],
    IsRequest: false
  }
  searchPractice(Id) {
    this.SearchModelPractice.Id = Id;
    this.service.searchPractice(this.SearchModelPractice).subscribe(data => {
      if (data) {
        this.setPractice(data.ListResult);
      }
    }, error => {
      this.messageService.showError(error);
    })
  }

  closeModal() {
    this.router.navigate(['thiet-bi/quan-ly-thiet-bi']);
  }

  synchronizedPractice() {
    this.productSketchesModel.ListModuleProduct = this.listModule;
    this.productService.synchronizedPractice(this.productSketchesModel).subscribe(data => {
      if (data) {
        this.listPractice = data;
        this.ListHeader = this.listPractice[0].ListModuleInPractice;
        this.ListPracticeFile = this.listPractice[0].ListPracticeFile;
        this.getMaxQuantityModuleByPractice();
      }
    }, error => {
      this.messageService.showError(error);
    })
  }

  getMaxQuantityModuleByPractice() {
    if (this.listPractice.length > 0) {
      this.listModule.forEach(module => {
        this.listPractice[0].ListModuleInPractice.forEach(element => {
          if (module.ModuleId == element.ModuleId) {
            module.MaxQtyByPractice = element.MaxQtyByPractice;
          }
        });
      });
      this.TotalAllPricePractice = this.listPractice.reduce((a, b) => a + b.TotalPrice, 0);
    }
  }

  showConfirmUploadVersion() {
    this.messageService.showConfirmFile("Bạn có muốn thay đổi version không?").then(
      async data => {
        if (data) {
          await this.showEditContent();
        } else {
          this.save("");
        }
      }
    );
  }

  async showEditContent() {
    let activeModal = this.modalService.open(HistoryVersionComponent, { container: 'body', windowClass: 'show-history-version-modal', backdrop: 'static' });
    activeModal.componentInstance.id = this.Id;
    activeModal.componentInstance.type = this.constant.HistoryVersion_Version_Product;
    activeModal.result.then(async (result) => {
      if (result) {
        await this.save(result);
        //await this.updateVersion(result);
      }
    }, (reason) => {
    });
  }

  async updateVersion(model: any) {
    this.serviceHistory.updateVersion(model).subscribe(
      async () => {
        this.messageService.showSuccess('Cập nhật version thành công!');
      }, error => {
        this.messageService.showError(error);
      }
    );
  }
  loadInforModule(ModuleId: string) {

  }

  // Vật tư phụ
  totalAmountAccessories = 0;
  @ViewChild('scrollAccessories',{static:false}) scrollAccessories: ElementRef;
  @ViewChild('scrollAccessoriesHeader',{static:false}) scrollAccessoriesHeader: ElementRef;
  heightAccessories = 500;

  listData: any = [];
  modelProductAccessories: any = {
    page: 1,
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    Name: '',
    Code: '',
    ProductId: '',
    MaterialId: '',
    ListMaterial: []
  }

  materialModel: any = {
    Id: '',
    Name: '',
    Code: '',
    Manafacture: '',
    Quantity: 0,
    Price: 0,
    Amount: 0,
    Note: '',
    Type: 1
  }

  showClick() {
    let activeModal = this.modalService.open(ProductaccessorieschooseComponent, { container: 'body', windowClass: 'Productaccessorieschoose-model', backdrop: 'static' });
    activeModal.componentInstance.ProductId = this.modelProductAccessories.ProductId;
    var ListIdSelect = [];
    this.listData.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          var model = Object.assign({}, this.materialModel);
          model.Id = element.Id;
          model.Name = element.Name;
          model.Code = element.Code;
          model.Manafacture = element.ManufactureName;
          model.Quantity = element.Quantity;
          model.Price = element.Pricing;
          model.Amount = element.Quantity * element.Pricing;
          model.Note = element.Note;
          model.Type = 1;
          this.listData.push(model);
        });
      }
    }, (reason) => {
    });
  }

  getProductaccessoriesInfo() {
    this.serviceAccessories.getProductAccessoriesInfo(this.modelProductAccessories).subscribe((data: any) => {
      if (data.ListResult) {
        this.listData = data.ListResult;
        this.modelProductAccessories.TotalItems = data.TotalItem;

        this.totalAmountAccessories = 0;
        this.listData.forEach(element => {
          this.totalAmountAccessories += element.Quantity * element.Price;
        });
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDelete(index: number) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá phụ kiện này không?").then(
      data => {
        this.delete(index);
      },
      error => {
        
      }
    );
  }

  delete(index: number) {
    this.listData.splice(index, 1);
    this.messageService.showSuccess('Xóa phụ kiện thành công!');
  }

  clear() {
    this.modelProductAccessories = {
      page: 1,
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      Id: '',
      Name: '',
      Code: '',
      ProductId: this.Id,
    }
    this.getProductaccessoriesInfo();
  }

  ExportExcel() {
    var model = {
      ProductId: this.modelProductAccessories.ProductId,
      ListData: this.listData
    }
    this.serviceAccessories.exportExcel(model).subscribe(d => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerApi + d;
      link.download = 'Download.docx';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, e => {
      this.messageService.showError(e);
    });
  }

  updateProductAccessories(row) {
    this.serviceAccessories.updateProductAccessories({ Id: row.Id, Quantity: row.Quantity, Amount: row.Amount }).subscribe(
      data => {
        // this.messageService.showSuccess('Chỉnh sửa phụ kiện thành công!');
        this.getProductaccessoriesInfo();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getListMatarial() {
    this.listData = [];
    this.productService.getListMatarial(this.listPractice).subscribe((data: any) => {
      if (data) {
        this.listData = data;
        this.modelProductAccessories.TotalItems = data.TotalItem;

        this.totalAmountAccessories = 0;
        this.listData.forEach(element => {
          this.totalAmountAccessories += element.Quantity * element.Price;
        });
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  DownloadAFileSetup(ListPracticeFile) {
    if (ListPracticeFile.length == 0) {
      this.messageService.showMessage("Không có file để tải");
      return;
    }
    else {
      var listFilePath: any[] = [];
      ListPracticeFile.forEach(element => {
        listFilePath.push({ Path: element.FilePath, FileName: element.FileName });
      });
      this.model.Name = "Hướng dẫn thực hành";
      this.model.ListDatashet = listFilePath;
      this.productService.downAllDocumentProcess(this.model).subscribe(data => {
        var link = document.createElement('a');
        link.setAttribute("type", "hidden");
        link.href = this.config.ServerFileApi + data.PathZip;
        link.download = 'Download.zip';
        document.body.appendChild(link);
        // link.focus();
        link.click();
        document.body.removeChild(link);
      }, e => {
        this.messageService.showError(e);
      });
    }
  }
}
