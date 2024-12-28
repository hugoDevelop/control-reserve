import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ManageSpacesComponent } from './manage-spaces.component';
import { SpacesService } from '../../services/spaces.service';

describe('ManageSpacesComponent', () => {
  let component: ManageSpacesComponent;
  let fixture: ComponentFixture<ManageSpacesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, ToastrModule.forRoot(), BrowserAnimationsModule, ManageSpacesComponent], // Importar BrowserAnimationsModule
      providers: [SpacesService]
    }).compileComponents();

    fixture = TestBed.createComponent(ManageSpacesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});