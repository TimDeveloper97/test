import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseMaterialGroupModalComponent } from './choose-material-group-modal.component';

describe('ChooseMaterialGroupModalComponent', () => {
  let component: ChooseMaterialGroupModalComponent;
  let fixture: ComponentFixture<ChooseMaterialGroupModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChooseMaterialGroupModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseMaterialGroupModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
