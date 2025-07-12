import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Doctor } from '../../models/doctor.model';
import { DoctorService } from '../../services/doctor.service';

@Component({
  selector: 'app-doctor',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './doctor.component.html',
  styleUrls: ['./doctor.component.scss']
})
export class DoctorComponent implements OnInit {
  doctors: Doctor[] = [];
  newDoctor: Doctor = { Id: 0, Name: '', Specialty: '' };

  searchTerm: string = '';
  selectedSpecialty: string = '';
  sortBy: string = '';
  sortOrder: 'asc' | 'desc' = 'asc';

  currentPage = 1;
  pageSize = 10;
  totalPages = 0;

  showValidation: boolean = false;

  constructor(private doctorService: DoctorService) {}

  ngOnInit(): void {
    this.getAllDoctors();
  }

  getAllDoctors(): void {
    this.doctorService.getAllDoctors({
      searchTerm: this.searchTerm,
      selectedSpecialty: this.selectedSpecialty,
      sortBy: this.sortBy,
      sortOrder: this.sortOrder,
      pageNumber: this.currentPage,
      pageSize: this.pageSize
    }).subscribe((res) => {
      this.doctors = res.data;
      this.totalPages = res.totalPages;
    });
  }

  addDoctor(): void {
    if (!this.newDoctor.Name.trim() || !this.newDoctor.Specialty.trim()) {
      this.showValidation = true;
      return;
    }

    this.doctorService.create(this.newDoctor).subscribe(() => {
      this.newDoctor = { Id: 0, Name: '', Specialty: '' };
      this.showValidation = false;
      this.getAllDoctors();
    });
  }

  deleteDoctor(id: number): void {
    this.doctorService.delete(id).subscribe(() => {
      this.getAllDoctors();
    });
  }

  onFilterChange(): void {
    this.currentPage = 1;
    this.getAllDoctors();
  }

  toggleSort(field: string): void {
    if (this.sortBy === field) {
      this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortBy = field;
      this.sortOrder = 'asc';
    }
    this.getAllDoctors();
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.getAllDoctors();
    }
  }

  prevPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.getAllDoctors();
    }
  }

  downloadCsv(): void {
    this.doctorService.downloadCsv();
  }

  downloadExcel(): void {
    this.doctorService.downloadExcel();
  }
}