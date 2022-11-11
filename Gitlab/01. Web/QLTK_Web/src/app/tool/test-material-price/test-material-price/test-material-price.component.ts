import { Component, OnInit, OnDestroy, ViewChild, ElementRef, ViewEncapsulation , AfterViewInit} from '@angular/core';
import { MaterialService } from 'src/app/material/services/material-service';
import { Constants, MessageService, AppSetting, Configuration, ComponentService } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-test-material-price',
  templateUrl: './test-material-price.component.html',
  styleUrls: ['./test-material-price.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TestMaterialPriceComponent implements OnInit, OnDestroy, AfterViewInit {

  constructor(
    public config: Configuration,
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private materialService: MaterialService,
    public constant: Constants,
    private componentService: ComponentService

  ) { }

  StartIndex = 1;
  listData: any[] = [];
  model: any = {
    TotalItem: 0,
    TotalAmount: 0,
    ListResult: []
  }
  height = 300;
  selectIndex = -1;
  @ViewChild('scrollMaterial',{static:false}) scrollMaterial: ElementRef;
  @ViewChild('scrollHeaderOne',{static:false}) scrollHeaderOne: ElementRef;

  ngOnInit() {
    this.appSetting.PageTitle = "Kiểm tra giá vật tư";

    this.height = window.innerHeight - 230;
  }

  ngAfterViewInit(){
    this.scrollMaterial.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollHeaderOne.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollMaterial.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  clear() {
    this.model = {
      TotalItem: 0,
      TotalAmount: 0,
      ListResult: []
    }
  }

  importModel: any = {
    Path: ''
  }
  fileTemplateMaterial = '';

  showImportMaterialPopup() {
    //this.materialService.templateMaterialPrice().subscribe(d => {
    this.fileTemplateMaterial = this.config.ServerApi + 'Template/Template_Material_Price.xlsm';
    this.componentService.showImportExcel(this.fileTemplateMaterial, false).subscribe(file => {
      if (file) {
        this.selectIndex = -1;
        this.materialService.checkPriceMaterial(file).subscribe(data => {
          if (data) {
            this.model = data;
            this.messageService.showSuccess('Import vật tư thành công!');
          }
        },
          error => {
            this.model = {
              TotalItem: 0,
              TotalAmount: 0,
              ListResult: []
            }
            this.messageService.showError(error);
          });
      }
    });
    // }, e => {
    //   this.messageService.showError(e);
    // });
  }

  exportExcel() {
    this.materialService.exportCheckPrice(this.model.ListResult).subscribe(d => {
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

  selectRow(index) {
    this.selectIndex = index;
  }

}
