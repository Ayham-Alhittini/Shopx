// import { Injectable } from '@angular/core';
// import {
//   HttpRequest,
//   HttpHandler,
//   HttpEvent,
//   HttpInterceptor
// } from '@angular/common/http';
// import { delay, EMPTY, finalize, identity, Observable } from 'rxjs';
// import { BusyService } from '../_services/busy.service';
// import { environment } from 'src/environments/environment';

// @Injectable()
// export class LoadingInterceptor implements HttpInterceptor {

//   constructor(private busyService : BusyService) {}

//   intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {

//     ///don't show loading for messages
//     var url = request.url;
//     if (url.includes(environment.apiBase + 'messages/thread/'))
//     {
//       ///don't make same request while the last one not response yet
//       if (this.busyService.chatRequestState)
//       {
//         console.log('Busy now');
        
//         return EMPTY;
//       }
//       else
//       {
//         console.log('chat loading...');
        
//         this.busyService.chatRequestState = true;
//         return next.handle(request).pipe(
//           finalize(()=> {
//             this.busyService.chatRequestState = false;
//           })
//         );
//       }
//     }
//     this.busyService.busy();
//     return next.handle(request).pipe(
//       (environment.production ? identity : delay(500)),
//       finalize(()=> {
//         this.busyService.idle();
//       })
//     );
//   }
// }
