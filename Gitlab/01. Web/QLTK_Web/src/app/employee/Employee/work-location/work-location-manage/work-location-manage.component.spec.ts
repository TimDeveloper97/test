import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkLocationManageComponent } from './work-location-manage.component';

describe('WorkLocationManageComponent', () => {
  let component: WorkLocationManageComponent;
  let fixture: ComponentFixture<WorkLocationManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WorkLocationManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkLocationManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
