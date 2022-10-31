import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccessProductViewComponent } from './access-product-view.component';

describe('AccessProductViewComponent', () => {
  let component: AccessProductViewComponent;
  let fixture: ComponentFixture<AccessProductViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccessProductViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccessProductViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
