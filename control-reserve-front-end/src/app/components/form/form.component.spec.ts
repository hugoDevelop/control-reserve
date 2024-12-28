import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule, Validators } from '@angular/forms';
import { FormComponent } from './form.component';

describe('FormComponent', () => {
  let component: FormComponent;
  let fixture: ComponentFixture<FormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, FormComponent] // Importar FormComponent en lugar de declararlo
    }).compileComponents();

    fixture = TestBed.createComponent(FormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form with the given fields and validations', () => {
    component.fields = [
      { name: 'name', label: 'Name', type: 'text', required: true },
      { name: 'email', label: 'Email', type: 'email', required: true }
    ];
    component.validations = {
      name: [Validators.required],
      email: [Validators.required, Validators.email]
    };
    component.initialData = {
      name: 'John Doe',
      email: 'john.doe@example.com'
    };

    component.ngOnInit();
    fixture.detectChanges();

    expect(component.form.get('name')?.value).toBe('John Doe');
    expect(component.form.get('email')?.value).toBe('john.doe@example.com');
  });

  it('should emit the form value on submit', () => {
    spyOn(component.submitForm, 'emit');

    component.fields = [
      { name: 'name', label: 'Name', type: 'text', required: true }
    ];
    component.validations = {
      name: [Validators.required]
    };
    component.initialData = {
      name: 'John Doe'
    };

    component.ngOnInit();
    fixture.detectChanges();

    component.onSubmit();
    expect(component.submitForm.emit).toHaveBeenCalledWith({ name: 'John Doe' });
  });
});