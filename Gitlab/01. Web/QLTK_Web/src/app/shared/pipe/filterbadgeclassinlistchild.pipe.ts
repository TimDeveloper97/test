import { Pipe, PipeTransform } from '@angular/core';


@Pipe({
  name: 'filterbadgeclassinlistchild'
})
export class FilterBadgeClassInListChildPipe {
  transform(value, listargs: any[], child): string {
    let className = '';
    listargs.forEach(item => {
      if (item.Id == value) {
        if (item.BadgeClassList && item.BadgeClassList.length > 0) {
          item.BadgeClassList.forEach(itemClass => {
            if (itemClass.Id == child) {
              className = itemClass.Class;
            }
          });
        }
        else {
          className = item.BadgeClass;
        }
      }
    });

    return className;
  }
}
