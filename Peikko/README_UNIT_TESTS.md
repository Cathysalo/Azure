# Unit Tests – Peikko Precast Wall Designer

## Overview
This document provides an overview of the unit testing implementation that validates the correctness of computations.

## Testing Framework
The project uses **xUnit** as the unit testing framework.

## Test Structure
The unit tests are located in:
```
PeikkoPrecastWallDesigner.UnitTests/
```

Key testing files:
- `ComputingDomainServiceTests.cs`

## Test Cases and Validations

### 1. **Geometry Validation - Valid Data**
- Ensures that layers and hole dimensions conform to expected constraints.
- Validates the **InternalLayer**, **ExternalLayer**, and **Hole**.

### 2. **Valid Hole Position**
- Tests if the provided hole position is valid (`Internal`, `External`, or `Both`).

### 3. **Invalid Insulation Thickness**
- Ensures insulation thickness follows design constraints.
- Throws `GeometryValidationException` if the value is out of range.

### 4. **Out-of-Bounds Center of Gravity**
- Ensures the center of gravity for wall layers is correctly positioned.
- Fails if calculations place it outside acceptable limits.

### 5. **Invalid Hole Position Input**
- Ensures the position value is valid (`Internal`, `External`, `Both`).
- Throws `GeometryValidationException` if invalid.

## Running Unit Tests
To run the tests, navigate to the unit test directory and execute:

```sh
dotnet test
```

This command runs all unit tests and outputs results.

## Expected Test Results
- Tests should pass without exceptions if data is valid.
- Validation exceptions should be thrown for incorrect inputs.
- Ensures stability before deploying changes.
