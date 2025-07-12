import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  standalone: true,
  template: ''
})
export class RedirectComponent {
  private router = inject(Router);

  constructor() {
    const token = localStorage.getItem('token');

    if (token) {
      this.router.navigate(['/patients']);
    } else {
      this.router.navigate(['/login']);
    }
  }
}
