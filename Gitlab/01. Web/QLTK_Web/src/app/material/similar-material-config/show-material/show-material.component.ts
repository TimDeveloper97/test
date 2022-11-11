import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';

import { DxTreeListComponent } from 'devextreme-angular';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';

import { FileProcess, Configuration, AppSetting } from 'src/app/shared';
import { SimilarMaterialConfigService } from '../../services/similar-material-config.service';

@Component({
  selector: 'app-show-material',
  templateUrl: './show-material.component.html',
  styleUrls: ['./show-material.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ShowMaterialComponent implements OnInit {

  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    private activeModal: NgbActiveModal,
    private materialService: SimilarMaterialConfigService,
    public fileProcess: FileProcess,
    public fileProcessDataSheet: FileProcess,
    private config: Configuration,
    public fileProcessImage: FileProcess,
    public appSetting: AppSetting,
  ) { }

  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };
  isAction: boolean = false;
  Id: string;
  myFiles: any = [];

  materialmodel: any = {
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
    Pricing: '',
    DeliveryDays: '',
    ImagePath: '',
    ThumbnailPath: '',
    LastBuyDate: '',
    IsUsuallyUse: false,
    MaterialType: '',
    RawMaterialId: '',
    MaterialDesign3DId: '',
    MaterialDataSheetId: '',
    Is3D: false,
    IsDataSheet: false,
    IsSetup: false,
    RawMaterialName: '',
    MaterialGroupName: '',
    MechanicalType: '',
    Status: '',
    Weight: '',
    ModuleGroupId: '',
    RawMaterial: '',

    ListMaterialParameter: [],
    ListFileDesign3D: [],
    ListFileDataSheet: [],
    ListMaterialPart: []

  }

  fileImage = {
    Id: '',
    MaterialId: '',
    Path: '',
    ThumbnailPath: ''
  }
  
  ngOnInit() {
    this.materialmodel.Id = this.Id;
    if (this.Id) {
      this.getById();
      this.Valid = true;
    }

    this.galleryOptions = [
      {
        width: '100%',
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide
      },
      // max-width 800
      {
        breakpoint: 800,
        width: '100%',
        imagePercent: 80,
        thumbnailsPercent: 20,
        thumbnailsMargin: 20,
        thumbnailMargin: 20
      },
      // max-width 400
      {
        breakpoint: 400,
        preview: false
      }
    ];
  }

  getById() {
    this.materialService.getMaterialInfo(this.materialmodel).subscribe(data => {
      this.materialmodel = data;
      for (var item of data.ListImage) {
        this.galleryImages.push({
          small: this.config.ServerFileApi + item.ThumbnailPath,
          medium: this.config.ServerFileApi + item.Path,
          big: this.config.ServerFileApi + item.Path
        });
      }
    });
  }

  //preview ảnh
  ListImage = [];
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[] = [];

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  Valid: boolean;
}
