import { Component, OnInit, Input } from '@angular/core';
import { MessageService, Constants, Configuration } from 'src/app/shared';
import { ProjectProductBomService } from '../../service/project-product-bom.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ProjectProductBomCreateComponent } from '../project-product-bom-create/project-product-bom-create.component';
import { DownloadService } from 'src/app/shared/services/download.service';

@Component({
  selector: 'app-show-project-product-bom',
  templateUrl: './show-project-product-bom.component.html',
  styleUrls: ['./show-project-product-bom.component.scss']
})
export class ShowProjectProductBomComponent implements OnInit {

  @Input() ProjectProductId: string;
  @Input() ModuleId: string;
  constructor(
    public constant: Constants,
    private dowloadservice: DownloadService,
    private config: Configuration,
    private modalService: NgbModal,
    private messageService: MessageService,
    private service: ProjectProductBomService
  ) { }

  listData: any[] = [];
  model: any = {
    Id: '',
    ModuleId: '',
    ProjectProductId: '',
    Version: 1,
  }

  modelDowload: any = {
    Name: '',
    ListDatashet: []
  }

  ngOnInit() {
    this.model.ProjectProductId = this.ProjectProductId;
    this.model.ModuleId = this.ModuleId;
    this.searchBOMDesignTwo();
  }

  searchBOMDesignTwo() {
    this.service.searchBOMDesignTwo(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.listData = data.ListResult;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá BOM này không?").then(
      data => {
        this.deleteBOMDesignTwo(Id);
      },
      error => {
        
      }
    );
  }

  deleteBOMDesignTwo(Id: string) {
    this.service.deleteBOMDesignTwo({ Id: Id }).subscribe(
      data => {
        this.searchBOMDesignTwo();
        this.messageService.showSuccess('Xóa BOM thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  exportExcel(Id: string) {
    this.model.Id = Id;
    this.service.exportExcel(this.model).subscribe(d => {
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

  downAllDocumentProcess(Datashet: any) {
    if (Datashet.length <= 0) {
      this.messageService.showMessage("Không có file để tải");
      return;
    }
    var listFilePath: any[] = [];
    Datashet.forEach(element => {
      listFilePath.push({
        Path: element.Path,
        FileName: element.FileName
      });
    });
    this.modelDowload.Name = "BOMTK2";
    this.modelDowload.ListDatashet = listFilePath;
    this.dowloadservice.downloadAll(this.modelDowload).subscribe(data => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerFileApi + data.PathZip;
      link.download = 'Download.zip';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, e => {
      this.messageService.showError(e);
    });
  }

  showCreate() {
    let activeModal = this.modalService.open(ProjectProductBomCreateComponent, { container: 'body', windowClass: 'project-product-bom-create', backdrop: 'static' })
    activeModal.componentInstance.projectProductId = this.ProjectProductId;
    activeModal.componentInstance.moduleId = this.ModuleId;
    activeModal.result.then((result) => {
      if (result) {
        this.searchBOMDesignTwo();
      }
    }, (reason) => {
    });
  }

}
