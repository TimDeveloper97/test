import { Component, OnInit, Input, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { EmployeeUpdateService } from '../../service/employee-update.service';
import { Constants, Configuration, MessageService, AppSetting } from 'src/app/shared';
import { CourseService } from 'src/app/employee/service/course.service';
import { CourseCreateComponent } from 'src/app/employee/course/course-create/course-create.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-show-employee-course',
  templateUrl: './show-employee-course.component.html',
  styleUrls: ['./show-employee-course.component.scss']
})
export class ShowEmployeeCourseComponent implements OnInit {

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
    private courseService: CourseService
  ) { }
  StartIndex = 1;
  listData: any[] = [];
  model: any = {
    Id: '',
  }
  @ViewChild('scrollPracticeMaterial',{static:false}) scrollPracticeMaterial: ElementRef;
  @ViewChild('scrollPracticeMaterialHeader',{static:false}) scrollPracticeMaterialHeader: ElementRef;
  ngOnInit() {
    this.model.Id = this.Id;
    this.getListCourse();
    
  }

  ngAfterViewInit(){
    this.scrollPracticeMaterial.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollPracticeMaterialHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollPracticeMaterial.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  getListCourse() {
    this.service.getListCourse(this.model).subscribe(
      data => {
        this.listData = data;
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

}
