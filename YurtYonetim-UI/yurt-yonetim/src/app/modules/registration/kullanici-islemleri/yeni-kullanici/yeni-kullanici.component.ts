import { UserService } from './../services/user.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { User } from '../models/user.model';
declare var $;
@Component({
  selector: 'app-yeni-kullanici',
  templateUrl: './yeni-kullanici.component.html',
  styleUrls: ['./yeni-kullanici.component.scss'],
})
export class YeniKullaniciComponent implements OnInit {
  userForm: FormGroup;
  user: User = new User();
  constructor(
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private userService: UserService
  ) {}
  ngOnInit() {
    this.createUserForm();
  }

  saveUser() {
    if (this.userForm.valid) {
      let model:User = Object.assign({}, this.userForm.value);
      model.fullName = model.name + ' ' +model.surname;
      
      this.userService.postUser(model).subscribe(
        (response) => {
          console.log(response, 'res');
          this.toastr.success(
            response.success.message,
            'Başarılı!'
          );
        },
        (responseError) => {
          console.log(responseError, 'res');
          this.toastr.error(
                 responseError.error.error.message,
                 'Hata'
               );
        }
      );
    } else {
      this.toastr.error('Zorunlu alanları kontrol ediniz!','Hata');
    }
  }

  createUserForm() {
    this.userForm = this.formBuilder.group({
      name: ['', Validators.required],
      surname: ['', Validators.required],
      phoneNumber: ['', Validators.required],
      email: ['', Validators.required],
      password: ['', Validators.required],
    });
  }
}
