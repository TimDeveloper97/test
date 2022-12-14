import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AppSetting, MessageService, Constants, ComboboxService } from 'src/app/shared';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { ExpertService } from '../service/expert.service';
import { DegreeCreateComponent } from '../../degree/degree-create/degree-create.component';
import { DegreeService } from '../../degree/service/degree.service';
import { SpecializeService } from '../../specialize/service/specialize.service';
import { SpecializeCreateComponent } from '../../specialize/specialize-create/specialize-create.component';
import { WorkPlaceService } from '../../workplace/service/work-place.service';
import { SelectWorkPlaceComponent } from '../select-work-place/select-work-place/select-work-place.component';
import { SelectSpecializeComponent } from '../select-specialize/select-specialize/select-specialize.component';
import { WorkPlaceCreateComponent } from '../../workplace/work-place-create/work-place-create.component';
import { BankCreateComponent } from '../bank-create/bank-create.component';
import { BankService } from '../service/bank.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-expert-create',
  templateUrl: './expert-create.component.html',
  styleUrls: ['./expert-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ExpertCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private expertService: ExpertService,
    private checkSpecialCharacter: CheckSpecialCharacter,
    public constant: Constants,
    private modalService: NgbModal,
    private degreeService: DegreeService,
    private specializeService: SpecializeService,
    private comboboxService: ComboboxService,
    private workPlaceService: WorkPlaceService,
    private serviceBank: BankService,
    private routeA: ActivatedRoute,
  ) { }

  ngOnInit() {
    if (this.Id) {
      this.ModalInfo.Title = 'Ch???nh s???a chuy??n gia';
      this.ModalInfo.SaveText = 'L??u';
      this.getExpertInfo();
    }
    else {
      this.ModalInfo.Title = "Th??m m???i chuy??n gia";
    }
    this.searchDegree();
    //this.searchSpecialize();
    this.getDegree();
    this.getSpecialize();
    //this.searchWorkPlace();
    this.removeRowWorkPlace(this.Id);

  }

  ModalInfo = {
    Title: 'Th??m m???i chuy??n gia',
    SaveText: 'L??u',
  }

  isAction: boolean = false;
  Id: string;
  listExpert: any[] = []
  DegreeId: String;

  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Description: '',
    Email: '',
    BankAccount: '',
    BankName: '',
    PhoneNumber: '',
    BankAccountName: '',
    DegreeId: '',
    ListSpecialize: [],
    ListWorkPlace: [],
    Status: '1',
    ListBank: [],
  }

  listStatus: any[] = [
    { id: '1', name: '??ang l??m vi???c' },
    { id: '2', name: '???? ngh??? vi???c' },
  ]

  getDegreeId() {
    this.degreeService.getDegree({ Id: this.DegreeId }).subscribe(data => {
      this.modelDegree = data;
    });
  }

  getExpertInfo() {
    this.expertService.getExpert({ Id: this.Id }).subscribe(data => {
      this.model = data;
      this.listBase = data.ListWorkPlace;
      this.listData = data.ListSpecialize;
      this.listBank = data.ListBank;
    });

  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  saveAndContinue() {
    this.save(true);
  }

  createExpert(isContinue) {
    //ki???m tra k?? t??? ?????c vi???t trong M??
    this.model.ListSpecialize = this.listData;
    this.model.ListWorkPlace = this.listBase;
    this.model.ListBank = this.listBank;
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {

      this.addExpert(isContinue);
    } else {
      this.messageService.showConfirmCode("M?? kh??ng ???????c ch???a k?? t??? ?????c bi???t!").then(
        data => {

          this.addExpert(isContinue);
        },
        error => {
          
        }
      );
    }
  }

  addExpert(isContinue) {
    this.expertService.createExpert(this.model).subscribe(
      data => {
        this.listData = [];
        this.listBase = [];
        this.listBank = [];
        if (isContinue) {
          this.isAction = true;
          this.model = {
            Id: '',
            Name: '',
            Code: '',
            Description: '',
            Email: '',
            BankAccount: '',
            BankName: '',
            PhoneNumber: '',
            BankAccountName: '',
            DegreeId: '',
            ListSpecialize: '',
            ListWorkPlace: '',
            ListBank: '',
            Status: '1',
          };
          this.messageService.showSuccess('Th??m m???i chuy??n gia th??nh c??ng!');
        }
        else {
          this.messageService.showSuccess('Th??m m???i chuy??n gia th??nh c??ng!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.updateExpert();
    }
    else {
      this.createExpert(isContinue);
    }
  }

  saveExpert() {
    this.expertService.updateExpert(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('C???p nh???t chuy??n gia th??nh c??ng!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateExpert() {
    //ki???m tra k?? t??? ?????c vi???t trong M??
    this.model.ListWorkPlace = this.listBase;
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.saveExpert();
    } else {
      this.messageService.showConfirmCode("M?? kh??ng ???????c ch???a k?? t??? ?????c bi???t!").then(
        data => {
          this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
          this.saveExpert();
        },
        error => {
          
        }
      );
    }
  }

  showCreateUpdate(temp: boolean, Id) {
    if (temp) {
      //tr??nh ?????
      let activeModal = this.modalService.open(DegreeCreateComponent, { container: 'body', windowClass: 'degree-create-model', backdrop: 'static' })
      //activeModal.componentInstance.Id = DregreeId;
      activeModal.result.then((result) => {
        if (result) {
          this.getDegree();
          this.model.DegreeId = result
        }
      }, (reason) => {
      });
    } else {
      //chuy??n m??n
      let activeModal = this.modalService.open(SpecializeCreateComponent, { container: 'body', windowClass: 'specialize-create-model', backdrop: 'static' })
      activeModal.componentInstance.Id = Id;
      activeModal.result.then((result) => {
        if (result) {
          this.searchSpecialize();
          this.listData.push(result);
        }
      }, (reason) => {
      });
    }
  }

  showCreateUpdateWorkPlace() {
    let activeModal = this.modalService.open(WorkPlaceCreateComponent, { container: 'body', windowClass: 'work-place-create-model', backdrop: 'static' })
    //activeModal.componentInstance.Id = WorkPlaceId;
    activeModal.result.then((result) => {
      if (result) {
        this.searchWorkPlace();
        this.listBase.push(result);
      }
    }, (reason) => {
    });
  }
  ListSelect: any = [];



  StartIndexDegree = 0;
  listDataModelDegree: any[] = [];
  modelDegree: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    DregreeId: '',
    Name: '',
    Code: ''
  }

  listDegree = [];
  searchDegree() {
    this.degreeService.searchDegree(this.modelDegree).subscribe((data: any) => {
      if (data.ListResult) {
        this.StartIndexDegree = ((this.modelDegree.PageNumber - 1) * this.modelDegree.PageSize + 1);
        this.listDataModelDegree = data.ListResult;
        this.modelDegree.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
  getDegree() {
    this.comboboxService.getListDegree().subscribe(
      data => {
        this.listDegree = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showConfirmDeleteDegree(DegreeId: string) {
    this.messageService.showConfirm("B???n c?? ch???c mu???n xo?? tr??nh ????? n??y kh??ng?").then(
      data => {
        this.deleteDegree(DegreeId);
      },
      error => {
        
      }
    );
  }

  deleteDegree(DegreeId: string) {
    this.degreeService.deleteDegree({ Id: DegreeId }).subscribe(
      data => {
        this.searchDegree();
        this.messageService.showSuccess('X??a tr??nh ????? th??nh c??ng!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  ////////////////////////////////////////////////// specialize
  showSelectSpecialize() {
    let activeModal = this.modalService.open(SelectSpecializeComponent, { container: 'body', windowClass: 'select-specialize-model', backdrop: 'static' });
    //var ListIdSelectRequest = [];
    var ListIdSelect = [];
    this.listData.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listData.push(element);
        });
      }
    }, (reason) => {

    });
  }

  ////////////////////////////////////////////////////////////WorkPlace
  ///////////////////////////2
  showSelectWorkPlace() {
    let activeModal = this.modalService.open(SelectWorkPlaceComponent, { container: 'body', windowClass: 'select-work-place-model', backdrop: 'static' });
    //var ListIdSelectRequest = [];
    var ListIdSelect = [];
    this.listBase.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listBase.push(element);
        });
      }
    }, (reason) => {
    });
  }

  showConfirmDeleteWorkPlace(row) {
    this.messageService.showConfirm("B???n c?? ch???c mu???n xo?? ?????a ch??? c??ng t??c n??y kh??ng?").then(
      data => {
        this.removeRowWorkPlace(row);
      },
      error => {
        
      }
    );
  }

  removeRowWorkPlace(row) {
    this.listBase.splice(row, 1);
  }

  showConfirmDeleteSpecialize(row) {
    this.messageService.showConfirm("B???n c?? ch???c mu???n xo?? chuy??n m??n n??y kh??ng?").then(
      data => {
        this.removeSpecialize(row);
      },
      error => {
        
      }
    );
  }

  removeSpecialize(row) {
    this.listData.splice(row, 1);
  }

  listBase: any = [];
  listData: any = [];
  StartIndexSpecialize = 0;
  listSpecialize: any = [];
  listDataModelSpecialize: any[] = [];
  modelSpecialize: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    SpecializeId: '',
    Name: '',
    Code: ''
  }

  searchSpecialize() {
    this.specializeService.searchSpecialize(this.modelSpecialize).subscribe((data: any) => {
      if (data.ListResult) {
        this.StartIndexSpecialize = ((this.modelSpecialize.PageNumber - 1) * this.modelSpecialize.PageSize + 1);
        this.listDataModelSpecialize = data.ListResult;
        this.modelSpecialize.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getSpecialize() {
    this.comboboxService.getListSpecialize().subscribe(
      data => {
        this.listSpecialize = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  StartIndexWorkPlace = 0;
  listDataModelWorkPlace: any[] = [];
  modelWorkPlace: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    WorkPlaceId: '',
    Name: '',
    Code: ''
  }

  listWorkPlace = [];
  searchWorkPlace() {
    this.workPlaceService.searchWorkPlace(this.modelWorkPlace).subscribe((data: any) => {
      if (data.ListResult) {
        this.StartIndexWorkPlace = ((this.modelWorkPlace.PageNumber - 1) * this.modelWorkPlace.PageSize + 1);
        this.listDataModelWorkPlace = data.ListResult;
        this.modelWorkPlace.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getWorkPlace() {
    this.comboboxService.getListWorkPlace().subscribe(
      data => {
        this.listWorkPlace = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  //////////////////////////////////////////////////////////////////////////// TK- ng??n h??ng
  modelBank: any = {
    Id: '',
    ExpertId: '',
    Name: '',
    AccountName: '',
    Account: ''
  }

  listBank: any[] = [];
  showCreateUpdateBank(row, index, isAdd: boolean) {
    let activeModal = this.modalService.open(BankCreateComponent, { container: 'body', windowClass: 'bank-create-model', backdrop: 'static' })
    activeModal.componentInstance.row = Object.assign({}, row);
    activeModal.componentInstance.isAdd = isAdd;
    activeModal.componentInstance.listTemp = this.listBank;
    activeModal.result.then((result) => {
      if (result.modelTemp) {
        if (result.isAdd == true) {
          result.modelTemp.forEach(element => {
            if(this.listBank.length > 0 ){
              this.listBank.forEach(data => {
               if(data.Account  != element.Account){
                this.listBank.push(element);
                this.messageService.showSuccess('Th??m m???i t??i kho???n ng??n h??ng th??nh c??ng!');
               }
               else{
                this.messageService.showSuccess('S??? t??i kho???n kh??ng ???????c tr??ng nhau!');
               }
             });
            }
            else{
              this.listBank.push(element);
            }
          });
        } else {
          this.listBank[index].Name = result.modelTemp.Name;
          this.listBank[index].AccountName = result.modelTemp.AccountName;
          this.listBank[index].Account = result.modelTemp.Account;
        }
      }
    }, (reason) => {
    });
  }

  searchBank() {
    this.serviceBank.searchBank(this.modelBank).subscribe((data: any) => {
      this.listBank = data.data;
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDeleteBank(bankId: string, index) {
    this.messageService.showConfirm("B???n c?? ch???c mu???n xo?? t??i kho???n ng??n h??ng n??y kh??ng?").then(
      data => {
        this.checkDeleteBank(bankId, index);
      },
      error => {
        
      }
    );
  }

  checkDeleteBank(bankId, index) {
    if (bankId == '' || bankId == undefined) {
      this.removeBank(index);
    } else {
      this.expertService.checkDeleteBank(bankId).subscribe((data: any) => {
        this.removeBank(index);
      },
        error => {
          this.messageService.showError(error);
        });
    }
  }

  removeBank(index) {
    this.listBank.splice(index, 1);
  }

  deleteBank(Id: string) {
    this.serviceBank.deleteBank(Id).subscribe(
      data => {
        this.searchBank();
        this.messageService.showSuccess('X??a t??i kho???n ng??n h??ng th??nh c??ng!');
      },
      error => {
        this.messageService.showError(error);
      });
  }
}
