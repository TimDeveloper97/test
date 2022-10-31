import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseFlowStageComponent } from './choose-flow-stage.component';

describe('ChooseFlowStageComponent', () => {
  let component: ChooseFlowStageComponent;
  let fixture: ComponentFixture<ChooseFlowStageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChooseFlowStageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseFlowStageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
