import { Component, OnInit, ViewChild } from '@angular/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { DxTreeListComponent } from 'devextreme-angular';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { Subscription } from 'rxjs';
import { Configuration, MessageService } from 'src/app/shared';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SolutionService } from '../service/solution.service';
import { ShowEditContenComponent } from '../show-edit-conten/show-edit-conten.component';

@Component({
  selector: 'app-solution-choose-folder-upload-modal',
  templateUrl: './solution-choose-folder-upload-modal.component.html',
  styleUrls: ['./solution-choose-folder-upload-modal.component.scss']
})
export class SolutionChooseFolderUploadModalComponent implements OnInit {
  @BlockUI() blockUI: NgBlockUI;
  @ViewChild(DxTreeListComponent) treeView;
  private onSendListFolder = new BroadcastEventListener<any>('sendListFolder');
  private onUploadFolderSolution = new BroadcastEventListener<any>('uploadFolderSolution');
  private notiSub: Subscription;
  private uploadSolutionSub: Subscription;

  solutionId: string;
  curentVersion: number;
  checkSelect: boolean = false;
  ListFolder: any[] = [];
  ListFolderId: any[] = [];
  selectedId = '';
  listCodeRule = [];

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
    this.signalRService.listen(this.onUploadFolderSolution, true);
    this.uploadSolutionSub = this.onUploadFolderSolution.subscribe((data: any) => {
      this.blockUI.stop();
      if (data) {
        if (data.LstError && data.LstError.length > 0) {
          var errorMessage = data.LstError.join("<br/>")
          this.messageService.showMessage(errorMessage);
        }
        else {
          data.SolutionId = this.solutionId;
          data.DesignType = this.solutionModel.DesignType;
          data.CurentVersion = this.OldVersionModel.CurentVersion;
          data.EditContent = this.OldVersionModel.EditContent;
          this.solutionService.uploadDesignDocument(data).subscribe((data: any) => {
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
    this.signalRService.stopListening(this.onSendListFolder);
    this.signalRService.stopListening(this.onUploadFolderSolution);
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
      this.signalRService.invoke('UploadSolutionDesignDocument', this.solutionModel).subscribe(data => {
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
}
