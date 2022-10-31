import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplicationManageComponent } from './application-manage.component';

describe('ApplicationManageComponent', () => {
  let component: ApplicationManageComponent;
  let fixture: ComponentFixture<ApplicationManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ApplicationManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ApplicationManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
