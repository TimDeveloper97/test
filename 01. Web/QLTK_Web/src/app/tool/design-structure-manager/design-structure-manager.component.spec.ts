import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DesignStructureManagerComponent } from './design-structure-manager.component';

describe('DesignStructureManagerComponent', () => {
  let component: DesignStructureManagerComponent;
  let fixture: ComponentFixture<DesignStructureManagerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DesignStructureManagerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DesignStructureManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
