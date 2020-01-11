import { trigger, transition, style, animate, state } from '@angular/animations';

export let  fade = trigger('fade', [
    state('void', style({ opacity: 0 })),

    transition('void <=> *', [
      animate(2000)
    ]),
  ]);
