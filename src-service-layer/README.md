# Эталонная копия: Clean Architecture + service layer

Снимок `src/` **до** перехода на CQRS. Не входит в `MinimalWebApp.slnx` и не собирается —
лежит здесь только для построчного сравнения.

Три стадии одного и того же приложения в этом репозитории:

| Папка | Архитектура | В сборке |
|---|---|---|
| `MinimalWebApp/` | всё в одном `Program.cs` (153 строки) | нет |
| `src-service-layer/` | Clean Architecture + `IProductService` | нет |
| `src/` | Clean Architecture + CQRS | да |

Ключевое отличие от `src/`: один `IProductService` обслуживал и чтение, и запись,
читал и писал через один `IProductRepository`, и собирал `ProductResponse` из
entity в обоих случаях.
