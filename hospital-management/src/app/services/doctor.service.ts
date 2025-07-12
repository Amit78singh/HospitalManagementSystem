import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Doctor } from '../models/doctor.model';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {
  private apiUrl = 'https://localhost:7064/api/Doctors';
  message: string = '';

  constructor(private http: HttpClient) {}

  setMessage(msg: string) {
    this.message = msg;
    setTimeout(() => this.message = '', 3000);
  }

  getAllDoctors(filters?: {
    searchTerm?: string;
    selectedSpecialty?: string;
    sortBy?: string;
    sortOrder?: 'asc' | 'desc';
    pageNumber?: number;
    pageSize?: number;
  }): Observable<{ data: Doctor[], totalPages: number }> {
    let params = new HttpParams();

    if (filters?.searchTerm) params = params.set('search', filters.searchTerm);
    if (filters?.selectedSpecialty) params = params.set('specialty', filters.selectedSpecialty);
    if (filters?.sortBy) params = params.set('sortBy', filters.sortBy);
    if (filters?.sortOrder) params = params.set('sortOrder', filters.sortOrder);
    if (filters?.pageNumber) params = params.set('pageNumber', filters.pageNumber);
    if (filters?.pageSize) params = params.set('pageSize', filters.pageSize);

    return this.http.get<{ data: Doctor[], totalPages: number }>(this.apiUrl, { params });
  }

  create(doctor: Doctor): Observable<Doctor> {
    return this.http.post<Doctor>(this.apiUrl, doctor).pipe(
      tap(() => this.setMessage('✅ Doctor added successfully!'))
    );
  }

  update(id: number, doctor: Doctor): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, doctor).pipe(
      tap(() => this.setMessage('✏️ Doctor updated!'))
    );
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
      tap(() => this.setMessage('🗑️ Doctor deleted.'))
    );
  }

  downloadCsv(): void {
    this.http.get(`${this.apiUrl}/export`, { responseType: 'blob' }).subscribe(blob => {
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = 'doctors.csv';
      a.click();
      window.URL.revokeObjectURL(url);
    });
  }

  downloadExcel(): void {
    this.http.get(`${this.apiUrl}/export-excel`, { responseType: 'blob' }).subscribe(blob => {
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = 'doctors.xlsx';
      a.click();
      window.URL.revokeObjectURL(url);
    });
  }
}
