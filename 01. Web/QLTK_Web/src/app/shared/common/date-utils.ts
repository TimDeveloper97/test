import { Injectable } from '@angular/core';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import * as moment from 'moment';
import { ObjectDate } from './ObjectDate';

@Injectable({
    providedIn: 'root'
})
export class DateUtils {
    constructor() {

    }

    getDateNowYYYYMMDD(): any {
        var dateNow = new Date();
        var month = (dateNow.getMonth() + 1);
        var day = dateNow.getDate();
        return dateNow.getFullYear() + '-' + (month < 10 ? '0' + month : month) + "-" + (day < 10 ? '0' + day : day);
    }

    getDateNowToObject(): any {
        var dateNow = new Date();
        return { year: dateNow.getFullYear(), month: (dateNow.getMonth() + 1), day: dateNow.getDate() };
    }

    getFiscalYearStart(): any {
        var dateNow = new Date();
        return { year: dateNow.getFullYear(), month: 4, day: 1 };
    }

    getFiscalYearEnd(): any {
        var dateNow = new Date();
        return { year: dateNow.getFullYear() + 1, month: 3, day: 31 };
    }

    getObjectDateByDate(date: Date): any {
        return { year: date.getFullYear(), month: (date.getMonth() + 1), day: date.getDate() };
    }

    convertDateToObject(date: string) {
        let temp = date.split('T')[0].split('-');
        return { year: Number(temp[0]), month: Number(temp[1]), day: Number(temp[2]) };
    }

    convertDateStringToDDMMYY(date: string) {
        let temp = date.split('T')[0].split('-');
        return (Number(temp[2]) < 10 ? '0' + Number(temp[2]) : Number(temp[2])) + '/' + (Number(temp[1]) < 10 ? '0' + Number(temp[1]) : Number(temp[1])) + "/" + Number(temp[0]);
    }

    getDateToObject(date: any): any {
        let temp = date.split('-');
        return { year: Number(temp[0]), month: Number(temp[1]), day: Number(temp[2]) };
    }

    convertObjectToDate(object: any) {
        return object.year + "-" + object.month + "-" + object.day;
    }

    convertObjectToDateTime(object: any) {
        return object.getFullYear() + "-" + object.getMonth() + 1 + "-" + object.getDate() + " " + object.getHours() + ":" + object.getMinutes() + ":" + object.getSeconds();
    }

    convertObjectUTCToDate(object: any) {
        let year = object.getFullYear();
        let month = object.getMonth() + 1;
        let date = object.getDate();
        return  year + "-" + month + "-" + date;
    }

    dateDifference(date2, date1) {
        const _MS_PER_DAY = 1000 * 60 * 60 * 24;
    
        // Discard the time and time-zone information.
        const utc1 = Date.UTC(date1.getFullYear(), date1.getMonth(), date1.getDate());
        const utc2 = Date.UTC(date2.getFullYear(), date2.getMonth(), date2.getDate());
    
        return Math.floor((utc2 - utc1) / _MS_PER_DAY);
    }

    convertDateTimeToObject(dateTime: string) {
        let split = dateTime.split('T');
        let date = split[0].split('-');
        let time = split[1].split(':');
        return { year: Number(date[0]), month: Number(date[1]), day: Number(date[2]), hours:Number(time[0]), minutes:Number(time[1]),seconds:Number(time[2])};
    }

    convertObjectToTime(object: any) {
        return object.hour + ":" + object.minute;
    }

    convertObjectToTimeString(object: any) {
        return object.hour + "" + object.minute;
    }

    convertTimeToObject(time: string) {
        let temp = time.split(':');
        return { hour: Number(temp[0]), minute: Number(temp[1]) };
    }

    getNgbDateStructNow(): NgbDateStruct {
        var dateNow = new Date();
        var dateSubtract: NgbDateStruct = {
            year: 1950
            , month: dateNow.getMonth() + 1,
            day: dateNow.getDate()
        };
        return dateSubtract;
    }

    getDateNowStart(): any {
        var dateNow = new Date();
        return { year: dateNow.getFullYear(), month: dateNow.getMonth() + 1, day: 1 };
    }

    getDateNowEnd(): any {
        var dateNow = new Date();
        var dateNext = new Date(dateNow.getFullYear(), dateNow.getMonth() + 1, 0);
        return { year: dateNow.getFullYear(), month: dateNow.getMonth() + 1, day: dateNext.getDate() };
    }

    createObjectDate(year: number, month: number, day: number) {
        var objectDate = new ObjectDate();
        objectDate.day = day;
        objectDate.month = month;
        objectDate.year = year;
        return objectDate;
    }
}
