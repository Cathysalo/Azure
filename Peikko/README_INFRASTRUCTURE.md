# Infrastructure Layer – Peikko Precast Wall Designer

## Location

```
src/PeikkoPrecastWallDesigner.Infrastructure/
```

## Purpose

The Infrastructure layer handles external system integrations and service configuration. It registers services for:

- Azure CosmosDB
- Azure Service Bus
- Azure Key Vault
- Application settings and logging

All services are registered through extension methods consumed by the API.

## 1. App Settings Registration

Located in:
```
AppSettings/AddAppSettings.cs
```

```csharp
services.AddAppSettings(builder);
```

- Binds configuration from `appsettings.json` into the `AppSettings` class.
- Registers `AppSettings` as a singleton for global access.

**Model:**  
```csharp
public class AppSettings
{
	public LoggingOptions Logging { get; set; }
	public string AllowedHosts { get; set; }
	public string KeyVaultURL { get; set; }
	public MessageBrokerOptions MessageBrokers { get; set; }
	public CosmosOptions CosmosDb { get; set; }
}
```

## 2. Azure Key Vault Integration

Located in:
```
KeyVaults/AddKeyVault.cs
```

```csharp
services.AddKeyVault(builder);
```

- Reads secrets from Azure Key Vault using `KeyVaultURL` from `appsettings.json`.
- Adds secrets to the configuration pipeline.
- Refresh interval is 5 minutes.

```csharp
builder.Configuration.AddAzureKeyVault(
	new Uri(keyVaultUrl),
	new DefaultAzureCredential(),
	new AzureKeyVaultConfigurationOptions
	{
		ReloadInterval = TimeSpan.FromMinutes(5)
	});
```

## 3. Azure Service Bus – Message Broker Setup

### Configuration Model:
```
External/MessageBrokers/MessageBrokerOptions.cs
```

```csharp
public class MessageBrokerOptions
{
	public List<string> Providers { get; set; } = new();
	public AzureServiceBusOptions AzureServiceBus { get; set; } = new();
	public bool UsedAzureServiceBus() =>
		Providers.Any(p => p == "AzureServiceBus");
}
```

### Registration Method:
```
External/MessageBrokers/MessageBrokerDependencyInjection.cs
```

```csharp
services.AddMessageBusSender<T>(
	configuration,
	appSettings.MessageBrokers,
	"pwd-bus-backend-connection-string");
```

- Requires valid key vault secret for the connection string.
- If AzureServiceBus is listed in `Providers`, registers a typed message sender.

## 4. Azure CosmosDB Setup

### Configuration Model:
```
Persistence/CosmosDB/CosmosOptions.cs
```

```csharp
public class CosmosOptions
{
	public string Endpoint { get; set; }
	public string Key { get; set; }
	public List<CosmosDatabaseOptions> Databases { get; set; }
}

public class CosmosDatabaseOptions
{
	public string DatabaseName { get; set; }
	public List<CosmosContainerOptions> Containers { get; set; }
}

public class CosmosContainerOptions
{
	public string ContainerName { get; set; }
	public string PartitionKey { get; set; }
}
```

- Supports multiple databases and containers.
- Used in `DependencyInjection.cs` to call `AddCosmosDB()`.

## 5. Logging Configuration

### Model:
```
Logging/LoggingOptions.cs
```

```csharp
public class LoggingOptions
{
	public Dictionary<string, string> LogLevel { get; set; }
}
```

- Bound from `Logging:LogLevel` in `appsettings.json`.
- Allows scoped logging configuration per component.

## 6. Dependency Injection Entry Point

Main registration method:
```
DependencyInjection.cs
```

```csharp
public static IServiceCollection AddInfrastructure(
	this IServiceCollection services,
	WebApplicationBuilder builder)
{
	var configuration = builder.Configuration;
	var appSettings = builder.Services
		.BuildServiceProvider()
		.GetRequiredService<AppSettings>();

	services.AddCosmosDB(configuration, appSettings.CosmosDb, "pwd-result-db-endpoint", "pwd-result-db-key");

	services.AddMessageBusSender<LayerLoadComputingResultDto>(
		configuration,
		appSettings.MessageBrokers,
		"pwd-bus-backend-connection-string");

	return services;
}
```

