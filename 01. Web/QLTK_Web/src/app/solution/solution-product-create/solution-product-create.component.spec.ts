import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SolutionProductCreateComponent } from './solution-product-create.component';

describe('SolutionProductCreateComponent', () => {
  let component: SolutionProductCreateComponent;
  let fixture: ComponentFixture<SolutionProductCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SolutionProductCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SolutionProductCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
