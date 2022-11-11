import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowProjectErrorsComponent } from './show-project-errors.component';

describe('ShowProjectErrorsComponent', () => {
  let component: ShowProjectErrorsComponent;
  let fixture: ComponentFixture<ShowProjectErrorsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowProjectErrorsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowProjectErrorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
