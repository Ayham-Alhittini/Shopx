import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-dynamic-form',
  templateUrl: './dynamic-form.component.html',
  styleUrls: ['./dynamic-form.component.scss']
})
export class DynamicFormComponent implements OnInit, OnDestroy {
  
  @Output() newform = new EventEmitter<FormGroup>();
  
  dynamicFormGroup: FormGroup;

  private _model;

  @Input() 
  set model(value) {
    this._model = value;
    this.buildForm();
    this.emit();
  }
  get model() {
    return this._model;
  }

  fields = [];

  inputChanges: Subscription;
  
  ngOnInit(): void {
    this.buildForm();
    
    this.emit(); // initilize parent form 
    
    this.inputChanges = this.dynamicFormGroup.valueChanges.subscribe(() => {
      this.emit(); // update parent form
    })
  }
  
  ngOnDestroy(): void {
    this.inputChanges.unsubscribe()
  }
  
  buildForm() {
    const formGroupFields = this.getFormControlFields();
    this.dynamicFormGroup = new FormGroup(formGroupFields);
  }

  getFormControlFields() {
    const formGroupFields = {};
    const model = this.model;
    if(model) {
      let varFields = [];
      for(const field of Object.keys(model)) {
        const fieldProps = model[field];
        const validatorsList = this.addValidators(fieldProps.rules);
        const isDisabled = this.checkIfDisabled(fieldProps.rules);
        formGroupFields[field] = new FormControl({ value: fieldProps.value, disabled: isDisabled }, { validators: validatorsList} );
    
        varFields = [ ...varFields, { ...fieldProps, fieldName: field }] ;
      }
      this.fields = varFields;
    }
    return formGroupFields;
  }

  addValidators(rules) {
    if(!rules) {
      return [];
    }

    var validators: ValidatorFn[] = [];

    for(var i = 0; i < rules.length; i++) {
      if(rules[i] === "disabled") {
        continue; 
      }
      validators.push(Validators[rules[i]]);
    }

    return validators;
  }

  checkIfDisabled(rules) {
    for(var i = 0; i < rules.length; i++) {
      if(rules[i] === "disabled") {
        return true;
      }
    }
    
    return false;
  }

  emit() {
    this.newform.emit(this.dynamicFormGroup);
  }

}
