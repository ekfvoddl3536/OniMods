# Types
## Primitive
| Type | Size | Range | Example |
| :--: | :--: | :---- | :------ |
| `bool` | `1 byte` | `.true` OR `.false` | `.true` |
| `int` | `4 byte` | `-2147483648` ~ `2147483647` | `10` |
| `uint` | `4 byte` | `0` ~ `4294967296` | `10u` |
| `long` | `8 byte` | `-9223372036854775808` ~ `9223372036854775807` | `10L` |
| `ulong` | `8 byte` | `0` ~ `18446744073709551616` | `10UL` |
| `float` | `4 byte` | `±3.4028235E±38` (Epsilon: `1.401298E-45`) | `10f` |
| `double` | `8 byte` | `±1.7976931348623157E±308` (Epsilon: `4.94065645841247E-324`) | `10d` |
| `string` | `4 or 8 byte` | `utf-8` | `"hello"` |

## Instance
| Type | Constructors |
| :--: | :----------- |
| `offset` | `(vec2)`, `(int, int)` |
| `vec2` | `(float, float)` |
| `vec3` | `(float, float)`, `(float, float, float)` |
| `vec4` | `(float, float)`, `(float, float, float)`, `(float, float, float, float)` |
| `marg` |  |
| `entities` |  |
| `entity` |  |
| `levels` |  |
| `level` |  |

## Array
#### Base
```asm
<type>[]
```
#### Example
**Only bool type is possible**
```asm
void sample(bool[] vs):
  ret
```

# Declare
## Method
#### Base
```asm
<type> <name>(<type> <name>,...):
  ret
```

#### Example
```asm
void myMethod():
  ret
```
```asm
int sum(int x, int b):
  x + b
  ret
```

### Local Variable
#### Base
```asm
local(<type>) <name>
```

#### Example
```asm
void method():
  local(int) a
  a = 7
  ret
```


## Field
#### Base
```asm
global <type> <name>

void main():
  ret
```
#### Example
```asm
global int myField_1
global int a
global int b

int sum(int a):
  a + this.myField_1
  ret

int complex(int a, int b):
  local(int) result
  result = a + b
  result = result * this.a
  result = result - this.b
  result
  ret
```

# Main
## Script Trigger
```csharp
void tmain(marg cmd):
  ret
```

## Script Trigger Control
```csharp
bool tcmain(bool input, marg cmd):
  .false
  ret
```

## Script Trigger Gate
```csharp
bool tgmain(bool input1, bool input2 marg cmd):
  .false
  ret
```

## Script Ribbon Reader Panel
```csharp
void rpmain(bool[] inputs, bool[] outputs, marg cmd):
  ret
```

## Script Ribbon Bit Calculator
```chsarp
void rbmain(bool[] input0, bool[] input1, bool[] outputs, bool reset, marg cmd):
  ret
```

## `update` Special Method
Run once every 33ms
```chsarp
void update(float dt, marg cmd):
  ret
```

# Details
see [here](https://github.com/ekfvoddl3536/OniMods/blob/test/documents/en-us/Magic%20Script%20V5%20Update%20Notice.md)
