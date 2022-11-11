import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowEmployeedegreesComponent } from './show-employeedegrees.component';

describe('ShowEmployeedegreesComponent', () => {
  let component: ShowEmployeedegreesComponent;
  let fixture: ComponentFixture<ShowEmployeedegreesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowEmployeedegreesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowEmployeedegreesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
