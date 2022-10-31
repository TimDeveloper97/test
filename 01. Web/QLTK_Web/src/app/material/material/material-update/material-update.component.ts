import { Component, OnInit, ElementRef, ViewChild, ViewEncapsulation } from '@angular/core';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import {
  MessageService, FileProcess, Configuration, AppSetting, ComboboxService,
  Constants, PermissionService
} from 'src/app/shared';
import { MaterialGroupService } from '../../services/materialgroup-service';
import { MaterialService } from '../../services/material-service';
import { ConfigMaterialService } from '../../services/config-material.service';
import { Router, ActivatedRoute } from '@angular/router';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { CreateNsmaterialCodeModalComponent } from '../../ns-material-group/create-nsmaterial-code-modal/create-nsmaterial-code-modal.component';
import { MaterialBuyHistoryModalComponent } from '../material-buy-history-modal/material-buy-history-modal.component';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { DxTreeListComponent } from 'devextreme-angular';
import { UnitCreateComponent } from '../../unit/unit-create/unit-create.component';
import { MaterialgrouptpaCreateComponent } from '../../materialgrouptpa/materialgrouptpa-create/materialgrouptpa-create.component';
import { ManufacturerCreateComponent } from '../../manufacturer/manufacturer-create/manufacturer-create.component';
import { ModuleGroupCreateComponent } from 'src/app/module/modulegroup/module-group-create/module-group-create.component';
import { MaterialgroupCreateComponent } from '../../materialgroup/materialgroup-create/materialgroup-create.component';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { Subscription, forkJoin } from 'rxjs';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

