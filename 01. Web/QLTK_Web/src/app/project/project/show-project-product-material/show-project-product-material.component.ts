import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ModuleShowSimilarMaterialComponent } from 'src/app/module/Module/module-show-similar-material/module-show-similar-material.component';
import { ModuleServiceService } from 'src/app/module/services/module-service.service';
import { AppSetting, Configuration, Constants, FileProcess, MessageService } from 'src/app/shared';
import { ProjectProductBomService } from '../../service/project-product-bom.service';

@Component({
  selector: 'app-show-project-product-material',
  templateUrl: './show-project-product-material.component.html',
  styleUrls: ['./show-project-product-material.component.scss']
})
export class ShowProjectProductMaterialComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private config: Configuration,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public constants: Constants,
    public appset: AppSetting,
    private projectProductService: ProjectProductBomService,
    private serviceModuleMaterial: ModuleServiceService) { }

  @Input() ModuleId: string;
  @Input() ParentModuleId: string;
  @Input() ContractCode: string = "";
  @Input() ProjectProductId: string;

  IsParent: boolean = false;
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
    ManufacturerCode: '',
    IsParent: false,
    ContractCode: '',
    ProductProjectId: ''
  }

  totalAmount = 0;
  MaxDeliveryDay = 0;
  selectIndex = -1;
  height = 0;

  listData: any[] = [];

  @ViewChild('scrollHeaderOne') scrollHeaderOne: ElementRef;

  ngOnInit(): void {
    window.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollHeaderOne.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.height = window.innerHeight - 330;



    this.searchModuleMaterial();
  }

  ngOnDestroy() {
    window.removeEventListener('ps-scroll-x', null);
  }

  searchModuleMaterial() {
    if (this.ModuleId != null && this.ModuleId != "") {
      this.modelModuleMaterial.ModuleId = this.ModuleId;
      this.IsParent = false;
    } else if (this.ParentModuleId != null && this.ParentModuleId != "") {
      this.modelModuleMaterial.ModuleId = this.ParentModuleId;
      this.IsParent = true;
    }
    this.modelModuleMaterial.ProjectProductId = this.ProjectProductId;
    this.modelModuleMaterial.IsParent = this.IsParent;
    this.modelModuleMaterial.ContractCode = this.ContractCode;
    this.projectProductService.searchModuleMaterial(this.modelModuleMaterial).subscribe((data: any) => {
      if (data.ListResult) {
        this.listData = data.ListResult;
        var totalPrice =0;
        var resultModels =[];
        var resultModel ={
          parentIndex:'',
          parentQuantity : 0,
          parentPrice : 0,
          results :[],
        }
        this.listData.forEach(element =>{

          if(element.Index.indexOf(".") == -1){
            var resultModel ={
              parentIndex:element.Index,
              parentQuantity : element.Quantity,
              parentTotalPrice : element.ReadQuantity*element.Pricing,
              results :[],
            }
            resultModels.push(resultModel);
          }
        });
        this.listData.forEach(element =>{
          resultModels.forEach(e =>{
            if(element.Index.indexOf(".") != -1 && element.Index.substring(0, element.Index.indexOf(".")) == e.parentIndex){
              e.results.push(element);
            }
          });
        });

        resultModels.forEach(element =>{
          this.listData.forEach(e =>{
            if(e.Index == element.parentIndex && element.results.length > 0){
              var price =0;
              element.results.forEach(r =>{
                price = price + (r.ReadQuantity*r.Pricing);
              });
              e.Pricing = price;
            }
          });
        });

        resultModels.forEach(element =>{
          var total =0;
          element.results.forEach(e =>{
            total = total + (e.ReadQuantity*e.Pricing);
          });
          totalPrice = totalPrice + element.parentQuantity * total ;
          if(total == 0){
            totalPrice += element.parentTotalPrice;
          }
        });
        this.totalAmount = totalPrice;
        this.MaxDeliveryDay = data.MaxDeliveryDay;
        ;
      }
    },
      error => {
        this.messageService.showError(error);
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
      Id: '',
      ModuleId: '',
      MaterialId: '',
      MaterialCode: '',
      MaterialName: '',
      Check: '',
      ManufacturerCode: '',
      IsParent: false,
      ContractCode: '',
      ProductProjectId: ''
    }
    this.modelModuleMaterial.ModuleId = this.ModuleId;
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

  selectRow(index) {
    this.selectIndex = index;
  }


}
