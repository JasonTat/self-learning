import { Component } from '@angular/core';
import { HomeComponent } from './home/home.component';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    HomeComponent    
  ],
  template: `
  <section>
    <form>
      <input type="text" placeholder="Filter by city">
        <button class="primary" type="button">Search</button>
    </form>
  </section>  
  
  `,
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'homes';
}
