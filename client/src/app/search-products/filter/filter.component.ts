import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Category } from 'src/app/models/categories';

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss']
})
export class FilterComponent implements OnInit {

  @Input() categories: Category[];
  @Input() model;
  @Input() isGeneric;

  @Output() filterEvent = new EventEmitter();
  
  queryForm: FormGroup;
  detailsForm: FormGroup;
  searchContent: string = '';

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.queryForm = this.fb.group({
      searchContent: [this.searchContent],
      minPrice: [''],
      maxPrice: ['']
    })

    this.route.queryParamMap.subscribe(paramMap => {
      this.searchContent = paramMap['params']['searchContent'];
      if(this.searchContent) {
        this.queryForm.get('searchContent').setValue(this.searchContent);
        this.filter();
      }
    });
  }

  setForm(form: FormGroup) {
    this.detailsForm = form;
  }

  resetFilters() {
    for(let key of Object.keys(this.detailsForm.controls)) {
      this.detailsForm.controls[key].setValue('');
    }
    this.queryForm.reset({ searchContent: '', minPrice: '', maxPrice: '' });

    ////// remove query params from route
    this.router.navigate(
      [],
      { 
        relativeTo: this.route,
        queryParams: {}
      }
    );

    this.filter();
  }

  filter() {
    const searchContent = this.queryForm.get('searchContent').value;
    const minPrice = this.queryForm.get('minPrice').value;
    const maxPrice = this.queryForm.get('maxPrice').value;
    
    //// emit object
    const emitObject = { model: this.detailsForm?.getRawValue(), searchContent: searchContent, minPrice: minPrice, maxPrice: maxPrice };
    this.filterEvent.emit(emitObject);
  }

}
