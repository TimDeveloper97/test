import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SolutionTabProductComponent } from './solution-tab-product.component';

describe('SolutionTabProductComponent', () => {
  let component: SolutionTabProductComponent;
  let fixture: ComponentFixture<SolutionTabProductComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SolutionTabProductComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SolutionTabProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
