
# Peikko API - Validation Rules

## Overview
This document outlines the validation rules enforced on API inputs.

---

## LayersDto Validation
Defines the structure and constraints for layers and associated properties.

| Property                  | Type      | Required | Validation Rules |
|---------------------------|----------|----------|------------------|
| InternalLayer             | LayerDto | Yes      | Must contain valid layer details |
| ExternalLayer             | LayerDto | Yes      | Must contain valid layer details |
| InsulatedLayerThickness   | double   | Yes      | Should be a positive number |
| Hole                      | HoleDto  | Yes      | Must contain valid hole details |

---

## LayerDto Validation
Represents the structure of a single layer within the computations.

| Property   | Type    | Required | Validation Rules |
|------------|--------|----------|------------------|
| Name       | string | Yes      | Cannot not be empty  |
| X          | double | Yes      | Must be a valid coordinate |
| Y          | double | Yes      | Must be a valid coordinate |
| Width      | double | Yes      | Must be greater than zero |
| Height     | double | Yes      | Must be greater than zero |
| Thickness  | double | Yes      | Must be greater than zero |

---

## LayerLoadDto Validation
Represents computed load properties for a layer.

| Property       | Type    | Required | Validation Rules |
|---------------|--------|----------|------------------|
| Name          | string | Yes      | Cannot not be empty |
| SurfaceArea   | double | No       | Auto computed |
| Volume        | double | No       | Auto computed |
| WeightKg      | double | No       | Auto computed |
| WeightKn      | double | No       | Auto computed |

---

## HoleDto Validation
Defines the properties of holes within a layer.

| Property   | Type    | Required | Validation Rules |
|------------|--------|----------|------------------|
| Name       | string | Yes      | Cannot not be empty  |
| X          | double | Yes      | Must be a valid coordinate |
| Y          | double | Yes      | Must be a valid coordinate |
| Width      | double | Yes      | Should be greater than zero |
| Height     | double | Yes      | Should be greater than zero |
| Position   | string | Yes      | Must be either "Internal" or "External" |

---

## Model Validation in Controller
The controller checks if input data follows these rules before processing:
- If a required field is missing or invalid, the API returns a bad request.
- If ModelState validation fails, the API responds with an error message detailing incorrect values.