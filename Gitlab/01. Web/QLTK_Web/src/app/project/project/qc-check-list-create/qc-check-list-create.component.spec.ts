import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QcCheckListCreateComponent } from './qc-check-list-create.component';

describe('QcCheckListCreateComponent', () => {
  let component: QcCheckListCreateComponent;
  let fixture: ComponentFixture<QcCheckListCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ QcCheckListCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(QcCheckListCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
