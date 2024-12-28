import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-form',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.sass']
})
export class FormComponent implements OnInit {
  @Input() fields: any[] = [];
  @Input() validations: any = {};
  @Input() initialData: any = {};
  @Input() title: string = 'Formulario';
  @Input() submitLabel: string = 'Enviar';
  @Output() submitForm = new EventEmitter<any>();

  form: FormGroup;
  isLoading: boolean = false;

  constructor(private formBuilder: FormBuilder) {
    this.form = this.formBuilder.group({}); // Inicializar como un FormGroup vacío en el constructor
  }

  ngOnInit() {
    this.initializeForm();
  }

  initializeForm() {
    const formGroupConfig = this.fields.reduce((config, field) => {
      config[field.name] = [{ value: this.initialData[field.name] || '', disabled: field.disabled || false }, this.validations[field.name] || []];
      return config;
    }, {});

    this.form = this.formBuilder.group(formGroupConfig);
  }

  onSubmit() {
    if (this.form.valid) {
      this.submitForm.emit(this.form.value);
    }
  }
}