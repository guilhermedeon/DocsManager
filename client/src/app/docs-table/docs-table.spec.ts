import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DocsTable } from './docs-table';

describe('DocsTable', () => {
  let component: DocsTable;
  let fixture: ComponentFixture<DocsTable>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DocsTable]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DocsTable);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
