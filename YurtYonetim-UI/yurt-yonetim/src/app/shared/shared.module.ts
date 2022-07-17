import { ConfirmationDialogComponent } from './tools/confirmation-dialog/confirmation-dialog.component';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ToastrModule } from 'ngx-toastr';
import { InLoadingPopupComponent } from './tools/in-loading-popup/in-loading-popup.component';
import { ConfirmationDialogService } from './services/confirmation-dialog.service';


@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
        ReactiveFormsModule,
    ],
    declarations: [
        InLoadingPopupComponent,
        ConfirmationDialogComponent
    ],
    providers: [ ConfirmationDialogService ],
    exports: [
        CommonModule,
        FormsModule,
        RouterModule,
        ReactiveFormsModule,
        InLoadingPopupComponent,
    ],
    entryComponents: [ ConfirmationDialogComponent ],

})
export class SharedModule { }
