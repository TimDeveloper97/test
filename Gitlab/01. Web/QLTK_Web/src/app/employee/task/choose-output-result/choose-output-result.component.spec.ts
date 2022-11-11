import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseOutputResultComponent } from './choose-output-result.component';

describe('ChooseOutputResultComponent', () => {
  let component: ChooseOutputResultComponent;
  let fixture: ComponentFixture<ChooseOutputResultComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChooseOutputResultComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseOutputResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
