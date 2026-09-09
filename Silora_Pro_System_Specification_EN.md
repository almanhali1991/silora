# Silora Pro — Complete System Specification

## Retail Financial Movement Management System — Full Build Specification

**Document type:** Product requirements specification / complete build spec for an AI coding agent
**Product:** Silora Pro — Financial Movement Management System
**Platform:** Windows desktop application
**UI language:** Arabic (full RTL)
**System role:** Back-office financial administration only — **not** a Point of Sale system

---

# 1. System Overview

Silora Pro is a professional back-office financial application for a retail business that already runs a separate sales system responsible for issuing invoices and completing customer checkout. This system's job is not selling — it is the **financial bookkeeping that surrounds the sale**: tracking sales proceeds by payment method, bank and cash-drawer movements, expenses, and management reporting.

The system runs on a single machine, for a single user (the owner or financial manager), fully offline, with no cloud services of any kind. All data lives locally on the user's machine.

## 1.1 What the system is, exactly

The system answers these everyday questions:

- How much did today's cash sales bring into the cash drawer?
- How much entered the bank account via direct customer transfers?
- What were today's Tamara (BNPL) sales and their net after deduction?
- What were today's network POS card sales (Mada, Visa, MasterCard) and their net after network deductions?
- How much was withdrawn from the bank and placed in the cash drawer?
- How much was spent from bank expenses and from cash-drawer expenses?
- What are the current bank and drawer balances, and what moved during any date range?

## 1.2 What the system is NOT (hard boundaries)

The following are strictly forbidden:

- Checkout screens, shopping carts, or product sales entry.
- Item-level sales, product catalog, or inventory management.
- Barcode/QR scanning or payment-terminal control.
- Payroll/HR, supplier, or obligation management.
- Login screens, users, roles, or permissions — **there is no login screen at all**; the app opens straight into the main window.
- Anything that requires an internet connection.

The existing sales system remains the source of truth for invoice details; this system records only the financial outcome of those sales.

---

# 2. Core Financial Model

## 2.1 Money locations

The system is built on exactly two financial locations:

| Location | Meaning |
|---|---|
| Bank | The business's single bank account |
| Drawer | The cash drawer (cash on hand in the store) |

Each location is treated as an independent financial account with an opening balance, inflows, and outflows. Multiple bank accounts are intentionally out of scope — one bank account and one drawer. This simplification is deliberate.

## 2.2 Balance derivation rule (non-negotiable)

Balances are **never stored and never directly modified**. Every balance is derived at query time:

```
Bank Balance  = Bank Opening Balance  + Bank Inflows  − Bank Outflows
Drawer Balance = Drawer Opening Balance + Drawer Inflows − Drawer Outflows
Total Liquidity = Bank Balance + Drawer Balance
```

The subtraction is internal; the user always sees **positive magnitudes with an explicit direction label** (In / Out / Balance). No negative number is ever shown to the user on any screen or report.

If a data-entry error produces an actually negative computed balance, display the magnitude with a clear red warning: "Warning: the drawer balance has gone negative — review your entries." Direction is conveyed by the label, never by a minus sign.

## 2.3 The ledger movement model

Every financial effect in the system is a **Movement** with:

- Location: bank or drawer.
- Direction: In or Out.
- Amount: a positive decimal always (zero and negative are rejected).
- Date.
- Description (as it will appear in the account statement).
- **SourceKey**: the identifier of the entry that generated this movement — the posting link and cascade-delete anchor (Section 8).

Movements are never created manually from the UI. They are created automatically by the posting engine when entries are saved, edited, or deleted.

## 2.4 Aggregated posting rule

Each journal entry posts to the accounts as **one aggregated amount** (or exactly one pair of movements for dual-direction transfers) — never one movement per line. Line details live inside the entry for reporting; the account ledger sees only the aggregate. This rule prevents double counting and keeps statements clean.

---

# 3. Technology Requirements

The system must be built with the following stack; substitution is not permitted without an explicit documented decision:

