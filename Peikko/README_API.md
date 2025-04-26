# Peikko Precast Wall API - Documentation

## Base URL
```
http://localhost:5000/api/computations
```

## 1. GET /api/computations/test
**Description:**
This endpoint verifies if the API is active and responsive.

**Request Format:**
```
GET /api/computations/test
```

**Expected Response:**
```json
"Success"
```

**Status Codes (based on implementation):**
- 200 OK – API is operational

---

## 2. POST /api/computations/loads
**Description:**
Processes the given layer parameters and computes load values accordingly.

**Request Format:**
```
POST /api/computations/loads
```

**Headers:**
```json
Content-Type: application/json
```

**Example Request Body:**
```json
{
  "internalLayer": {
    "name": "InternalTest",
    "x": 1,
    "y": 1,
    "width": 1,
    "height": 1,
    "thickness": 1
  },
  "externalLayer": {
    "name": "ExternalTest",
    "x": 1,
    "y": 1,
    "width": 1,
    "height": 1,
    "thickness": 1
  },
  "insulatedLayerThickness": 90,
  "hole": {
    "name": "bothTest",
    "x": 1,
    "y": 1,
    "width": 1,
    "height": 1,
    "position": "External"
  }
}
```

**Response:**
Returns the computed result with an ID and processing status.

**Status Codes (based on implementation):**
- 200 OK – Computation started successfully
- 400 Bad Request – Model state invalid
- 500 Internal Server Error – Exception thrown during processing

---

## 3. POST /api/computations/background/loads
**Description:**
Initiates background computation of the layer loads.

**Request Format:**
```
POST /api/computations/background/loads
```

**Headers:**

```json
Content-Type: application/json
```

**Request Body:**
Same as POST /loads.

**Response:**
Returns background computation results.

**Status Codes (based on implementation):**

- 200 OK – Computation processed
- 400 Bad Request – Model state invalid
- 500 Internal Server Error – Exception occurred

---

## 4. GET /api/computations/loads/{id}
**Description:**
Retrieves the computed load results based on the provided identifier.

**Request Format:**
```
GET /api/computations/loads/{id}
```

**Response:**
Returns computation result matching the ID.

**Status Codes (based on implementation):**
- 200 OK – Computation found
- 500 Internal Server Error

---

## 5. PATCH /api/computations/loads
**Description:**
Modifies an existing computation result with updated values.

**Request Format:**
```
PATCH /api/computations/loads
```

**Headers:**
```json
Content-Type: application/json
```

**Example Request Body:**
```json
 Note: The `"value"` field should be a serialized JSON string (i.e., an array converted to a string). Ensure the array format is valid before serialization.

{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "value": "[{\"Name\":\"UpdatedTest\",\"SurfaceArea\":10,\"Volume\":20,\"WeightKg\":30,\"WeightKn\":40}]",
  "status": "Completed"
}
```

**Response:**
```json
"Success"
```

**Status Codes (based on implementation):**
- 200 OK – Update successful
- 400 Bad Request – Model state invalid or ID is empty
- 500 Internal Server Error – Exception occurred during patch

---

## API Status Code Summary

| Endpoint                                 | Method | 200 OK                       | 400 Bad Request                      | 500 Internal Server Error             |
|------------------------------------------|--------|------------------------------|--------------------------------------|----------------------------------------|
| /api/computations/test                   | GET    | API is operational           | –                                    | –                                      |
| /api/computations/loads                  | POST   | Returns computation result   | Model state invalid                  | Exception during computation           |
| /api/computations/background/loads       | POST   | Returns background result    | Model state invalid                  | Exception during background execution  |
| /api/computations/loads/{id}             | GET    | Returns result by ID         | –                                    | Exception during retrieval             |
| /api/computations/loads                  | PATCH  | Updates result               | Invalid model state or missing ID   | Exception during update                |

---

## Automated Testing
The API incorporates automated testing to validate its functionality and ensure stability before deployment.

### Running the Tests
To execute the test suite, use the following command:
```sh
cd path/to/PeikkoPrecastWallDesigner.Api.Tests
dotnet test
```
This command runs all unit tests and provides a summary of results in the terminal.

### Test Scope
The following components are validated through automated testing:
- API availability (`GET /test`)
- Layer load computation (`POST /loads`)
- Computation retrieval (`GET /loads/{id}`)
- Updating computation results (`PATCH /loads`)

---