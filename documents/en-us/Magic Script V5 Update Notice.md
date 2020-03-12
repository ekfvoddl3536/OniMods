# Changes (2020. 02. 13.)
* Apply new parser  
    * Completely new grammar/syntax  
    * Generate compiled code
    * High code execution speed
    * Higher readability
    * Support `function`
    * Support `local variables`
    * Support `global variables`
    * Add `data type` concept
* Apply `UTF-8 (no BOM)` encoding
* Removed all commands except goto
* Added `label`, `local` keywords
* Added `+ operator`, `- operator`, `* operator`, `/ operator`, `% operator`
* Added `& operator`, `| operator`, `^ operator`
* Added `if`, `else if`, `else`
* Added `== operator`, `!= operator`, `>= operator`, `<= operator`, `> operator`, `< operator` 
* Added `int`, `long`, `float`, `double`, `string` data types
* Added `= operator`
* Added `global`, `this` keywords


# Additional Changes (2020. 03. 01.)
* Apply `UTF-8 (no BOM)` encoding for all `.oni.script`
* Apply `C# Default` encoding for all `.txt`
* Old commands merged into `marg`
* Added `<< operator`, `>> operator`
* Added `for`, `foreach`
* Added `bool`, `byte`, `sbyte`, `ushort`, `short` data types
* Added `marg`, `entities`, `entity`, `levels`, `level` **special data types**
* Support `tmain`, `tcmain`, `tgmain`, `rpmain`, `rbmain` **special functions**
* Add `array` concept
   * Added `[] operator`
* Support `bool[]` data type
* Support `++ operator`, `-- operator` unary operator
* Supports call functions or access fields defined in the instance type.
   * Added `:: operator`


# Additional Changes (2020. 03. 01. Final)
* **Removed** `byte`, `sbyte`, `ushort`, `short` data types
* Added `uint`, `ulong` data types
* Added `new` operator
   * Support `vec2` `vec2i`, `vec3`, `vec3i`, `vec4` struct data type
   * Support `offset` struct data type




# Additional Changes (2020. 03. 03. Final)
* No longer supports `.oni.script`, use `.onisr` instead
* Apply `UTF-8 (no BOM)` encoding for all `.onisr`


### Before
```
push 0;
.main;
cmp %0, 100;
jnl .l1;
add %0, 1;
goto .main;
.l1;
pop;
```

### After
```
void __main():
  local(int) a
  a = 0
  
  label(main):
  
  if (a < 100)
  {
    a = a + 1
    goto main
  }
  
  ret
```

# Upgrade script
**Non-upgraded scripts will no longer be supported starting with the 'Magic Script V5' update**  



## Label
Note the difference between the semicolon(`;`) and the colon(`:`) 

### Example
#### Before
```
.label_0;
.label_1;
.my_label;
```

#### After 
```
label(.label_0):
label(.label_1):
label(.my_label):
```



## Cmp ... Jump
The `cmp` and `je`, `jne`, `jl`, `jg`, `jge/jnl`, `jle/jng` commands are replaced with a single 'if'  

| Before | After |
| :---- | :----- |
| cmp `x`, `y` | if (`x` `<operator>` `y`) |

### Operators
| (BF) Command | (AF) Operator |
| :--------: | :------: |
| je | `!=` |
| jne | `==` |
| jl | `>=` |
| jg | `<=` |
| jge/jnl | `<` |
| jle/jng | `>` |

### Example
#### Before
```
cmp 10, 20;
jne .end_if;
(somecode)
.end_if;
```

#### After
```
if (10 == 20)
{
  (somecode)
}
```



## Arithmetic
The `add`, `sub`, `mul`, `div`, and `mod` commands are replaced by `arithmetic operators`.  


| Command | Operator |
| :-----: | :------: |
| add | `+` |
| sub | `-` |
| mul | `*` |
| div | `/` |
| mod | `%` |

### Example
#### Before
```
add %0, 100
```

#### After
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






## Data types
### Constant Value
Now all constant values must specify a data type  
Refer to the table below to determine the data type of the constant value  

| Expression | Data Type | Range |
| :--------: | :-------: | :---- |
| 100 | `int` | `-2147483648` ~ `2147483647` |
| 100L | `long` | `-9223372036854775808` ~ `9223372036854775807` |
| 100u | `uint` | `0` ~ `4294967295` |
| 100UL | `ulong` | `0` ~ `18446744073709551615` |
| 100f | `float` |  `-3.4E+38` ~ `3.4E+38` (epsilon: `3.4E-38`) |
| 100d | `double` | `-1.79E+308` ~ `1.79E+308` (epsilon: `1.79E-308`) |
| "100" | `string` | `utf-8` |





## Function
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
