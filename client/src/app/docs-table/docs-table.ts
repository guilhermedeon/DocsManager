import { DocumentsResponse } from './../Models/documents-response';
import { Component } from '@angular/core';
import { TableModule } from 'primeng/table';
import { HttpClient } from '@angular/common/http';
import { ApiBaseUrl } from '../app.config';
import { take } from 'rxjs';

@Component({
  selector: 'app-docs-table',
  imports: [TableModule],
  templateUrl: './docs-table.html',
  styleUrl: './docs-table.css',
})
export class DocsTable {
  documentsResponse: DocumentsResponse = { documents: [], totalCount: 0 };

  constructor(private http: HttpClient) {
    http
      .get<DocumentsResponse>(`${ApiBaseUrl}/Documents`)
      .pipe(take(1))
      .subscribe((data: DocumentsResponse) => {
        this.documentsResponse = data;
      });
  }
}
