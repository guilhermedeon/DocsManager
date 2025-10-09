import { DocumentEntry } from "./document";

export interface DocumentsResponse {
  documents : DocumentEntry[];
  totalCount: number;
}