| Item | Requirement |
|---|---|
| OS | Windows 10 / 11 (64-bit) |
| Framework | .NET 8 (LTS) or newer — latest stable LTS preferred |
| UI technology | WPF with C# |
| Pattern | MVVM via CommunityToolkit.Mvvm (or equivalent command generators) |
| Database | Local SQLite via Entity Framework Core with migrations |
| Reports | QuestPDF with full Arabic RTL support and an Arabic font embedded in the app |
| Testing | xUnit — a test suite that runs without a graphical UI |
| Money type | `decimal` exclusively for all monetary values; `double`/`float` forbidden |
| Async | All database and PDF work is async on background threads — the UI must never freeze |
| Connectivity | Works fully offline, no server, no Administrator rights required |

Additional structural constraints:

- The financial calculation and validation logic must be pure, UI-free, and testable without Windows so it can be verified automatically on any environment.
- The data-access layer must be replaceable later with another relational provider (e.g. SQL Server) without rewriting business logic.
- All user-visible text is Arabic; code identifiers and database object names are English.

---

# 4. Golden UI Rules (non-negotiable)

These rules bind every screen, dialog, and report:

1. **No login screen:** the app opens directly into the main screen in under 3 seconds.
2. **Arabic RTL first:** every user-visible string is Arabic; the entire UI flows right-to-left — windows, menus, tables, forms, reports — and the main navigation bar sits on the right side of the window.
3. **Tables are display-only:** every data grid is **permanently read-only** — no in-cell editing, ever. All create/edit/delete operations happen exclusively through forms beside or above the grid.
4. **Input through named form fields:** every user-entered value has a labeled Arabic field. Generic text-dialog prompts and grid editing are forbidden.
5. **Errors under fields:** validation messages appear directly under the offending field in clear red — never as generic popups.
6. **Light, clean UI:** light backgrounds, comfortable spacing, a readable Arabic font at a comfortable size, professional contrast, no decorations and no dark theme.
7. **Positive amounts always:** no negative numbers anywhere the user can see; direction is read from the label (In/Out).
8. **Clean number formatting:** no trailing decimal zeros — show 100 not 100.00, show 25000.75 as is, with thousands separators for readability (12,500.75). Internal precision stays exact.
9. **Arabic-Indic digit input:** amount fields accept Arabic-Indic (٠-٩) and Persian (۰-۹) digits and the decimal separator as either comma or dot, and normalize them to Latin digits while typing; any non-numeric character is rejected instantly.
10. **Validation on save only:** no annoying validation while typing; full validation runs when Save is pressed and all errors appear at once under their fields.
11. **Busy indication:** any non-instant operation (save, load, PDF generation) shows a busy indicator and disables buttons to prevent double submission.
12. **Confirm before financial commitment:** saving, deleting, or changing an opening balance is preceded by a clear confirmation dialog summarizing the financial effect before it executes.

---

# 5. Main Window Structure

## 5.1 General frame

One maximized main window composed of:

- **Top header:** app name (Silora Pro) + business name (from Settings) + current date in the Gregorian calendar with Arabic day names (Saturday, Sunday, …).
- **Right-side navigation rail** (RTL — navigation belongs on the right) with four main sections:
  1. Unified Journal (the default screen on launch)
  2. Balances & Overview
  3. Reports
  4. Settings
- **Page content area** to the left of the rail, hosting the active page, with a busy overlay covering the content during operations.

## 5.2 Bottom status bar

Shows: current bank balance, current drawer balance, and total liquidity — as formatted positive numbers — so the manager sees the immediate effect of every saved entry without leaving the screen.

---

# 6. Core Module: The Unified Journal

The Unified Journal is the heart of the system, where the user records all daily entries. Each section shows an entry form at the top or side and a read-only table of saved entries with their lines.

The journal consists of **seven sections**, shown as tabs:

1. Cash Sales → posts: Inflow to Drawer.
2. Bank Transfer Sales → posts: Inflow to Bank.
3. Tamara Sales → posts: Inflow to Bank (net after deduction).
4. Network POS Sales → posts: Inflow to Bank (net after deduction).
5. Bank-to-Drawer Withdrawal → posts: Outflow from Bank + Inflow to Drawer (one atomic pair).
6. Bank Expenses → posts: Outflow from Bank.
7. Cash (Drawer) Expenses → posts: Outflow from Drawer.

## 6.1 The unified entry form (shared behavior)

Every section uses the same entry structure:

