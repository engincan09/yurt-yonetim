import { AuthService } from 'src/app/shared/services/auth.service';
import { Component, OnInit } from '@angular/core';
import { LoginUser } from 'src/app/shared/core/dto/general.dto';

@Component({
  selector: 'in-top-bar',
  templateUrl: './top-bar.component.html',
  styleUrls: ['./top-bar.component.scss']
})
export class TopBarComponent implements OnInit {

  loginUser = new LoginUser();
  constructor(private authService:AuthService) { }

  ngOnInit() {
    this.authService.getLoginUser().subscribe((res)=>{
      this.loginUser = res;
    });
  }

  signOut(){
    this.authService.logoutWithRefresh()
  }

}
