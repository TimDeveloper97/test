import { Pipe, PipeTransform } from '@angular/core';


@Pipe({
  name: 'filtertextclassinlist'
})
export class FilterTextClassInListPipe {
  transform(value, listargs: any[]): string {
    let className = '';
    listargs.forEach(item=>{
      if(item.Id == value){
        className = item.TextClass;
      }
    });

    return className;
  }
}
