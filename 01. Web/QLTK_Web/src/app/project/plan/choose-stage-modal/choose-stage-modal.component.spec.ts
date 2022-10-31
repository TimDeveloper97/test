import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseStageModalComponent } from './choose-stage-modal.component';

describe('ChooseStageModalComponent', () => {
  let component: ChooseStageModalComponent;
  let fixture: ComponentFixture<ChooseStageModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChooseStageModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseStageModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
