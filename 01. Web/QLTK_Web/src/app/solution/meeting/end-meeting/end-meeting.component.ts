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
    Title: 'T???o y??u c???u kh??ch h??ng',
    SaveText: 'L??u',
  };
  isAction: boolean = false;

  columnName: any[] = [{ Name: 'Code', Title: 'M??' }, { Name: 'Name', Title: 'T??n' },];
  columnNameCustomerContact: any[] = [{ Name: 'Name', Title: 'T??n' }, { Name: 'Address', Title: '??ia ch???' },];
  columnNameEmployee: any[] = [{ Name: 'Code', Title: 'M??' }, { Name: 'Name', Title: 'T??n' },];
  columnNameDepartment: any[] = [{ Name: 'Code', Title: 'M??' }, { Name: 'Name', Title: 'T??n' },];
  listCustomer: any[] = [];
  listCustomerContact: any[] = [];
  listDepartment: any[] = [];
  listEmployee: any[] = [];
  listEmployee1: any[] = [];
  listProjectPhase : any[]=[];




  listCustomerRequirementRequestType: any[] = [
    { Id: 1, Name: "L??m m??y" },
    { Id: 2, Name: "Y??u c???u kh??c" },
  ]

  listCustomerRequirementRequestSource: any[] = [
    { Id: 1, Name: "Kh??ch h??ng li??n h???" },
    { Id: 2, Name: "TPA li??n h???" },
    { Id: 3, Name: "Online" },
  ]

  listCustomerRequirementRequestStatus: any[] = [
    { Id: 0, Name: "Y??u c???u kh??ch h??ng" },
    { Id: 1, Name: "Ph??n t??ch y??u c???u kh??ch h??ng" },
    { Id: 2, Name: "kh???o s??t" },
    { Id: 3, Name: "Ph??n t??ch gi???i Ph??p" },
    { Id: 4, Name: "L??m gi???i ph??p" },
    { Id: 5, Name: "L???p d??? to??n" },
    // { Id: 6, Name: "B???o v??? gi???i ph??p" },
    // { Id: 7, Name: "Ch???t gi???i Ph??p" },
    // { Id: 8, Name: "H???y gi???i Ph??p" },
    { Id: 9, Name: "??ang c???n b??? sung th??m th??ng tin"},
  ]

  state : any[] = [
    {Id: 0, Name:"??ang l??m"},
    {Id: 1, Name:"Ho??n th??nh"},
    {Id:2 , Name:"B??? qua"},
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
