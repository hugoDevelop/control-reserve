import { Pipe, PipeTransform } from '@angular/core';
import { formatDate } from '@angular/common';

@Pipe({
  name: 'dateFormat'
})
export class DateFormatPipe implements PipeTransform {
  transform(value: string, format: string = 'dd/MM/yyyy hh:mm a'): string {
    if (!value) return '';
    const date = new Date(value);
    return formatDate(date, format, 'en-US');
  }
}