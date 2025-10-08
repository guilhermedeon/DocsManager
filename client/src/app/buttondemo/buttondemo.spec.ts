import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Buttondemo } from './buttondemo';

describe('Buttondemo', () => {
  let component: Buttondemo;
  let fixture: ComponentFixture<Buttondemo>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Buttondemo]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Buttondemo);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
