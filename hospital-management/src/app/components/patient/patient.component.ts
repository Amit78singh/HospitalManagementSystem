import { Component, OnInit } from '@angular/core';
import { Patient } from '../../models/patient.model';
import { PatientService } from '../../services/patient.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-patient',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './patient.component.html',
  styleUrls: ['./patient.component.scss'],
})
export class PatientComponent implements OnInit {
cancelEdit(): void {
  this.resetForm();
}
downloadCsv(): void {
  this.patientService.downloadCsv();
}
downloadExcel(): void {
  this.patientService.downloadExcel();
}

  patients: Patient[] = [];
  newPatient: Patient = { Id: 0, Name: '', Age: 0, Disease: '', DoctorId: 0 };
  isEditing: boolean = false;

  searchTerm: string = '';
  selectedDisease: string = '';
  selectedDoctorId: number | null = null;
  sortBy: string = '';
  sortOrder: 'asc' | 'desc' = 'asc';

  currentPage = 1;
  pageSize = 10;
  totalPage = 1;

  showValidation: boolean = false;

  constructor(private patientService: PatientService) {}

  ngOnInit(): void {
    this.getAllPatients();
  }

  getAllPatients(): void {
    this.patientService.getAllPatients(
      this.searchTerm,
      this.selectedDisease,
      this.selectedDoctorId,
      this.sortBy,
      this.sortOrder,
      this.currentPage,
      this.pageSize
    ).subscribe ((response: { data: Patient[], totalPages: number }) =>{
      this.patients = response.data;
      this.totalPage = response.totalPages || 1;
    });
  }

  addPatient(): void {
    if (!this.validateForm()) return;

    this.patientService.create(this.newPatient).subscribe(() => {
      this.resetForm();
      this.getAllPatients();
    });
  }

  editPatient(patient: Patient): void {
    this.newPatient = { ...patient };
    this.isEditing = true;
  }

  updatePatient(): void {
    if (!this.validateForm()) return;

    this.patientService.update(this.newPatient.Id, this.newPatient).subscribe(() => {
      this.resetForm();
      this.getAllPatients();
    });
  }

  deletePatient(id: number): void {
    this.patientService.delete(id).subscribe(() => {
      this.getAllPatients();
    });
  }

  resetForm(): void {
    this.newPatient = { Id: 0, Name: '', Age: 0, Disease: '', DoctorId: 0 };
    this.isEditing = false;
    this.showValidation = false;
  }

  onFilterChange(): void {
    this.currentPage = 1;
    this.getAllPatients();
  }

  toggleSort(field: string): void {
    if (this.sortBy === field) {
      this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortBy = field;
      this.sortOrder = 'asc';
    }
    this.getAllPatients();
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPage) return;
    this.currentPage = page;
    this.getAllPatients();
  }

  private validateForm(): boolean {
    this.showValidation = !(
      this.newPatient.Name.trim() &&
      this.newPatient.Age > 0 &&
      this.newPatient.Disease.trim() &&
      this.newPatient.DoctorId > 0
    );
    return !this.showValidation;
  }
}
