import { Component, OnInit, Input, ViewChild, ElementRef, ViewEncapsulation } from '@angular/core';

import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { AppSetting, Constants, FileProcess, MessageService, Configuration } from 'src/app/shared';
import { ModuleShowSimilarMaterialComponent } from '../module-show-similar-material/module-show-similar-material.component';
import { ModuleServiceService } from '../../services/module-service.service';

@Component({
  selector: 'app-module-material-tab',
  templateUrl: './module-material-tab.component.html',
  styleUrls: ['./module-material-tab.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ModuleMaterialTabComponent implements OnInit {
  @Input() Id: string
  constructor(
    private activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private config: Configuration,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public constants: Constants,
    public appset: AppSetting,
    private serviceModuleMaterial: ModuleServiceService
  ) { }
  listData: any[] = [];
  modelModuleMaterial: any = {
    page: 1,
    PageSize: 10,
    TotalItem: 0,
    PageNumber: 1,
    OrderBy: 'MaterialCode',
    OrderType: true,
    Id: '',
    ModuleId: '',
    MaterialId: '',
    MaterialCode: '',
    MaterialName: '',
    Check: '',
  }
  totalAmount = 0;
  MaxDeliveryDay = 0;
  selectIndex = -1;
  height = 0;

  @ViewChild('scrollHeaderOne') scrollHeaderOne: ElementRef;
  ngOnInit() {
    window.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollHeaderOne.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.height = window.innerHeight - 330;
    this.modelModuleMaterial.ModuleId = this.Id;
    this.searchModuleMaterial();
  }

  ngOnDestroy() {
    window.removeEventListener('ps-scroll-x', null);
  }

  searchModuleMaterial() {
    this.serviceModuleMaterial.searchModuleMaterial(this.modelModuleMaterial).subscribe((data: any) => {
      if (data.ListResult) {
        this.listData = data.ListResult;
        this.totalAmount = data.TotalAmount;
        this.MaxDeliveryDay = data.MaxDeliveryDay;
        ;

      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  exportExcel() {
    this.serviceModuleMaterial.exportExcelModuleMaterial(this.modelModuleMaterial).subscribe(d => {
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

  clear() {
    this.modelModuleMaterial = {
      page: 1,
      PageSize: 10,
      TotalItem: 0,
      PageNumber: 1,
      OrderBy: 'MaterialCode',
      OrderType: true,
      MaterialCode: '',
      MaterialName: ''
    }
    this.modelModuleMaterial.ModuleId = this.Id;
    this.searchModuleMaterial();
  }

  showSimilarMaterialConfig(MaterialId: string) {
    let activeModal = this.modalService.open(ModuleShowSimilarMaterialComponent, { container: 'body', windowClass: 'similar-material-model', backdrop: 'static' })
    activeModal.componentInstance.Id = MaterialId;
    activeModal.result.then((result) => {
      if (result) {
        this.searchModuleMaterial();
      }
    }, (reason) => {
    });
  }

  // Down file vật tư cài đặt 
  DownloadAFileSetup(SetupFilePath: string, fileName: string) {
    if (SetupFilePath == null || SetupFilePath == "") {
      this.messageService.showMessage("Không có file để tải");
      return;
    }
    this.fileProcess.downloadFileBlob(SetupFilePath, fileName)
    // else {
    // var link = document.createElement('a');
    // link.setAttribute("type", "hidden");
    // link.href = this.apiURL + SetupFilePath;
    // link.download = "aaaaa";
    // document.body.appendChild(link);
    // link.focus();
    // link.click();
    //}
  }

  selectRow(index) {
    this.selectIndex = index;
  }
}
