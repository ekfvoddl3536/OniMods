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
| add | '+' |
| sub | '-' |
| mul | '*' |
| div | '/' |
| mod | '%' |

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
| AND | '&' |
| OR | '|' |
| XOR | '^' |





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