@Component({
  selector: 'app-material-update',
  templateUrl: './material-update.component.html',
  styleUrls: ['./material-update.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class MaterialUpdateComponent implements OnInit {
  @BlockUI() blockUI: NgBlockUI;
  @ViewChild(DxTreeListComponent) treeView;
  private onCheckFile3D = new BroadcastEventListener<any>('CheckFile3D');
  private upfile: Subscription;
  constructor(
    private signalRService: SignalRService,
    private messageService: MessageService,
    private materialGroupService: MaterialGroupService,
    private materialService: MaterialService,
    private comboboxService: ComboboxService,
    private configMaterialService: ConfigMaterialService,
    private modalService: NgbModal,
    public fileProcess: FileProcess,
    public fileProcessDataSheet: FileProcess,
    private uploadService: UploadfileService,
    private router: Router,
    private config: Configuration,
    private routeA: ActivatedRoute,
    public fileProcessImage: FileProcess,
    public appSetting: AppSetting,
    private checkSpecialCharacter: CheckSpecialCharacter,
    public constant: Constants,
    public permissionService: PermissionService,
  ) { }

  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };
  ModalInfo = {
    Title: 'Chỉnh sửa vật tư',
    SaveText: 'Lưu',

  };


  isAction: boolean = false;
  Id: string;
  StartIndexMaterialParameter = 1;

  ListMaterialGroup: any[] = [];
  ListMaterial: any[] = [];
  ListManufacture: any[] = [];
  ListManufactureId: any[] = [];
  ListModuleGroup: any[] = [];
  ListRawMaterial: any[] = [];
  ListUnit: any[] = [];
  ListMaterialGroupTPA: any[] = [];
  ListMaterialParameter: any[] = [];
  ListMaterialParameterValue: any[] = [];
  myFiles: any = [];
  fileToUpload: File = null;
  //manufactureIds: number;
  selectItem = [];
  treeBoxValue: string[];
  isDropDownBoxOpened = false;
  selectedRowKeys: any[] = [];
  rowFocusIndex = -1;
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
  treeBoxValueMaterialGroup: string;
  isDropDownBoxOpenedMaterialGroup = false;
  materialGroupId: any[] = [];

  DateNow = new Date();
  UserName = JSON.parse(localStorage.getItem('qltkcurrentUser')).userfullname;

  modelMaterial: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    UnitId: '',
    MaterialGroupId: '',
    ManufactureId: '',
    ManufactureName: '',
    MaterialGroupTPAId: '',
    Code: '',
    Name: '',
    Note: '',
    Pricing: 0,
    DeliveryDays: 0,
    ImagePath: '',
    ThumbnailPath: '',
    LastBuyDate: '',
    IsUsuallyUse: false,
    MaterialType: '',
    RawMaterialId: '',
    MaterialDesign3DId: '',
    MaterialDataSheetId: '',
    Is3D: true,
    IsDataSheet: true,
    IsSetup: false,
    RawMaterialName: '',
    MaterialGroupName: '',
    MechanicalType: '',
    Status: '',
    Weight: '',
    ModuleGroupId: '',
    RawMaterial: '',
    IsRedundant: false,
    RedundantAmount: 0,
    RedundantDescription:'',
    RedundantDeliveryNote:'',

    ListMaterialParameter: [],
    ListFileDesign3D: [],
    ListFileDataSheet: [],
    ListMaterialPart: [],
    ListImage: []

  }

  fileDesign3D = {
    Id: '',
    FilePath: '',
    FileName: '',
    Size: '',
    CreateBy: '',
    CreateDate: '',
    UpdateBy: '',
    UpdateDate: ''
  }

  fileDataSheet = {
    Id: '',
    FilePath: '',
    FileName: '',
    Size: '',
    CreateBy: '',
    CreateDate: '',
    UpdateBy: '',
    UpdateDate: ''
  }
  fileImage = {
    Id: '',
    MaterialId: '',
    Path: '',
    ThumbnailPath: ''
  }

  ListFileDesign3D = [];
  ListFileDataSheet = [];
  ListMaterialPart = [];

  thumbnailActions: NgxGalleryOptions[];

  deleteImage(event, index): void {
    let imageDelete = this.galleryImages[index].medium;
    let indexDelete;
    this.modelMaterial.ListImage.forEach(element => {
      let url = this.config.ServerFileApi + element.ThumbnailPath;
      if (url == imageDelete) {
        indexDelete = this.modelMaterial.ListImage.indexOf(element);
      }
    });
    this.galleryImages.splice(index, 1);
    this.modelMaterial.ListImage.splice(indexDelete, 1);
  }

  ngOnInit() {

    this.modelMaterial.Id = this.routeA.snapshot.paramMap.get('Id');
    forkJoin([
      this.comboboxService.getCbbManufacture(),
      this.comboboxService.getCbbMaterialGroup(),
      this.comboboxService.getCbbModuleGroup(),
      this.materialService.getMaterialInfo(this.modelMaterial)
    ]).subscribe(([res1, res2, res3, res4]) => {
      this.ListManufacture = res1;
      this.ListMaterialGroup = res2;
      this.ListModuleGroup = res3.ListResult;
      this.setMaterialInfo(res4);
    });

    //this.getCBBManufacture();
    //this.getCBBMaterialGroup();
    this.getCBBMaterialGroupTPA();
    // this.getCBBRawMaterial();
    this.getCBBUnit();
    //this.getCBBModuleGroup();
    // this.getById();

    this.Valid = true;

    // this.appSetting.PageTitle = "Chỉnh sửa Vật tư - " + this.modelMaterial.Code + " - " + this.modelMaterial.Name;
    this.ModalInfo.Title = 'Chỉnh sửa vật tư';
    this.ModalInfo.SaveText = 'Lưu';

    this.signalRService.listen(this.onCheckFile3D, false);
    this.upfile = this.onCheckFile3D.subscribe((data: any) => {
      if (data) {

      }
      else {
        this.messageService.showMessage('Không tồn tại thiết kế cho sản phẩm này trong ổ D!');
      }
      this.blockUI.stop();
    });

    this.galleryOptions = [
      {
        width: '100%',
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        thumbnailActions: [{ icon: 'fa fa-times-circle', onClick: this.deleteImage.bind(this), titleText: 'delete'}]

      },
      // max-width 800
      {
        breakpoint: 800,
        width: '100%',
        imagePercent: 80,
        thumbnailsPercent: 20,
        thumbnailsMargin: 20,
        thumbnailMargin: 20,
        thumbnailActions: [{ icon: 'fa fa-times-circle', onClick: this.deleteImage.bind(this), titleText: 'delete' }]

      },
      // max-width 400
      {
        breakpoint: 400,
        preview: false
      }
    ];

  }

  ngOnDestroy() {
    this.signalRService.stopListening(this.onCheckFile3D);
  }

  NameMaterialGroupTPA: string;
  getName(MaterialGroupId) {
    for (var item of this.ListMaterialGroup) {
      if (item.Id == MaterialGroupId) {
        this.NameMaterialGroupTPA = item.Exten;
      }
    }
    this.check();
  }


  getById() {
    this.materialService.getMaterialInfo(this.modelMaterial).subscribe(data => {
      this.setMaterialInfo(data);
      this.searchModuleMaterial(data);
    });
  }

  setMaterialInfo(data) {
    this.modelMaterial = data;
    this.appSetting.PageTitle = "Chỉnh sửa Vật tư - " + this.modelMaterial.Code + " - " + this.modelMaterial.Name;
    this.materialGroupId = data.Id;
    this.ListMaterialPart = data.ListMaterialPart;
    this.treeBoxValue = data.ModuleGroupId;
    this.getName(this.materialGroupId);
    for (var item of data.ListImage) {
      this.galleryImages.push({
        small: this.config.ServerFileApi + item.ThumbnailPath,
        medium: this.config.ServerFileApi + item.Path,
        big: this.config.ServerFileApi + item.Path
      });
    }
    this.ListManufacture.forEach(element => {
      if (this.modelMaterial.ManufactureId == element.Id) {
        this.modelMaterial.ManufactureName = element.Name;
      }
    });
    this.treeBoxValue = this.modelMaterial.ListModuleGroupId;
    this.selectedRowKeys = this.treeBoxValue;
  }

  selectIndex = -1;
  loadValue(param, index, rowId) {
    this.selectIndex = index;
    this.getValueByParameterId(index);

  }

  //preview ảnh
  ListImage = [];
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[] = [];
  uploadFileClickImage($event) {
    this.ListImage = [];
    var fileImage = this.fileProcessImage.getFileOnFileChange($event);
    for (var item of fileImage) {
      this.ListImage.push(item);
    }

    this.uploadService.uploadListFile(this.ListImage, 'ImageMaterial/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach(item => {
          var file = Object.assign({}, this.fileImage);
          file.Path = item.FileUrl;
          file.ThumbnailPath = item.FileUrlThum;
          this.modelMaterial.ListImage.push(file);
          this.galleryImages.push({
            small: this.config.ServerFileApi + item.FileUrlThum,
            medium: this.config.ServerFileApi + item.FileUrlThum,
            big: this.config.ServerFileApi + item.FileUrl
          });
        });
      }
    }, error => {
      this.messageService.showError(error);
    });

  }

  // up file design3d
  uploadFileClick($event) {
    var isExist = false;
    var file3d = this.fileProcess.getFileOnFileChange($event);
    var materialCode = this.modelMaterial.Code + ".ipt";
    for (var item of file3d) {
      var replaceName = item.name.split(')').join('/');
      if (replaceName == materialCode) {
        if (replaceName == materialCode) {
          isExist = false;
          for (var ite of this.modelMaterial.ListFileDesign3D) {
            if (ite.Id != null) {
              if (item.name == ite.FileName) {
                isExist = true;
              }
            }
            else {
              if (item.name == ite.name) {
                isExist = true;
              }
            }
          }
        }

        if (isExist) {
          this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không").then(
            data => {
              this.updateFileAndReplace3d(file3d, true);
            }, error => {
              this.updateFileAndReplace3d(file3d, false);
            });
        }
        else {
          this.updateFileManual3d(file3d);
        }
        // }
      } else {
        this.messageService.showMessage("Tên file chưa đúng định dạng (Tên file phải trùng với mã và chỉ chấp nhận định dạng .ipt)");
      }
    }
  }

  updateFileAndReplace3d(fileManualDocuments, isReplace) {
    var isExist = false;

    for (var file of fileManualDocuments) {
      for (let index = 0; index < this.modelMaterial.ListFileDesign3D.length; index++) {

        if (this.modelMaterial.ListFileDesign3D[index].Id != null) {
          if (file.name == this.modelMaterial.ListFileDesign3D[index].FileName) {
            isExist = true;
            if (isReplace) {
              this.modelMaterial.ListFileDesign3D.splice(index, 1);
            }
          }
        }
        else if (file.name == this.modelMaterial.ListFileDesign3D[index].name) {
          isExist = true;
          if (isReplace) {
            this.modelMaterial.ListFileDesign3D.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        file.IsFileUpload = true;
        this.modelMaterial.ListFileDesign3D.push(file);
      }
    }
  }

  updateFileManual3d(fileManualDocuments) {
    for (var file of fileManualDocuments) {
      file.IsFileUpload = true;
      this.modelMaterial.ListFileDesign3D.push(file);
    }
  }


  showConfirmDeleteFile3D(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file đính kèm này không?").then(
      data => {
        this.modelMaterial.ListFileDesign3D.splice(index, 1);
      }
    );
  }



  //up file datasheet
  uploadFileClickDataSheet($event) {

    var fileDataSheet = this.fileProcessDataSheet.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.modelMaterial.ListFileDataSheet) {
        if (ite.Id != null) {
          if (file.name == ite.FileName) {
            isExist = true;
          }
        }
        else {
          if (file.name == ite.name) {
            isExist = true;
          }
        }
      }
    }

    if (isExist) {
      this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không").then(
        data => {
          this.updateFileAndReplaceManualDocument(fileDataSheet, true);
        }, error => {
          this.updateFileAndReplaceManualDocument(fileDataSheet, false);
        });
    }
    else {
      this.updateFileManualDocument(fileDataSheet);
    }
  }

  updateFileAndReplaceManualDocument(fileManualDocuments, isReplace) {
    var isExist = false;

    for (var file of fileManualDocuments) {
      for (let index = 0; index < this.modelMaterial.ListFileDataSheet.length; index++) {

        if (this.modelMaterial.ListFileDataSheet[index].Id != null) {
          if (file.name == this.modelMaterial.ListFileDataSheet[index].FileName) {
            isExist = true;
            if (isReplace) {
              this.modelMaterial.ListFileDataSheet.splice(index, 1);
            }
          }
        }
        else if (file.name == this.modelMaterial.ListFileDataSheet[index].name) {
          isExist = true;
          if (isReplace) {
            this.modelMaterial.ListFileDataSheet.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        file.IsFileUpload = true;
        this.modelMaterial.ListFileDataSheet.push(file);
      }
    }
  }

  updateFileManualDocument(fileManualDocuments) {
    for (var file of fileManualDocuments) {
      file.IsFileUpload = true;
      this.modelMaterial.ListFileDataSheet.push(file);
    }
  }

  showConfirmDeleteFileDataSheet(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file đính kèm này không?").then(
      data => {
        this.modelMaterial.ListFileDataSheet.splice(index, 1);
      },
      error => {

      }
    );
  }
  //Lấy list các combobox
  ListMaterialGroupId = []; //expandedRowKeys
  getCBBMaterialGroup() {
    this.comboboxService.getCbbMaterialGroup().subscribe((data: any) => {
      if (data.ListResult) {
        this.ListMaterialGroup = data;
        for (var item of this.ListMaterialGroup) {
          this.ListMaterialGroupId.push(item.Id);
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getCBBMaterialGroupTPA() {
    this.comboboxService.getCbbMaterialGroupTPA().subscribe((data: any) => {
      if (data) {
        this.ListMaterialGroupTPA = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getCBBManufacture() {
    this.comboboxService.getCbbManufacture().subscribe((data: any) => {
      if (data) {
        this.ListManufacture = data;
        for (var item of this.ListManufacture) {
          this.ListManufactureId.push(item.Id);
        }
      }
      this.ListManufacture.forEach(element => {
        if (this.modelMaterial.ManufactureId == element.Id) {
          this.modelMaterial.ManufactureName = element.Name;
        }
      });
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getCBBRawMaterial() {
    this.comboboxService.getCbbRawMaterial().subscribe((data: any) => {
      if (data) {
        this.ListRawMaterial = data;
        for (var i of this.ListRawMaterial) {
          if (i.Id == "" || i.Id == null) {
            this.ListRawMaterial.splice(this.ListRawMaterial.indexOf(i), 1);
          }
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getCBBUnit() {
    this.comboboxService.getCbbUnit().subscribe((data: any) => {
      if (data) {
        this.ListUnit = data;
        for (var i of this.ListUnit) {
          if (i.Id == "" || i.Id == null) {
            this.ListUnit.splice(this.ListUnit.indexOf(i), 1);
          }
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getCBBModuleGroup() {
    this.comboboxService.getCbbModuleGroup().subscribe((data: any) => {
      if (data) {
        this.ListModuleGroup = data.ListResult;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getParameterByGroupId() {
    this.configMaterialService.getParameterByGroupId(this.modelMaterial.MaterialGroupId).subscribe(
      data => {
        this.modelMaterial.ListMaterialParameter = data;
        this.ListMaterialParameterValue = [];
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getValueByParameterId(i) {
    this.ListMaterialParameterValue = this.modelMaterial.ListMaterialParameter[i].ListValue;
  }


  closeModal(isOK: boolean) {
    this.router.navigate(['vat-tu/quan-ly-vat-tu']);
  }

  updateMaterial() {
    var validCode = this.checkSpecialCharacter.checkCodeMaterial(this.modelMaterial.Code);
    if (validCode) {
      var validUnicode = this.checkSpecialCharacter.checkUnicode(this.modelMaterial.Code);
      if (validUnicode) {
        this.ListFileDesign3D = [];
        this.ListFileDesign3D = this.modelMaterial.ListFileDesign3D;
        this.modelMaterial.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
        this.uploadService.uploadListFile(this.ListFileDesign3D, 'Design3D/').subscribe((event: any) => {
          if (event.length > 0) {
            event.forEach((item, index) => {
              var file = Object.assign({}, this.fileDesign3D);
              file.FileName = item.FileName;
              file.Size = item.FileSize;
              file.FilePath = item.FileUrl;
              this.modelMaterial.ListFileDesign3D.push(file);
            });
          }

          for (var item of this.modelMaterial.ListFileDesign3D) {
            if (item.FilePath == null || item.FilePath == "") {
              this.modelMaterial.ListFileDesign3D.splice(this.modelMaterial.ListFileDesign3D.indexOf(item), 1);
            }
          }

          this.uploadService.uploadListFile(this.modelMaterial.ListFileDataSheet, 'DataSheet/').subscribe((even: any) => {
            if (even.length > 0) {
              even.forEach((item, index) => {
                var file = Object.assign({}, this.fileDataSheet);
                file.FileName = item.FileName;
                file.Size = item.FileSize;
                file.FilePath = item.FileUrl;
                this.modelMaterial.ListFileDataSheet.push(file);
              });
            }
            for (var item of this.modelMaterial.ListFileDataSheet) {
              if (item.FilePath == null || item.FilePath == "") {
                this.modelMaterial.ListFileDataSheet.splice(this.modelMaterial.ListFileDataSheet.indexOf(item), 1);
              }
            }

            this.materialService.updateMaterial(this.modelMaterial, this.myFiles).subscribe(
              data => {
                this.messageService.showSuccess('Sửa vật tư thành công!');
                //this.closeModal(true);                
                if (this.ListImage.length > 0) {
                  this.modelMaterial.ListImage = [];
                  this.ListImage = [];
                  this.galleryImages = [];
                }
                if (this.ListFileDesign3D.length > 0) {
                  this.modelMaterial.ListFileDesign3D = [];
                  this.ListFileDesign3D = [];
                }
                if (this.ListFileDataSheet.length > 0) {
                  this.modelMaterial.ListFileDataSheet = [];
                  this.ListFileDataSheet = [];
                }
                this.getById();
              },
              error => {
                this.messageService.showError(error);
              }
            );

          }, error => {
            this.messageService.showError(error);
          });
        }, error => {
          this.messageService.showError(error);
        });
      }
      else {
        this.messageService.showMessage("Mã vật tư không được có dấu!");
      }
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.modelMaterial.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
          this.uploadService.uploadListFile(this.ListFileDesign3D, 'Design3D/').subscribe((event: any) => {
            if (event.length > 0) {
              event.forEach(item => {
                var file = Object.assign({}, this.fileDesign3D);
                file.FileName = item.FileName;
                file.Size = item.FileSize;
                file.FilePath = item.FileUrl;
                this.modelMaterial.ListFileDesign3D.push(file);
              });
            }

            this.uploadService.uploadListFile(this.modelMaterial.ListFileDataSheet, 'DataSheet/').subscribe((event: any) => {
              if (event.length > 0) {
                event.forEach(item => {
                  var file = Object.assign({}, this.fileDataSheet);
                  file.FileName = item.FileName;
                  file.Size = item.FileSize;
                  file.FilePath = item.FileUrl;
                  this.modelMaterial.ListFileDataSheet.push(file);
                });
              }

              this.materialService.updateMaterial(this.modelMaterial, this.myFiles).subscribe(
                data => {
                  this.messageService.showSuccess('Sửa vật tư thành công!');
                  //this.closeModal(true);
                  this.modelMaterial.ListFileDesign3D = [];
                  this.modelMaterial.ListFileDataSheet = [];
                  this.ListFileDataSheet = [];
                  this.ListFileDesign3D = [];
                  this.getById();
                },
                error => {


                  this.messageService.showError(error);
                }
              );

            }, error => {
              this.messageService.showError(error);
            });
          }, error => {
            this.messageService.showError(error);
          });
        },
        error => {

        });
    }
  }

  File() {
    this.uploadFile();
  }

  save(isContinue: boolean) {
    this.updateMaterial();
  }
  saveAndContinue() {
    this.save(true);
  }

  showCreateCode(Id: string) {
    let activeModal = this.modalService.open(CreateNsmaterialCodeModalComponent, { container: 'body', windowClass: 'create-nsmaterial-code-modal', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        this.modelMaterial.Code = result;

      }
    }, (reason) => {
    });
  }

  DownloadAFile(file) {
    this.fileProcess.downloadFileBlob(file.FilePath, file.FileName);
  }

  Valid: boolean;
  check() {

    if (this.modelMaterial.MaterialGroupId != '' && this.modelMaterial.MaterialGroupId != undefined && this.modelMaterial.Name != '' && this.modelMaterial.Name != undefined && this.modelMaterial.Code != '' && this.modelMaterial.Code != undefined && this.modelMaterial.ManufactureId != '' && this.modelMaterial.ManufactureId != undefined && this.modelMaterial.Status != '' && this.modelMaterial != undefined) {
      this.Valid = true;
    } else {
      this.Valid = false;
    }
  }

  selectMaterialType(exten: string) {
    for (var item of this.ListManufacture) {
      if (item.Id == exten) {
        this.modelMaterial.MaterialType = item.Exten;
      }
    }
  }

  showBuyHistory(Id) {
    let activeModal = this.modalService.open(MaterialBuyHistoryModalComponent, { container: 'body', windowClass: 'buy-history-modal', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
  }

  syncTreeViewSelection(e) {
    var component = (e && e.component) || (this.treeView && this.treeView.instance);
    // if (!this.treeBoxValue) {
    //   this.selectedRowKeys = [];
    // } else {
    //   this.selectedRowKeys = [this.treeBoxValue];
    // }
  }
  onRowDblClick() {
    this.isDropDownBoxOpened = false;

  }
  treeView_itemSelectionChanged(e) {
    this.treeBoxValue = e.selectedRowKeys;
    this.modelMaterial.ListModuleGroupId = e.selectedRowKeys;
    //this.model.MaterialGroupSourceId = e.selectedRowKeys[0];
    // this.getParameterByGroupSourceId();
    //this.closeDropDownBox();
  }
  closeDropDownBox() {
    this.isDropDownBoxOpened = false;
  }

  // Gọi popup thêm mới các combobox
  showAddUnit() {
    let activeModal = this.modalService.open(UnitCreateComponent, { container: 'body', windowClass: 'unit-create-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        this.getCBBUnit();
      }
    }, (reason) => {
    });
  }

  showAddMaterialGroupTPA() {
    let activeModal = this.modalService.open(MaterialgrouptpaCreateComponent, { container: 'body', windowClass: 'materialgrouptpa-create-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        this.getCBBMaterialGroupTPA();
      }
    }, (reason) => {
    });
  }

  showAddManufacture() {
    let activeModal = this.modalService.open(ManufacturerCreateComponent, { container: 'body', windowClass: 'manufacturecreate-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        this.getCBBManufacture();
      }
    }, (reason) => {
    });
  }

  showAddModuleGroup() {
    let activeModal = this.modalService.open(ModuleGroupCreateComponent, { container: 'body', windowClass: 'modulegroupgreate-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        this.getCBBModuleGroup();
      }
    }, (reason) => {
    });
  }

  showAddMaterialGroup() {
    let activeModal = this.modalService.open(MaterialgroupCreateComponent, { container: 'body', windowClass: 'materialgroup-create-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        this.getCBBMaterialGroup();
      }
    }, (reason) => {
    });
  }

  nameFile = "";

  handleFileInput(files: FileList) {
    this.fileToUpload = files.item(0);
    this.nameFile = files.item(0).name;
    this.modelMaterial.ListFileDesign3D.push(files);
  }

  importModel: any = {
    Path: ''
  }

  uploadFile() {
    var Link = "D:\\test\\PCB.G540100.ipt";
    this.signalRService.invoke('CheckFile3D', Link).subscribe(data => {
      if (data) {

      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  // change() {
  //   if (this.manufactureIds != null) {
  //     this.modelMaterial.ManufactureId = this.manufactureIds[0];
  //   } else {
  //     this.modelMaterial.ManufactureId = "";
  //   }
  //   this.closeDropDownBoxManufacture();
  //   this.check();
  // }
  searchModuleMaterial(data) {
    this.ListMaterialPart = data.ListMaterialPart;
  }

  isConfirm: boolean = false;
  syncSaleMaterial(isConfirm: boolean) {
    let list = [];
    list.push(this.modelMaterial.Id);
    this.materialService.syncSaleMaterial(false, isConfirm, list).subscribe(
      data => {
        if (data) {
          this.confirm(data);
        } else {
          this.messageService.showSuccess('Đồng bộ sản phẩm kinh doanh thành công!');
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  confirm(message: string) {
    this.messageService.showConfirm(message).then(
      data => {
        this.syncSaleMaterial(true);
      }
    );
  }
}


