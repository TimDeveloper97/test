import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowEmployeeWorkTabComponent } from './show-employee-work-tab.component';

describe('ShowEmployeeWorkTabComponent', () => {
  let component: ShowEmployeeWorkTabComponent;
  let fixture: ComponentFixture<ShowEmployeeWorkTabComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowEmployeeWorkTabComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowEmployeeWorkTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
