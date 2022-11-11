import { Component, OnInit } from '@angular/core';
import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';
import { CourseService } from '../../service/course.service';
import { CourseCreateComponent } from '../course-create/course-create.component';

@Component({
  selector: 'app-course-manage',
  templateUrl: './course-manage.component.html',
  styleUrls: ['./course-manage.component.scss']
})
export class CourseManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    public constant: Constants,
    private courseService: CourseService,
  ) { }

  StartIndex = 0;
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    
    Id: '',
    Name: '',
    Code: '',
    Status: ''
  }
  listData : any[] = [];
  // Tool search.
  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã khóa học',
    Items: [
      {
        Name: 'Tên khóa học',
        FieldName: 'Name',
        Placeholder: 'Nhập tên khóa học',
        Type: 'text'
      },
      // {
      //   Name: 'Tình trạng',
      //   FieldName: 'Status',
      //   Placeholder: 'Tình trạng',
      //   Type: 'select',
      //   Data: this.constant.statusCourse,
      //   DisplayName: 'Name',
      //   ValueName: 'Id'
      // },
      {
        Name: 'Thiết bị cho khóa học',
        FieldName: 'DeviceForCourse',
        Placeholder: 'Nhập tên thiết bị khóa học',
        Type: 'text'
      },
    ]
  };

  height : any;
  ngOnInit() {
    this.height = window.innerHeight - 140;
    this.appSetting.PageTitle = "Quản lý khóa học";
    this.searchCourse();
  }

  searchCourse() {
    this.courseService.searchCourse(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(CourseCreateComponent, { container: 'body', windowClass: 'Course-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchCourse();
      }
    }, (reason) => {
    });
  }

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá khóa học này không?").then(
      data => {
        this.delete(Id);
      },
      error => {
        
      }
    );
  }

  delete(Id: string) {
    this.courseService.deleteCourse({ Id: Id }).subscribe(
      data => {
        this.searchCourse();
        this.messageService.showSuccess('Xóa khóa học thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',

      OrderType: true,
      Id: '',
      Name: '',
      Code: '',
      Status: ''
    }
    this.searchCourse();
  }

  selectedMaterialGroupId:'';

  onSelectionChanged(e) {
    this.selectedMaterialGroupId = e.selectedRowKeys[0];
  }
}
