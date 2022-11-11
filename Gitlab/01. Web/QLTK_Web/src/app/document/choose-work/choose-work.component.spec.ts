import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseWorkComponent } from './choose-work.component';

describe('ChooseWorkComponent', () => {
  let component: ChooseWorkComponent;
  let fixture: ComponentFixture<ChooseWorkComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChooseWorkComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseWorkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
