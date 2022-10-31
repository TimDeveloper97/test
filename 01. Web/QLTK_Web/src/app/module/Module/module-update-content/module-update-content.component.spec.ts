import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuleUpdateContentComponent } from './module-update-content.component';

describe('ModuleUpdateContentComponent', () => {
  let component: ModuleUpdateContentComponent;
  let fixture: ComponentFixture<ModuleUpdateContentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModuleUpdateContentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModuleUpdateContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
