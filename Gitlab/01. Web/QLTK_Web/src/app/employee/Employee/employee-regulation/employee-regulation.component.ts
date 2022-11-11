import { Component, OnInit, Input, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { EmployeeUpdateService } from '../../service/employee-update.service';
import { Constants, Configuration, MessageService, AppSetting } from 'src/app/shared';
import { CourseService } from 'src/app/employee/service/course.service';
import { CourseCreateComponent } from 'src/app/employee/course/course-create/course-create.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DocumentViewComponent } from 'src/app/document/document-view/document-view.component';
import { DownloadService } from 'src/app/shared/services/download.service';

@Component({
  selector: 'app-employee-regulation',
  templateUrl: './employee-regulation.component.html',
  styleUrls: ['./employee-regulation.component.scss']
})
export class EmployeeRegulationComponent implements OnInit {

  @Input() Id: string;
  @Input() EmployeeName: string;
  @Input() EmployeeCode: string;
  constructor(
    public appSetting: AppSetting,
    public constant: Constants,
    private config: Configuration,
    private service: EmployeeUpdateService,
    private modalService: NgbModal,
    private messageService: MessageService,
    private courseService: CourseService,
    private dowloadservice: DownloadService,
  ) { }
  startIndex = 1;
  listData: any[] = [];
  listRegulation : any[] =[];
  listProcedure : any[] =[];
  model: any = {
    Id: '',
  }
  documents: any [];
  @ViewChild('scrollPracticeMaterial',{static:false}) scrollPracticeMaterial: ElementRef;
  @ViewChild('scrollPracticeMaterialHeader',{static:false}) scrollPracticeMaterialHeader: ElementRef;
  @ViewChild('scrollPracticeMaterial1',{static:false}) scrollPracticeMaterial1: ElementRef;
  @ViewChild('scrollPracticeMaterialHeader1',{static:false}) scrollPracticeMaterialHeader1: ElementRef;
  ngOnInit() {
    this.model.Id = this.Id;
    this.getListRegulation();
    this.getListProcedure();
  }

  ngAfterViewInit(){
    this.scrollPracticeMaterial.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollPracticeMaterialHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.scrollPracticeMaterial1.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollPracticeMaterialHeader1.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollPracticeMaterial.nativeElement.removeEventListener('ps-scroll-x', null);
    this.scrollPracticeMaterial1.nativeElement.removeEventListener('ps-scroll-x', null);

  }

  getListRegulation() {
    this.service.getListRegulation(this.model).subscribe(
      data => {
        this.listRegulation = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  getListProcedure() {
    this.service.getListProcedure(this.model).subscribe(
      data => {
        this.listProcedure = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  
  viewCourse(Id: string, disableData : true) {
    let activeModal = this.modalService.open(CourseCreateComponent, { container: 'body', windowClass: 'Course-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.componentInstance.disableData = disableData;
    activeModal.result.then((result) => {
      if (result) {
      }
    }, (reason) => {
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

    let modelDowload: any = {
      Name: '',
      ListDatashet: []
    }

    modelDowload.Name = "Tài liệu";
    modelDowload.ListDatashet = listFilePath;
    this.dowloadservice.downloadAll(modelDowload).subscribe(data => {
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

  viewDocument(document: any) {
    let activeModal = this.modalService.open(DocumentViewComponent, { container: 'body', windowClass: 'document-viewpdf-file-modal', backdrop: 'static' })
    activeModal.componentInstance.id = document.Id;
    activeModal.componentInstance.documentName = document.Name;
    activeModal.componentInstance.documentCode = document.Code;
    activeModal.result.then((result: any) => {
      if (result) {
      }
    });
  }
}
