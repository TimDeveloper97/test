import { Component, OnInit, ViewChild, ElementRef, ViewEncapsulation } from '@angular/core';
import { MessageService, Configuration, FileProcess, ComboboxService, Constants } from 'src/app/shared';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { ProjectProductBomService } from '../../service/project-product-bom.service';
import { ProjectGeneralDesignService } from '../../service/project-general-design.service';

@Component({
  selector: 'app-project-material-three-table-compare',
  templateUrl: './project-material-three-table-compare.component.html',
  styleUrls: ['./project-material-three-table-compare.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProjectMaterialThreeTableCompareComponent implements OnInit {
  modalInfo = {
    Title: 'Danh sách vật tư',
    SaveText: 'Lưu',
  };
  totalAmount : number =0;
  listData : any[];
  listData1 : any[];
  listData2 : any[];
  ProjectProductId :string;
  ModuleId : string;
  height =0;
  isAction: boolean = false;

  ModuleProjectProducts : any[];

  model: any = {
    ListModule : [],
    ModuleId :'',
    ProjectProductId :''
  }

  ListResult : any[] ;
  ListResult1 : any[] ;
  ListResult2 : any[] ;

  Material :{
    Type : number,
    ListResult :any[];
  }
  ModuleMaterials :any[]=[];

  constructor(
    public constant: Constants,
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
    private config: Configuration,
    private uploadService: UploadfileService,
    public fileProcess: FileProcess,
    private service: ProjectProductBomService,
    private combobox: ComboboxService,
    private modalService: NgbModal,
    private serviceA: ProjectGeneralDesignService,

  ) { }

  @ViewChild('scrollPlanDesignProject',{static:false}) scrollPlanDesignProject: ElementRef;
  @ViewChild('scrollPlanDesignProject1',{static:false}) scrollPlanDesignProject1: ElementRef;
  @ViewChild('scrollPlanDesignProject2',{static:false}) scrollPlanDesignProject2: ElementRef;

  @ViewChild('scrollPlanDesignProjectHeader',{static:false}) scrollPlanDesignProjectHeader: ElementRef;
  @ViewChild('scrollPlanDesignProjectHeader1',{static:false}) scrollPlanDesignProjectHeader1: ElementRef;
  @ViewChild('scrollPlanDesignProjectHeader2',{static:false}) scrollPlanDesignProjectHeader2: ElementRef;


  ngOnInit(): void {
    this.listData;
    this.listData1;
    this.listData2;
    this.model.ListModule = this.ModuleProjectProducts;
    this.model.ProjectProductId = this.ProjectProductId;
    this.model.ModuleId = this.ModuleId;
    this.editData();
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }
  ngAfterViewInit() {    

    if (this.scrollPlanDesignProject && this.scrollPlanDesignProject.nativeElement) {
      this.scrollPlanDesignProject.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
        this.scrollPlanDesignProjectHeader.nativeElement.scrollLeft = event.target.scrollLeft;
      }, true);
    }
    if (this.scrollPlanDesignProject1 && this.scrollPlanDesignProject1.nativeElement) {
      this.scrollPlanDesignProject1.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
        this.scrollPlanDesignProjectHeader1.nativeElement.scrollLeft = event.target.scrollLeft;
      }, true);
    }
    if (this.scrollPlanDesignProject2 && this.scrollPlanDesignProject2.nativeElement) {
      this.scrollPlanDesignProject2.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
        this.scrollPlanDesignProjectHeader2.nativeElement.scrollLeft = event.target.scrollLeft;
      }, true);
    }
  }

  ngOnDestroy() {
    if (this.scrollPlanDesignProject && this.scrollPlanDesignProject.nativeElement) {
      this.scrollPlanDesignProject.nativeElement.removeEventListener('ps-scroll-x', null);
    }
    if (this.scrollPlanDesignProject1 && this.scrollPlanDesignProject1.nativeElement) {
      this.scrollPlanDesignProject1.nativeElement.removeEventListener('ps-scroll-x', null);
    }
    if (this.scrollPlanDesignProject2 && this.scrollPlanDesignProject2.nativeElement) {
      this.scrollPlanDesignProject2.nativeElement.removeEventListener('ps-scroll-x', null);
    }
  }

  save() {
    this.serviceA.UpdateMaterialImportBOM(this.model).subscribe(
      data => {
        this.closeModal(true)
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  editData(){
    var moduleIndex=0;
    this.model.ListModule.forEach(mpp => {
      var stt1 ='';
      var stt2 ='';
      var stt3 ='';
      var ma1='';
      var ma2 ='';
      var ma3 ='';
      var hang1 ='';
      var hang2 ='';
      var hang3 ='';
      var sl1 = '';
      var sl2 = '';
      var sl3 = '';
      var dv1 ='';
      var dv2 ='';
      var dv3 ='';
      var length1 =0;
      var length2 =0;
      var length3 =0;
      var list1 :any[] =[];
      var list2 :any[] =[];
      var list3 :any[] =[];
      this.listData.forEach(element =>{
        if(mpp.ModuleId == element.ModuleId){
          list1.push(element);
          length1++;
        }
      });
      this.listData1.forEach(element =>{
        if(mpp.ModuleId == element.ModuleId){
          list2.push(element);
          length2++;
        }
      });
      this.listData2.forEach(element =>{
        if(mpp.ModuleId == element.ModuleId){
          list3.push(element);
          length3++;
        }
      });
      var max = Math.max(length1,length2,length3);
      var i = 0;
      for (i = 0; i < max; i++){
        if(list1.length >i){
        stt1 = list1[i].Index;
        ma1 = list1[i].Code;
        hang1 =list1[i].ManufactureCode;
        sl1 =''+list1[i].Quantity;
        dv1 = list1[i].UnitName;
        }else{
        stt1 = '';
        ma1 = '';
        sl1 ='';
        dv1 = '';
        hang1 ='';
        }
        if(list2.length >i){
        stt2 = list2[i].NewIndex;
        ma2 = list2[i].Code;
        hang2 =list2[i].NewManufactureCode;
        sl2 =''+list2[i].NewQuantity;
        dv2 = list2[i].NewUnitName;
        }else{
        stt2 = '';
        ma2 = '';
        hang2 ='';
        sl2 ='';
        dv2 = '';
        }
        if(list3.length >i){
        stt3 = list3[i].THTKIndex;
        ma3 = list3[i].Code;
        hang3 =list3[i].THTKManufactureCode;
        sl3 =''+list3[i].THTKQuantity;
        dv3 = list3[i].THTKUnitName;
        }else{
        stt3 = '';
        ma3 = '';
        hang3 ='';
        sl3 ='';
        dv3 = '';
        }
        var modelMaterial ={
          stt1 :stt1,
          ma1  :ma1,
          hang1:hang1,
          sl1 :sl1,
          dv1 :dv1,
          stt2 :stt2,
          ma2  :ma2,
          hang2:hang2,
          sl2 :sl2,
          dv2 :dv2,
          stt3 :stt3,
          ma3  :ma3,
          hang3:hang3,
          sl3 :sl3,
          dv3 :dv3,
        }
        this.ModuleMaterials.push(modelMaterial);
      }
      if(this.ModuleMaterials.length > 0){
        this.ModuleMaterials[moduleIndex].ModuleCode = mpp.ModuleCode;
      }
      moduleIndex =moduleIndex+max;
    });
  }
}
