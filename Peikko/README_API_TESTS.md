# API Tests – Peikko Precast Wall Designer

## Overview

This document provides an overview of the API testing implementation that validates endpoint behavior and integration across layers.

## Testing Framework

The project uses **xUnit**, **WebApplicationFactory**, and **HttpClient** to simulate real HTTP requests to the API.

## Test Structure

The API tests are located in:

```
PeikkoPrecastWallDesigner.Api.Tests/
```

Key testing files:

- `ComputingApiTests.cs`

## Test Cases and Validations

### 1. **GET /api/computations/test**

- Verifies if the API is up and running.
- Expected result: `"Success"` with status `200 OK`.

### 2. **POST /api/computations/loads**

- Sends a valid `LayersDto` payload.
- Verifies that the API starts the computation and returns a valid ID and `Processing` status.

### 3. **PATCH /api/computations/loads**

- Updates an existing computation result.
- Verifies that status changes to `Completed`.

### 4. **GET /api/computations/loads/{id}**

- Retrieves the result by ID.
- Verifies that the response contains the correct values and updated status.

## Running API Tests

To run the tests, navigate to the test directory and execute:

```sh
dotnet test
```
```

This command runs all API integration tests and outputs results.

## Expected Test Results

- Successful responses should return status code `200 OK`.
- Data integrity is verified by comparing IDs and status.
- Provides end-to-end confidence that the system behaves as expected when accessed via HTTP.
