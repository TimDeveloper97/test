import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class NTSModalService {

    constructor() {

    }

    /*
    * Đóng modal khi sử dụng nhiều modal:   
    */
    closeMultiModal() {
        // if (document.querySelector('body > .modal')) {
        //     document.body.classList.add('modal-open');
        // }
    }
}
