import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Patient } from '../models/patient.model';
import { Doctor } from '../models/doctor.model';
import { Observable ,tap} from 'rxjs';
import { HttpParams } from '@angular/common/http';  

@Injectable({
  providedIn: 'root'
})
export class PatientService {
  private apiUrl = 'https://localhost:7064/api/Patients';

  message: string = '';

  constructor(private http: HttpClient) {}


  setMessage(msg: string) {
    this.message = msg;
    setTimeout(() => this.message = '', 3000);
  }

 getAllPatients(
  search?: string,
  disease?: string,
  doctorId?: number | null,
  sortBy?: string,
  sortOrder: 'asc' | 'desc' = 'asc',
  pageNumber :number=1,
  pageSize:number=10
): Observable<{data: Patient[]; totalPages :number}> {
  let params = new HttpParams();
 

  if (search) params = params.set('search', search);
  if (disease) params = params.set('disease', disease);
  if (doctorId !== null && doctorId !== undefined) params = params.set('doctorId', doctorId.toString());
  if (sortBy) params = params.set('sortBy', sortBy);
  if (sortOrder) params = params.set('sortOrder', sortOrder);

  params = params.set('pageNumber', pageNumber);
  params = params.set('pageSize', pageSize);

  return this.http.get<{data:Patient[]; totalPages:number}>(`${this.apiUrl}`, { params });
}



  get(id: number): Observable<Patient> {
    return this.http.get<Patient>(`${this.apiUrl}/${id}`);
  }

  create(patient: Patient): Observable<Patient> {
    return this.http.post<Patient>(this.apiUrl, patient).pipe(
      tap(() => this.setMessage('✅ Patient added successfully!'))
    );
    
  }

  update(id: number, patient: Patient): Observable<Patient> {
    return this.http.put<Patient>(`${this.apiUrl}/${id}`, patient).pipe(
      tap(() => this.setMessage('✏️ Patient updated!'))
    );
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
      tap(() => this.setMessage('🗑️ Patient deleted.'))
    );

  }


downloadCsv(): void {
  this.http.get('https://localhost:7064/api/Patients/export', {
    responseType: 'blob' // important!
  }).subscribe(blob => {
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = 'patients.csv';
    a.click();
    window.URL.revokeObjectURL(url);
  });
 
}
downloadExcel(): void {
  this.http.get('https://localhost:7064/api/Patients/export-excel', { responseType: 'blob' })
    .subscribe(blob => {
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = 'patients.xlsx';
      a.click();
      window.URL.revokeObjectURL(url);
    });
  }
}
