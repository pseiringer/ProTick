import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateSubprocessComponent } from './create-subprocess.component';

describe('CreateSubprocessComponent', () => {
  let component: CreateSubprocessComponent;
  let fixture: ComponentFixture<CreateSubprocessComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateSubprocessComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateSubprocessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