- **Date:** date field, defaults to today, required.
- **General description:** free text, optional summary of the entry (e.g. "Saturday sales").
- **Lines:** a line list managed with Add Line / Remove Selected Line / Edit Selected Line buttons. Editing a line opens a line form with the same fields as adding — no grid editing.
- **Totals bar:** shows the running total live; in Tamara and POS sections it also shows total deduction and total net.
- **Commands:** Save Entry (posts immediately) and Clear Form.

When Save is pressed:

1. Full validation runs (Section 13) and errors appear under their fields.
2. A confirmation dialog summarizes: entry type, date, line count, total, and the effect ("Will add to drawer balance: 5,250 SAR").
3. On confirmation the entry, its lines, and its ledger movements are created in **one atomic database transaction**.
4. The new entry appears immediately in the entries table, the status-bar balances update, and the form clears for the next entry.

## 6.2 Section entries table (read-only)

Common columns for all sections:

| Column | Content |
|---|---|
| Entry # | Sequence number within the section |
| Date | Entry date |
| Description | Entry description |
| Lines | Number of detail lines |
| Total | Sum of line amounts (or net for Tamara/POS) |
| Effect | Posting direction label, e.g. "Inflow to Drawer" |
| Actions | View (opens the lines in a details panel) and Delete |

Selecting an entry shows a **details panel below** with its full lines (read-only) — this is the way to view detail, instead of any editing.

## 6.3 Section 1: Cash Sales

**Purpose:** record sales paid in cash and placed in the cash drawer. Source: the daily total from the sales system or the drawer count.

Cash sales line fields:

| Field | Type | Required | Notes |
|---|---|---|---|
| Description | text | no | e.g. "Morning shift sales" |
| Amount | positive money | yes | greater than zero |

**Posting rule:** one entry record with its lines + one movement: "Inflow to Drawer — Cash Sales" for the total.

Example:

```
Date: 2026-09-05 | Description: Saturday sales
  Line 1: Morning shift sales — 3,250
  Line 2: Evening shift sales — 2,000
Total: 5,250 → one drawer inflow movement of 5,250
```

## 6.4 Section 2: Bank Transfer Sales

**Purpose:** record sales paid by customers through direct bank transfer into the business account.

Bank transfer line fields:

| Field | Type | Required | Notes |
|---|---|---|---|
| Customer name | text | no | for traceability and reports |
| Description | text | no | e.g. "Transfer against invoice 1234" |
| Amount | positive money | yes | the amount actually received |

**Posting rule:** one movement: "Inflow to Bank — Bank Transfer" for the total.

## 6.5 Section 3: Tamara Sales

**Purpose:** record sales paid through Tamara (buy-now-pay-later). Tamara remits the net amount after its deduction.

Tamara line fields:

| Field | Type | Required | Notes |
|---|---|---|---|
| Customer name | text | no | |
| Invoice number | text | no | sales-system reference — useful for search |
| Description | text | no | |
| Gross amount | positive money | yes | invoice value before deduction |
| Deduction rate % | 0–100 | yes | defaults from Settings, editable per line |
| Deduction amount | computed | automatic | gross × rate |
| Net amount | computed | automatic | gross − deduction |

**Posting rule:** one movement: "Inflow to Bank — Tamara" for the **total net** (not the gross). The deduction amount is stored on the line for reporting only — it is not posted as a separate movement and is not treated as an expense.

**Historical preservation rule:** the line stores a copy of the deduction rate used at save time; later changes to the default rate in Settings never modify saved lines.

Example:

```
Date: 2026-09-05
  Line 1: Sara — invoice 1201 — gross 1,000 — 6% — deduction 60 — net 940
  Line 2: Mohammed — invoice 1202 — gross 2,000 — 6% — deduction 120 — net 1,880
Total: 3,000 | Total deduction: 180 | Total net: 2,820
→ one bank inflow movement of 2,820
```

## 6.6 Section 4: Network POS Sales

**Purpose:** record card sales through POS terminals: Mada, Visa, MasterCard, or any network added in Settings. Each network has its own deduction rate; the bank receives the net.

Network POS line fields:

| Field | Type | Required | Notes |
|---|---|---|---|
| Network | list (from Settings) | yes | Mada / Visa / MasterCard / custom |
| Description | text | no | |
| Gross amount | positive money | yes | network sales before deduction |
| Deduction rate % | 0–100 | yes | auto-filled from the selected network's rule, editable |
| Deduction amount | computed | automatic | |
| Net amount | computed | automatic | |

