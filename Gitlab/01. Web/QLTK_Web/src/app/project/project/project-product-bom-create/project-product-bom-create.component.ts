import { Component, OnInit, ViewChild, ElementRef, ViewEncapsulation } from '@angular/core';
import { MessageService, Configuration, FileProcess, ComboboxService, Constants } from 'src/app/shared';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { ProjectProductBomService } from '../../service/project-product-bom.service';
import { ProjectProductMaterialCompareComponent } from '../project-product-material-compare/project-product-material-compare.component';
import { MaterialService } from 'src/app/material/services/material-service';
import { ProjectProductService } from '../../service/project-product.service';

@Component({
  selector: 'app-project-product-bom-create',
  templateUrl: './project-product-bom-create.component.html',
  styleUrls: ['./project-product-bom-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProjectProductBomCreateComponent implements OnInit {

  modalInfo = {
    Title: 'Chọn file import dữ liệu'
  }

  importModel: any = {
    Path: ''
  }
  fileTemplateElectric = this.config.ServerApi + 'Template/ImportMaterialElectric_Template.xlsm';
  fileTemplateManufacture = this.config.ServerApi + 'Template/ImportMaterialManuFacture_Template.xlsm';
  fileTemplateTPA = this.config.ServerApi + 'Template/ImportMaterialTPA_Template.xlsm';
  fileTemplateOther = this.config.ServerApi + 'Template/ImportMaterialOther_Template.xlsm';
  fileTemplateMaterial = this.config.ServerApi + 'Template/MaVatTu_Import_Template.xlsx';
  columnName: any[] = [{ Name: 'Code', Title: 'Mã Module' }, { Name: 'Name', Title: 'Tên Module' }];

  projectProductId: string;
  moduleId: string;
  nameFileElictric = "";
  nameFileManufacture = "";
  nameFileTPA = "";
  nameFileOther = "";
  nameFileMaterial = "";
  fileToUploadElictric: File = null;
  fileToUploadManufacture: File = null;
  fileToUploadTPA: File = null;
  fileToUploadBulong: File = null;
  fileToUploadOther: File = null;
  fileToUploadMaterial: File = null;
  listFile: File[] = [];
  listFileImport: any[] = [];
  isAction: boolean = false;
  listModule: any[] = [];
  listDA: any[] = [];
  projectId : string;

  model: any = {
    Id: '',
    ModuleId: '',
    ProjectProductId: '',
    Version: 0,
    ListFile: []
  }

  fileModel = {
    Id: '',
    Path: '',
    FileName: '',
    FileSize: '',
  }

  fileImportModel: any = {
    Index: 0,
    File: null
  }

  @ViewChild('fileInput', { static: false }) myInputVariable: ElementRef;
  @ViewChild('fileInputElictric', { static: false }) myfileInputElictric: ElementRef;
  @ViewChild('fileInputManufacture', { static: false }) myfileInputManufacture: ElementRef;
  @ViewChild('fileInputTPA', { static: false }) myfileInputTPA: ElementRef;
  @ViewChild('fileInputBulong', { static: false }) myfileInputBulong: ElementRef;
  @ViewChild('fileInputOther', { static: false }) myfileInputOther: ElementRef;
  @ViewChild('fileInputMaterial', { static: false }) myfileInputMaterial: ElementRef;

  @ViewChild('scrollPlanDesignProject',{static:false}) scrollPlanDesignProject: ElementRef;
  @ViewChild('scrollPlanDesignProject1',{static:false}) scrollPlanDesignProject1: ElementRef;

  @ViewChild('scrollPlanDesignProjectHeader',{static:false}) scrollPlanDesignProjectHeader: ElementRef;
  @ViewChild('scrollPlanDesignProjectHeader1',{static:false}) scrollPlanDesignProjectHeader1: ElementRef;

  templatePath: string = '';
  listData: any[] =[];
  listData1 : any[] =[];

  constructor(
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
    private config: Configuration,
    private uploadService: UploadfileService,
    public fileProcess: FileProcess,
    private service: ProjectProductBomService,
    private combobox: ComboboxService,
    private modalService: NgbModal,
    private materialService: MaterialService,
    private serviceA: ProjectProductService,
    public constant: Constants,
  ) { }

  ngOnInit() {
    this.model.ProjectProductId = this.projectProductId;
    this.model.ModuleId = this.moduleId;
    this.getListModule();
  }

  getVersion() {
    this.service.getVersion(this.model).subscribe(data => {
      this.model.Version = data;
    }, error => {
      this.messageService.showError(error);
    });
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
  }

  ngOnDestroy() {
    if (this.scrollPlanDesignProject && this.scrollPlanDesignProject.nativeElement) {
      this.scrollPlanDesignProject.nativeElement.removeEventListener('ps-scroll-x', null);
    }
    if (this.scrollPlanDesignProject1 && this.scrollPlanDesignProject1.nativeElement) {
      this.scrollPlanDesignProject1.nativeElement.removeEventListener('ps-scroll-x', null);
    }
  }
  
  handleFileInput(files: FileList, index: number) {
    if (index == 1) {
      this.nameFileElictric = files.item(0).name;
      this.fileToUploadElictric = files.item(0);
      var fileimport = Object.assign({}, this.fileImportModel);
      fileimport.Index = 1;
      fileimport.File = this.fileToUploadElictric;
      this.listFileImport.push(fileimport);
    } else if (index == 2) {
      this.nameFileManufacture = files.item(0).name;
      this.fileToUploadManufacture = files.item(0);
      var fileimport = Object.assign({}, this.fileImportModel);
      fileimport.Index = 2;
      fileimport.File = this.fileToUploadManufacture;
      this.listFileImport.push(fileimport);
    }
    else if (index == 3) {
      this.nameFileTPA = files.item(0).name;
      this.fileToUploadTPA = files.item(0);
      var fileimport = Object.assign({}, this.fileImportModel);
      fileimport.Index = 3;
      fileimport.File = this.fileToUploadTPA;
      this.listFileImport.push(fileimport);
    }
    else if (index == 4) {
      this.nameFileOther = files.item(0).name;
      this.fileToUploadBulong = files.item(0);
      var fileimport = Object.assign({}, this.fileImportModel);
      fileimport.Index = 4;
      fileimport.File = this.fileToUploadBulong;
      this.listFileImport.push(fileimport);
    }
    else if (index == 5) {
      this.nameFileOther = files.item(0).name;
      this.fileToUploadOther = files.item(0);
      var fileimport = Object.assign({}, this.fileImportModel);
      fileimport.Index = 5;
      fileimport.File = this.fileToUploadOther;
      this.listFileImport.push(fileimport);
    } else if (index == 6) {
      this.nameFileMaterial = files.item(0).name;
      this.fileToUploadMaterial = files.item(0);
      var fileimport = Object.assign({}, this.fileImportModel);
      fileimport.Index = 6;
      fileimport.File = this.fileToUploadMaterial;
      this.listFileImport.push(fileimport);
      var isExit = false;
      var confirm = false;
      this.materialService.importFileMateria(files.item(0), this.model.ProjectProductId, this.model.ModuleId, isExit, confirm).subscribe(
        data => {
          if (data.IsExit) {
            // this.closeModal(true);
            this.listData = data.oldMaterialChangeModels;
            this.listData1 = data.newMaterialChangeModels;
          }
        },
        error => {
          this.closeModal(true);
          this.messageService.showError(error);
        });
    }
    this.listFile.push(files.item(0));
  }

  saveAndContinue() {
    this.save(true);
  }

  save(isContinue) {
    this.uploadService.uploadListFile(this.listFile, 'FileBOM/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach(item => {
          var file = Object.assign({}, this.fileModel);
          file.FileName = item.FileName;
          file.FileSize = item.FileSize;
          file.Path = item.FileUrl;
          this.model.ListFile.push(file);
        });
      }
      this.listFileImport.forEach(element => {
        if (element.Index == 6) {
          this.materialService.importFileMateria(element.File, this.model.ProjectProductId, this.model.ModuleId, false, true).subscribe(
            data => {
              this.model.Content = data.Content;
              this.service.importListFile(this.listFileImport, this.model).subscribe(
                data => {
                  if (isContinue) {
                    this.isAction = true;
                    this.model = {
                      Id: '',
                      ProjectProductId: '',
                      Version: 0,
                      ListFile: []
                    };
                    this.model.ProjectProductId = this.projectProductId;
                    this.nameFileElictric = "";
                    this.nameFileManufacture = "";
                    this.nameFileTPA = "";
                    this.nameFileOther = "";
                    this.nameFileMaterial = "";
                    this.fileToUploadElictric = null;
                    this.fileToUploadManufacture = null;
                    this.fileToUploadTPA = null;
                    this.fileToUploadBulong = null;
                    this.fileToUploadOther = null;
                    this.fileToUploadMaterial = null;
                    this.listFile = [];
                    this.listFileImport = [];
                    this.myfileInputElictric.nativeElement.value = '';
                    this.myfileInputManufacture.nativeElement.value = '';
                    this.myfileInputTPA.nativeElement.value = '';
                    this.myfileInputBulong.nativeElement.value = '';
                    this.myfileInputOther.nativeElement.value = '';
                    this.myfileInputMaterial.nativeElement.value = '';
                    this.messageService.showSuccess('Import file BOM thành công!');
                  }
                  else {
                    this.messageService.showSuccess('Import file BOM thành công!');
                    this.closeModal(true);
                  }
                  var view = {
                    Id: '',
                    ProjectId: this.projectId,
                    ContractCode: '',
                    ContractName: '',
                    Code: '',
                    Name: '',
                    DataType: 0,
                    ModuleStatus: 0,
                    DesignStatus: 0,
                    CostType: null,
                    MaterialName: null

                  };
                  this.serviceA.searchProjectProduct(view).subscribe((data: any) => {
                    if (data.ListResult) {
                      this.listDA = data.ListResult;
                    }
                  },
                    error => {
                      this.messageService.showError(error);
                    });
                },
                error => {
                  this.messageService.showError(error);
                });
            },
            error => {
              this.messageService.showError(error);
            });
        }
      });

    }, error => {
      this.messageService.showError(error);
    });

  }

  getListModule() {
    this.combobox.getListModule().subscribe((data: any) => {
      this.listModule = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  download(Index: number) {
    if (Index == 1) {
      this.dowloadFileTemplate(this.fileTemplateElectric);
    } else if (Index == 2) {
      this.dowloadFileTemplate(this.fileTemplateManufacture);
    } else if (Index == 3) {
      this.dowloadFileTemplate(this.fileTemplateTPA);
    } else if (Index == 4) {
      this.dowloadFileTemplate(this.fileTemplateOther);
    } else if (Index == 5) {
      this.dowloadFileTemplate(this.fileTemplateOther);
    } else if (Index == 6) {
      this.dowloadFileTemplate(this.fileTemplateMaterial);
    }
  }

  dowloadFileTemplate(fileTemplate: string) {
    var link = document.createElement('a');
    link.setAttribute("type", "hidden");
    link.href = fileTemplate;
    link.download = 'Download.zip';
    document.body.appendChild(link);
    // link.focus();
    link.click();
    document.body.removeChild(link);
  }

}
