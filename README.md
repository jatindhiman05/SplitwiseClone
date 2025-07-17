# SplitwiseClone – Expense Management System 🧾

A console-based C# application that replicates the core logic of the Splitwise app — allowing users to share expenses, manage debts, and simplify settlements within groups or individually. Built using .NET 8 and clean object-oriented principles.

---

## 🚀 Features

- ✅ Add group or individual expenses
- ➗ Split expenses by **Equal**, **Exact**, or **Percentage**
- 🔔 Real-time notifications using **Observer pattern**
- 🤝 Prevents users from leaving a group without clearing dues
- 💳 Group-wise expense tracking and real-time balances
- 🔄 Greedy debt simplification to minimize transactions
- ♻️ Modular, extensible, and follows **SOLID** design principles

---

## 🔧 Design Patterns Used

| Pattern     | Purpose |
|------------|---------|
| **Strategy**     | Supports multiple split types: Equal, Exact, Percentage |
| **Observer**     | Notifies users about expenses and settlements |
| **Singleton**    | Centralized management of users, groups, and expenses |
| **Factory**      | Creates split strategy objects based on user input |

---

## 📁 Project Structure

```
SplitwiseClone/
│
├── Program.cs                     // Entry point
│
├── Enums/
│   └── SplitType.cs              // Enum for split types
│
├── Interfaces/
│   ├── IObserver.cs              // Observer pattern
│   └── ISplitStrategy.cs         // Strategy interface
│
├── Models/
│   ├── User.cs                   // User with balances
│   ├── Expense.cs                // Expense data
│   ├── Split.cs                 // Represents a split between users
│
├── Strategy/
│   ├── EqualSplit.cs            // Equal split logic
│   ├── ExactSplit.cs            // Exact split logic
│   ├── PercentageSplit.cs       // Percentage split logic
│   └── SplitFactory.cs          // Factory to get split strategy
│
├── Services/
│   ├── Group.cs                 // Group with balances and expenses
│   ├── Splitwise.cs             // Singleton manager/controller
│
├── Utils/
│   └── DebtSimplifier.cs        // Greedy algorithm to simplify debts
```

=========== Creating Users ====================
User created: Aditya (ID: user1)
User created: Rohit (ID: user2)
User created: Manish (ID: user3)
User created: Saurav (ID: user4)

=========== Creating Group and Adding Members ====================
Group created: Hostel Expenses (ID: group1)
Aditya added to group Hostel Expenses
Rohit added to group Hostel Expenses
Manish added to group Hostel Expenses
Saurav added to group Hostel Expenses

=========== Adding Expenses in group ====================

=========== Sending Notifications ====================
[NOTIFICATION to Aditya]: New expense added: Lunch (Rs 800)
[NOTIFICATION to Rohit]: New expense added: Lunch (Rs 800)
[NOTIFICATION to Manish]: New expense added: Lunch (Rs 800)
[NOTIFICATION to Saurav]: New expense added: Lunch (Rs 800)

=========== Expense Message ====================
Expense added to Hostel Expenses: Lunch (Rs 800) paid by Aditya
Involved members:
Aditya, Rohit, Manish, Saurav,
Will be Paid Equally

=========== Sending Notifications ====================
[NOTIFICATION to Aditya]: New expense added: Dinner (Rs 700)
[NOTIFICATION to Rohit]: New expense added: Dinner (Rs 700)
[NOTIFICATION to Manish]: New expense added: Dinner (Rs 700)
[NOTIFICATION to Saurav]: New expense added: Dinner (Rs 700)

=========== Expense Message ====================
Expense added to Hostel Expenses: Dinner (Rs 700) paid by Manish
Involved members:
Aditya : 200
Manish : 300
Saurav : 200

=========== printing Group-Specific Balances ====================

=== Group Balances for Hostel Expenses ===
Aditya's balances in group:
  Rohit owes: Rs 200.00
  Saurav owes: Rs 200.00
Rohit's balances in group:
  Owes Aditya: Rs 200.00
Manish's balances in group:
  Saurav owes: Rs 200.00
Saurav's balances in group:
  Owes Aditya: Rs 200.00
  Owes Manish: Rs 200.00

=========== Debt Simplification ====================

Debts have been simplified for group: Hostel Expenses

=========== printing Group-Specific Balances ====================

=== Group Balances for Hostel Expenses ===
Aditya's balances in group:
  Saurav owes: Rs 400.00
Rohit's balances in group:
  Owes Manish: Rs 200.00
Manish's balances in group:
  Rohit owes: Rs 200.00
Saurav's balances in group:
  Owes Aditya: Rs 400.00

=========== Adding Individual Expense ====================
Individual expense added: Coffee (Rs 40) paid by Rohit for Saurav

=========== printing User Balances ====================

=========== Balance for Aditya ====================
Total you owe: Rs 0.00
Total others owe you: Rs 0.00
Detailed balances:

=========== Balance for Rohit ====================
Total you owe: Rs 0.00
Total others owe you: Rs 40.00
Detailed balances:
  Saurav owes you: Rs40.00

=========== Balance for Manish ====================
Total you owe: Rs 0.00
Total others owe you: Rs 0.00
Detailed balances:

=========== Balance for Saurav ====================
Total you owe: Rs 40.00
Total others owe you: Rs 0.00
Detailed balances:
  You owe Rohit: Rs40.00

========== Attempting to remove Rohit from group ==========

User not allowed to leave group without clearing expenses

======== Making Settlement to Clear Rohit's Debt ==========
[NOTIFICATION to Aditya]: Settlement: Rohit paid Manish Rs 200
[NOTIFICATION to Rohit]: Settlement: Rohit paid Manish Rs 200
[NOTIFICATION to Manish]: Settlement: Rohit paid Manish Rs 200
[NOTIFICATION to Saurav]: Settlement: Rohit paid Manish Rs 200
Settlement in Hostel Expenses: Rohit settled Rs 200 with Manish

======== Attempting to Remove Rohit Again ==========
Rohit successfully left Hostel Expenses

=========== Updated Group Balances ====================

=== Group Balances for Hostel Expenses ===
Aditya's balances in group:
  Saurav owes: Rs 400.00
Manish's balances in group:
  No outstanding balances
Saurav's balances in group:
  Owes Aditya: Rs 400.00
---
