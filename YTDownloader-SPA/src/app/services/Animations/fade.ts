import { trigger, transition, style, animate, state } from '@angular/animations';
import { relative } from 'path';

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

export let  translateRight = trigger('translateRight', [
    state('void', style({  transform: 'translateX(-10%)', opacity: 0})),

    transition('void <=> *', [
      animate(500)
    ]),
  ]);

export let  translateLeft = trigger('translateLeft', [
    state('void', style({ transform: 'translateX(10%)', opacity: 0})),

    transition('void <=> *', [
      animate(500)
    ]),
  ]);
