# Zip engineering interview exercises

Coding exercises for engineering candidates at Zip. Each folder is self-contained — clone the repository, open the
one your interviewer pointed you at, and start with its `EXERCISE.md`.

| Exercise | Role | Stack |
| --- | --- | --- |
| [zip-promotions-exercise](zip-promotions-exercise) | Senior Backend Engineer | .NET 10, ASP.NET Core |

## Before your session

Please get the build running ahead of time so the hour is spent on the exercise rather than on tooling.

```powershell
cd zip-promotions-exercise
dotnet build Zip.Promotions.Exercise.sln
dotnet run --project src/Zip.Promotions.Exercise --no-launch-profile --urls "http://localhost:5199"
```

The .NET 10 SDK is the only requirement — no Docker, no database, no cache server. A clean build and a service that
starts is the expected state; what the service *does* is the exercise. If you run `dotnet test` you will be told
there are no tests, which is also expected.

Use the editor and tooling you would normally use. Each exercise says where AI assistance is welcome and where we
would rather see you work unaided.
