import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AppSetting, Configuration, MessageService, Constants } from 'src/app/shared';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { ModuleProjectService } from '../../services/module-project.service';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

@Component({
  selector: 'app-module-project-test-criteia',
  templateUrl: './module-project-test-criteia.component.html',
  styleUrls: ['./module-project-test-criteia.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ModuleProjectTestCriteiaComponent implements OnInit {
  
  constructor(
    private activeModal: NgbActiveModal,
    public appSetting: AppSetting,
    private config: Configuration,
    private messageService: MessageService,
    private projectService: ModuleProjectService,
    public constant: Constants
  ) {this.pagination = Object.assign({}, appSetting.Pagination);  }
  startIndex = 0;
  pagination;
  lstpageSize = [5, 10, 15, 20, 25, 30];
  listDA: any[] = [];
  logUserId: string;
  isAction: boolean = false;

  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };
  Id: string;
  model: any ={
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Name: '',
    Code: '', 
    Id:'',
  }
  ngOnInit() {
    this.appSetting.PageTitle = "Dự án";
    this.model.Id = this.Id;
    this.searchTestCriteria();
  }
  searchTestCriteria(){
    this.projectService.SearchTestCriteria(this.model).subscribe((data: any)=>{
      if(data.ListResult){
        this.startIndex = ((this.model.PageNumber -1)*this.model.PageSize +1);
        this.listDA = data.ListResult;
        this.model.totalItems = data.TotalItem;
      }
    }, 
    error => {
      this.messageService.showError(error);
    });
  }
  clear() {
    this.model = {
      page: 1,
      PageSize: 10,
      totalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      Id: '',
      TestCriteriaGroupName: '',
      Name: '',
      Code: '', 
      TechnicalRequirements: '',
      Note:'',
      ResuldStatusTest:'',
      NoteResuld:'',
    }
    this.searchTestCriteria();
  }

  exportExcel() {
    this.projectService.ExportExcel(this.model).subscribe(d => {
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
  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }
}
