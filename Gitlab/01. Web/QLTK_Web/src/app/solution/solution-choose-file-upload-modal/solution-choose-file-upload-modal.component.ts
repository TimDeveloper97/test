import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DxTreeListComponent } from 'devextreme-angular';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { Subscription } from 'rxjs';
import { Configuration, Constants, MessageService } from 'src/app/shared';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { SolutionService } from '../service/solution.service';
import { ShowEditContenComponent } from '../show-edit-conten/show-edit-conten.component';

@Component({
  selector: 'app-solution-choose-file-upload-modal',
  templateUrl: './solution-choose-file-upload-modal.component.html',
  styleUrls: ['./solution-choose-file-upload-modal.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SolutionChooseFileUploadModalComponent implements OnInit {

  @BlockUI() blockUI: NgBlockUI;
  @ViewChild(DxTreeListComponent) treeView;
  private onSendListFolder = new BroadcastEventListener<any>('sendListFolder');
  private onUploadFileSolution = new BroadcastEventListener<any>('uploadFileSolution');
  private onSendListFile = new BroadcastEventListener<any>('sendListFile');
  private notiSub: Subscription;
  private uploadSolutionSub: Subscription;
  private sendListFileSub: Subscription;

  solutionId: string;
  curentVersion: number;
  checkSelect: boolean = false;
  ListFolder: any[] = [];
  ListFolderId: any[] = [];
  selectedId = '';
  listCodeRule = [];
  ListFile: any[]=[];

  solutionModel = {
    SolutionId: '',
    FolderId: '',
    Path: '',
    ApiUrl: '',
    FileApiUrl: '',
    Token: '',
    DesignType: 0,
    CurentVersion: 0
  };

  OldVersionModel: any = {
    CurentVersion: 0,
    EditContent: ''
  }

  folderModel = {
    Id: '',
    Path: '',
    SelectPath: '',
    ListFolder: []
  }

  uploadModel = {
    ModuleId: '',
    ListFile: [],
    DesignType: 0,
    LstError: [],
    Status: false
  }

  searchCodeRuleModel = {
    Code: ''
  }

  constructor(
    private config: Configuration,
    private modalService: NgbModal,
    private signalRService: SignalRService,
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private solutionService: SolutionService,
    public constants: Constants,
  ) { }


  ngOnInit() {
    this.OldVersionModel.CurentVersion = this.curentVersion;
    this.getFolder();
    this.signalRService.listen(this.onSendListFolder, false);
    this.notiSub = this.onSendListFolder.subscribe((data: any) => {
      this.blockUI.stop();
      if (data) {
        this.folderModel.ListFolder = data.ListForder;
        if (this.checkSelect) {
          this.folderModel.SelectPath = data.Path;
          this.selectPathForder();
        }
      }
    });
    this.signalRService.listen(this.onSendListFile, false);
    this.sendListFileSub = this.onSendListFile.subscribe((data: any) => {
      this.blockUI.stop();
      if (data) {
        this.ListFile= data;
      }
    });
    this.signalRService.listen(this.onUploadFileSolution, true);
    this.uploadSolutionSub = this.onUploadFileSolution.subscribe((data: any) => {
      this.blockUI.stop();
      if (data) {
        if (data.LstError && data.LstError.length > 0) {
          var errorMessage = data.LstError.join("<br/>")
          this.messageService.showMessage(errorMessage);
        }
        else {
          data.Id = this.solutionId;
          data.CurentVersion = this.OldVersionModel.CurentVersion;
          data.EditContent = this.OldVersionModel.EditContent;
          this.solutionService.uploadFileDesignDocument(data).subscribe((data: any) => {
            this.messageService.showSuccess('Upload tài liệu thiết kế thành công!');
          },
            error => {
              this.messageService.showError(error);
            });
        }
      }
    });
  }

  ngOnDestroy() {
    this.notiSub.unsubscribe();
    this.uploadSolutionSub.unsubscribe();
    this.sendListFileSub.unsubscribe();
    this.signalRService.stopListening(this.onSendListFolder);
    this.signalRService.stopListening(this.onUploadFileSolution);
    this.signalRService.stopListening(this.onSendListFile);
  }

  getFolder() {
    this.signalRService.invoke('GetFolder', this.folderModel).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  getLink(type) {
    var model = {
      SolutionId: this.solutionId,
      Token: '',
      DesignType: type,
      ApiUrl: this.config.ServerApi,
    };
    this.checkSelect = true;
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      model.Token = currentUser.access_token;
    }
    this.signalRService.invoke('GetSelectFolderSolution', model).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  selectPathForder() {
    var data = this.folderModel.ListFolder.filter(i => i.Id == this.folderModel.SelectPath);
    if (data.length > 0) {
      this.selectedId = data[0].Id;
      this.solutionModel.Path = data[0].Id;
      this.folderModel.Path = data[0].Id;
      this.treeView.selectedRowKeys = [this.selectedId];
    }
  }

  onSelectionChanged(e) {
    this.selectedId = e.selectedRowKeys[0];
    this.solutionModel.Path = e.selectedRowKeys[0];
    this.folderModel.Path = e.selectedRowKeys[0];
  }

  onRowExpanding(e) {
    this.checkSelect = false;
    this.folderModel.Id = e.key;
    this.signalRService.invoke('GetFolder', this.folderModel).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
    // this.notiSub = this.onSendListFolder.subscribe((data: any) => {
    //   this.folderModel.ListFolder = data;
    // });
  }

  closeModal() {
    this.activeModal.close(this.OldVersionModel.CurentVersion);
  }

  showConfirmUploadVersion() {
    this.messageService.showConfirmFile("Bạn có muốn thay đổi version không?").then(
      data => {
        if (data) {
          this.showEditContent();
        } else {
          this.chooseFolder(this.curentVersion);
        }
      }
    );
  }

  chooseFolder(version: number) {
    this.solutionModel.SolutionId = this.solutionId;
    this.solutionModel.ApiUrl = this.config.ServerApi;
    this.solutionModel.FileApiUrl = this.config.ServerFileApi;
    this.solutionModel.CurentVersion = version;
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser && currentUser.access_token) {
      this.solutionModel.Token = currentUser.access_token;
    }
    this.signalRService.invoke('UploadFileSolutionDesignDocument', this.solutionModel).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  showEditContent() {
    let activeModal = this.modalService.open(ShowEditContenComponent, { container: 'body', windowClass: 'show-edit-content-modal', backdrop: 'static' });
    activeModal.componentInstance.curentVersion = this.curentVersion;
    activeModal.result.then((result) => {
      this.OldVersionModel = result;
      this.chooseFolder(result.CurentVersion)
    }, (reason) => {
    });
  }

  getFile(){
    this.folderModel.Id = this.selectedId;
    this.signalRService.invoke('GetListFile', this.folderModel).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  chooseFile(path){
    this.solutionModel.Path = path;
  }
}
