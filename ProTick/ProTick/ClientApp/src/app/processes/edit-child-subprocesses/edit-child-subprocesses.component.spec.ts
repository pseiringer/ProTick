import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditChildSubprocessesComponent } from './edit-child-subprocesses.component';

describe('EditChildSubprocessesComponent', () => {
  let component: EditChildSubprocessesComponent;
  let fixture: ComponentFixture<EditChildSubprocessesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditChildSubprocessesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditChildSubprocessesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
