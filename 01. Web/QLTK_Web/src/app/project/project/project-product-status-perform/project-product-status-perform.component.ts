import { Component, Input, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { count } from 'console';
import { AppSetting, ComboboxService, Constants, DateUtils, MessageService, PermissionService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { ProjectPaymentService } from '../../service/project-payment.service';
import { ProjectProductService } from '../../service/project-product.service';
import { ProjectServiceService } from '../../service/project-service.service';

@Component({
  selector: 'app-project-product-status-perform',
  templateUrl: './project-product-status-perform.component.html',
  styleUrls: ['./project-product-status-perform.component.scss']
})
export class ProjectProductStatusPerformComponent implements OnInit {

  @Input() Id: string;

  constructor(public appSetting: AppSetting,
    private router: Router,
    private messageService: MessageService,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private routeA: ActivatedRoute,
    private titleservice: Title,
    private comboboxService: ComboboxService,
    public constant: Constants,
    public dateUtils: DateUtils,
    public activeModal: NgbActiveModal,
    private projectProductService: ProjectProductService,
    public permissionService: PermissionService,
    public paymentService : ProjectPaymentService,
    //#region thaint
    private modalService: NgbModal,
    ) { }

    ListData : any[] =[];
    ListResult : any[] =[];
    ListStage  : any[] =[];
    model : any ={
      Id :'',
      ProjectId :'',
      ParentId :'',
      ModuleId :'',
      ProductId :'',
      Name :'',
      StageId :'',
      StageName:'',
      StageIndex :'',
      PlanStatus :'',
      PlanWeight :'',
      PlanDoneRatio :'',
    }
    modelStage : any ={
      StageId : '',
      StageName: '',
      StageIndex :'',
    }

    PlanStage: any ={
      ProjectProductName :'',
      ListStage : []
    }

    ListPlan: any[]=[];
    ListPlanResult: any[]=[];
  ngOnInit(): void {
    this.getProjectProductStatusPerform();
  }

  getProjectProductStatusPerform() {
    this.projectProductService.getProjectProductStatusPerform(this.Id).subscribe(
      data => {
        this.ListData = data;
        this.ListData.forEach(element =>{
          var model ={
            Id :element.Id,
            ProjectProductId : element.ProjectProductId,
            ProjectId :element.ProjectId,
            ParentId :element.ParentId,
            ModuleId :element.ModuleId,
            ProductId :element.ProductId,
            Name :element.Name,
            StageId : element.StageId,
            StageName: element.StageName,
            StageIndex :element.StageIndex,
            PlanStatus :element.PlanStatus,
            PlanWeight :element.PlanWeight,
            PlanDoneRatio :element.PlanDoneRatio,
            ListChild: []
          }
          this.ListResult.push(model);
          if(element.StageId != null){
            var modelStage ={
              StageId : element.StageId,
              StageName: element.StageName,
              StageIndex : element.StageIndex,
              ListPlan :[],
              ProjectProductId:'',
              ProjectId:''
            }
            var count =0;
            this.ListStage.forEach(item => {
              if(item.StageId == modelStage.StageId){
                count++;
              }
            });
            if(count ==0){
              this.ListStage.push(modelStage);
            }
          }
        });

        // add project product vào  stage
        this.ListResult.forEach( r => {
          this.ListResult.forEach(r1 =>{
            if(r1.StageId == null && r1.ParentId == r.Id && r1.ProjectProductId == null){
              r.ListChild.push(r1);
            }
          })
        });
        this.ListResult.forEach((item) =>{
          if(item.StageId !=null){
            this.ListPlan.push(item);
          }
        });
        //
        this.ListPlan.forEach( item =>{
          var count =0;
          this.ListPlanResult.forEach( item1 =>{
            if(item1.ProjectProductId == item.ProjectProductId && item1.ProjectId== item.ProjectId){
              count ++;
            }
          });
          if(count == 0){
            var planStage = {
              ProductName : item.Name,
              ProjectProductId : item.ProjectProductId,
              ProjectId : item.ProjectId,
              ListStage : [],
            }
            //
            this.ListStage.forEach(plan=>{
              var modelStage ={
                StageId : plan.StageId,
                StageName: plan.StageName,
                StageIndex : plan.StageIndex,
                ListPlan :[],
                ProjectProductId:item.ProjectProductId,
                ProjectId:item.ProjectId,
                Status : 4,
              }
              planStage.ListStage.push(modelStage);
            });
            //
            this.ListPlanResult.push(planStage);
          }
        });
        //
        this.ListPlanResult.forEach(element =>{
          element.ListStage.forEach(item =>{
            this.ListPlan.forEach(item1 =>{
              if(item1.ProjectProductId == item.ProjectProductId && item1.ProjectId == item.ProjectId ){
                if(item1.StageId == item.StageId){
                  if(item1.ListChild.length > 0 ){
                    item.ListPlan = item1.ListChild;
                    item.IsParent =true;
                    item.PlanParent = item1;

                    var open =1;
                    var countOpen =0;
                    var onGoing =2;
                    var countOnGoing =0;
                    var close =3;
                    var countClose =0;
                    var stop =4;
                    var countStop =0;
                    item1.ListChild.forEach(child =>{
                      if(child.PlanStatus == open){
                        countOpen++;
                      }else if(child.PlanStatus == onGoing){
                        countOnGoing++;
                      }else if(child.PlanStatus == close){
                        countClose++;
                      }else if(child.PlanStatus == stop){
                        countStop++;
                      }
                    });
                    if(countOpen ==item1.ListChild.length){
                      item.Status =open;
                    }else if(countOnGoing ==item1.ListChild.length){
                      item.Status =onGoing;
                    }else if(countClose ==item1.ListChild.length){
                      item.Status =close;
                    }else if(countStop ==item1.ListChild.length){
                      item.Status =stop;
                    }else{
                      item.Status =onGoing;
                    }

                  }else{
                    item.ListPlan.push(item1)
                    item.IsParent =false;
                    item.Status = item1.PlanStatus;
                  }
                }
              }
            });
          });
        });
        // tính done ratio
        this.ListPlanResult.forEach(element =>{
          element.ListStage.forEach(stage =>{
              var totalDoneRatio =0;
              var totalWeight =0;
              stage.ListPlan.forEach(plan =>{
                totalDoneRatio =totalDoneRatio +plan.PlanWeight *plan.PlanDoneRatio;
                totalWeight = totalWeight+plan.PlanWeight;
              });
              if(totalWeight !=0){
                stage.DoneRatio =(totalDoneRatio/totalWeight);
              }else{
                stage.DoneRatio =0;
              }
          });
        });
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

}
