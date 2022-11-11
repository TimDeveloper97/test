import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};


@Injectable({
    providedIn: 'root'
})

export class ScheduleProjectService {
    constructor(
        private http: HttpClient,
        private config: Configuration
    ) { }



    createStage(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/stage/choose', model, httpOptions);
        return tr
    }


    getVersionName(): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/getVersionName', httpOptions);
        return tr;
    }

    getListPlanByProjectId(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/GetListPlanByProjectId', model, httpOptions);
        return tr;
    }

    getListPlanByMonth(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/GetListPlanByMonth', model, httpOptions);
        return tr;
    }

    getListProjectProductByProjectId(ProjectId): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/GetListProjectProductByProjectId?ProjectId=' + ProjectId, httpOptions);
        return tr
    }

    getTaskTimeStandByTaskId(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/GetTaskTimeStandardByTaskId', model, httpOptions);
        return tr
    }

    getStatisticalProject(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/GetStatisticalProject', model, httpOptions);
        return tr
    }

    exportExcel(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/ExportExcel', model, httpOptions);
        return tr
    }

    exportExcelProjectSchedule(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/ExportExcelProjectSchedule', model, httpOptions);
        return tr
    }

    importExcel(file): Observable<any> {
        let formData: FormData = new FormData();
        formData.append('File', file);
        //formData.append('ProjectId', projectId);
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/ImportExcel', formData);
        return tr
    }

    updateOTPlan(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/UpdateOTPlan', model, httpOptions);
        return tr
    }

    searchWorkingReport(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/SearchWorkingReport', model, httpOptions);
        return tr;
    }

    exportExcelWorkingReport(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/ExportExcelWorkingReport', model, httpOptions);
        return tr
    }

    searchOverallProject(Id: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'OverallProject/SearchOverallProject/' + Id, httpOptions);
        return tr;
    }

    searchRiskProblemProject(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'OverallProject/searchRiskProblemProject', model, httpOptions);
        return tr;
    }

    createPlanAdjustment(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/createPlanAdjustment', model, httpOptions);
        return tr
    }


    exportExcelProjectPlan(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/ExportExcelProjectPlan', model, httpOptions);
        return tr
    }

    getListEmployee(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/GetListEmployee', model, httpOptions);
        return tr
    }

    getListPlanAdjustment(planId: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/GetListPlanAdjustment?planId=' + planId, httpOptions);
        return tr
    }

    updatePlanAssignment(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/UpdatePlanAssignment', model, httpOptions);
        return tr
    }

    getDataCopy(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/GetDataCopy', model, httpOptions);
        return tr;
    }

    createPlanCopy(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/CreatePlanCopy', model, httpOptions);
        return tr;
    }

    createStageCopy(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/CreateStageCopy', model, httpOptions);
        return tr;
    }

    getListTask(): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'Plans/GetListTask', httpOptions);
        return tr;
    }

    getListHoliday(): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/GetListHoliday', httpOptions);
        return tr;
    }

    getPlanAdjustment(planId: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/get-plan-assignment?planId=' + planId, httpOptions);
        return tr;
    }

    getListOrder(projectProductId: string, stageId: string, type: number): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/GetListOrder?projectProductId=' + projectProductId + '&stageId=' + stageId + '&type=' + type, httpOptions);
        return tr;
    }

    getPlanHistoryInfo(id: string): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/GetPlanHistoryInfo?id=' + id, httpOptions);
        return tr;
    }

    getListSupplier(): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/GetListSuppliers', httpOptions);
        return tr;
    }

    createPlan(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/plan/create', model, httpOptions);
        return tr;
    }

    deletePlan(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/plan/delete', model, httpOptions);
        return tr;
    }

    deleteStage(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/stage/delete', model, httpOptions);
        return tr;
    }

    deleteMultiPlan(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/plan/delete/multiple', model, httpOptions);
        return tr;
    }

    updatePlan(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/plan/update', model, httpOptions);
        return tr;
    }

    modifyPlan(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/plan/modify', model, httpOptions);
        return tr;
    }

    pending(planId: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/plan/pending?planId=' + planId, httpOptions);
        return tr;
    }

    cancel(planId: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/plan/cancel?planId=' + planId, httpOptions);
        return tr;
    }

    resume(planId: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/plan/resume?planId=' + planId, httpOptions);
        return tr;
    }

    ganttChart(model: any): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/GanttChart', model, httpOptions);
        return tr;
    }

    ganttChartHoliday(): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'ScheduleProject/GetListHolidayGanttChart', httpOptions);
        return tr;
    }
}