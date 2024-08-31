import {
  Directive,
  ElementRef,
  HostListener,
  input,
  Renderer2,
} from '@angular/core';

@Directive({
  selector: '[appMessageInput]',
  standalone: true,
})
export class MessageInputDirective {
  constructor(private elementRef: ElementRef, private renderer: Renderer2) {}

  GetTextWidth(text: string) {
    const span = document.createElement('span');
    span.textContent = text;
    span.style.visibility = 'hidden';
    span.style.whiteSpace = 'nowrap';
    document.body.appendChild(span);

    const width = span.offsetWidth;
    document.body.removeChild(span);

    return width;
  }

  @HostListener('input', ['$event']) OnInput() {
    const inputEl = this.elementRef.nativeElement as HTMLInputElement;
    const text = inputEl.value;
    const height = inputEl.offsetHeight;
    const textWidth = this.GetTextWidth(text!);
  }
}
