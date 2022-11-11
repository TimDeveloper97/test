import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OutputResultCreateComponent } from './output-result-create.component';

describe('OutputResultCreateComponent', () => {
  let component: OutputResultCreateComponent;
  let fixture: ComponentFixture<OutputResultCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OutputResultCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OutputResultCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
