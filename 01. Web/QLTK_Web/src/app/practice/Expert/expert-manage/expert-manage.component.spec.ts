import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpertManageComponent } from './expert-manage.component';

describe('ExpertManageComponent', () => {
  let component: ExpertManageComponent;
  let fixture: ComponentFixture<ExpertManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExpertManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExpertManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
