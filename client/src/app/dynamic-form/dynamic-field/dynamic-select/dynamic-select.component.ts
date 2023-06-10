import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-dynamic-select',
  templateUrl: './dynamic-select.component.html',
  styleUrls: ['./dynamic-select.component.scss']
})
export class DynamicSelectComponent {
  
  @Input() field;
  @Input() formName: FormGroup;

  getOptions() {
    const field = this.field;
    let value = '';
    value = this.formName.get(field.dependsOn)?.value;

    if(value === "") {
      return [];
    }
    
    const index = field.options.findIndex(obj => obj.name === value );

    if(index === -1) { 
      return ["No Type"];
    }

    return field.options[index].list;
  }
  
}
