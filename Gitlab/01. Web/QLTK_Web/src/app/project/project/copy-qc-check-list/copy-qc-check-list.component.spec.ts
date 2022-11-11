import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CopyQcCheckListComponent } from './copy-qc-check-list.component';

describe('CopyQcCheckListComponent', () => {
  let component: CopyQcCheckListComponent;
  let fixture: ComponentFixture<CopyQcCheckListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CopyQcCheckListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CopyQcCheckListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
