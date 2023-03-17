import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {AuthResponseData, AuthService} from "../services/auth/auth.service";
import {Observable} from "rxjs";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent implements OnInit{
  loggingForm!: FormGroup;
  isLoginMode: boolean = true;
  error: string | null = null;
  isLoggedin = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute) {}
  async ngOnInit(){
    this.loggingForm = new FormGroup({
      name: new FormControl(null, {
        nonNullable: true, validators: [
          Validators.required
        ]
      }),
      password: new FormControl(null, {
        nonNullable: true, validators: [
          Validators.required,
          Validators.minLength(4),
        ]
      }),
    });
  }

  onSwitchMode(){
    this.isLoginMode = !this.isLoginMode;
  }

  onSubmit(){
    if(this.loggingForm.invalid){
      return;
    }

    let authObserve: Observable<AuthResponseData>;

    if(this.isLoginMode){
      authObserve = this.authService.login(this.loggingForm.value);
    } else {
      authObserve = this.authService.signup(this.loggingForm.value);
    }

    authObserve.subscribe({
      next: () => {
        this.error = null;
        this.isLoggedin = true;
        //this.router.navigate(['home']);
      },
      error: (errorMessage: string) => {
        this.error = errorMessage;
        alert(this.error);
      }
    });

    this.loggingForm.reset();
  }

  toHome(){
    this.router.navigate(['home'],{relativeTo: this.route});
  }
}
