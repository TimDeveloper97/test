import { Injectable } from '@angular/core';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import * as printJS from 'print-js';

@Injectable({
    providedIn: 'root'
})
export class AppSetting {
    public PageTitle = '';
    public MenuFolded = false;
    public chatFolded = false;
    private scrollConfig: PerfectScrollbarConfigInterface = {
        suppressScrollX: false,
        suppressScrollY: true,
        minScrollbarLength: 20,
        wheelPropagation: true
    };

    public Pagination = {
        TotalItems: 0,
        CurrentPage: 1,
        NumPerPage: 10,
        MaxSize: 5,
        NumPages: 5
    };

    public CurrentUser = {};

    public Locale_vi: any = {
        firstDayOfWeek: 0,
        dayNames: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],
        dayNamesShort: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
        dayNamesMin: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"],
        monthNames: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
        monthNamesShort: ["T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12"],
        today: 'Hôm nay',
        clear: 'Xóa'
    };

    closedatepicker(event, datepicker) {
        let path = event.path.map(p => p.localName);
        if (!path.includes('ngb-datepicker')) {
            datepicker.close();
        }
    }

    public PrintjsPdf(url: any) {
        printJS({ printable: url, type: 'pdf', showModal: true, modalMessage: 'retrieving...' });
    }
}