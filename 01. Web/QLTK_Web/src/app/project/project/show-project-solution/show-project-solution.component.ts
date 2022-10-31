import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { Constants, MessageService, Configuration } from 'src/app/shared';
import { ProjectSolutionService } from '../../service/project-solution.service';
import { ChooseSolutionComponent } from '../choose-solution/choose-solution.component';
import { DownloadService } from 'src/app/shared/services/download.service';
import { forkJoin } from 'rxjs';


@Component({
  selector: 'app-show-project-solution',
  templateUrl: './show-project-solution.component.html',
  styleUrls: ['./show-project-solution.component.scss']
})
export class ShowProjectSolutionComponent implements OnInit {

  @Input() Id: string;
  constructor(
    public constants: Constants,
    private config: Configuration,
    private modalService: NgbModal,
    private service: ProjectSolutionService,
    private servicedown: DownloadService,
    private messageService: MessageService,

  ) { }

  result = {
    TotalItems: 0,
    ListResult: []
  };

  model: any = {
    ProjectId: '',
    ListResult: [],
    ListProduct: [],
    ProjectProductSolutionId: ''
  }

  downloadModel: any = {
    Name: '',
    ListDatashet: []
  }

  ngOnInit() {
    this.model.ProjectId = this.Id;
    this.searchProjectSolution();
    this.getProjectProductToSolution();
    this.StatusSolutionProduct();
  }

  getProjectProductToSolution() {
    this.service.getProjectProductByProjectId(this.Id, this.SolutionSelect).subscribe(data => {
      if (data) {
        this.listProjectProduct = data;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  searchProjectSolution() {
    this.service.searchProjectSolution(this.model).subscribe(
      data => {
        this.model.ListResult = data.ListResult;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  createProjectSolution() {
    if (this.selectIndex == -1) {
      this.model.ListProduct = [];
    } else {
      this.model.ListProduct = [];
      this.listProjectProduct.forEach(element => {
        if (element.Checked) {
          this.model.ListProduct.push(element);
        }
      });
    }
    this.service.addProjectSolution(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật giải pháp thành công!');
        this.searchProjectSolution();
        this.StatusSolutionProduct();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showConfirmDeleteSolution(index, Id) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá giải pháp này không?").then(
      data => {
        if (Id == "" || Id == null) {
          this.model.ListResult.splice(index, 1);
        }
        else {
          this.model.ListResult[index].IsDelete = true;
        }
        this.messageService.showSuccess("Xóa giải pháp thành công!");
        this.selectIndex = -1;
        this.loadParam('', '', '')
      },
      error => {
        
      }
    );
  }

  showClick() {
    let activeModal = this.modalService.open(ChooseSolutionComponent, { container: 'body', windowClass: 'choose-solution', backdrop: 'static' });
    var listIdSelect = [];
    this.model.ListResult.forEach(element => {
      listIdSelect.push(element.Id);
    });
    activeModal.componentInstance.listIdSelect = listIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          element.IsDelete = false;
          this.model.ListResult.push(element);
        });

      }
    }, (reason) => {

    });
  }

  downAllDocumentProcess(data: any, type: any) {
    if (data.length == 0) {
      this.messageService.showError("Không có file để tải");
      return;
    }
    var listFilePath: any[] = [];
    data.forEach(element => {
      if (element.Type == type) {
        listFilePath.push({
          Path: element.Path,
          FileName: element.FileName
        });
      }
    });
    this.downloadModel.Name = "GiaiPhap";
    this.downloadModel.ListDatashet = listFilePath;
    this.servicedown.downloadAll(this.downloadModel).subscribe(data => {
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

  FileIdSelect: '';
  selectIndex = -1;
  listProjectProduct: any[] = [];
  SolutionSelect: '';

  loadParam(index, projectSulotionId, solutionId) {
    this.selectIndex = index;
    this.SolutionSelect = projectSulotionId;
    this.model.ProjectProductSolutionId = projectSulotionId;
    this.model.SolutionId = solutionId;
    forkJoin([
      this.service.getProjectProductByProjectId(this.Id, this.SolutionSelect)]
    ).subscribe(([data1]) => {
      this.listProjectProduct = data1;
    });
  }
  checked: false;
  listTemp: any[] = [];
  StartIndex = 1;

  selectAllFunction() {
    if (this.checked) {
      this.listProjectProduct.forEach(element => {
        element.Checked = true;
        //this.listTemp.push(element);
      });
    } else {
      this.listProjectProduct.forEach(element => {
        element.Checked = false;
        //this.listTemp = [];
      });
    }
  }

  selectFunction(event) {
    this.listProjectProduct.forEach(element => {
      if (event.Id == element.Id) {
        if (event.Checked) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      }
    });
  }
  totalProduct: number;
  totalProjectSolutionProduct: number;
  statusProduct: string;
  StatusSolutionProduct() {
    this.service.StatusSolutionProduct(this.Id).subscribe(data => {
      if (data) {
        this.totalProduct = data.TotalProduct;
        this.totalProjectSolutionProduct = data.TotalProjectSolutionProduct
        this.statusProduct = data.Status;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

}
