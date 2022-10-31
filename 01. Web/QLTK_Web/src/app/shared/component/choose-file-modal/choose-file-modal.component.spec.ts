import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseFileModalComponent } from './choose-file-modal.component';

describe('ChooseFileModalComponent', () => {
  let component: ChooseFileModalComponent;
  let fixture: ComponentFixture<ChooseFileModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChooseFileModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseFileModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
