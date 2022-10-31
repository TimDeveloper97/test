import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NsMaterialGroupCreateComponent } from './ns-material-group-create.component';

describe('NsMaterialGroupCreateComponent', () => {
  let component: NsMaterialGroupCreateComponent;
  let fixture: ComponentFixture<NsMaterialGroupCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NsMaterialGroupCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NsMaterialGroupCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
