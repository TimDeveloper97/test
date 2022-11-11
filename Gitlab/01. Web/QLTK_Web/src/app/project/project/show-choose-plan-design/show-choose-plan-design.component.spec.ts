import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowChoosePlanDesignComponent } from './show-choose-plan-design.component';

describe('ShowChoosePlanDesignComponent', () => {
  let component: ShowChoosePlanDesignComponent;
  let fixture: ComponentFixture<ShowChoosePlanDesignComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShowChoosePlanDesignComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowChoosePlanDesignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
