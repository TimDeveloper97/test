import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MaterialCreateComponent } from './material-create.component';

describe('MaterialCreateComponent', () => {
  let component: MaterialCreateComponent;
  let fixture: ComponentFixture<MaterialCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MaterialCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MaterialCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
