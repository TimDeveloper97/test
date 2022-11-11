import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectPaymentCreateModalComponent } from './project-payment-create-modal.component';

describe('ProjectPaymentCreateModalComponent', () => {
  let component: ProjectPaymentCreateModalComponent;
  let fixture: ComponentFixture<ProjectPaymentCreateModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectPaymentCreateModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectPaymentCreateModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
