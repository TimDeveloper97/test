import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MaterialgroupCreateComponent } from './materialgroup-create.component';

describe('MaterialgroupCreateComponent', () => {
  let component: MaterialgroupCreateComponent;
  let fixture: ComponentFixture<MaterialgroupCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MaterialgroupCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MaterialgroupCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
