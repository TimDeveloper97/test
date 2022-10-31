import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateSaleTartgetmentComponent } from './create-update-sale-tartgetment.component';

describe('CreateUpdateSaleTartgetmentComponent', () => {
  let component: CreateUpdateSaleTartgetmentComponent;
  let fixture: ComponentFixture<CreateUpdateSaleTartgetmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateUpdateSaleTartgetmentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdateSaleTartgetmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
