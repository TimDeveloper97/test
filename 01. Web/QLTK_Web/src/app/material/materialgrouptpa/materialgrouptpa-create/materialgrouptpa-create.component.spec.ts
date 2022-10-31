import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MaterialgrouptpaCreateComponent } from './materialgrouptpa-create.component';

describe('MaterialgrouptpaCreateComponent', () => {
  let component: MaterialgrouptpaCreateComponent;
  let fixture: ComponentFixture<MaterialgrouptpaCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MaterialgrouptpaCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MaterialgrouptpaCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