## Summary

This layer wires together the following services:

| Feature         | Source File                                 |
|------------------|----------------------------------------------|
| App Settings     | AddAppSettings.cs + AppSettings.cs           |
| Key Vault        | AddKeyVault.cs                               |
| Message Bus      | MessageBrokerOptions.cs, AddMessageBusSender.cs |
| CosmosDB         | CosmosOptions.cs                             |
| Logging          | LoggingOptions.cs                            |
| Service Entry    | DependencyInjection.cs                       |

All services are added via `.AddInfrastructure()` during API startup.

Last updated: March 2025


## CosmosDB Dependency Injection & Repository

### File: `DependencyInjection/AddCosmosDB.cs`

This file defines the registration of CosmosDB using values pulled from Key Vault.

```csharp
services.AddCosmosDB(configuration, appSettings.CosmosDb, "pwd-result-db-endpoint", "pwd-result-db-key");
```

#### Behavior

- Pulls `Endpoint` and `Key` from Key Vault.
- Initializes `CosmosClient` with camelCase serialization.
- Binds the correct container to `ICosmosRepository<TEntity, TId>` based on `CosmosOptions`.

If the specified database or container is not found, a descriptive exception is thrown.

---

### File: `Repositories/CosmosRepository.cs`

This is a generic CosmosDB repository implementation:

```csharp
public class CosmosRepository<TEntity, Tid> : ICosmosRepository<TEntity, Tid>
	where TEntity : Entity<Tid>
```

#### Provided Methods

| Method | Description |
|--------|-------------|
| `AddAsync` | Inserts a new item into the container. |
| `UpdateAsync` | Upserts (inserts or replaces) the item. |
| `PatchAsync` | Applies partial updates using key-value pairs. |
| `DeleteAsync` | Deletes an item by ID and partition key. |
| `GetByIdAsync` | Retrieves a single item or returns null if not found. |
| `FindAsync` | Filters items using a LINQ predicate expression. |

All operations are partition key-aware and follow asynchronous best practices.

---

### Design Highlights

- Abstracted via `ICosmosRepository<TEntity, TId>` for testability and reusability.
- Generic support for all domain entities inheriting from `Entity<Tid>`.
- Repository instance is scoped and injected via DI.

## CosmosDB Dependency Injection & Repository

### File: `DependencyInjection/AddCosmosDB.cs`

This file defines the registration of CosmosDB using values pulled from Key Vault.

```csharp
services.AddCosmosDB(configuration, appSettings.CosmosDb, "pwd-result-db-endpoint", "pwd-result-db-key");
```

#### Behavior

- Pulls `Endpoint` and `Key` from Key Vault.
- Initializes `CosmosClient` with camelCase serialization.
- Binds the correct container to `ICosmosRepository<TEntity, TId>` based on `CosmosOptions`.

If the specified database or container is not found, a descriptive exception is thrown.

---

### File: `Repositories/CosmosRepository.cs`

This is a generic CosmosDB repository implementation:

```csharp
public class CosmosRepository<TEntity, Tid> : ICosmosRepository<TEntity, Tid>
	where TEntity : Entity<Tid>
```

#### Provided Methods

| Method | Description |
|--------|-------------|
| `AddAsync` | Inserts a new item into the container. |
| `UpdateAsync` | Upserts (inserts or replaces) the item. |
| `PatchAsync` | Applies partial updates using key-value pairs. |
| `DeleteAsync` | Deletes an item by ID and partition key. |
| `GetByIdAsync` | Retrieves a single item or returns null if not found. |
| `FindAsync` | Filters items using a LINQ predicate expression. |

All operations are partition key-aware and follow asynchronous best practices.

---

### Design Highlights

- Abstracted via `ICosmosRepository<TEntity, TId>` for testability and reusability.
- Generic support for all domain entities inheriting from `Entity<Tid>`.
- Repository instance is scoped and injected via DI.