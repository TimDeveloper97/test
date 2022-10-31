import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReasonEndworkingCreateComponent } from './reason-endworking-create.component';

describe('ReasonEndworkingCreateComponent', () => {
  let component: ReasonEndworkingCreateComponent;
  let fixture: ComponentFixture<ReasonEndworkingCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReasonEndworkingCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReasonEndworkingCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
