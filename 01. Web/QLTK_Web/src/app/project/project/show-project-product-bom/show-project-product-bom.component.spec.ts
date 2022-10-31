import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowProjectProductBomComponent } from './show-project-product-bom.component';

describe('ShowProjectProductBomComponent', () => {
  let component: ShowProjectProductBomComponent;
  let fixture: ComponentFixture<ShowProjectProductBomComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowProjectProductBomComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowProjectProductBomComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
