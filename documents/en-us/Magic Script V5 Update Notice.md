# Label
Note the difference between the semicolon(`;`) and the colon(`:`) 

## Example
```
label(.label_0):
label(.label_1):
label(.my_label):
```



# if, else
## Operators
| (AF) Operator |
|  :------: |
| `!=` |
|  `==` |
|  `>=` |
|  `<=` |
|  `<` |
| `>` |

## Example
```
if (10 == 20)
{
  (somecode)
}
else if (...)
{
   
}
else
{
   
}
```



## Arithmetic
| Command | Operator |
| :-----: | :------: |
| add | `+` |
| sub | `-` |
| mul | `*` |
| div | `/` |
| mod | `%` |

## Example
```
x = x + 100
```



## Bitwise
The `and`, `or`, and `xor` commands are replaced by `bitwise operators`.  

| Command | Operator |
| :-----: | :------: |
| AND | `&` |
| OR | `|` |
| XOR | `^` |
| - | `<<` |
| - | `>>` |



## Operator precedence
The default parser does not take into account the run order and the operator on the left always runs first.  



## Method
### Basic
```
void MyFirstFunc():
  <...do something...>
  ret
```

### Return & Parameters
The following is an example of a function that returns the sum of `a` and` b`  
```
int MyFirstFunc(int a, int b):
   a + b
   ret
```
```
int MyFirstFunc(int x, int y):
  local(int) tmp
  tmp = x + y
  tmp
  ret
```

### Declare Local Variable
**Local variable names cannot be the same as parameter names**  
```
local(<type>) <name>
```

### Call
You can call other functions from within the function  
Only `constants` or `parameters` or `local variables` or `global variables` are allowed for `<value>`  

`<value>` is not needed if the function you want to call does not have any parameters  
```
has parameter:
<name>(<value>,...)

doesn't has parameter:
<name>()
```

If the return type of the function you want to call is not `void`, you can save the return value to a local variable  
```
local(<return type>) <var name>

<var name> = <func name>(<value>,...)
```

### `global` & `this` Keyword
This new keyword is basically used when declaring a global variable outside of a function  
When using declared global variables must be used with `this` keyword  

```
global int a

int func(int a):
  this.a + a
  ret
```



## Instance type
### Call function

```
void func(marg value):
   value::minions()
   ret
```

### Access to field
**WARNING** The example below does not work  
```
void func(marg value):
   value::minions
   ret
```





## Loop statement
### for
It is a loop composed of `Initialization`,` Conditional`, and `Updation`  

```csharp
for (int a = 0; a < 4; a++)
{
   ...
}
```

#### Initialization
You can declare local variables to be used here.  
You can declare multiple variables with different names of the same type.   

```csharp
for (int a = 0, b = 0, c = 0, ...; ;)
{
}
```

#### Conditional
```csharp
for (; a < 10;)
{
}
```


#### Updation
This is where it runs later than `for body`  

```csharp
for (; ; a++)
{
}
```


### foreach
Some instance types support `foreach` iterations  


```csharp
void func(marg value):
   foreach (entity a : value::minions())
   {
         
   }
   ret
```





## Array
Types use the `[]` suffix and use it as a variable name `[<number> or <int value>]`  


```csharp
int get():
   1
   ret

void func(bool[] values):
   values[0] = .false
   values[get()] = .false
   ret
```





## ++, -- Unary operators
Each acts as `x = x + 1` and` x = x-1`, resulting in more efficient code.  
Unary operators should only be used alone.  


#### ok
```csharp
int func(int x):
   x++
   ret
```

#### wrong
```csharp
int func(int x):
   x++ ^ 100
   ret
```



### `new` operator 
To avoid errors during execution, you must call only the public constructor.  
(Please refer to the provided API documentation)  

```chsarp
offset func():
   new offset(10, 10)
   ret
```





## Special data types
| Name | Desc. | Note |
| :---: | :---- | :--- |
| `marg` |  |  |
| `entities` | **list** of duplicant or animal data | can be used for `foreach` |
| `entity` | duplicant or animal **single** data | |
| `levels` | **list** of duplicant skill data (`animals` do not have this) | can be used for `foreach` |
| `level` | **single** skill data (`animals` do not have this) | |





## Special functions
The following function names and signatures are used as the entry point to the script.  
You can define multiple entry points in one file.  

If you define all entry points in one file, you can use that for all script triggers.  



| Name | Signature | Caller |
| :---: | :---- | :--- |
| `tmain` | `void()` | Script Trigger | 
| `tcmain` | `bool(bool)` | Script Trigger Control |
| `tgmain` | `bool(bool, bool)` | Script Trigger Gate |
| `rpmain` | `void(bool[], bool[])` | Script Ribbon Reader Panel |
| `rbmain` | `void(bool[], bool[], bool[], bool)` | Script Ribbon Bit Calculator |

