import { Injectable } from '@angular/core';
@Injectable({
    providedIn: 'root'
})
export class PermissionService {

    constructor() {

    }

    checkPermission(permission: any[]): boolean {
        var isAuthorize = false;
        var user = localStorage.getItem('qltkcurrentUser');
        if (user) {
            var listPermissionString = JSON.parse(user).permissions;
            var listPermission = listPermissionString != null ? JSON.parse(listPermissionString) : null;
            if (listPermission != null && listPermission.length > 0 && permission && permission.length > 0) {
                for (let index = 0; index < permission.length; index++) {
                    if (!isAuthorize && listPermission.indexOf(permission[index]) != -1) {
                        isAuthorize = true;
                        index = permission.length;
                    }
                }
            }
        }

        if (!permission || permission.length == 0) {
            isAuthorize = true;
        }

        return !isAuthorize
    }
}
