import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminService } from '../../services/admin.service';
import { NgChartsModule } from 'ng2-charts';
import { ChartConfiguration, ChartData, ChartType } from 'chart.js';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, NgChartsModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  totalDoctors = 0;
  totalPatients = 0;

  pieChartData: ChartData<'pie', number[], string> = {
    labels: [],
    datasets: []
  };

  barChartData: ChartData<'bar'> = {
    labels: [],
    datasets: []
  };

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.adminService.getDashboardStats().subscribe(stats => {
      this.totalDoctors = stats.totalDoctors;
      this.totalPatients = stats.totalPatients;

      const labels = stats.patientsPerDoctor.map((d: any) => d.Name);
      const data = stats.patientsPerDoctor.map((d: any) => d.patientCount);

      // Pie chart
      this.pieChartData = {
        labels,
        datasets: [
          {
            data,
            backgroundColor: [
              '#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0',
              '#9966FF', '#FF9F40', '#66FF66', '#FF6666',
              '#C9CBCF', '#F7464A', '#46BFBD', '#FDB45C'
            ]
          }
        ]
      };

      // Bar chart
      this.barChartData = {
        labels,
        datasets: [
          {
            data,
            label: 'Patients per Doctor',
            backgroundColor: 'rgba(54, 162, 235, 0.7)',
            borderColor: 'rgba(54, 162, 235, 1)',
            borderWidth: 1
          }
        ]
      };
    });
  }
}