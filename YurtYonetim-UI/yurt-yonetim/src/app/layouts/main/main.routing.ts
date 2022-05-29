import { Routes } from '@angular/router';

import { MainHomeComponent } from './main-home/main-home.component';
import { MainComponent } from './main.component';

export const MainRoutes: Routes = [
  {
    path: '',
    component: MainComponent,
    children: [
      {
        path: 'idari-isler',
        loadChildren: () =>
          import('../../modules/registration/registration.module').then(
            (m) => m.RegistrationModule
          ),
      },
      {
        path: '**',
        component: MainHomeComponent,
      },
    ],
  },
];
