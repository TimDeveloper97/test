import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmOverwriteModalComponent } from './confirm-overwrite-modal.component';

describe('ConfirmOverwriteModalComponent', () => {
  let component: ConfirmOverwriteModalComponent;
  let fixture: ComponentFixture<ConfirmOverwriteModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfirmOverwriteModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmOverwriteModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
