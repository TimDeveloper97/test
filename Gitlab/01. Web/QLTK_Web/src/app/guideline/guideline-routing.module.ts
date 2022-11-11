import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../auth/guards/auth.guard';
import { GuideLineComponent } from './guide-line/guide-line.component';

const routes: Routes = [
  {
    path: 'guideline',
    canActivate: [AuthGuard],
    children: [
      { path: '', component: GuideLineComponent }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GuidelineRoutingModule { }
