import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  userName = '';
  email = '';
  fullName = '';
  password = '';
  confirmPassword = '';
  errorMessage = '';
  successMessage = '';

  constructor(private authService: AuthService, public router: Router){}

  register(form: NgForm) {
    if (form.invalid) {
      this.errorMessage = 'Por favor, preencha todos os campos corretamente.';
      return;
    }
  
    if (this.password !== this.confirmPassword) {
      this.errorMessage = 'As senhas não coincidem!';
      return;
    }
  
    const newUser = {
      userName: this.userName,
      email: this.email,
      fullName: this.fullName,
      password: this.password,
    };
  
    this.authService.register(newUser).subscribe({
      next: () => {
        this.successMessage = 'Cadastro realizado com sucesso!';
        this.errorMessage = '';
        setTimeout(() => this.router.navigate(['/login']), 2000);
      },
      error: (err) => {
        if (err.status === 409 && err.error === 'Esse nome de usuário já está em uso.') {
          this.errorMessage = 'Nome de usuário já está em uso.';
        } else {
          this.errorMessage = 'Erro ao cadastrar. Tente novamente.';
        }
      }
    });
  }
}
