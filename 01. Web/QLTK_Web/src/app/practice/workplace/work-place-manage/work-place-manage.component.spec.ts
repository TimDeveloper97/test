import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkPlaceManageComponent } from './work-place-manage.component';

describe('WorkPlaceManageComponent', () => {
  let component: WorkPlaceManageComponent;
  let fixture: ComponentFixture<WorkPlaceManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkPlaceManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkPlaceManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
