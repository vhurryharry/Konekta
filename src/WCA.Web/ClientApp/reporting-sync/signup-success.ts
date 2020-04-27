import { autoinject } from 'aurelia-framework';
import { RouteConfig, Router } from 'aurelia-router';
import { AppInfoService } from '../services/app-info-service';

@autoinject
export class SignupSuccess {

  constructor(private router: Router,
    private appInfoService: AppInfoService) {
  }
}
