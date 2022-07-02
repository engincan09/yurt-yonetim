import { Routes } from '@angular/router';
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service';
import { TumKullanicilarComponent } from './tum-kullanicilar/tum-kullanicilar.component';

export const KullaniciIslemleriRoutes: Routes = [
    {
      path: '',
      children: [
        {
          path: 'tum-kullanicilar',
         canActivate: [AuthGuardService],
          data: { pageId: 5 },
          component: TumKullanicilarComponent,
        },
      ],
    },
  ];
  