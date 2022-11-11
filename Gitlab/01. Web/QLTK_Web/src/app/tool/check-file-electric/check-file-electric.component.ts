import { Component, OnInit, Input } from '@angular/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants, MessageService, Configuration } from 'src/app/shared';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { TestDesignStructureService } from '../services/test-designstructure-service';
import { TestDesignService } from '../services/test-design-service';
import { Subscription } from 'rxjs';
// import { ChooseFolderFileComponent } from '../choose-folder-file/choose-folder-file.component';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
// import { ChooseFolderModalComponent } from 'src/app/module/choose-folder-modal/choose-folder-modal.component';

@Component({
  selector: 'app-check-file-electric',
  templateUrl: './check-file-electric.component.html',
  styleUrls: ['./check-file-electric.component.scss']
})
export class CheckFileElectricComponent implements OnInit {
  @Input() pathDMVT: string;
  @Input() moduleCode: string;
  @Input() txtPathSC: string;
  @Input() selectedPath: string;
  @BlockUI() blockUI: NgBlockUI;
  constructor(
    private modalService: NgbModal,
    public constant: Constants,
    private signalRService: SignalRService,
    private testDesignStructureService: TestDesignStructureService,
    private messageService: MessageService,
    private config: Configuration,
    private testDesignService: TestDesignService, ) { }
  private listFoderElec = new BroadcastEventListener<any>('listFoderElectric');
  private onUploadFolder = new BroadcastEventListener<any>('listFoderElec');
  private uploadSub: Subscription;


  pathFile: string;
  modelCheck: any = {
    ApiUrl: '',
    ModuleId: '',
    TypeFunctionDefinitionId: 2,
    Path: '',
    ModuleCode: '',
    ModuleGroupCode: '',
    ModuleGroupId: '',

    ListError: [],
    ListMaterialModel: [],
    ListRawMaterialsModel: [],
    ListManufacturerModel: [],
    ListFolderModel: [],
    ListFileModel: [],
    ListMaterialGroupModel: [],
    ListUnitModel: [],
    ListCodeRule: [],
    Message: '',
    ListDesignStrcture: []
  }
  listCheckFile: any = [];
  ngOnInit() {
    // this.searchListDesignStrcture();
    // this.signalRService.listen(this.onUploadFolder, false);
    // this.uploadSub = this.onUploadFolder.subscribe((data: any) => {
    //   this.blockUI.stop();
    //   this.listCheckFile = data.ListCheckFile;
      
    //   if (data.LstError.length > 0) {
    //     var errorMessage = data.LstError.join(".")
    //     this.messageService.showMessage(errorMessage);
    //   }
    //   // else if (data.Message) {
    //   //   this.messageService.showMessage("Thư mục đúng với định nghĩa");
    //   // }

    // });
  }
  listData: any = [];
  private notiSub: Subscription;

  // checkElectric() {
  //   if (this.pathFile) {
  //     this.modelCheck.ApiUrl = this.config.ServerApi;
  //     this.modelCheck.Path = this.pathFile;
  //     this.signalRService.invoke('CheckElectric', this.modelCheck).subscribe(data => {
  //       if (data) {
  //         this.blockUI.start();
  //       }
  //       else {
  //         this.messageService.showMessage("Không kết nối được service");
  //       }
  //     });   
  //   } else {
  //     this.messageService.showMessage('Chưa chọn thư mục!');
  //   }
  // }

  // listFoder: [];
  // showChooseFolderWindow() {
  //   let activeModal = this.modalService.open(ChooseFolderModalComponent, { container: 'body', windowClass: 'choose-folder-modal', backdrop: 'static' });
  //   activeModal.result.then((result) => {
  //     if (result) {
  //       this.pathFile = result;
  //     }
  //   }, (reason) => {

  //   });
  // }

  // searchListDesignStrcture() {
  //   this.testDesignStructureService.searchListDesignStrcture().subscribe((data: any) => {
  //     if (data) {
  //       this.modelCheck.ListDesignStrcture = data;
  //     }
  //   },
  //     error => {
  //       this.messageService.showError(error);
  //     });
  // }


}
