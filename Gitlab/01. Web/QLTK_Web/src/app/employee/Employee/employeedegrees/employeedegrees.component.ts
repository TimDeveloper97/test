import { Component, OnInit, Input } from '@angular/core';
import { MessageService, Constants } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { EmployeeDegreeServiceService } from '../../service/employee-degree-service.service';
import { EmployeeDegreesCreateComponent } from './employee-degrees-create/employee-degrees-create.component';

@Component({
  selector: 'app-employeedegrees',
  templateUrl: './employeedegrees.component.html',
  styleUrls: ['./employeedegrees.component.scss']
})
export class EmployeedegreesComponent implements OnInit {
  @Input() Ids: string;
  @Input() EmployeeName: string;
  @Input() EmployeeCode: string;
  constructor(
    private messageService: MessageService,
    private modalService: NgbModal,
    private employeeDegressService: EmployeeDegreeServiceService,
    public constant: Constants,
  ) { }
  startIndex = 0;
  listData: any = [];

  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    QualificationId: '',
    EmployeeId: '',
    Name: '',
    Code: '',
    Year: '',
    School: '',
    Rank: '',
  }
  ngOnInit() {
    this.model.EmployeeId = this.Ids;
    this.search();

  }
  search() {
    this.employeeDegressService.SearchModel(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(EmployeeDegreesCreateComponent, { container: 'body', windowClass: 'employee-degrees-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = this.Ids;
    activeModal.componentInstance.IdDegrees = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.search();
      }
    }, (reason) => {
    });
  }

  showConfirmDeleteTestCriteria(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá bằng cấp này không?").then(
      data => {
        this.delete(Id);
      },
      error => {
        
      }
    );
  }
  delete(Id: string) {
    this.employeeDegressService.Deletes({ Id: Id }).subscribe(
      data => {
        this.search();
        this.messageService.showSuccess('Xóa bằng cấp thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }
}
