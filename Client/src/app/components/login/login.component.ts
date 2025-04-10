import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  userName = '';
  password = '';
  errorMessage = '';

  constructor(private authService: AuthService, public router: Router) {}

  login() {
    this.authService.login({ userName: this.userName, password: this.password }).subscribe({
      next: (res) => {
        this.authService.saveToken(res.token);
        this.router.navigate(['/projects']);
      },
      error: () => {
        this.errorMessage = 'Usuário ou senha inválidos!';
      }
    });
  }
}
