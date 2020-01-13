import { trigger, transition, style, animate, state, animation } from '@angular/animations';

export let  fade = trigger('fade', [
    state('void', style({ opacity: 0 })),

    transition('void <=> *', [
      animate(1000)
    ]),
  ]);

export let  fadeFast = trigger('fadeFast', [
    state('void', style({ opacity: 0 })),

    transition('void <=> *', [
      animate(300)
    ]),
  ]);
