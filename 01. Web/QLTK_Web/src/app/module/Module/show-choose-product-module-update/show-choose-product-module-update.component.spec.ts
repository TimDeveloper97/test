import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowChooseProductModuleUpdateComponent } from './show-choose-product-module-update.component';

describe('ShowChooseProductModuleUpdateComponent', () => {
  let component: ShowChooseProductModuleUpdateComponent;
  let fixture: ComponentFixture<ShowChooseProductModuleUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShowChooseProductModuleUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowChooseProductModuleUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
