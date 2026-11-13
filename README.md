# 🧪 Unit Testing & Architecture Testing: Mini Checkout Engine

> .NET Diploma · Cycle 1 · Simulation Academy

## 👩‍💻 Student Information

| | |
|---|---|
| **Name** | Jessica Wael Khalaf |
| **Group / ID** | Group 3 |
| **Assignment** | Unit Testing & Architecture Testing |

---

## 📌 Overview

The checkout code (`CheckoutService.CheckoutAsync`) was already written. This repository adds **only tests** that prove it works: business rules, mocks, generated test data, a snapshot, architecture rules, and a coverage report. No production code in `src/` was changed.

---

## 📂 Project Structure

```text
.
├── src/                                  # Production code (unchanged)
│   ├── Simulation.Checkout.Domain
│   ├── Simulation.Checkout.Application
│   └── Simulation.Checkout.Infrastructure
├── tests/
│   └── Simulation.Checkout.Tests/
│       ├── CheckoutServiceTests.cs       # Tasks 1, 2, 3
│       ├── TestData/                     # AutoFixture + Bogus helpers
│       ├── ReceiptSnapshotTests.cs       # Task 4 (+ .verified.txt)
│       └── ArchitectureTests.cs          # Task 5
├── coverage-report/                      # HTML coverage report (Task 6)
├── TESTING_NOTES.md
└── README.md
```

---

## ✅ What Is Tested

| Task | Topic | Tools |
|------|-------|-------|
| 1 | Business rules (validation, discount, totals) | xUnit, FluentAssertions |
| 2 | Interaction verification (Save / Send called Once or Never) | Moq |
| 3 | Generated test data | AutoFixture, Bogus (fixed seed) |
| 4 | Receipt snapshot (Example C) | Verify.Xunit |
| 5 | Architecture and naming rules | NetArchTest.Rules |
| 6 | Coverage report | coverlet, ReportGenerator |

---

## ▶️ How to Run

Requires the **.NET 10 SDK**.

```bash
dotnet restore
dotnet build
dotnet test
```

The HTML coverage report is in [`coverage-report/`](./coverage-report) (open `index.html`).

---

## 📊 Results

- **Tests:** 20 total, 20 passing, 0 failing
- **Line coverage:** 90.5% overall (Domain 100%, Application 95.8%)

See [`TESTING_NOTES.md`](./TESTING_NOTES.md) for the short answers.