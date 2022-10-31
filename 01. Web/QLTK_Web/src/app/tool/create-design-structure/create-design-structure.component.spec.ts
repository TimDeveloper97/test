import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateDesignStructureComponent } from './create-design-structure.component';

describe('CreateDesignStructureComponent', () => {
  let component: CreateDesignStructureComponent;
  let fixture: ComponentFixture<CreateDesignStructureComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateDesignStructureComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateDesignStructureComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
