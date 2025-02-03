import { Injectable } from "@angular/core";

@Injectable()
export class ConfigService {
  host: string;
  port: string;
  authKeyToken: string;

  constructor() {
    this.host = window.location.host;
    this.port = window.location.port;
    this.authKeyToken = "auth_AE5B0A04790C45F199955B1C8454F1D2";
  }

  authKey(): string {
    return this.authKeyToken;
  }

  isProduction() {
    if (this.port === '4200' || this.port === '8100' || this.port === '8200' || this.port === "8080" || this.port === "9090" || this.port === "8500")
      return false;
    return true;
  }

  dataServiceBaseUrl(): string {
    if (this.port === '4200')
      return "http://localhost:5000" + "/api/";;

    if (this.host.indexOf("hireats.com"))
      return "http://" + this.host + "/api/"

    if (!this.isProduction())
      return "http://" + this.host + "/api/"

    return "https://" + this.host + "/api/"
  }

  webSiteUrl(): string {

    if (this.port === '4200')
      return "http://localhost:5000/";


    if (this.host.indexOf("hireats.com"))
      return "http://" + this.host + "/wwwroot/"

    if (!this.isProduction())
      return "http://" + this.host + "/wwwroot/";

    return "https://" + this.host + "/wwwroot/";
  }


}
