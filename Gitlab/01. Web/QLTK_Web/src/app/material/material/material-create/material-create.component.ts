import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService, FileProcess, Configuration, AppSetting, ComboboxService, PermissionService } from 'src/app/shared';
import { MaterialGroupService } from '../../services/materialgroup-service';
import { MaterialService } from '../../services/material-service';
import { ConfigMaterialService } from '../../services/config-material.service';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { CreateNsmaterialCodeModalComponent } from '../../ns-material-group/create-nsmaterial-code-modal/create-nsmaterial-code-modal.component';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { DxTreeListComponent, DxDataGridModule } from 'devextreme-angular';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { UnitCreateComponent } from '../../unit/unit-create/unit-create.component';
import { MaterialgrouptpaCreateComponent } from '../../materialgrouptpa/materialgrouptpa-create/materialgrouptpa-create.component';
import { ManufacturerCreateComponent } from '../../manufacturer/manufacturer-create/manufacturer-create.component';
import { ModuleGroupCreateComponent } from 'src/app/module/modulegroup/module-group-create/module-group-create.component';
import { MaterialgroupCreateComponent } from '../../materialgroup/materialgroup-create/materialgroup-create.component';
@Component({
  selector: 'app-material-create',
  templateUrl: './material-create.component.html',
  styleUrls: ['./material-create.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class MaterialCreateComponent implements OnInit {
  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private materialGroupService: MaterialGroupService,
    private materialService: MaterialService,
    private comboboxService: ComboboxService,
    private configMaterialService: ConfigMaterialService,
    private modalService: NgbModal,
    public fileProcessImage: FileProcess,
    public fileProcess: FileProcess,
    public fileProcessDataSheet: FileProcess,
    private uploadService: UploadfileService,
    private router: Router,
    private config: Configuration,
    public appSetting: AppSetting,
    private service: ConfigMaterialService,
    private routeA: ActivatedRoute,
    private checkSpecialCharacter: CheckSpecialCharacter,
    public permissionService: PermissionService

  ) { }

  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };

  ModalInfo = {
    Title: 'Thêm mới vật tư',
    SaveText: 'Lưu',

  };

  isAction: boolean = false;
  Id: string;
  GroupId: string;
  StartIndexMaterialParameter = 1;

  ListMaterialGroup: any[] = [];
  ListMaterial: any[] = [];
  ListManufacture: any[] = [];
  ListManufactureId: any[] = [];
  ListModuleGroup: any[] = [];
  ListRawMaterial: any[] = [];
  ListUnit = [];
  ListMaterialGroupTPA: any[] = [];
  ListMaterialParameter: any[] = [];
  ListMaterialParameterValue: any[] = [];
  //manufactureIds = [];
  manufactureId: string;
  myFiles: any = [];
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

  modelMaterialGroup: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    MaterialGroupTPAId: '',
    MaterialGroupTPAName: '',
    MaterialGroupTPACode: '',
    Name: '',
    Code: '',
    ParentId: '',
    Description: '',
  }

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
    ListModuleGroupId: '',
    RawMaterial: '',
    IsRedundant: false,
    RedundantAmount: 0,
    RedundantDescription:'',
    RedundantDeliveryNote:'',

    ListMaterialParameter: [],
    ListFileDesign3D: [],
    ListFileDataSheet: [],
    ListImage: [],

  }
  fileImage = {
    Id: '',
    MaterialId: '',
    Path: '',
    ThumbnailPath: ''
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

  ListFileDesign3D = [];
  ListFileDataSheet = [];
  ListModuleUse = [];
  ngOnInit() {
    this.modelMaterial.Status = '0';
    this.modelMaterial.MaterialType = '1';
    this.appSetting.PageTitle = "Thêm mới Vật tư";
    this.getCBBMaterialGroup();
    this.getCBBMaterialGroupTPA();
    this.getCBBManufacture();
    this.getCBBRawMaterial();
    this.getCBBUnit();
    this.getCBBModuleGroup();
    this.modelMaterial.MaterialGroupId = localStorage.getItem("selectedMaterialGroupId");
    if (this.modelMaterial.MaterialGroupId != null && this.modelMaterial.MaterialGroupId != '') {
      this.getCBBMaterialGroup();
    }
    // if (this.modelMaterial.MaterialGroupId) {
    //   this.treeBoxValueMaterialGroup = this.modelMaterial.MaterialGroupId;
    //   this.materialGroupId = [this.treeBoxValueMaterialGroup];
    // }
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa vật tư';
      this.ModalInfo.SaveText = 'Lưu';
      // this.getMaterialGroupInfo();
    }
    else {
      this.ModalInfo.Title = "Thêm vật tư";
    }
    this.galleryOptions = [
      {
        width: '100%',
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        thumbnailActions: [{ icon: 'fa fa-times-circle', onClick: this.deleteImage.bind(this), titleText: 'Xoá ảnh' }]
      },
      // max-width 800
      {
        breakpoint: 800,
        width: '100%',
        imagePercent: 80,
        thumbnailsPercent: 20,
        thumbnailsMargin: 20,
        thumbnailMargin: 20,
        thumbnailActions: [{ icon: 'fa fa-times-circle', onClick: this.deleteImage.bind(this), titleText: 'Xoá ảnh' }]
      },
      // max-width 400
      {
        breakpoint: 400,
        preview: false
      },
    ];

  }

  selectIndex = -1;
  loadValue(param, index, rowId) {
    this.selectIndex = index;
    this.getValueByParameterId(index);
  }

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
    var file3d = this.fileProcess.getFileOnFileChange($event);
    
    var materialCode = this.modelMaterial.Code + ".ipt";
    for (var item of file3d) {
      var replaceName = item.name.split(')').join('/');
      if (replaceName == materialCode) {
        var file = Object.assign({}, this.fileDesign3D);
        file.FileName = item.name;
        file.Size = item.size;
        if (this.modelMaterial.ListFileDesign3D.length > 0) {
          this.modelMaterial.ListFileDesign3D.forEach(element => {
            if (item.name == element.FileName) {
              var index = this.modelMaterial.ListFileDesign3D.indexOf(element);
              this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không").then(
                data => {
                  this.modelMaterial.ListFileDesign3D.splice(index, 1);
                  this.modelMaterial.ListFileDesign3D.push(file);
                  this.ListFileDesign3D.push(item);
                },
                error => {
                  
                });
            }
          });
        }
        else {
          this.modelMaterial.ListFileDesign3D.push(file);
          this.ListFileDesign3D.push(item);
        }
      } else {
        this.messageService.showMessage("Tên file chưa đúng định dạng (Tên file phải trùng với mã và chỉ chấp nhận định dạng .ipt)");
      }

    }

  }

  showConfirmDeleteFile3D(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file đính kèm này không?").then(
      data => {
        this.modelMaterial.ListFileDesign3D.splice(index, 1);
      },
      error => {
        
      }
    );
  }

  //up file datasheet

  uploadFileClickDataSheet($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
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
      if (data) {
        this.ListMaterialGroup = data;
        for (var item of this.ListMaterialGroup) {
          if (item.Id == this.modelMaterial.MaterialGroupId) {
            this.NameMaterialGroupTPA = item.Exten;
          }
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
      }
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
        if (this.ListUnit.length > 0) {
          this.modelMaterial.UnitId = this.ListUnit[0].Id;
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

  selectMaterialType(exten: string) {
    for (var item of this.ListManufacture) {
      if (item.Id == exten) {
        this.modelMaterial.MaterialType = item.Exten;
      }
    }
  }

  selectMaterialGroupTpa(materialGroupId: string) {
    for (var item of this.ListMaterialGroup) {
      if (item.Id == materialGroupId) {
        this.modelMaterial.MaterialGroupTPAId = item.MaterialGroupTPAId;
      }
    }
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
  NameMaterialGroup: string;
  NameMaterialGroupTPA: string;
  getName(MaterialGroupId) {
    for (var item of this.ListMaterialGroup) {
      if (item.Id == MaterialGroupId) {
        this.NameMaterialGroupTPA = item.Exten;
      }
    }
    this.check();
  }

  getValueByParameterId(i) {
    this.ListMaterialParameterValue = this.modelMaterial.ListMaterialParameter[i].ListValue;
  }


  closeModal(isOK: boolean) {
    this.router.navigate(['vat-tu/quan-ly-vat-tu']);
    // this.ListFileDataSheet = [];
    // this.ListFileDesign3D = [];
    // this.activeModal.close(isOK ? isOK : this.isAction);
  }

  createMaterial(isContinue) {
    var validCode = this.checkSpecialCharacter.checkCodeMaterial(this.modelMaterial.Code);

    if (validCode) {
      var validUnicode = this.checkSpecialCharacter.checkUnicode(this.modelMaterial.Code);
      if (validUnicode) {
        this.modelMaterial.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;

        this.uploadService.uploadListFile(this.ListFileDesign3D, 'Design3D/').subscribe((event: any) => {
          if (event.length > 0) {
            var count = 0;
            this.modelMaterial.ListFileDesign3D.forEach(item => {
              if (item.FilePath == '') {
                item.FilePath = event[count].FileUrl;
                count++;
              }
            });
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

            this.materialService.createMaterial(this.modelMaterial, this.myFiles).subscribe(
              data => {
                if (isContinue) {
                  this.Valid = false;
                  this.isAction = true;
                  this.modelMaterial = {};
                  this.modelMaterial.Is3D = true;
                  this.modelMaterial.IsDataSheet = true;
                  this.modelMaterial.MaterialType = '1';
                  this.modelMaterial.UnitId = this.ListUnit[0].Id;
                  this.modelMaterial.ListFileDataSheet = [];
                  this.modelMaterial.ListFileDesign3D = [];
                  this.ListFileDataSheet = [];
                  this.ListFileDesign3D = [];
                  this.modelMaterial.MaterialGroupTPAId = '';
                  this.modelMaterial.ListMaterialParameter = [];
                  this.modelMaterial.ManufactureId = this.manufactureId;
                  this.modelMaterial.MaterialGroupId = this.treeBoxValueMaterialGroup;
                  this.modelMaterial.Status = '0';
                  this.messageService.showSuccess('Thêm mới vật tư thành công!');
                }
                else {
                  this.messageService.showSuccess('Thêm mới vật tư thành công!');
                  this.closeModal(true);
                }
              },
              error => {
                this.ListFileDataSheet = [];
                this.ListFileDesign3D = [];
                this.modelMaterial.ListFileDataSheet = [];
                this.modelMaterial.ListFileDesign3D = [];
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

            this.uploadService.uploadListFile(this.ListFileDataSheet, 'DataSheet/').subscribe((event: any) => {
              if (event.length > 0) {
                event.forEach(item => {
                  var file = Object.assign({}, this.fileDataSheet);
                  file.FileName = item.FileName;
                  file.Size = item.FileSize;
                  file.FilePath = item.FileUrl;
                  this.modelMaterial.ListFileDataSheet.push(file);
                });
              }

              this.materialService.createMaterial(this.modelMaterial, this.myFiles).subscribe(
                data => {
                  if (isContinue) {
                    this.Valid = false;
                    this.isAction = true;
                    this.modelMaterial = {};
                    this.modelMaterial.Is3D = true;
                    this.modelMaterial.IsDataSheet = true;
                    this.modelMaterial.MaterialType = '1';
                    this.modelMaterial.UnitId = this.ListUnit[0].Id;
                    this.modelMaterial.ListFileDataSheet = [];
                    this.modelMaterial.ListFileDesign3D = [];
                    this.ListFileDataSheet = [];
                    this.ListFileDesign3D = [];
                    this.modelMaterial.MaterialGroupTPAId = '';
                    this.modelMaterial.ListMaterialParameter = [];
                    this.modelMaterial.ManufactureId = this.manufactureId;
                    this.modelMaterial.MaterialGroupId = this.treeBoxValueMaterialGroup;
                    this.modelMaterial.Status = '0';
                    this.messageService.showSuccess('Thêm mới vật tư thành công!');
                  }
                  else {
                    this.messageService.showSuccess('Thêm mới vật tư thành công!');
                    this.closeModal(true);
                  }
                },
                error => {
                  this.ListFileDataSheet = [];
                  this.ListFileDesign3D = [];
                  this.modelMaterial.ListFileDataSheet = [];
                  this.modelMaterial.ListFileDesign3D = [];
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

  save(isContinue: boolean) {
    if (this.Id) {
      //this.updateManufacture();
    }
    else {
      this.createMaterial(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  showCreateCode(Id: string) {
    if (this.modelMaterial.ManufactureId) {
      let activeModal = this.modalService.open(CreateNsmaterialCodeModalComponent, { container: 'body', windowClass: 'create-nsmaterial-code-modal', backdrop: 'static' });
      activeModal.componentInstance.manufactureId = this.modelMaterial.ManufactureId;
      activeModal.result.then((result) => {
        if (result) {
          this.modelMaterial.Code = result;
        }
      }, (reason) => {
      });
    }
    else {
      this.messageService.showMessage('Bạn chưa chọn Hãng sản xuất');
    }
  }


  DownloadAFile(path: string) {
    if (!path) {
      this.messageService.showError("Không có file để tải");
    }
    this.materialService.DownloadAFile({ Path: path }).subscribe(() => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerApi + path;
      link.download = 'Download.zip';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, error => {
      this.messageService.showError(error);
    });
  }

  Valid: boolean;
  check() {
    if (this.modelMaterial.MaterialGroupId != '' && this.modelMaterial.MaterialGroupId != undefined &&
      this.modelMaterial.Name != '' && this.modelMaterial.Name != undefined &&
      this.modelMaterial.Code != '' && this.modelMaterial.Code != undefined &&
      this.modelMaterial.ManufactureId != '' && this.modelMaterial.ManufactureId != undefined &&
      this.modelMaterial.UnitId != '' && this.modelMaterial.UnitId != undefined &&
      this.modelMaterial.MaterialType != '' && this.modelMaterial.MaterialType != undefined) {
      this.Valid = true;
    } else {
      this.Valid = false;
    }
  }

  syncTreeViewSelection(e) {
    var component = (e && e.component) || (this.treeView && this.treeView.instance);
  }
  onRowDblClick() {
    this.isDropDownBoxOpened = false;

  }
  treeView_itemSelectionChanged(e) {
    this.treeBoxValue = e.selectedRowKeys;
    this.modelMaterial.ListModuleGroupId = e.selectedRowKeys;
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

  materialTypeOnchange() {
    if (this.modelMaterial.MaterialType == '1') {
      this.modelMaterial.Is3D = true;
      this.modelMaterial.IsDataSheet = true;
    }
    this.check();
  }

}
