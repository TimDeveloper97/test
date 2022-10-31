import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NsMaterialGroupUpdateComponent } from './ns-material-group-update.component';

describe('NsMaterialGroupUpdateComponent', () => {
  let component: NsMaterialGroupUpdateComponent;
  let fixture: ComponentFixture<NsMaterialGroupUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NsMaterialGroupUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NsMaterialGroupUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
