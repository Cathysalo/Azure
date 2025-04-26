
# Peikko API - Data Models Documentation

## **Overview**
This document outlines the data models (DTOs) used in the Peikko Precast Wall API for requests and response structures.

---

## **LayersDto**
Represents a **set of layers** and associated properties for computations.

### **Properties:**
| Name                     | Type      | Description |
|--------------------------|----------|-------------|
| `InternalLayer`          | `LayerDto` | The internal layer properties. |
| `ExternalLayer`          | `LayerDto` | The external layer properties. |
| `InsulatedLayerThickness` | `double`  | Thickness of the insulation layer. |
| `Hole`                  | `HoleDto` | Information about a hole in the layers. |

### **Example JSON:**
```json
{
  "internalLayer": {
    "name": "Internal",
    "x": 1.0,
    "y": 1.0,
    "width": 10.0,
    "height": 5.0,
    "thickness": 20.0
  },
  "externalLayer": {
    "name": "External",
    "x": 2.0,
    "y": 2.0,
    "width": 15.0,
    "height": 6.0,
    "thickness": 25.0
  },
  "insulatedLayerThickness": 90.0,
  "hole": {
    "name": "MainHole",
    "x": 3.0,
    "y": 3.0,
    "width": 2.0,
    "height": 2.0,
    "position": "External"
  }
}
```

---

## **LayerDto**
Describes **basic geometry** for the wall layers.
### **Properties:**
| Name       | Type     | Description |
|------------|---------|-------------|
| `Name`     | `string` | Layer name (required). |
| `X`        | `double` | X coordinate of the layer. |
| `Y`        | `double` | Y coordinate of the layer. |
| `Width`    | `double` | Width of the layer. |
| `Height`   | `double` | Height of the layer. |
| `Thickness` | `double` | Thickness of the layer. |

### **Example JSON:**
```json
{
  "name": "InternalLayer",
  "x": 1.0,
  "y": 1.0,
  "width": 10.0,
  "height": 5.0,
  "thickness": 20.0
}
```

---

## **LayerLoadDto**
Represents **computed load properties** for a layer.

### **Properties:**
| Name         | Type     | Description |
|-------------|---------|-------------|
| `Name`      | `string` | Name of the layer (required). |
| `SurfaceArea` | `double` | Computed surface area of the layer. |
| `Volume`    | `double` | Computed volume of the layer. |
| `WeightKg`  | `double` | Weight in kilograms. |
| `WeightKn`  | `double` | Weight in kilonewtons. |

### **Example JSON:**
```json
{
  "name": "ConcreteLayer",
  "surfaceArea": 50.0,
  "volume": 100.0,
  "weightKg": 2400.0,
  "weightKn": 24.0
}
```

---

## **HoleDto**
Defines the properties of **holes** within a layer.

### **Properties:**
| Name       | Type     | Description |
|------------|---------|-------------|
| `Name`     | `string` | Name of the hole (required). |
| `X`        | `double` | X coordinate of the hole. |
| `Y`        | `double` | Y coordinate of the hole. |
| `Width`    | `double` | Width of the hole. |
| `Height`   | `double` | Height of the hole. |
| `Position` | `string` | Position of the hole (e.g., `Internal`, `External`). |

### **Example JSON:**
```json
{
  "name": "Hole1",
  "x": 3.0,
  "y": 3.0,
  "width": 2.0,
  "height": 2.0,
  "position": "Internal"
}
```

---

## **Usage**
This documentation is based on the current API implementation.

---