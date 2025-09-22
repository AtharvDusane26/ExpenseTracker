using ExpenseTracker.Model.IncomeSources;
using ExpenseTracker.Model.OutcomeSources;
using ExpenseTracker.Model.Expenses;
using System;
using System.Collections.Generic;
using System.Linq;
using ExpenseTracker.Model.StaticData;

namespace ExpenseTracker.Model.Transactions
{
    internal class TransactionManager
    {
        private readonly ITransactionProvider _user;

        internal TransactionManager(ITransactionProvider user)
        {
            _user = user;
        }
        public IIncomeProvider IncomeProvider
        {
            get => _user as IIncomeProvider;
        }
        public IOutcomeProvider OutcomeProvider
        {
            get => _user as IOutcomeProvider;
        }
        // ---------------- INCOME ----------------
        internal IIncome CreateIncome(IncomeType type, string name, float amount, DateTime? date = null)
        {
            IIncome income = type switch
            {
                IncomeType.Daily => new DailyIncome(name, amount),
                IncomeType.Monthly => new MonthlyIncome(name, amount) { DateOfIncome = date ?? DateTime.Today },
                IncomeType.Yearly => new YearlyIncome(name, amount) { DateOfIncome = date ?? DateTime.Today },
                _ => throw new ArgumentException("Invalid income type")
            };

            IncomeProvider.AddIncome(income);
            Save();
            return income;
        }

        internal void DeleteIncome(string name)
        {
            var income = IncomeProvider.GetIncome(name);
            if (income != null)
                IncomeProvider.DeleteIncome(name);
            Save();
        }

        internal void UpdateIncome(IIncome updatedIncome)
        {
            if (updatedIncome != null)
                IncomeProvider.UpdateIncome(updatedIncome);
            Save();
        }

        internal List<IIncome> GetAllIncomes() => IncomeProvider.Incomes;

        // ---------------- OUTCOME ----------------
        internal IOutcome CreateOutcome(string name, float amount, OutcomeType type, int dayOfTransaction)
        {
            var outcome = new Outcome(name, amount, type);
            outcome.UpdateTransactionDay(dayOfTransaction);
            OutcomeProvider.AddOutcome(outcome);
            Save();
            return outcome;
        }

        internal void DeleteOutcome(string name)
        {
            var outcome = OutcomeProvider.GetOutcome(name);
            if (outcome != null)
                OutcomeProvider.DeleteOutcome(name);
            Save();
        }

        internal void UpdateOutcome(IOutcome updatedOutcome)
        {
            if (updatedOutcome != null)
                OutcomeProvider.UpdateOutcome(updatedOutcome);
            Save();
        }

        internal List<IOutcome> GetAllOutcomes() => OutcomeProvider.Outcomes;

        // ---------------- REMINDERS ----------------
        internal List<string> GetUpcomingReminders(int daysBefore = 3)
        {
            var reminders = new List<string>();
            DateTime today = DateTime.Today;

            foreach (var t in _user.Transactions)
            {
                if (!t.Freeze && t.DayOfTransaction - today.Day <= daysBefore)
                    reminders.Add(t.Remind());
            }
            return reminders;
        }

        // ---------------- DUE OUTCOMES ----------------
        internal void CreateDueOutcomesAsExpense()
        {
            DateTime today = DateTime.Today;
            foreach (var outcome in OutcomeProvider.Outcomes)
            {
                if (!outcome.Freeze && outcome.IsDue(today))
                {
                    var em = new ExpenseManager(_user as User);
                    em.CreateExpense(
                        outcome.Name,
                        outcome.Amount,
                        today,
                        "Paid recurring outcome",
                        "Outcome"
                    );
                }
            }
        }
        private void Save()
        {
            UserManager.Save(_user as User);
        }
    }


}
