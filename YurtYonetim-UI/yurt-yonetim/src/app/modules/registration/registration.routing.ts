import { Routes } from '@angular/router';
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service';

export const RegistrationRoutes: Routes = [
    {
      path: '',
      children: [
        {
          path: 'kullanici-islemleri',
          loadChildren: () =>
            import('../../modules/registration/registration.module').then(
              (m) => m.RegistrationModule
            ),
        },
      ],
    },
  ];
  