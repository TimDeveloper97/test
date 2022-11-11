import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowProductModuleUpdateComponent } from './show-product-module-update.component';

describe('ShowProductModuleUpdateComponent', () => {
  let component: ShowProductModuleUpdateComponent;
  let fixture: ComponentFixture<ShowProductModuleUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShowProductModuleUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowProductModuleUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
