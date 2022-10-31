import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InformationErrorComponent } from './information-error.component';

describe('InformationErrorComponent', () => {
  let component: InformationErrorComponent;
  let fixture: ComponentFixture<InformationErrorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InformationErrorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InformationErrorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
