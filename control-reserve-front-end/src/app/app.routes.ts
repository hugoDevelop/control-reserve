import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { CreateReservationsComponent } from './pages/create-reservations/create-reservations.component';
import { ManageSpacesComponent } from './pages/manage-spaces/manage-spaces.component';
import { ManageUserComponent } from './pages/manage-user/manage-user.component';

export const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'create-reservation', component: CreateReservationsComponent },
    { path: 'manage-spaces', component: ManageSpacesComponent },
    { path: 'manage-users', component: ManageUserComponent },
    { path: '**', redirectTo: '' }
];
