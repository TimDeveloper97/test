import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ComboboxService, Constants, MessageService } from 'src/app/shared';
import { CustomerRequirementService } from '../../customer-requirement/service/customer-requirement.service';
import { MeetingService } from '../../service/meeting.service';

@Component({
  selector: 'app-end-meeting',
  templateUrl: './end-meeting.component.html',
  styleUrls: ['./end-meeting.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class EndMeetingComponent implements OnInit {

  constructor(
    private messageService: MessageService,
    public constant: Constants,
    private comboboxService: ComboboxService,
    private activeModal: NgbActiveModal,
    private meetingService: MeetingService,
    private customerRequirementService : CustomerRequirementService

  ) { }

  requirementModel: any = {
    Id: '',
    Name: '',
    Code: '',
    CustomerContactId: '',
    CustomerId: '',
    Request: '',
    Status: 0,
    Note: '',
    Budget: '',
    Version: '',
    RequestType: '',
    ListAttach: [],
    Survey: [],
    ListSurvey: [],
    Materials: [],
    ListMaterial: [],
    ListUser: [],
    ListEstimate: [],
    ListProduct: [],
    ListSolutionAnalysisRisk: [],
    ListSupplier: [],
    RateRating: '',
    Petitioner: '',
    DepartmentRequest: '',
    Reciever: '',
    DepartmentReceive: '',
    RequestSource: '',
    ProjectPhaseId: '',
    Competitor: '',
    CustomerSupplier: '',
    PriorityLevel: '',
    MaterialId: '',
    ProductId: '',
    UserId: '',
    SurveyId: '',
    Index: 0,
    Step: 0,
    ListContent: [],
    listSolitionAnalysisSupplier: [],
    TradeConditions: "",
    TotalPrice: 0,
    ListRequireEstimateMaterialAttach: [],
    ListRequireEstimateFCMlAttach: [],
    Content1 :'',
    Conclude1 :'',
    Person1 :'',
    Content2 :'',
    Conclude2 :'',
    Person2 :'',
    Content3 :'',
    Conclude3 :'',
    Person3 :'',
    // PlanFinishDate:'',
    // StartDate:'',
    Duration:'',
    CustomerRequirementState:0,
    CustomerRequirementAnalysisState:0,
    SurveyState:0,
    SolutionAnalysisState:0,
    EstimateState:0,
    DoSolutionAnalysisState:0,
  };
  modalInfo = {
    Title: 'Tạo yêu cầu khách hàng',
    SaveText: 'Lưu',
  };
  isAction: boolean = false;

  columnName: any[] = [{ Name: 'Code', Title: 'Mã' }, { Name: 'Name', Title: 'Tên' },];
  columnNameCustomerContact: any[] = [{ Name: 'Name', Title: 'Tên' }, { Name: 'Address', Title: 'Đia chỉ' },];
  columnNameEmployee: any[] = [{ Name: 'Code', Title: 'Mã' }, { Name: 'Name', Title: 'Tên' },];
  columnNameDepartment: any[] = [{ Name: 'Code', Title: 'Mã' }, { Name: 'Name', Title: 'Tên' },];
  listCustomer: any[] = [];
  listCustomerContact: any[] = [];
  listDepartment: any[] = [];
  listEmployee: any[] = [];
  listEmployee1: any[] = [];
  listProjectPhase : any[]=[];




  listCustomerRequirementRequestType: any[] = [
    { Id: 1, Name: "Làm máy" },
    { Id: 2, Name: "Yêu cầu khác" },
  ]

  listCustomerRequirementRequestSource: any[] = [
    { Id: 1, Name: "Khách hàng liên hệ" },
    { Id: 2, Name: "TPA liên hệ" },
    { Id: 3, Name: "Online" },
  ]

  listCustomerRequirementRequestStatus: any[] = [
    { Id: 0, Name: "Yêu cầu khách hàng" },
    { Id: 1, Name: "Phân tích yêu cầu khách hàng" },
    { Id: 2, Name: "khảo sát" },
    { Id: 3, Name: "Phân tích giải Pháp" },
    { Id: 4, Name: "Làm giải pháp" },
    { Id: 5, Name: "Lập dự toán" },
    // { Id: 6, Name: "Bảo vệ giải pháp" },
    // { Id: 7, Name: "Chốt giải Pháp" },
    // { Id: 8, Name: "Hủy giải Pháp" },
    { Id: 9, Name: "Đang cần bổ sung thêm thông tin"},
  ]

  state : any[] = [
    {Id: 0, Name:"Đang làm"},
    {Id: 1, Name:"Hoàn thành"},
    {Id:2 , Name:"Bỏ qua"},
  ]

  ListAttach : any[] =[];
  ngOnInit(): void {
    this.getListCustomer();
    this.getCbbDepartment();
    this.GetProjectPhaseType();
    this.getCustomerContact();
    this.requirementModel.ListContent.forEach(element =>{
      element.MeetingContentAttaches.forEach(item =>{
        this.ListAttach.push(item);
      })
    });

    // this.generateCode();
  }

  changeCustomer() {
    this.requirementModel.CustomerCode = this.listCustomer.find(a => a.Id == this.requirementModel.CustomerId).Code;
    this.getCustomerContact();
  }

  changeCustomerContact() {
    this.requirementModel.CustomerContactPosition = this.listCustomerContact.find(a => a.Id == this.requirementModel.CustomerContactId).Position;
    this.requirementModel.CustomerContactPhoneNumber = this.listCustomerContact.find(a => a.Id == this.requirementModel.CustomerContactId).PhoneNumber;
    this.requirementModel.CustomerContactEmail = this.listCustomerContact.find(a => a.Id == this.requirementModel.CustomerContactId).Email;
  }

  getCustomerContact() {
    this.comboboxService.getCustomerContact(this.requirementModel.CustomerId).subscribe(
      data => {
        this.listCustomerContact = data;
        this.requirementModel.CustomerContactPosition = this.listCustomerContact.find(a => a.Id == this.requirementModel.CustomerContactId).Position;
        this.requirementModel.CustomerContactPhoneNumber = this.listCustomerContact.find(a => a.Id == this.requirementModel.CustomerContactId).PhoneNumber;
        this.requirementModel.CustomerContactEmail = this.listCustomerContact.find(a => a.Id == this.requirementModel.CustomerContactId).Email;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  changeDepartmentRequest() {
    this.getListDepartmentRequestEmployees();
    //this.requirementModel.NamePetitioner = this.listEmployee.find(a => a.DepartmentId == this.requirementModel.DepartmentRequest).Name;
  }

  changeDepartmentReceive() {
    this.getListDepartmentReceiveEmployees();
  }

   getListDepartmentRequestEmployees() {
    this.comboboxService.getListDepartmentRequestEmployees(this.requirementModel.DepartmentRequest).subscribe(
      data => {
        this.listEmployee = data;
        //this.requirementModel.NamePetitioner = this.listEmployee.find(a => a.DepartmentId == this.requirementModel.DepartmentRequest).Name;
        var NamePetitioner = this.listEmployee.find(a => a.DepartmentId == this.requirementModel.DepartmentReceive);
        if (NamePetitioner != null) {
          NamePetitioner = NamePetitioner.Name;
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  getListDepartmentReceiveEmployees() {
    this.comboboxService.getListDepartmentReceiveEmployees(this.requirementModel.DepartmentReceive).subscribe(
      data => {
        this.listEmployee1 = data;
        //this.requirementModel.NameReciever = this.listEmployee1.find(a => a.DepartmentId == this.requirementModel.DepartmentReceive).Name;
        var NameReciever = this.listEmployee1.find(a => a.DepartmentId == this.requirementModel.DepartmentReceive);
        if (NameReciever != null) {
          NameReciever = NameReciever.Name;
        }
        error => {
          this.messageService.showError(error);
        }
      }
    );
  }
  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }
  save(isOK: boolean){
      this.meetingService.createCustomerRequimentMeetingContent(this.requirementModel).subscribe(
        data => {
            this.closeModal(isOK);
        },
        error => {
          this.messageService.showError(error);
        }
      );
  }
  getListCustomer() {
    this.comboboxService.getListCustomer().subscribe(
      data => {
        this.listCustomer = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  getCbbDepartment() {
    this.comboboxService.getCbbDepartment().subscribe(
      data => {
        this.listDepartment = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  GetProjectPhaseType() {
    this.comboboxService.getProjectPhaseType().subscribe(data => {
      this.listProjectPhase = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  // generateCode() {
  //   this.customerRequirementService.generateCode().subscribe((data: any) => {
  //     if (data) {
  //       this.requirementModel.Code = data.Code;
  //       this.requirementModel.Index = data.Index;
  //     }
  //   });
  // }
}
