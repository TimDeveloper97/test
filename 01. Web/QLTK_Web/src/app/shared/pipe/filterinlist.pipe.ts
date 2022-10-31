import { Pipe, PipeTransform } from '@angular/core';


@Pipe({
  name: 'filterinlist'
})
export class FilterInListPipe {
  transform(value, listargs: any[]): string {
    let name = '';
    listargs.forEach(item => {
      if (item.Id == value) {
        name = item.Name;
      }
    });

    return name;
  }
}
