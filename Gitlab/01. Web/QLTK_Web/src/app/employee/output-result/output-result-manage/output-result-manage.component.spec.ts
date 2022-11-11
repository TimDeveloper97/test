import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OutputResultManageComponent } from './output-result-manage.component';

describe('OutputResultManageComponent', () => {
  let component: OutputResultManageComponent;
  let fixture: ComponentFixture<OutputResultManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OutputResultManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OutputResultManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
