# turnflow-core

## run tests

```bash
dotnet test TurnFlow.Core.Tests/TurnFlow.Core.Tests.csproj
```

## Build and move

```bash
dotnet build TurnFlow.Core/TurnFlow.Core.csproj -c Release
```

then move the TurnFlow.Core.dll from `TurnFlow.Core/bin/Release/net8.0/TurnFlow.Core.dll` to `TurnFlow.Unity/Runtime/Plugins/TurnFlow.Core.dll`