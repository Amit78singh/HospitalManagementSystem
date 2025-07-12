import { Doctor } from './doctor.model';

export interface Patient {
  Id: number;
  Name: string;
  Age: number;
  Disease: string;
  DoctorId: number;
  Doctor?: Doctor; // Optional
}
