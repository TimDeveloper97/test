import { Component, OnInit, Input, ViewEncapsulation, OnDestroy } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ChooseFolderFileComponent } from '../choose-folder-file/choose-folder-file.component';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { Subscription } from 'rxjs';
import { MessageService, Configuration, Constants, ComponentService } from 'src/app/shared';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';

@Component({
  selector: 'app-check-electronic',
  templateUrl: './check-electronic.component.html',
  styleUrls: ['./check-electronic.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CheckElectronicComponent implements OnInit, OnDestroy {

  @Input() pathDMVT: string;
  @Input() moduleCode: string;
  @Input() txtPathSC: string;
  @Input() selectedPath: string;
  @Input() listError: any;
  @BlockUI() blockUI: NgBlockUI;
  constructor(
    public constant: Constants,
    private config: Configuration,
    private modalService: NgbModal,
    private signalRService: SignalRService,
    private messageService: MessageService,
    private componentService: ComponentService
  ) { }

  pathFile: string;
  listDMVT: any = [];
  private notiSub: Subscription;
  private onCheckElectronic = new BroadcastEventListener<any>('checkElectronic');
  modelCheck: any = {
    ModuleId: '',
    TypeFunctionDefinitionId: 3,
    Path: '',
    // ModuleCode: '',
    // ModuleGroupCode: '',
    // ModuleGroupId: '',
    // ListError: [],
    // ListMaterialModel: [],
    // ListRawMaterialsModel: [],
    // ListManufacturerModel: [],
    // ListFolderModel:[],
    // ListFileModel: [],
    // ListMaterialGroupModel: [],
    // ListUnitModel:[],
    // LstError:[],
    // ListResult:[],
    // ListCodeRule:[]
  }

  ngOnInit() {
    // this.signalRService.listen(this.onUploadFolder);
    // this.uploadSub = this.onUploadFolder.subscribe((data: any) => {
    //   this.blockUI.stop();
    //   if (data.LstError.length > 0) {
    //     var errorMessage = data.LstError.join(".")
    //     this.messageService.showMessage(errorMessage);
    //   }
    //   // else{
    //   //   this.insertDatabase(data);
    //   // }  
    // });
    // this.signalRService.listen(this.onCheckElectronic, false);
    // this.notiSub = this.onCheckElectronic.subscribe((data: any) => {
    //   this.blockUI.stop();
    //   if (data) {
    //     this.listError = data.LstError;
    //     this.listDMVT = data.resultCheckDMVTModel.ListResult;
    //   }
    // });
  }

  ngOnDestroy() {
    // this.notiSub.unsubscribe();
    // this.signalRService.stopListening(this.onCheckElectronic);
  }

  // showChooseFolderWindow() {
  //   this.componentService.showChooseFolder().subscribe(data => {
  //     this.pathFile = data;
  //   });
  // }

  // checkElectronic() {
  //   if (this.pathFile) {
  //     this.modelCheck.ApiUrl = this.config.ServerApi;
  //     this.modelCheck.Path = this.pathFile;
  //     this.signalRService.invoke('CheckElectronic', this.modelCheck).subscribe(data => {
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

}
