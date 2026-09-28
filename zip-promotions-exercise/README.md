# Zip Promotions

A Promotions Service for a Buy Now Pay Later platform. It tells a merchant checkout which promotion an order
qualifies for, given who the customer is and what they have bought before.

Everything runs in process. No Docker, no database, no cache server.

## Running it

.NET 10 SDK is the only requirement.

```powershell
dotnet build Zip.Promotions.Exercise.sln
dotnet test Zip.Promotions.Exercise.sln
dotnet run --project src/Zip.Promotions.Exercise --no-launch-profile --urls "http://localhost:5199"
```

Swagger UI is at <http://localhost:5199/swagger>. The **Authorize** button sets the `X-Customer-Id` header, which is
all the authentication there is.

Promotions and order history both come from `appsettings.json` and nothing writes to them, so the service returns
the same answer for the same request every time.

`dotnet run` launches the app as a child process, so killing the terminal can leave it holding the port and locking
`bin`. If a rebuild fails with MSB3027:

```powershell
Get-Process -Name Zip.Promotions.Exercise -ErrorAction SilentlyContinue | Stop-Process -Force
```

## Endpoints

| Method | Route | Purpose |
| --- | --- | --- |
| `POST` | `/promotions/assess` | Best promotion for an order |
| `GET` | `/health` | Liveness, no authentication |

```powershell
$customer = @{ 'X-Customer-Id' = 'c3000000-0000-0000-0000-000000000001' }

Invoke-RestMethod -Method Post -Uri http://localhost:5199/promotions/assess -Headers $customer `
  -ContentType 'application/json' `
  -Body '{"orderId":"order-1","merchantId":"b2000000-0000-0000-0000-000000000001","orderAmount":175.00}'
```

From PowerShell, `curl.exe` mangles inline JSON — use `Invoke-RestMethod` above, or Swagger.

## Layout

One project for the service and one for its tests. Folders group code by subject, not by architectural layer, so a
promotion's data, the services that act on it and the interfaces they depend on sit near each other.

```text
src/Zip.Promotions.Exercise
  Promotions/                     the promotion itself and the assessment service
  Eligibility/                    the eligibility rules, including how audiences are decided
  Selection/                      picking a winner from the promotions that qualify
  Customers/                      order history summaries
  Persistence/                    repository interfaces and their in-memory implementations
  Caching/                        fake Redis, and the decorator that caches the catalog
  Configuration/                  options bound from appsettings and validated on start
  Contracts/, Validation/         the request body and the rules it must satisfy
  Controllers/, Authentication/   the HTTP surface and its stand-in customer authentication
  Testing/                        a host factory and the seeded ids, for whoever writes the tests
  ServiceCollectionExtensions.cs  every registration, in one place

tests/Zip.Promotions.Exercise.Tests
  Integration/                    one empty shell, waiting for you
```

**There is no test suite.** `AssessEndpointTests` is empty apart from its wiring, and writing what belongs in it is
part of the exercise.

`PromotionApiFactory`, in `Testing/`, boots the real host against the real `appsettings.json`, so anything you write
against it sees the seeded data in the tables below. `api.CreateClientFor(customerId)` gives you a client carrying the
customer header, and `SeedIds` names the seeded GUIDs so you do not have to paste them. The factory also freezes the
clock, which is what keeps the dates in configuration meaningful whatever today is.

Nothing stops you adding your own files, folders, or a different style of test entirely.

## Seed data

Customers, merchants and promotions are all GUIDs. Order ids are free-text strings you invent per request. They are
named for you in `src/Zip.Promotions.Exercise/Testing/SeedIds.cs`.

| Promotion | Id | Audience | Merchant | Threshold → discount |
| --- | --- | --- | --- | --- |
| $10 off $100 | `a1000000-…-0001` | AllCustomers | any | $100 → $10 |
| $25 off your first Zip order | `a1000000-…-0002` | NewZipCustomer | any | $150 → $25 |
| Nike tiered savings | `a1000000-…-0003` | AllCustomers | Nike | $100 → $8, $200 → $20, $300 → $35 |
| New to Nike on Zip | `a1000000-…-0004` | NewMerchantCustomer | Nike | $150 → $30 |
| $15 off $200 for returning customers | `a1000000-…-0005` | ExistingZipCustomer | any | $200 → $15 |
| Retired $50 off $250 | `a1000000-…-0006` | AllCustomers | any | archived, never offered |
| We miss you - $20 off $150 | `a1000000-…-0007` | LapsedZipCustomer | any | $150 → $20, after 180 days idle |

Merchants are `b2000000-…-0001` (Nike), `…0002` (Adidas) and `…0003` (New Balance). New Balance has no promotions of
its own, so use it when you want to isolate the merchant-agnostic ones.

Three customers are seeded:

| Customer | History |
| --- | --- |
| `c3000000-…-0001` | No order history at all |
| `c3000000-…-0002` | Three confirmed orders at Nike and two at Adidas, most recently July 2026 |
| `c3000000-…-0003` | Two confirmed orders at Nike, none since January 2024 |

Any other GUID starts with no history, so generate a fresh one (`[guid]::NewGuid()`) whenever you need a brand-new
customer.

## Constraints

- .NET 10, C# 14, ASP.NET Core.
- Tests use xUnit, Shouldly and AutoFixture.

## MSC

- The exercise will be provided at the start of the interview.