**Posting rule:** one aggregated movement: "Inflow to Bank — Network POS" for the total net. The bank receives one aggregated amount, not one movement per network; per-network detail is available in reports from line data (double-counting prevention).

**Historical preservation rule:** same as Tamara — the rate used is stored on the line at save time.

Example:

```
Date: 2026-09-05
  Line 1: Mada — gross 40,000 — 1% — net 39,600
  Line 2: Visa — gross 15,000 — 1.5% — net 14,775
  Line 3: MasterCard — gross 10,000 — 1.5% — net 9,850
Total: 65,000 | Deduction: 775 | Net: 64,225
→ one bank inflow movement of 64,225
```

## 6.7 Section 5: Bank-to-Drawer Withdrawal

**Purpose:** internal cash transfer — withdraw from the bank and place in the drawer for daily cash liquidity.

Entry header fields: Date + description (purpose recommended, e.g. "providing change and daily liquidity").

Line fields:

| Field | Type | Required |
|---|---|---|
| Description | text | no |
| Amount | positive money | yes |

**Atomic dual posting rule:** exactly two movements in the same transaction, linked to the same entry via the SourceKey:

- "Outflow from Bank — Withdrawal to Drawer" for the total.
- "Inflow to Drawer — Withdrawal from Bank" for the total.

This transfer is internal — it is never an expense or a revenue, and never appears in expense reports.

## 6.8 Section 6: Bank Expenses

**Purpose:** record expenses paid from the bank account.

Expense line fields:

| Field | Type | Required | Notes |
|---|---|---|---|
| Category | list (from Settings) | yes | Rent, Electricity, Marketing, … |
| Description | text | no | expense detail |
| Amount | positive money | yes | |

**Posting rule:** one movement: "Outflow from Bank — Expense: [Category]" for the total.

## 6.9 Section 7: Cash (Drawer) Expenses

Identical structure to Section 6 (category, description, amount), but posts: "Outflow from Drawer — Expense: [Category]" for the total. The only difference between Sections 6 and 7 is the paying account.

---

# 7. Balances & Overview Screen

A page presenting the current financial position with no input at all:

- **Balance cards:** bank balance, drawer balance, total liquidity — formatted positive numbers, updated automatically after every entry.
- **Today summary:** today's totals for each of the seven journal sections.
- **Last 7 days:** a daily table showing, per day: bank in, bank out, drawer in, drawer out, and each account's closing balance.
- **Top categories:** the top 5 expense categories of the current month.

All numbers on this page are read-only, derived from movements, and the page contains no input forms.

---

# 8. Posting and Financial Integrity Rules

These engine rules must never be broken:

1. **Single posting source:** all bank and drawer movements are created exclusively by one central posting engine. No screen or form may modify a balance or write a movement directly.
2. **Atomicity:** an entry, its lines, and its movements are inserted, modified, or deleted in one transaction. If any part fails, the whole transaction rolls back — partial financial state is never acceptable.
3. **SourceKey:** every entry carries a unique key, and every movement it generates stores a copy of that key. This link is the basis of traceability and cascade delete.
4. **Cascade delete:** deleting an entry deletes all movements linked to its key, in one transaction, restoring balances exactly to what they were before the entry. The pre-delete confirmation states this explicitly: "The entry and all its linked financial movements will be deleted (two movements in the case of a bank-to-drawer transfer)."
5. **Edit = re-post:** editing a saved entry opens the same entry form; on save, the old movements are replaced with the new ones inside one transaction. No edit path may leave orphaned old movements.
6. **Double-counting prevention:** each entry posts exactly once — the aggregated total prevents line-level duplication, and the structure guarantees the movement count per entry type (1 or 2).
7. **No direct balance assignment:** logic like `balance = balance − amount` is forbidden as the financial architecture. Balances are always derived: opening + inflows − outflows.
8. **Historical rule preservation:** values that may change in the future (Tamara and network deduction rates) are copied onto the line at save time and are immune to later Settings changes.
9. **No logical negative amounts:** entered amounts are always positive; direction is the movement type. The engine may subtract internally when deriving, but user-facing presentation is always positive.

---

# 9. Conceptual Data Model (concepts, not file layouts)

