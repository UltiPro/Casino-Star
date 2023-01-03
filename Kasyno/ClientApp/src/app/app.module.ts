import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { BlackjackComponent } from './blackjack/blackjack.component';
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

import { UserService } from './services/user.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    BlackjackComponent,
    LoginComponent,
    RouletteComponent,
    SettingsComponent,
    SignInComponent,
    LoginSignInComponent,
    NotFoundComponent,
    AlertComponent,
    OnlyForLogedComponent,
    CoinFlipComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: CasinoStarComponent, pathMatch: 'full' },
      { path: 'BlackJack', component: BlackjackComponent },
      { path: 'Roulette', component: RouletteComponent },
      { path: 'Coin-Flip', component: CoinFlipComponent },
      { path: 'Settings', component: SettingsComponent },
      { path: 'Login', component: LoginSignInComponent },
      { path: '**', redirectTo: '/not-found' },
      { path: 'not-found', component: NotFoundComponent }
    ]),
  ],
  providers: [UserService],
  bootstrap: [AppComponent],
})
export class AppModule { }