import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  imports: [CommonModule, ReactiveFormsModule, RouterModule]
})
export class RegisterComponent {
  form: FormGroup;

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
       role: ['Select', Validators.required],
    });
  }

  register() {
    if (this.form.invalid) return;

    const user = {
      email: this.form.value.email,
      passwordHash: this.form.value.password,
       role: this.form.value.role
    };

    this.http.post('https://localhost:7064/api/Auth/register', user,{responseType:'text'}).subscribe({
      next: () => {
        alert('Registration successful!');
        this.router.navigate(['/login']);
      },
        error: (err) => {
    console.error(err);
    const backendMessage = err?.error;
    if (typeof backendMessage === 'string') {
      alert(`Registration failed: ${backendMessage}`);
    } else {
      alert('Registration failed. Please try again.');
    }
    }
    });
  }
}
