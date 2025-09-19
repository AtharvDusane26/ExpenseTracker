# ExpenseTracker
                          +-------------------+
                          |      User UI      |
                          +-------------------+
                                    |
                                    v
                          +-------------------+
                          |   UserManager     |
                          +-------------------+
     Income/Outcome  |         |         | Expense
 (Transactions)      |         |         |
                     v         v         v
          +----------------+  +----------------+  
          | Transaction    |  |  ExpenseManager|  
          |   Manager      |  +----------------+  
          +----------------+           |
             |        |                v
             v        v        +-------------------+
     +----------+  +----------+ |   User.Expenses   |
     | Incomes  |  | Outcomes | +-------------------+
     +----------+  +----------+


                Savings / Goals Module
                -----------------------

                        |
                        v
             +-------------------+
             |  SavingsManager   |
             +-------------------+
                        |
                        v
              +--------------------+
              |  User.Savings      |
              +--------------------+
                        |
                        v
             [Add / Withdraw / Reminders]


                        |
                        v
             +--------------------------+
             | FinancialGoalsManager     |
             +--------------------------+
                        |
                        v
          +--------------------------------+
          |  User.Goals (Financial Goals)  |
          +--------------------------------+
                        |
                        v
         [Allocate Monthly Contribution -> Savings]


                Balance & Reminders
                -------------------
                     |
                     v
          +-----------------------------+
          |  User.Balance / Reminders   |
          +-----------------------------+
