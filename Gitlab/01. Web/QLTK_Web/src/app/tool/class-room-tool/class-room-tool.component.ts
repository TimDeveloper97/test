import { Component, OnInit, ViewChild, ElementRef, OnDestroy, ViewEncapsulation, AfterViewInit } from '@angular/core';
import { ClassRoomToolService } from '../services/class-room-tool.service';
import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';
import { SelectSkillComponent } from 'src/app/education/classroom/select-skill/select-skill/select-skill.component';
import { SelectPracticeComponent } from 'src/app/education/classroom/select-practice/select-practice.component';
import { ChooseProductToolComponent } from '../choose-product-tool/choose-product-tool.component';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-class-room-tool',
  templateUrl: './class-room-tool.component.html',
  styleUrls: ['./class-room-tool.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ClassRoomToolComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild('scrollHeaderSkill', { static: false }) scrollHeaderSkill: ElementRef;
  @ViewChild('scrollSkill', { static: false }) scrollSkill: ElementRef;
  @ViewChild('scrollHeaderPractice', { static: false }) scrollHeaderPractice: ElementRef;
  @ViewChild('scrollPractice', { static: false }) scrollPractice: ElementRef;
  @ViewChild('scrollHeaderProduct', { static: false }) scrollHeaderProduct: ElementRef;
  @ViewChild('scrollProduct', { static: false }) scrollProduct: ElementRef;
  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    private service: ClassRoomToolService,
    public constant: Constants
  ) { }

  StartIndex = 0;
  totalItems: number = 0;
  listSkillName: any[] = [];
  listPraticeName: any[] = [];
  listPractice: any[] = [];
  listProduct: any[] = [];
  listSkill: any[] = [];

  ngOnInit() {
    this.appSetting.PageTitle = "Tool phòng học";
    this.getClassRoomToolInfo();
  }

  ngAfterViewInit() {
    this.scrollHeaderSkill.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollSkill.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.scrollHeaderPractice.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollPractice.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.scrollHeaderProduct.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollProduct.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollHeaderSkill.nativeElement.removeEventListener('ps-scroll-x', null);
    this.scrollHeaderPractice.nativeElement.removeEventListener('ps-scroll-x', null);
    this.scrollHeaderProduct.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  onDrop(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.listSkill, event.previousIndex, event.currentIndex);
    moveItemInArray(this.listSkillName, event.previousIndex, event.currentIndex);
    this.listPractice.forEach(element => {
      moveItemInArray(element.ListCheckPracticeSkill, event.previousIndex, event.currentIndex);
    });
  }

  practiceAndSkillModel: any = {
    ListSkill: [],
    ListPractice: []
  }

  practiceAndProductModel: any = {
    ListPractice: [],
    ListProduct: []
  }

  model: any = {
    ListSkill: [],
    ListPractice: [],
    ListProduct: []
  }

  save() {
    this.model.ListSkill = this.listSkill;
    this.model.ListPractice = this.listPractice;
    this.model.ListProduct = this.listProduct;
    this.service.addClassRoomTool(this.model).subscribe((data: any) => {
      if (data) {
        this.messageService.showSuccess('Lưu tool thành công!');
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  showConfirmCreate(index: number) {
    this.messageService.showConfirm("Thêm mới sẽ xóa dữ liệu hiện tại. Bạn có muốn thêm mới không?").then(
      data => {
        this.create();
      },
      error => {
        
      }
    );
  }

  create() {
    this.model.ListSkill = [];
    this.model.ListPractice = [];
    this.model.ListProduct = [];
    this.service.addClassRoomTool(this.model).subscribe((data: any) => {
      this.listSkill = [];
      this.listPractice = [];
      this.listProduct = [];
      this.messageService.showSuccess('Tạo mới thành công!');
    }, error => {
      this.messageService.showError(error);
    });
  }

  showConfirmDeleteSkill(index: number) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá kỹ năng này không?").then(
      data => {
        this.listSkill.splice(index, 1);
        this.getPracticeAndSkill();
        this.messageService.showSuccess('Xóa kỹ năng thành công!');
      },
      error => {
        
      }
    );
  }

  showConfirmDeletePractice(index: number) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá bài thực hành này không?").then(
      async data => {
        this.listPractice.splice(index, 1);
        await this.getPracticeAndSkill();
        await this.getPracticeAndProduct();
        this.messageService.showSuccess('Xóa bài thực hành thành công!');
      }
    );
  }

  showConfirmDeleteProduct(index: number) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá thiết bị này không?").then(
      data => {
        this.listProduct.splice(index, 1);
        //this.getPracticeAndProduct();
        this.messageService.showSuccess('Xóa thiết bị thành công!');
      },
      error => {
        
      }
    );
  }

  getClassRoomToolInfo() {
    this.service.getClassRoomToolInfo().subscribe((data: any) => {
      if (data) {
        this.listSkill = data.ListSkill;
        this.listPractice = data.ListPractices;
        this.listSkillName = data.ListSkillName;
        this.listPraticeName = data.ListPraticeName;
        this.listProduct = data.ListProducts;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  getPracticeAndSkill() {
    this.practiceAndSkillModel.ListSkill = this.listSkill;
    this.practiceAndSkillModel.ListPractice = this.listPractice;

    this.service.getPracticeAndSkill(this.practiceAndSkillModel).subscribe((data: any) => {
      if (data) {
        this.listPractice = data.ListPractices;
        this.listSkillName = data.ListSkillName;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  getPracticeAndProduct() {
    this.practiceAndProductModel.ListProduct = this.listProduct;
    this.practiceAndProductModel.ListPractice = this.listPractice;
    this.service.getPracticeAndProduct(this.practiceAndProductModel).subscribe((data: any) => {
      if (data) {
        this.listPraticeName = data.ListPraticeName;
        this.listProduct = data.ListProducts;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  showConfirmAutoPracticeWithSkill() {
    this.messageService.showConfirm("Bạn có chắc muốn đồng bộ không. Đồng bộ sẽ xóa hết BTH đã chọn?").then(
      data => {
        this.getAutoPracticeWithSkill();
      },
      error => {
        
      }
    );
  }

  getAutoPracticeWithSkill() {
    this.practiceAndSkillModel.ListSkill = this.listSkill;
    this.practiceAndSkillModel.ListPractice = this.listPractice;

    this.service.getAutoPracticeWithSkill(this.practiceAndSkillModel).subscribe((data: any) => {
      if (data) {
        this.listPractice = data.ListPractices;
        this.listSkillName = data.ListSkillName;
        this.listProduct = [];
        this.messageService.showSuccess('Đồng bộ thành công!');
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  showConfirmProductWithPractice() {
    this.messageService.showConfirm("Bạn có chắc muốn đồng bộ không. Đồng bộ sẽ xóa hết thiết bị đã chọn?").then(
      data => {
        this.getAutoProductWithPractice();
      },
      error => {
        
      }
    );
  }

  getAutoProductWithPractice() {
    this.practiceAndProductModel.ListProduct = this.listProduct;
    this.practiceAndProductModel.ListPractice = this.listPractice;
    this.service.getAutoProductWithPractice(this.practiceAndProductModel).subscribe((data: any) => {
      if (data) {
        this.listPraticeName = data.ListPraticeName;
        this.listProduct = data.ListProducts;
        this.messageService.showSuccess('Đồng bộ thành công!');
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  showSkill() {
    let activeModal = this.modalService.open(SelectSkillComponent, { container: 'body', windowClass: 'select-skill-model', backdrop: 'static' })
    var ListIdSelect = [];
    this.listSkill.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          var data = {
            Id: element.Id,
            Name: element.Name,
            Code: element.Code,
            SkillGroupName1: element.SkillGroupName1,
            SkillGroupName2: element.SkillGroupName2,
            SkillGroupName3: element.SkillGroupName3,
            SkillGroupName4: element.SkillGroupName4,
            SkillGroupName5: element.SkillGroupName5,
            SkillGroupName6: element.SkillGroupName6
          }
          this.listSkill.push(data);
        });
        this.getPracticeAndSkill();
      }
    }, (reason) => {

    });
  }

  showPractice() {
    let activeModal = this.modalService.open(SelectPracticeComponent, { container: 'body', windowClass: 'select-practice-model', backdrop: 'static' })
    var ListIdSelect = [];
    this.listPractice.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then(async (result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          var data = {
            Id: element.Id,
            Name: element.Name,
            Code: element.Code
          }
          this.listPractice.push(data);
        });
        await this.getPracticeAndSkill();
        await this.getPracticeAndProduct();
      }
    }, (reason) => {

    });
  }

  showProduct() {
    let activeModal = this.modalService.open(ChooseProductToolComponent, { container: 'body', windowClass: 'select-product-tool-model', backdrop: 'static' })
    var ListIdSelect = [];
    this.listProduct.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          var data = {
            Id: element.Id,
            Name: element.Name,
            Code: element.Code
          }
          this.listProduct.push(data);
        });
        this.getPracticeAndProduct();
      }
    }, (reason) => {

    });
  }
}
