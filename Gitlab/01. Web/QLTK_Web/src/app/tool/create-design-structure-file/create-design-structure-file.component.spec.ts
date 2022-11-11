import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateDesignStructureFileComponent } from './create-design-structure-file.component';

describe('CreateDesignStructureFileComponent', () => {
  let component: CreateDesignStructureFileComponent;
  let fixture: ComponentFixture<CreateDesignStructureFileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateDesignStructureFileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateDesignStructureFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
