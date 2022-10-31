import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppCareeDetailComponent } from './app-caree-detail.component';

describe('AppCareeDetailComponent', () => {
  let component: AppCareeDetailComponent;
  let fixture: ComponentFixture<AppCareeDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AppCareeDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AppCareeDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