These concepts must exist in the database as related entities (programming names are advisory; the substance is mandatory):

| Concept | Description | Key fields |
|---|---|---|
| Journal Entry | A journal voucher with a unique SourceKey | Type (the seven sections), date, description, total, SourceKey |
| Entry Line | Entry detail | Sequence, type-specific fields (description / customer / invoice / network / category / gross / stored deduction rate / deduction / net) |
| Ledger Movement | A financial effect on bank or drawer | Location, direction, amount, date, description, SourceKey |
| Payment Network | Configurable card method | Name, active flag, deduction rate |
| Expense Category | Manageable category | Name, active flag |
| Opening Balance | Book starting point per location | Location (bank/drawer), amount, date, reason |
| App Setting | General settings | Business name, default Tamara rate |

Mandatory relationships:

- Entry 1..* Entry Line (deleting the entry deletes its lines).
- Entry 1..* Ledger Movement (via SourceKey; cascade delete).
- Payment Network and Expense Category are references that can be activated/deactivated but never deleted once used by any saved line — deactivation only.
- Opening Balance: one record per location, modified through a full form (Section 11.4), never silently assigned.

---

# 10. Reports (Arabic RTL PDF)

All reports are generated as PDF through one shared PDF engine, fully Arabic RTL, on a background thread with a busy indicator. The Arabic font is embedded inside the application (e.g. Cairo, Noto Naskh Arabic, or equivalent) so reports work on any Windows machine without installing fonts.

## 10.1 Common PDF report structure

- **Page header (every page):** business name (from Settings) + report title + period (from–to or single date).
- **Page footer (every page):** page X of Y + generation timestamp.
- **RTL tables:** columns read right-to-left, headers with a distinct background, totals rows under tables.
- **Numbers:** thousands separators, no trailing zeros, currency "SAR" / "ر.س".
- **Paper:** A4 portrait, comfortable margins, a simple title page with period and executive summary.

## 10.2 Required reports

1. **Daily Report:** for a single date — totals of the seven sections, expense breakdown by category, opening and closing bank and drawer balances for that day, and the list of the day's entries.
2. **Period Report:** from date to date — the daily report content aggregated over the range, with total inflow/outflow per account and net movement.
3. **Per-Section Detail Report:** all entries and lines of one section over a period, with the section's specific columns (customer / invoice / network / category / deduction / net) and totals.
4. **Bank Statement:** all bank movements over a period in chronological order with a running balance after each movement, plus opening and closing balances.
5. **Drawer Statement:** the same statement structure for the cash drawer.
6. **Tamara Report:** lines over a period (customer, invoice, gross, rate, deduction, net) plus aggregated totals.
7. **Network Report:** aggregation per network (gross, deduction, net) plus line detail.
8. **Expenses Report:** by category, period, and paying account (bank/cash), with totals.

## 10.3 Generation behavior

- The Reports page shows: the report list, the period fields (from – to), and a Generate PDF button.
- After generation, a dialog offers: Open the file, or Save As into a user-chosen folder.
- Default file name: report name + period, e.g. `Daily-Report-2026-09-05.pdf`.
- Generation never freezes the UI regardless of data volume (background thread + fully async).

---

# 11. Settings

One settings page with full field forms (no generic text dialogs), divided into five parts:

## 11.1 Business identity

- **Business name:** required text, default "مؤسسة النخبة التجارية" (Al-Nukhba Trading Establishment), used in the app header and every PDF report.
- Currency is fixed: Saudi Riyal (SAR) — there is no currency setting.

## 11.2 Deduction rules

- **Default Tamara deduction rate %:** a number between 0 and 100, required. Used as the initial value for new Tamara lines; changes never touch saved lines.
- **Networks:** full management grid: add a network, edit name and deduction rate, activate/deactivate. Default networks seeded on first run: Mada, Visa, MasterCard.
- **Expense categories:** add / edit / activate / deactivate. Default seeded categories: Rent, Electricity, Water, Internet, Maintenance, Marketing, Government Fees, Bank Fees, Shipping & Delivery, Operating Purchases, Other (in Arabic: إيجار، كهرباء، ماء، إنترنت، صيانة، تسويق، رسوم حكومية، رسوم بنكية، شحن وتوصيل، مشتريات تشغيلية، أخرى).
- **Protection rule:** a category or network used by any saved line is never deleted — deactivation only (it disappears from new-entry dropdowns but remains in reports).

