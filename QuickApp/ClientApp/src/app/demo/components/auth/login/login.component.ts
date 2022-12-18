import { Component, OnDestroy, OnInit } from '@angular/core';
import { LayoutService } from 'src/app/layout/service/app.layout.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styles: [`
        :host ::ng-deep .pi-eye,
        :host ::ng-deep .pi-eye-slash {
            transform:scale(1.6);
            margin-right: 1rem;
            color: var(--primary-color) !important;
        }
    `]
})
export class LoginComponent implements OnInit,OnDestroy {

    valCheck: string[] = ['remember'];
    username!: string;
    password!: string;
    loginStatusSubscription: any;
    isModal = false;

    constructor(
        public layoutService: LayoutService,
        private authService : AuthService
        ) { }
    ngOnDestroy(): void {
        if (this.loginStatusSubscription) {
            this.loginStatusSubscription.unsubscribe();
          }
    }
    ngOnInit(): void {
        if (this.getShouldRedirect()) {
            this.authService.redirectLoginUser();
          } else {
            this.loginStatusSubscription = this.authService.getLoginStatusEvent().subscribe(isLoggedIn => {
              if (this.getShouldRedirect()) {
                this.authService.redirectLoginUser();
              }
            });
          }
    }

    getShouldRedirect() {
        return !this.isModal && this.authService.isLoggedIn && !this.authService.isSessionExpired;
      }
    

    login(){
        this.authService.loginWithPassword(this.username,this.password).subscribe(res => {
            
        });
    }
}
