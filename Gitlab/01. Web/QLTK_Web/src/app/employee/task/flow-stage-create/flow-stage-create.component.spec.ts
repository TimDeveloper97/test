import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FlowStageCreateComponent } from './flow-stage-create.component';

describe('FlowStageCreateComponent', () => {
  let component: FlowStageCreateComponent;
  let fixture: ComponentFixture<FlowStageCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FlowStageCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FlowStageCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
