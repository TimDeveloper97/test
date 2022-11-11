import { Component, OnInit, Input, ViewChild, ElementRef, OnDestroy, AfterViewInit } from '@angular/core';
import { MessageService, Configuration, Constants, AppSetting } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PracticeService } from '../../service/practice.service';
import { PracticeMaterialChooseComponent } from '../practice-material-choose/practice-material-choose.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-practice-material',
  templateUrl: './practice-material.component.html',
  styleUrls: ['./practice-material.component.scss']
})

export class PracticeMaterialComponent implements OnInit, OnDestroy, AfterViewInit {
  @Input() Id: string;

  constructor(
    private router: Router,
    private messageService: MessageService,
    private config: Configuration,
    private modalService: NgbModal,
    private practiceService: PracticeService,
    public constant: Constants,
    public appSetting: AppSetting,
  ) { }

  listMaterial: any = [];
  listData: any = [];
  model: any = {
    page: 1,
    PageSize: 10,
    TotalItem: 0,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true,
    Id: '',
    Name: '',
    Quantity: '',
    TotalPrice: '',
    MaterialId: '',
    PracticeId: '',
    listSelect: [],
    MaterialName: '',
    MaterialCode: '',
    Pricing: '',
    DeliveryDay: '',
    Operators: 0,
    MaterialPriceType: 0
  }

  listOperators = [
    { id: '1', value: '<=' },
    { id: '2', value: '>=' },
    { id: '3', value: '<' },
    { id: '4', value: '>' },
  ];
  totalAmount = 0;
  @ViewChild('scrollPracticeMaterial',{static:false}) scrollPracticeMaterial: ElementRef;
  @ViewChild('scrollPracticeMaterialHeader',{static:false}) scrollPracticeMaterialHeader: ElementRef;

  searchOptions: any = {
    FieldContentName: 'MaterialCode',
    Placeholder: 'Tìm kiếm theo mã hoặc tên vật tư ...',
    Items: [
      {
        Name: 'Giá vật tư',
        FieldName: 'Pricing',
        FieldNameType: 'MaterialPriceType',
        Placeholder: 'Nhập giá vật tư ...',
        Type: 'number'
      },
      {
        Name: 'Thời gian giao hàng',
        FieldName: 'DeliveryDay',
        FieldNameType: 'Operators',
        Placeholder: 'Nhập thời gian giao hàng ...',
        Type: 'number'
      },
    ]
  };
  height = 200;

  ngOnInit() {
    this.height = window.innerHeight - 290;
    this.appSetting.PageTitle = "Chỉnh sửa bài thực hành/công đoạn";
    this.model.PracticeId = this.Id;
    //this.searchPrachMaterial();
    this.searchModuleInPractice();
  }

  ngAfterViewInit(){
    this.scrollPracticeMaterial.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollPracticeMaterialHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollPracticeMaterial.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  showClick() {
    let activeModal = this.modalService.open(PracticeMaterialChooseComponent, { container: 'body', windowClass: 'practice-material-model', backdrop: 'static' });
    activeModal.componentInstance.PracticeId = this.model.PracticeId;
    var ListIdSelect = [];
    this.listData.forEach(element => {
      ListIdSelect.push(element.MaterialId);
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

  MaxPricing = 0;
  MaxDeliveryDay = 0;

  searchModuleInPractice() {
    this.model.PracticeId = this.Id;
    this.practiceService.searchModuleInPractice(this.model).subscribe(data => {
      this.listData = data.ListResult;
      this.MaxDeliveryDay = data.MaxDeliveryDay;
      this.MaxPricing = data.MaxPricing;
      this.totalAmount = data.TotalAmount;
    });
  }

  clear() {
    this.model = {
      MaterialName: '',
      MaterialCode: '',
      Pricing: '',
      DeliveryDay: '',
    }
    this.searchModuleInPractice();
  }

  // searchPrachMaterial() {
  //   this.practiceService.searchPracticeMaterial(this.model).subscribe(data => {
  //     this.listData = data.ListResult;
  //   });
  // }

  save() {
    this.listData = this.model.listSelect;
    this.practiceService.createPracticeMaterial(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Lưu thông tin vật tư thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showConfirmDelete(model:any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá vật tư này không?").then(
      data => {
        this.delete(model);
      },
      error => {
        
      }
    );
  }

  delete(model:any) {
    var index = this.listData.indexOf(model);
    if (index > -1) {
      this.listData.splice(index, 1);
    }
  }

  closeModal() {
    this.router.navigate(['thuc-hanh/quan-ly-bai-thuc-hanh']);
  }

  exportExcel() {
    this.practiceService.exportExcelPracticeMaterial(this.model).subscribe(d => {
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
}
