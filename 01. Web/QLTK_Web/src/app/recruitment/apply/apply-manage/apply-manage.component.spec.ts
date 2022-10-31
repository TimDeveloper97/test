import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplyManageComponent } from './apply-manage.component';

describe('ApplyManageComponent', () => {
  let component: ApplyManageComponent;
  let fixture: ComponentFixture<ApplyManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApplyManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ApplyManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
