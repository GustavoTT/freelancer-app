import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AuthService } from './services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  constructor(private authService: AuthService, private router: Router){}

  isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }

  logout(){
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
