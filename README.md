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
<img width="3840" height="3132" alt="Untitled diagram _ Mermaid Chart-2025-07-17-223735" src="https://github.com/user-attachments/assets/443a0773-414b-4790-9cc8-edbb3df3a22f" />

---