## 11.3 Settings edit policy

Every change saves via a Save Settings button with validation under the fields (e.g. "The deduction rate must be between 0 and 100", "Business name is required" — in Arabic). Changes apply immediately without restart.

## 11.4 Opening balance adjustment

Adjusting an opening balance (bank or drawer) uses a **full field form**, never a text prompt:

| Field | Required |
|---|---|
| Location (Bank / Drawer) | yes |
| New opening balance | yes — number ≥ 0 |
| Opening balance date | yes — defaults to today |
| Reason for the change | yes — explanatory text stored with the record |

On save: a confirmation shows the effect ("The bank balance will change from X to Y because the opening balance changed"), then the record updates and all balances re-derive automatically. The adjustment record (old value, new value, date, reason) is stored for review under "Opening balance change history" in Settings.

---

# 12. Search and Filtering

- Each of the seven journal sections shows above its table: **from date – to date** filters + **text search** (searches description, customer name, and invoice number as applicable to the section's columns) + quick range buttons: Today / This Week / This Month / All.
- Filtering applies to the entries table, with a totals line under the table computed over the filtered results only.
- The Balances page and report statements accept the same from–to period.
- Text search is case-insensitive, ignores Arabic diacritics, and collapses extra whitespace.

---

# 13. Input Validation (simplified)

Validation runs **on save only** and appears all at once under the relevant fields in red:

## 13.1 General rules

| Condition | Arabic message |
|---|---|
| Date empty or invalid | «التاريخ مطلوب» |
| No lines in the entry | «أضف سطرًا واحدًا على الأقل» |
| Amount empty, zero, negative, or non-numeric | «المبلغ يجب أن يكون رقمًا أكبر من صفر» |
| Deduction rate outside 0–100 | «نسبة الخصم يجب أن تكون بين 0 و100» |
| Network not selected | «اختر الشبكة» |
| Category not selected | «اختر التصنيف» |
| Business name empty in Settings | «اسم النشاط مطلوب» |
| Reason empty when adjusting an opening balance | «سبب التعديل مطلوب» |

## 13.2 Amount field behavior

- Accepts Arabic-Indic and Persian digits and normalizes them to Latin instantly.
- Accepts the decimal separator as `,` or `.` and normalizes it to a single dot.
- Rejects any other character at typing time (input simply does not appear; no error message while typing).
- Precision: up to two decimal places.

## 13.3 Deliberate simplification limits

- No maximum amount caps (the user is free).
- No validation while typing, except digit-only enforcement.
- Future dates are allowed (a user may deliberately post-date an entry) — only format validity is checked.
- No required fields beyond: date, each line's amount, and rate/network/category depending on the section type.

---

# 14. Storage and Backup

## 14.1 Local database

- A single-file SQLite database created automatically on first run in the per-user Windows application data folder (`%LOCALAPPDATA%\SiloraPro\`) — never next to the executable.
- On first run: migrations apply, default data is seeded (the three networks, expense categories, business name, default Tamara rate), and opening balances start at zero until the user sets them in Settings.
- The application runs without Administrator rights.

## 14.2 Backup and restore

- A "Backup" button in Settings copies the database file to a user-chosen folder with a timestamped name (e.g. `Silora-Backup-2026-09-05-1430.db`), or to a default internal backup folder.
- A "Restore" button lets the user pick a previous backup file, shows a clear confirmation ("The current data will be completely replaced by the backup's data — this cannot be undone"), then replaces the database and reloads all screens.
- Restore must handle open-database file locks (release connections before replacing the file) — this behavior is mandatory and tested.
- During backup and restore: busy indicator and disabled buttons.

---

# 15. Performance and Quality of Execution

## 15.1 Performance

- App startup to a ready UI: under 3 seconds with a database of 10,000 movements.
- Saving an entry: under half a second, with zero UI freezing.
- Generating a full-year PDF: a few seconds on a background thread, UI responsive throughout.
- Entry tables use standard UI virtualization with internal scrolling (no page may host the grid inside an infinite-height scroll container that defeats virtualization).
- Page navigation is responsive with stable memory — no accumulating connections or contexts per navigation.

## 15.2 Reliability

- A global exception handler catches any unexpected exception: shows a clear Arabic message ("An unexpected error occurred — see the log file") and keeps the app running instead of crashing silently, writing details to a text log file beside the database.
- All read, write, and PDF operations are asynchronous.
- Buttons are disabled during any in-flight operation to prevent duplication.

## 15.3 Tests (delivery condition)

The system must ship with an automated xUnit suite covering at least:

1. Posting math for each of the seven sections (movement amount, direction, account).
2. Cascade delete: deleting an entry restores the balance to exactly its prior value.
3. Balance derivation: opening + inflows − outflows.
4. Deduction calculations (Tamara and networks) and rounding.
5. Historical preservation: changing a rate in Settings does not modify saved lines.
6. Input validation (every message, every condition).
7. Arabic-Indic digit and separator normalization.
8. Default data seeding on first run.
9. Backup and restore (including with an active open connection).
10. An automated architecture guard that rejects any editable grid or any input outside forms.

Delivery standard: zero-error, zero-warning build + 100% green tests.

---

# 16. Final Acceptance Criteria

The system is not complete until every item below is provably true:

1. The app opens directly into the Unified Journal with no login screen.
2. The entire UI is Arabic RTL with the main navigation on the right of the window.
3. Every data grid is read-only; no input exists outside named-field forms.
4. Validation errors appear under fields in Arabic; no validation popups.
5. All seven sections work: each posts to the correct account, direction, and amount.
6. A Tamara entry posts the net (not the gross) and stores the deduction rate used.
7. A network POS entry posts the aggregated net as a single bank movement.
8. A bank-to-drawer transfer creates two linked movements atomically.
9. Deleting any entry removes its movements and restores the prior balance.
10. Balances are always derived, never stored.
11. No negative number is ever displayed to the user.
12. Numbers have no trailing decimal zeros and use thousands separators.
13. The amount field accepts ٠-٩, normalizes instantly, and rejects non-numeric input.
14. The eight PDF reports are Arabic RTL with an embedded font, header, footer, and totals.
15. PDF generation runs on a background thread without freezing the UI.
16. First run creates the database and seeds defaults without any error.
17. Backup and restore work even with an active connection.
18. Settings cover: business name, Tamara rate, networks, categories, and opening balance via a full form with a reason.
19. All automated tests pass and the build is clean with no warnings.
20. The application runs offline without Administrator rights on Windows 10/11.

---

# 17. Instructions for the Building Agent

- Implement this specification literally: the Golden UI Rules in Section 4 are non-negotiable under any circumstance.
- If you face an ambiguous situation not covered here, choose the simplest user-facing solution consistent with the spirit of this spec (simplify, never complicate), and document the decision in a short comment.
- Do not add modules outside this scope (users, suppliers, employees, inventory, fiscal calendar, fiscal year…) — the scope above is the complete intended scope.
- Build in this order: financial model and validation first (pure, testable logic), then database and migrations, then the UI, then reports, then tests.
- Keep all calculation and validation logic in pure units that run without Windows for automated testing, with the UI as a thin layer on top.
- On delivery: clean build + all tests green + short Arabic run instructions (runtime requirements: Windows 10/11 and the required .NET version, plus steps to open and run the project).

---

# 18. Glossary (English – Arabic)

| Term | المصطلح العربي |
|---|---|
| Unified Journal | اليومية الموحدة |
| Entry / Voucher | القيد |
| Entry Line | سطر القيد |
| Ledger Movement | الحركة المالية |
| SourceKey | مفتاح المصدر |
| Bank Account | البنك |
| Cash Drawer | الدرج |
| Opening Balance | الرصيد الافتتاحي |
| Inflow / Outflow | وارد / صادر |
| Cash Sales | المبيعات النقدية |
| Bank Transfer Sales | مبيعات التحويل البنكي |
| Tamara Sales | مبيعات تمارا |
| Network POS Sales | مبيعات نقاط البيع الشبكية |
| Bank-to-Drawer Withdrawal | السحب من البنك إلى الدرج |
| Bank Expenses | المصروفات البنكية |
| Cash Expenses | المصروفات النقدية |
| Payment Network | الشبكة |
| Deduction Rate | نسبة الخصم |
| Net Amount | الصافي |
| Total Liquidity | السيولة الكلية |
| Backup | نسخة احتياطية |
