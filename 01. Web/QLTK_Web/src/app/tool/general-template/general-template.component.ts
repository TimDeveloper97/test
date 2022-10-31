import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Configuration, MessageService, FileProcess, PermissionService, AppSetting } from 'src/app/shared';
import { GeneralTemplateService } from '../services/general-template.service';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { ChooseFolderFileComponent } from '../choose-folder-file/choose-folder-file.component';
import { Router } from '@angular/router';
import { NgBlockUI, BlockUI } from 'ng-block-ui';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { Subscription, forkJoin } from 'rxjs';
import { TestDesignService } from '../services/test-design-service';
import { ChooseMaterialForTemplateComponent } from '../choose-material-for-template/choose-material-for-template.component';
@ViewChild('fileInput')

@Component({
  selector: 'app-general-template',
  templateUrl: './general-template.component.html',
  styleUrls: ['./general-template.component.scss']
})
export class GeneralTemplateComponent implements OnInit, OnDestroy {
  @BlockUI() blockUI: NgBlockUI;
  constructor(
    private router: Router,
    private activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private config: Configuration,
    private messageService: MessageService,
    private service: GeneralTemplateService,
    private uploadService: UploadfileService,
    public fileProcess: FileProcess,
    private signalRService: SignalRService,
    private testDesignService: TestDesignService,
    public permissionService: PermissionService,
    public appSetting: AppSetting,


  ) { }
  private notiSub: Subscription;
  private onDownload = new BroadcastEventListener<any>('generalDownload');
  private getDownload: Subscription;
  private onCraeteMechanicalProfile = new BroadcastEventListener<any>('onCraeteMechanicalProfile');
  private subCraeteMechanicalProfile: Subscription;
  private loadListDMVT = new BroadcastEventListener<any>('generalDMVT');
  private subCraeteUserNamePC: Subscription;
  private getUserNamePC = new BroadcastEventListener<any>('getUserNamePC');
  userName = JSON.parse(localStorage.getItem('qltkcurrentUser')).userfullname;
  path = '';
  modelTest: any = {
    PathFileMaterial: '',
    List3D: [],
    ListModuleDesignDocument: [],
    ListRawMaterial: [],
    ListMaterialDB: [],
    ListModuleError: [],
    ListManufacture: [],
    ListConvertUnit: [],
    ListDesignStructure: [],
    ListDesignStructureFile: [],
    Module: '',
    ListModule: [],
    ListUnit: [],
    SelectedPath: '',
    ApiUrl: '',
    PathDownload: '',
    ModuleCode: '',
    FileName: '',
    Type: Number
  }

  fileName: any;
  pathFile: '';
  path_File: '';

