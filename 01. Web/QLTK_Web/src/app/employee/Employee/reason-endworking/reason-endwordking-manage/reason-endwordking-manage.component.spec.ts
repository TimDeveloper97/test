import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReasonEndwordkingManageComponent } from './reason-endwordking-manage.component';

describe('ReasonEndwordkingManageComponent', () => {
  let component: ReasonEndwordkingManageComponent;
  let fixture: ComponentFixture<ReasonEndwordkingManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReasonEndwordkingManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReasonEndwordkingManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
