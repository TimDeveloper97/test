import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[appUipermission]'
})
export class UipermissionDirective {

  constructor(private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef) {

  }

  @Input() set appUipermission(permission: any[]) {

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

    if (isAuthorize) {
      this.viewContainer.createEmbeddedView(this.templateRef);

    } else {
      this.viewContainer.clear();
    }
  }

}
