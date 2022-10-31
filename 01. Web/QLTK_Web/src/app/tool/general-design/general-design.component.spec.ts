import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GeneralDesignComponent } from './general-design.component';

describe('GeneralDesignComponent', () => {
  let component: GeneralDesignComponent;
  let fixture: ComponentFixture<GeneralDesignComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GeneralDesignComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GeneralDesignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
