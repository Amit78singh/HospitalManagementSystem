import { Patient } from './patient.model';

export interface Doctor {
  Id: number;
  Name: string;
  Specialty: string;
  Patients?: Patient[]; // Optional
}
