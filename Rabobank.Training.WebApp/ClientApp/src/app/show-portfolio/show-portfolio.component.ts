import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-show-portfolio',
  templateUrl: './show-portfolio.component.html',
  styleUrls: ['./show-portfolio.component.css']
})

export class ShowPortfolioComponent implements OnInit{
  public portfolio: Portfolio;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Portfolio>(baseUrl + 'portfolio').subscribe(result => {
      this.portfolio = result;
    }, error => console.error(error));
  }

  ngOnInit(): void {
    this.portfolio = new Portfolio();
    }
}

class Portfolio {
  positions: Position [];
}

class Position {
  code: string;
  name: string;
  value: number;
  mandates: Mandate [];
}

class Mandate {
  name: string;
  allocation: number;
  value: number
}
