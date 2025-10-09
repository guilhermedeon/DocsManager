import { take } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { DocumentsResponse } from './../Models/documents-response';
import { Component } from '@angular/core';
import { ApiBaseUrl } from '../app.config';

@Component({
  selector: 'app-simplegrid',
  imports: [],
  templateUrl: './simplegrid.html',
  styleUrl: './simplegrid.css',
})
export class Simplegrid {
  documentsResponse!: DocumentsResponse | null;

  constructor(private httpClient: HttpClient) {
    this.getDocuments().pipe(take(1)).subscribe((data) => {
      this.documentsResponse = data;
    });
  }

  getDocuments() {
    return this.httpClient.get<DocumentsResponse>(`${ApiBaseUrl}/Documents`);
  }
}
