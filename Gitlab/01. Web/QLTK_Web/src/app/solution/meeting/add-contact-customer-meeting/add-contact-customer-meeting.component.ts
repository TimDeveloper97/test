import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FileProcess, MessageService } from 'src/app/shared';
import { MeetingService } from '../../service/meeting.service';

@Component({
  selector: 'app-add-contact-customer-meeting',
  templateUrl: './add-contact-customer-meeting.component.html',
  styleUrls: ['./add-contact-customer-meeting.component.scss']
})
export class AddContactCustomerMeetingComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private meetingService: MeetingService,
    public fileProcess: FileProcess,
    private messageService: MessageService,

  ) { }

  listQuestion: any[] = [];
  isAction: boolean = false;
  model: any = {
    Id:'',
    Name: '',
    Position: '',
    Company: '',
    Phone: '',
    Email: '',
    Avatar :'',
    CustomerId:'',
    Note:'',
  };
  idUpdate: string;
  parentId: string;
  checkType: string;
  check: boolean;
  ModalInfo = {
    Title: 'Thêm mới liên hệ',
    SaveText: 'Lưu',

  };
  CustomerId:'';

  ngOnInit(): void {

    if (this.idUpdate) {
      this.ModalInfo.Title = 'Chỉnh sửa liên hệ';
      this.ModalInfo.SaveText = 'Lưu';
      if(this.checkType) {
        this.check=true;
      }
      else {
        this.check=false;
      }
    }
   
    else {
      this.ModalInfo.Title = "Thêm mới liên hệ";
    }

  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  save(isContinue: boolean) {
    this.model.CustomerId = this.CustomerId;

    if (this.fileProcess.FileDataBase == null) {
      if (this.idUpdate) {
        //this.listQuestion.push(this.model)
        this.meetingService.createMeetingCustomerContact(this.model).subscribe(
          data =>{
          },
          error =>{
    
          }
        );
        this.closeModal(true);
  
      }
      else {
        if (!isContinue) {
          this.meetingService.createMeetingCustomerContact(this.model).subscribe(
            data =>{
            },
            error =>{
      
            }
          );        
          this.closeModal(true);
        }
        else {
          this.meetingService.createMeetingCustomerContact(this.model).subscribe(
            data =>{
            },
            error =>{
      
            }
          );
            this.model = {
            Name: null,
            Position: null,
            Company: null,
            Phone: null,
            Email: null,
            Avatar :'',
          };
        }
  
      }
    }
    else {
      let modelFile = {
        FolderName: 'CustomerContact/'
      };
      this.meetingService.uploadImage(this.fileProcess.FileDataBase, modelFile).subscribe(
        data => {
          this.model.Avatar = data.FileUrl;
          if (this.idUpdate) {
            //this.listQuestion.push(this.model)
            this.meetingService.createMeetingCustomerContact(this.model).subscribe(
              data =>{
              },
              error =>{
        
              }
            );
            this.closeModal(true);
      
          }
          else {
            if (!isContinue) {
              this.meetingService.createMeetingCustomerContact(this.model).subscribe(
                data =>{
                },
                error =>{
          
                }
              );        
              this.closeModal(true);
            }
            else {
              this.meetingService.createMeetingCustomerContact(this.model).subscribe(
                data =>{
                },
                error =>{
          
                }
              );
                this.model = {
                Name: null,
                Position: null,
                Company: null,
                Phone: null,
                Email: null,
                Avatar :'',
              };
            }
      
          }
        },
        error => {
          this.messageService.showError(error);
        });
    }
    

  }

  saveAndContinue() {
    this.save(true);
  }


  //lấy info update
  getProductGroupInfo() {
    this.meetingService.getMeetingCustomerContactInfo({ Id: this.idUpdate }).subscribe(data => {
      this.model = data;
    },
      error => {
        //this.meetingService.showError(error);
      });
  }

  onFileChange($event) {
    this.fileProcess.onAFileChange($event);
  }

  
}
