import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { InLoadingPopupComponent } from './tools/in-loading-popup/in-loading-popup.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule
    ],
    declarations: [
        InLoadingPopupComponent,
    ],
    exports: [
        CommonModule,
        FormsModule,
        RouterModule,

        InLoadingPopupComponent,
    ],
})
export class SharedModule { }
