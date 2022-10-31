import { Pipe, PipeTransform } from '@angular/core';


@Pipe({
  name: 'filterErrorFix',
  pure: false
})
export class FilterErrorFixPipe implements PipeTransform {
  transform(items: any[], value: string): any {
    if (!items) {
      return items;
    }

    return items.filter(item => item.Id == value);
  }
}
