# BookChangeTracker

## Quick start

```powershell
# Start API and database
docker compose up --build
```
API available at http://localhost:8080

Swagger UI: http://localhost:8080/swagger

Database will auto-migrate on first run.

## TODOs and future consideration

- Authentication and authorization (also allows tracking and persisting who makes changes)

- Configure CORS, HTTPS

- Logging

- Exception-type-to-error-code mapping in the global exception handler (handle specific exceptions instead of generic Exception type)

- Unit and integration tests

- Consider separate migration project for production deployments

- Consider utilizing outbox pattern to prevent loss of events

- Consider cursor-based pagination when scaling

- Consider handling DB level concurrency by adding RowVersion to entities 

- Consider standardizing validation errors

- Consider rate limiting
