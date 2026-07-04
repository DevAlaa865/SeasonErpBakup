import { Component, OnInit } from '@angular/core';
;
import { Router } from '@angular/router';
import { ExpenseType } from '../../shared/models/expense-type.model';
import { ExpenseTypeService } from '../../services/Expenses/expense-type.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-expense-type-list',
  imports:[CommonModule],
  standalone: true,
  templateUrl: './expense-type-list.component.html',
})
export class ExpenseTypeListComponent implements OnInit {

  items: ExpenseType[] = [];
  loading = false;

  constructor(
    private service: ExpenseTypeService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.loading = true;
    this.service.getAll().subscribe({
      next: res => {
        this.items = res;
        this.loading = false;
      },
      error: _ => this.loading = false
    });
  }

  goCreate() {
    this.router.navigate(['cash-management/expenses/expense-type/create']);
  }

  goEdit(id: number) {
    this.router.navigate(['cash-management/expenses/expense-type/edit', id]);
  }

  activate(id: number) {
    this.service.activate(id).subscribe(() => this.loadData());
  }

  deactivate(id: number) {
    this.service.deactivate(id).subscribe(() => this.loadData());
  }
}
