import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { LoginComponent } from './login-sign-in/login/login.component';
import { RouletteComponent } from './roulette/roulette.component';
import { SettingsComponent } from './settings/settings.component';
import { SignInComponent } from './login-sign-in/sign-in/sign-in.component';
import { CasinoStarComponent } from './casino-star/casino-star.component';
import { LoginSignInComponent } from './login-sign-in/login-sign-in.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { AlertComponent } from './login-sign-in/alert/alert.component';
import { OnlyForLogedComponent } from './only-for-loged/only-for-loged.component';
import { CoinFlipComponent } from './coin-flip/coin-flip.component';
import { RemoveAccountComponent } from './settings/remove-account/remove-account.component';
import { RechargeAccountComponent } from './settings/recharge-account/recharge-account.component';
import { ChangeEmailComponent } from './settings/change-email/change-email.component';
import { ChangePasswordComponent } from './settings/change-password/change-password.component';
import { InfoAlertComponent } from './info-alert/info-alert.component';
import { AdminPanelComponent } from './settings/admin-panel/admin-panel.component';

import { UserService } from './services/user.service';
import { GamesService } from './services/games.service';

import { MatExpansionModule } from '@angular/material/expansion';
import { AccordionModule } from 'ngx-bootstrap/accordion';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AdminService } from './services/admin.service';
import { TermsAndPolicyComponent } from './terms-and-policy/terms-and-policy.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    LoginComponent,
    RouletteComponent,
    SettingsComponent,
    SignInComponent,
    LoginSignInComponent,
    NotFoundComponent,
    AlertComponent,
    OnlyForLogedComponent,
    CoinFlipComponent,
    RemoveAccountComponent,
    RechargeAccountComponent,
    ChangePasswordComponent,
    ChangeEmailComponent,
    InfoAlertComponent,
    AdminPanelComponent,
    TermsAndPolicyComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    MatExpansionModule,
    RouterModule.forRoot([
      { path: '', component: CasinoStarComponent, pathMatch: 'full' },
      { path: 'Roulette', component: RouletteComponent },
      { path: 'Coin-Flip', component: CoinFlipComponent },
      { path: 'Settings', component: SettingsComponent },
      { path: 'Login', component: LoginSignInComponent },
      { path: 'Admin-Panel', component: AdminPanelComponent },
      { path: 'Terms-of-Use-and-Private-Policy', component: TermsAndPolicyComponent },
      { path: '**', redirectTo: '/not-found' },
      { path: 'not-found', component: NotFoundComponent }
    ]),
    AccordionModule.forRoot(),
    BrowserAnimationsModule
  ],
  providers: [UserService, GamesService, AdminService],
  bootstrap: [AppComponent],
})
export class AppModule { }