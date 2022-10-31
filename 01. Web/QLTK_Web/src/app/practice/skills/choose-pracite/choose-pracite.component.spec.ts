import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChoosePraciteComponent } from './choose-pracite.component';

describe('ChoosePraciteComponent', () => {
  let component: ChoosePraciteComponent;
  let fixture: ComponentFixture<ChoosePraciteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChoosePraciteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChoosePraciteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
