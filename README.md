# Mock Server — .NET Minimal API

## Syfte

Det här projektet är en mock-server som ersätter tre externa API:er som `web-app-boarding`-backenden anropar mot `https://api-test.sundsvall.se`. Mocken låter dig köra hela applikationen lokalt utan tillgång till den riktiga miljön.

API:er som mockas:
- **Token** — OAuth2 Client Credentials
- **Employee API v2.0** — Hämtar personaldata
- **Checklist API v1.3** — Hanterar onboarding-checklistor
- **Company API v1.0** — Hämtar organisationsträd

---

## Projektstruktur

```
mock-server/
  MockServer.csproj          # .NET 10 Web-projekt
  Program.cs                  # App-konfiguration, route-registrering
  appsettings.json
  appsettings.Development.json  # Kör på port 5001
  Models/
    Employee/
      PortalPersonData.cs
    Checklist/
      Enums.cs
      Stakeholder.cs
      Mentor.cs
      EmployeeChecklist.cs
      EmployeeChecklistPhase.cs
      EmployeeChecklistTask.cs
      CustomTask.cs
      ChecklistTemplate.cs
      Phase.cs
      ChecklistTask.cs
      OngoingEmployeeChecklist.cs
      OngoingEmployeeChecklists.cs
      PagingMetaData.cs
      DelegatedEmployeeChecklistResponse.cs
      Events.cs
      ChecklistOrganization.cs
      SortorderRequest.cs
      RequestModels.cs
    Company/
      CompanyOrganization.cs
      OrganizationTree.cs
  Data/
    MockStore.cs               # In-memory state, seed-data
  Endpoints/
    TokenEndpoints.cs
    EmployeeEndpoints.cs
    ChecklistEndpoints.cs
    CompanyEndpoints.cs
  Dockerfile
```

---

## Köra lokalt

```bash
cd mock-server
dotnet run
# Lyssnar på http://localhost:5001
```

Snabbtest:
```bash
curl -X POST http://localhost:5001/token
# → { "access_token": "mock-...", "expires_in": 3600 }

curl http://localhost:5001/employee/2.0/2281/portalpersondata/PERSONAL/anna.lindgren
# → PortalPersonData JSON
```

---

## Koppla backenden till mocken

I `backend/.env.development.local`:
```
# Ändra från:
API_BASE_URL=https://api-test.sundsvall.se
# Till:
API_BASE_URL=http://localhost:5001
```

Starta sedan backenden normalt. Ingen kod i backenden behöver ändras.

---

## Docker

Bygg och kör med Docker:
```bash
cd mock-server
docker build -t mock-server .
docker run -p 5001:8080 mock-server
```

Eller med Docker Compose (lägg till i `docker-compose.override.yml`):
```yaml
services:
  mock-server:
    build:
      context: ./mock-server
    ports:
      - "5001:8080"
    container_name: ${COMPOSE_PROJECT_NAME:-boarding}-mock-server
```

När man kör via Docker Compose, använd container-namnet i backend-env:
```
API_BASE_URL=http://mock-server:8080
```

---

## Seed-data

Mocken startar med förseedd data för tre personas:
- `anna.lindgren` — manager
- `erik.johansson` — ny medarbetare
- `sara.nilsson` — delegations-testpersona

Se `docs/seed-data.md` för fullständiga datadefinitioner.

---

## Relaterade filer i repot

De TypeScript-kontrakt som C#-modellerna baseras på:
- `backend/src/data-contracts/checklist/data-contracts.ts`
- `backend/src/data-contracts/employee/data-contracts.ts`
- `backend/src/data-contracts/company/data-contracts.ts`
- `backend/src/services/api-token.service.ts` — token-request-format
- `backend/src/controllers/checklist.controller.ts` — URL-mönster
