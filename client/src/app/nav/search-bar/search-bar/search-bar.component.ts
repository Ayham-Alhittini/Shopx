import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { map, Observable, startWith } from 'rxjs';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SearchBarComponent implements OnInit {

  isCategory: boolean = true;

  constructor(
    private router: Router,
    private route: ActivatedRoute
  ) {
    const category = route.snapshot.paramMap.get('category');
    if(!category || category === 'all')
      this.isCategory = false;
  }

  myControl = new FormControl('', { validators: Validators.required });
  options: string[] = [
    'Computers & Laptops',
    'Pets',
    'Vehicles',
    'Accessories',
    'Mobile & Tablets',
    'Monitor',
    'Computer',
    'Laptop',
    'Cat',
    'Dog',
    'Parrot',
    'Animal Food',
    'Pets Grooming',
    'Bus-MiniVan',
    'Convertible',
    'Coupe',
    'Hatch-Back',
    'Pick-Up',
    'SUV',
    'Sedan',
    'Truck',
    'Mobile',
    'Tablet',
  ];

  filteredOptions!: Observable<string[]>;

  ngOnInit() {
    this.route.queryParamMap.subscribe(paramMap => {
      const searchContent = paramMap['params']['searchContent'];
      this.myControl.setValue(searchContent);
    });
    // this.myControl.valueChanges.subscribe((value) => {
    //   this.router.navigate(
    //   [],
    //   { 
    //     relativeTo: this.route,
    //     queryParams: { searchContent: value },
    //     replaceUrl: true,
    //   }
    //   );
    // });
    //// need to find a way to wait until api gets respones or cancel api call 
    this.filteredOptions = this.myControl.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value || '')),
    );
  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();
    if(value) {
      return this.options.filter(option => option.toLowerCase().includes(filterValue));
    }
    return [];
    
  }

  onSearch() {
    const searchContent = this.myControl.value;

    // if(this.isCategory) {
    //   this.router.navigate(
    //     [],
    //     { 
    //       relativeTo: this.route,
    //       queryParams: { searchContent: searchContent }
    //     }
    //   );
    //   return;
    // }
    this.router.navigate(
      ['/home/all'],
      { queryParams: { searchContent: searchContent }}
    );
  }
}
