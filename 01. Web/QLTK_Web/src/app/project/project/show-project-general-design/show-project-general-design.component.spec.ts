import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowProjectGeneralDesignComponent } from './show-project-general-design.component';

describe('ShowProjectGeneralDesignComponent', () => {
  let component: ShowProjectGeneralDesignComponent;
  let fixture: ComponentFixture<ShowProjectGeneralDesignComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowProjectGeneralDesignComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowProjectGeneralDesignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