  ngOnInit() {
    this.appSetting.PageTitle = "Tạo biểu mẫu";

    this.getUerNamePC();
    this.signalRService.listen(this.onDownload, true);
    this.getDownload = this.onDownload.subscribe((data: any) => {
      if (data) {
        this.messageService.showSuccess('Download biểu mẫu thành công!');
      }
      else {
        this.messageService.showMessage('Download biểu mẫu không thành công!');
      }
      this.blockUI.stop();
    });

    this.signalRService.listen(this.onCraeteMechanicalProfile, true);
    this.subCraeteMechanicalProfile = this.onCraeteMechanicalProfile.subscribe((data: any) => {
      if (data) {
        if (data.StatusCode == 1) {
          this.messageService.showSuccess('Download biểu mẫu thành công!');
        }
        else {
          this.messageService.showMessage(data.Message);
        }
      }
      else {
        this.messageService.showMessage('Download biểu mẫu không thành công!');
      }
      this.blockUI.stop();
    });
    this.signalRService.listen(this.loadListDMVT, true);

    this.notiSub = this.loadListDMVT.subscribe((data: any) => {
      if (data) {
        this.listMaterial = data;
        this.exportReportDMVT();
      }
      this.blockUI.stop();
    });

    this.signalRService.listen(this.getUserNamePC, true);
    this.notiSub = this.getUserNamePC.subscribe((data: any) => {
      if (data) {
        this.UserNamePC = data;
      }
      this.blockUI.stop();
    });
  }
  // lấy userNamePC
  UserNamePC = ''
  getUerNamePC(){
    this.signalRService.invoke('GetUserNamePC','ab').subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  ngOnDestroy() {
    this.subCraeteMechanicalProfile.unsubscribe();
    this.getDownload.unsubscribe();
    this.notiSub.unsubscribe();
    this.signalRService.stopListening(this.loadListDMVT);
    this.signalRService.stopListening(this.onCraeteMechanicalProfile);
    this.signalRService.stopListening(this.onDownload);
  }

  //////////////////////////////////////////////////////////////Tab HS mạch điện tử
  //Hồ sơ thiết kế mạch điện tử

  GeneralProfileElectronicDesign() {
    this.service.GeneralProfileElectronicDesign(this.modelProfileElectronicDesign).subscribe(d => {
      if (d) {
        this.modelTest.PathFileMaterial = d;
        this.modelTest.ApiUrl = this.config.ServerApi;
        this.modelTest.ModuleCode = this.modelProfileElectronicDesign.ProductCode;
        this.modelTest.Type = 3;
        this.modelTest.FileName = "HS.";
        this.DowloadTemplateToFolder();
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  ////////////////////////////////////////////////////Tab Hồ sơ thiết kế 

  modelElectronicRecord: any = {
    ProductCode: '',
    Code: '',
    Name: '',
    check_14_4: false,
    check_16_4: false,
    check_17_4: false,
    check_18_4: false,
    check_20_4: false,
    check_22_4: false,
    check_24_4: false,
    check_26_4: false,

    // Hồ sơ cơ khí
    check_14_3: true,
    check_15_3: false,
    check_16_3: true,
    check_17_3: false,
    check_18_3: true,
    check_19_3: true,
    check_20_3: false,
    check_21_3: false,
    check_22_3: false,
    check_23_3: false,
    check_24_3: false,
    check_25_3: false,
    check_26_3: false,
    check_27_3: false,
    check_28_3: false,
    check_29_3: false,
    check_30_3: false,
  }

  // Tạo File hồ sơ điện
  GeneralElectronicRecord() {
    this.service.GeneralElectronicRecord(this.modelElectronicRecord).subscribe(d => {
      if (d) {
        this.modelTest.PathFileMaterial = d;
        this.modelTest.ApiUrl = this.config.ServerApi;
        this.modelTest.ModuleCode = this.modelElectronicRecord.ProductCode;
        this.modelTest.Type = 2;
        this.modelTest.FileName = "HS.";
        this.DowloadTemplateToFolder();
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  // Hồ sơ cơ khí
  modelMechanicalRecord: any = {
    ProductCode: '',
    Code: '',
    Name: '',
    Path: '',
    UserName: '',
    check_14_3: true,
    check_15_3: true,
    check_16_3: true,
    check_17_3: false,
    check_18_3: true,
    check_19_3: true,
    check_20_3: false,
    check_21_3: false,
    check_22_3: false,
    check_23_3: false,
    check_24_3: false,
    check_25_3: false,
    check_26_3: false,
    check_27_3: false,
    check_28_3: false,
    check_29_3: false,
    check_30_3: false,
  }

  // Tạo File hồ sơ cơ khí

  // generalMechanicalRecord() {
  //   this.modelMechanicalRecord.ProductCode = this.modelElectronicRecord.ProductCode;
  //   this.modelMechanicalRecord.Path = this.path;
  //   this.service.GeneralMechanicalRecord(this.modelMechanicalRecord).subscribe(d => {
  //     if (d) {
  //       this.modelTest.PathFileMaterial = d;
  //       this.modelTest.ApiUrl = this.config.ServerApi;
  //       this.modelTest.ModuleCode = this.modelElectronicRecord.ProductCode;
  //       this.modelTest.Type = 1;
  //       this.modelTest.FileName = "HS.";
  //       this.DowloadTemplateToFolder();
  //     }
  //   }, e => {
  //     this.messageService.showError(e);
  //   });
  // }

  //////////////////////////////////////////////////////////////////////Tab Biểu mẫu điện

  modelCheckList: any = {
    ProductCode: '',
    Name: '',
    UserCheck: '',
  }

  // Lập bảng tín hiệu I/O
  GeneralProgramableDatad() {
    this.service.GeneralProgramableData(this.modelCheckList).subscribe(d => {
      if (d) {
        this.modelTest.PathFileMaterial = d;
        this.modelTest.ApiUrl = this.config.ServerApi;
        this.modelTest.ModuleCode = this.modelCheckList.ProductCode;
        this.modelTest.Type = 2;
        this.modelTest.FileName = "BLT.";
        this.DowloadTemplateToFolder();
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  // Vẽ sơ đồ thuật toán điều khiển
  GeneralDrawControlAlgorithmModel() {
    this.service.GeneralDrawControlAlgorithmModel(this.modelCheckList).subscribe(d => {
      if (d) {
        this.modelTest.PathFileMaterial = d;
        this.modelTest.ApiUrl = this.config.ServerApi;
        this.modelTest.ModuleCode = this.modelCheckList.ProductCode;
        this.modelTest.Type = 2;
        this.modelTest.FileName = "TTDK.";
        this.DowloadTemplateToFolder();
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  // Danh sách hàm chức năng
  GeneralListFunction() {
    this.service.GeneralListFunction(this.modelCheckList).subscribe(d => {
      if (d) {
        this.modelTest.PathFileMaterial = d;
        this.modelTest.ApiUrl = this.config.ServerApi;
        this.modelTest.ModuleCode = this.modelCheckList.ProductCode;
        this.modelTest.Type = 2;
        this.modelTest.FileName = "HCN.";
        this.DowloadTemplateToFolder();
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  // Quá trình thử nghiệm
  GeneralTestProcess() {
    this.service.GeneralTestProcess(this.modelCheckList).subscribe(d => {
      if (d) {
        this.modelTest.PathFileMaterial = d;
        this.modelTest.ApiUrl = this.config.ServerApi;
        this.modelTest.ModuleCode = this.modelCheckList.ProductCode;
        this.modelTest.Type = 2;
        this.modelTest.FileName = "KQTN.";
        this.DowloadTemplateToFolder();
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  // Danh mục thiết bị theo chức năng
  GeneralEquipmentByFunction() {
    this.service.GeneralEquipmentByFunction(this.modelCheckList).subscribe(d => {
      if (d) {
        this.modelTest.PathFileMaterial = d;
        this.modelTest.ApiUrl = this.config.ServerApi;
        this.modelTest.ModuleCode = this.modelCheckList.ProductCode;
        this.modelTest.Type = 2;
        this.modelTest.FileName = "CND.";
        this.DowloadTemplateToFolder();
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  // kiểm tra nguyên lý điện
  GeneralCheckPrinciplesElectric() {
    this.service.GeneralCheckPrinciplesElectric(this.modelCheckList).subscribe(d => {
      if (d) {
        this.modelTest.PathFileMaterial = d;
        this.modelTest.ApiUrl = this.config.ServerApi;
        this.modelTest.ModuleCode = this.modelCheckList.ProductCode;
        this.modelTest.Type = 2;
        this.modelTest.FileName = "BKT.";
        this.DowloadTemplateToFolder();
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  // Dữ liệu cài đặt 
  GeneralDataProgramElectric() {
    this.service.GeneralDataProgramElectric(this.modelCheckList).subscribe(d => {
      if (d) {
        this.modelTest.PathFileMaterial = d;
        this.modelTest.ApiUrl = this.config.ServerApi;
        this.modelTest.ModuleCode = this.modelCheckList.ProductCode;
        this.modelTest.Type = 2;
        this.modelTest.FileName = "BCD.";
        this.DowloadTemplateToFolder();
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  // Hạng mục thiết kế 
  GeneralDesignArticleElectric() {
    this.service.GeneralDesignArticleElectric(this.modelCheckList).subscribe(d => {
      if (d) {
        this.modelTest.PathFileMaterial = d;
        this.modelTest.ApiUrl = this.config.ServerApi;
        this.modelTest.ModuleCode = this.modelCheckList.ProductCode;
        this.modelTest.Type = 2;
        this.modelTest.FileName = "MTK.";
        this.DowloadTemplateToFolder();
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  // downLoadAllBMD
  downLoadAllBMD() {
    if (!this.permissionService.checkPermission(['F090301'])) {
      this.GeneralProgramableDatad();
    }
    if (!this.permissionService.checkPermission(['F090302'])) {
      this.GeneralDrawControlAlgorithmModel();
    }
    if (!this.permissionService.checkPermission(['F090303'])) {
      this.GeneralListFunction();
    }
    if (!this.permissionService.checkPermission(['F090304'])) {
      this.GeneralTestProcess();
    }
    if (!this.permissionService.checkPermission(['F090305'])) {
      this.GeneralEquipmentByFunction();
    }
    if (!this.permissionService.checkPermission(['F090306'])) {
      this.GeneralDataProgramElectric();
    }
    if (!this.permissionService.checkPermission(['F090307'])) {
      this.GeneralCheckPrinciplesElectric();
    }
    if (!this.permissionService.checkPermission(['F090308'])) {
      this.GeneralDesignArticleElectric();
    }
  }

  ////////////////////////////////////////////////////// Tab Biểu Mẫu Điện Tử

  model: any = {
    ProductCode: '',
    Approve: '',
    Check: '',
    Paint: '',
    Code: ''
  }

  //Biểu mẫu kiểm tra bản vẽ mạch in
  GeneralElectronic() {
    this.service.GeneralElectronic(this.model).subscribe(d => {
      if (d) {
        this.modelTest.PathFileMaterial = d;
        this.modelTest.ApiUrl = this.config.ServerApi;
        this.modelTest.ModuleCode = this.model.ProductCode;
        this.modelTest.Type = 3;
        this.modelTest.FileName = "KTMI.";
        this.DowloadTemplateToFolder();
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  // Biểu mẫu kiểm tra nguyên lý
  GeneralPrinciples() {
    this.service.GeneralPrinciples(this.model).subscribe(d => {
      if (d) {
        this.modelTest.PathFileMaterial = d;
        this.modelTest.ApiUrl = this.config.ServerApi;
        this.modelTest.ModuleCode = this.model.ProductCode;
        this.modelTest.Type = 3;
        this.modelTest.FileName = "KTNL.";
        this.DowloadTemplateToFolder();
      }
    });
  }

  // Biểu mẫu bản vẽ nguyên lý - Bảng tính toán
  GeneralPrinciplesCalculate() {
    this.service.GeneralPrinciplesCalculate(this.model).subscribe(d => {
      if (d) {
        this.modelTest.PathFileMaterial = d;
        this.modelTest.ApiUrl = this.config.ServerApi;
        this.modelTest.ModuleCode = this.model.ProductCode;
        this.modelTest.Type = 3;
        this.modelTest.FileName = "TTNL.";
        this.DowloadTemplateToFolder();
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  // Kiểm tra chất lượng sản phẩm mạch điện tử
  GeneralCheckElectronic() {
    this.service.GeneralCheckElectronic(this.model).subscribe(d => {
      if (d) {
        this.modelTest.PathFileMaterial = d;
        this.modelTest.ApiUrl = this.config.ServerApi;
        this.modelTest.ModuleCode = this.model.ProductCode;
        this.modelTest.Type = 3;
        this.modelTest.FileName = "KTCL.";
        this.DowloadTemplateToFolder();
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  // Biểu mẫu hướng dẫn lắp ráp mạch điện tử
  GeneralElectronicCircuitAssembly() {
    this.service.GeneralElectronicCircuitAssembly(this.model).subscribe(d => {
      if (d) {
        this.modelTest.PathFileMaterial = d;
        this.modelTest.ApiUrl = this.config.ServerApi;
        this.modelTest.ModuleCode = this.model.ProductCode;
        this.modelTest.Type = 3;
        this.modelTest.FileName = "LR.";
        this.DowloadTemplateToFolder();
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  // Biểu mẫu vật tư
  GeneralMaterial() {
    this.service.GeneralMaterial(this.model).subscribe(d => {
      if (d) {
        this.modelTest.PathFileMaterial = d;
        this.modelTest.ApiUrl = this.config.ServerApi;
        this.modelTest.ModuleCode = this.model.ProductCode;
        this.modelTest.Type = 3;
        this.modelTest.FileName = "VT.";
        this.DowloadTemplateToFolder();
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  // Phương án thiết kế - Mô tả chung - Sơ đồ khối
  GeneralDesignOptions() {
    this.service.GeneralDesignOptions(this.model).subscribe(d => {
      if (d) {
        this.modelTest.PathFileMaterial = d;
        this.modelTest.ApiUrl = this.config.ServerApi;
        this.modelTest.ModuleCode = this.model.ProductCode;
        this.modelTest.Type = 3;
        this.modelTest.FileName = "PATK-C.";
        this.DowloadTemplateToFolder();
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  // Phương án thiết kế - Danh mục khối chức năng
  GeneralFunctionDesignOptions() {
    this.service.GeneralFunctionDesignOptions(this.model).subscribe(d => {
      if (d) {
        this.modelTest.PathFileMaterial = d;
        this.modelTest.ApiUrl = this.config.ServerApi;
        this.modelTest.ModuleCode = this.model.ProductCode;
        this.modelTest.Type = 3;
        this.modelTest.FileName = "PATK-CN.";
        this.DowloadTemplateToFolder();
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  // Phương án thiết kế - Linh kiện chính, thông số mạch
  GeneralFunctionDesignMaterial() {
    this.service.GeneralFunctionDesignMaterial(this.model).subscribe(d => {
      if (d) {
        this.modelTest.PathFileMaterial = d;
        this.modelTest.ApiUrl = this.config.ServerApi;
        this.modelTest.ModuleCode = this.model.ProductCode;
        this.modelTest.Type = 3;
        this.modelTest.FileName = "PATK-LK.";
        this.DowloadTemplateToFolder();
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  downLoadAllBMDT() {
    if (!this.permissionService.checkPermission(['F090101'])) {
      this.GeneralPrinciples();
    }
    if (!this.permissionService.checkPermission(['F090102'])) {
      this.GeneralPrinciplesCalculate();
    }
    if (!this.permissionService.checkPermission(['F090103'])) {
      this.GeneralFunctionDesignOptions();
    }
    if (!this.permissionService.checkPermission(['F090104'])) {
      this.GeneralFunctionDesignMaterial();
    }
    if (!this.permissionService.checkPermission(['F090105'])) {
      this.GeneralCheckElectronic();
    }
    if (!this.permissionService.checkPermission(['F090106'])) {
      this.GeneralElectronicCircuitAssembly();
    }
    if (!this.permissionService.checkPermission(['F090107'])) {
      this.GeneralDesignOptions();
    }
    if (!this.permissionService.checkPermission(['F090108'])) {
      this.GeneralMaterial();
    }
    if (!this.permissionService.checkPermission(['F090109'])) {
      this.GeneralElectronic();
    }
  }


  //////////////////////////////////Tab Biểu Mẫu cơ khí

  modelMechanical: any = {
    ProductCode: '',
    Responsible: '',
    TechnologicalScheme: '',
    FaceModule: '',
    RelationshipClusters: '',
    ImageLink: '',
    IsCreateThumb: true,
    materials: [],
    UserNamePC:''
  }

  fileModel: any = {};
  localUrl: any[];

  // Kiểm tra phương án thiết kê
  CheckDesignPlan() {
    this.service.CheckDesignPlan(this.modelMechanical).subscribe(d => {
      if (d) {
        this.modelTest.PathFileMaterial = d;
        this.modelTest.ApiUrl = this.config.ServerApi;
        this.modelTest.ModuleCode = this.modelMechanical.ProductCode;
        this.modelTest.Type = 1;
        this.modelTest.FileName = "KTPA.";
        this.DowloadTemplateToFolder();
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  // Tạo phác thảo thiết kế
  GeneralDegignMechanical() {
    this.modelMechanical.UserNamePC = this.UserNamePC;
    this.service.GeneralDegignMechanical(this.modelMechanical).subscribe(d => {
      if (d) {
        this.modelTest.PathFileMaterial = d;
        this.modelTest.ApiUrl = this.config.ServerApi;
        this.modelTest.ModuleCode = this.modelMechanical.ProductCode;
        this.modelTest.Type = 1;
        this.modelTest.FileName = "PTTK.";
        this.DowloadTemplateToFolder();
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  showChooseFolderWindowTechnological() {
    let activeModal = this.modalService.open(ChooseFolderFileComponent, { container: 'body', windowClass: 'choose-folder-file-modal', backdrop: 'static' });
    activeModal.result.then((result) => {
      if (result) {
        this.modelMechanical.TechnologicalScheme = result;
      }
    }, (reason) => {

    });
  }
  ProductCode: '';
  // Dự toán sơ bộ
  ChooseMaterial() {
    this.materials = [];
    if (this.modelMechanical.ProductCode == "") {
      this.messageService.showMessage("Bạn phải nhập mã Module");
      return;
    }
    let activeModal = this.modalService.open(ChooseMaterialForTemplateComponent, { container: 'body', windowClass: 'choose-material-template', backdrop: 'static' });
    var ListIdSelect = [];
    activeModal.componentInstance.ProductCode = this.modelMechanical.ProductCode;
    //this.modelMechanical.ProductCode = this.ProductCode;
    this.materials.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.materials.push(element);
        });
        if (this.materials.length > 0) {
          this.GeneralPreliminaryEstimate();
        }
      }
    }, (reason) => {
    });
    
  }
  materials: any[] = [];
  GeneralPreliminaryEstimate() {
    this.modelMechanical.materials = this.materials;
    this.service.GeneralPreliminaryEstimate(this.modelMechanical).subscribe(d => {
      if (d) {
        this.modelTest.PathFileMaterial = d;
        this.modelTest.ApiUrl = this.config.ServerApi;
        this.modelTest.ModuleCode = this.modelMechanical.ProductCode;
        this.modelTest.Type = 1;
        this.modelTest.FileName = "DTSB.";
        this.DowloadTemplateToFolder();
      }
    }, e => {
      this.messageService.showError(e);
    });

  }


  /////////////////////////////////////
  modelProfileElectronicDesign: any = {
    ProductName: '',
    ProductCode: '',
    Code: '',
    Name: '',
    UserName: '',

    check_1_M: false,
    check_1_C: false,
    check_2_M: false,
    check_2_C: false,
    check_3_M: false,
    check_3_C: false,
    check_4_M: false,
    check_4_C: false,
    check_5_M: false,
    check_5_C: false,
    check_6_M: false,
    check_6_C: false,
    check_7_M: false,
    check_7_C: false,
    check_8_M: false,
    check_8_C: false,
    check_9_M: false,
    check_9_C: false,
    check_10_M: false,
    check_10_C: false,
    check_11a_M: false,
    check_11a_C: false,
    check_11b_M: false,
    check_11b_C: false,
    check_12a_M: false,
    check_12a_C: false,
    check_12b_M: false,
    check_12b_C: false,
  }

  // Thông số kỹ thuật  
  GeneralSetUpSpecification() {
    this.service.GeneralSetUpSpecification(this.modelMechanical).subscribe(d => {
      if (d) {
        this.modelTest.PathFileMaterial = d;
        this.modelTest.ApiUrl = this.config.ServerApi;
        this.modelTest.ModuleCode = this.modelMechanical.ProductCode;
        this.modelTest.Type = 1;
        this.modelTest.FileName = "TSKT.";
        this.DowloadTemplateToFolder();
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  // Xác nhân danh mục vật tư điện - điện tử
  //listMaterial: any[] = [];
  private listMaterial = new BroadcastEventListener<any>('listDMVTResult');

  // Kiểm tra cơ khí
  getDMVT() {
    this.modelTest.ApiUrl = this.config.ServerApi;
    this.modelTest.ModuleCode = this.modelMechanical.ProductCode;
    this.modelTest.Type = 1;
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      this.modelTest.Token = currentUser.access_token;
    }
    this.signalRService.invoke('GetDMVT', this.modelTest).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }
  // checkFolder(data) {
  //   this.blockUI.stop();
  //   if (data) {
  //     if (data.StatusCode == 1) {
  //       this.listMaterial = data.Data.listMaterial;
  //     } else {
  //       this.messageService.showMessage(data.Message);
  //     }
  //   }
  //   this.exportReportDMVT();
  // }

  modelElictric: any = {
    ModuleCode: '',
    ListMaterial: [],
    CreateBy: ''
  }

  exportReportDMVT() {
    this.modelElictric.ModuleCode = this.modelMechanical.ProductCode;
    this.modelElictric.ListMaterial = this.listMaterial;
    this.modelElictric.CreateBy = '';
    this.testDesignService.exportReportDMVT(this.modelElictric).subscribe(d => {
      this.modelTest.PathFileMaterial = d;
      this.modelTest.ApiUrl = this.config.ServerApi;
      this.modelTest.ModuleCode = this.modelMechanical.ProductCode;
      this.modelTest.Type = 1;
      this.modelTest.FileName = "XNVT.";
      this.DowloadTemplateToFolder();
    }, e => {
      this.messageService.showError(e);
    });
  }

  // GeneralConfirmElectronicRecord() {
  //   this.service.GeneralConfirmElectronicRecord(this.modelMechanical).subscribe(d => {
  //     if (d) {
  //       this.modelTest.PathFileMaterial = d;
  //       this.modelTest.ApiUrl = this.config.ServerApi;
  //       this.modelTest.ModuleCode = this.modelMechanical.ProductCode;
  //       this.modelTest.Type = 1;
  //       this.modelTest.FileName = "XNVT.";
  //       this.DowloadTemplateToFolder();
  //     }
  //   }, e => {
  //     this.messageService.showError(e);
  //   });
  // }


  generalDesign() {
    this.router.navigate(['tools/tong-hop-thiet-ke']);
  }

  showChooseFolderWindowRelationshipCluster() {
    let activeModal = this.modalService.open(ChooseFolderFileComponent, { container: 'body', windowClass: 'choose-folder-file-modal', backdrop: 'static' });
    activeModal.result.then((result) => {
      if (result) {
        this.modelMechanical.RelationshipClusters = result;
      }
    }, (reason) => {
    });
  }

  showChooseFolderWindowFaceModule() {
    let activeModal = this.modalService.open(ChooseFolderFileComponent, { container: 'body', windowClass: 'choose-folder-file-modal', backdrop: 'static' });
    activeModal.result.then((result) => {
      if (result) {
        this.modelMechanical.FaceModule = result;
      }
    }, (reason) => {
    });
  }

  // Hàm import template 
  DowloadTemplateToFolder() {
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      this.modelTest.Token = currentUser.access_token;
    }
    this.signalRService.invoke('DowloadTemplateToFolder', this.modelTest).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  craeteMechanicalProfile() {
    if (!this.modelElectronicRecord.ProductCode) {
      this.messageService.showMessage("Bạn chưa nhập mã Module");
      return;
    }
    this.modelMechanicalRecord.ApiUrl = this.config.ServerApi;
    this.modelMechanicalRecord.ModuleCode = this.modelElectronicRecord.ProductCode;
    this.modelMechanicalRecord.Type = 1;
    this.modelMechanicalRecord.FileName = "VT.";
    this.modelMechanicalRecord.UserName = this.userName;
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      this.modelMechanicalRecord.Token = currentUser.access_token;
    }
    this.signalRService.invoke('craeteMechanicalProfile', this.modelMechanicalRecord).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }
}
