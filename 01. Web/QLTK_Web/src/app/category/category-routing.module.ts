import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../auth/guards/auth.guard';
import { CategoryComponent } from './category/category.component';
import { QuoteStepManageComponent } from './quote-step/quote-step-manage/quote-step-manage.component';
import { TechnologySolutionManageComponent } from './technology-solution/technology-solution-manage/technology-solution-manage.component';

const routes: Routes = [
  { path: 'quan-ly-danh-muc', component: CategoryComponent, canActivate: [AuthGuard] },
  { path: 'cac-buoc-bao-gia', component: QuoteStepManageComponent, canActivate: [AuthGuard] },
  { path: 'cong-nghe-cho-giai-phap', component: TechnologySolutionManageComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CategoryRoutingModule { }
