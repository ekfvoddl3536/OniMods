# ITPMM - String Translation Guide
## Table of Contents
- [ITPMM - String Translation Guide](#itpmm---string-translation-guide)
  - [Table of Contents](#table-of-contents)
  - [Scope of translation](#scope-of-translation)
  - [Find original strings](#find-original-strings)
  - [Directory structure](#directory-structure)
  - [File naming convention](#file-naming-convention)
  - [Format of the file](#format-of-the-file)
    - [Caution](#caution)
  - [Download full-example](#download-full-example)

## Scope of translation
Not every MOD that exists can be translated this way.  
  
In order to support ITPMM, MOD developers must develop this method by following several rules.  
  
Some MODs developed that way can provide strings translated in this way, and it's simple and easy!   
   
   
## Find original strings
When a MOD that supports ITPMM is first installed and loaded for the first time, it exports the original string information used internally.  
  
The original string is the language the MOD uses by default when no translation from ITPMM or an external source is provided.  
For MODs I made, the default is **'한국어 (Korean)'**.   
  
**Step by step:** 
1. Subscribe or install a mod, and start playing the game.
2. Press the `Mods` button to open the mod management screen, enable the mod, and then restart the game.  
3. Locate the original string text file that was created.   
    + Using `Mod Manager // by @Ony` mod (**recommended**)   
        * Click the `Open Folder` button to the right of the Mod entry.
        * Check `original_strings_UTF8.txt` created in the `resources` folder.
    + Manual explore   
        * Open the following folder:  
            + Windows `C:\Users\[USERNAME]\Documents\Klei\OxygenNotIncluded\mods` or `%userprofile%\Documents\Klei\OxygenNotIncluded\mods`
            + Mac OS `/Users/[USERNAME]/Library/Application Support/unity.Klei.Oxygen Not Included/mods`  
            + Linux `~/.config/unity3d/Klei/Oxygen Not Included/mods`  
        * Open the `steam`(or `Steam`) folder.  
        * Get the URL of the subscribed mod, then check the `id`.  
        * Find and open the folder matching the number to `id`.
        * Check `original_strings_UTF8.txt` created in the `resources` folder.
  
  
## Directory structure
To provide translations, you need a folder structure that follows these rules:  
```
[name of translation mod (folder)]/
├─ [any name 1 (folder)]/
│  ├─   ... translatoin files ...
├─ [any name 2 (folder)]/
│  ├─   ... translatoin files ...
├─ scpatch.conf
```     
One translation mod can provide translations for multiple mods.   
   
The `scpatch.conf` file allows this folder to be loaded by `ITPMM`.   
So, it is an **essential file**. The content can be empty.  
  
The following structure shows providing English and Japanese and Korean translations for one mod:  
```
First Translation Mod/
├─ Mod 1/
│  ├─ en-us.txt
│  ├─ ja-jp.txt
│  ├─ ko-kr.txt
├─ scpatch.conf
```  
When sharing the completed translation mod, provide all contents including sub-folders within the `First Translation Mod` folder.  
   
The folder names `First Translation Mod` and `Mod 1` can be freely chosen.  
While it's recommended to set the `Mod 1` folder name to the name of the mod you intend to translate for identification purposes, it's not mandatory.  
  

## File naming convention
Translation files use the two-letter language code and two-letter country code as their names, as we saw earlier.  
  
The first two-letter language code follows `ISO 639-1`, and the second two-letter country code follows `ISO 3166-1`.  
  
* [List of ISO 639-1 codes (Wikipedia)](https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes)
* [List of ISO 3166-1 codes (Wikipedia)](https://en.wikipedia.org/wiki/ISO_3166-1)  
  
`ITPMM` is case insensitive for the file names provided.  
  
Currently, I have no plans to support `BCP-47` tags beyond the two-letter country codes.   
Therefore, the present `ITPMM` cannot differentiate and load Simplified and Traditional Chinese separately.   
   
As examples, the following file names are accepted:  
```
en-us

ko-kr

zh-cn

ja-jp
```


## Format of the file
The following is part of the contents of `original_strings_UTF8.txt`, which was automatically generated by one of my mods.
```
# Strings index [ 5 ]
STRINGS.BUILDINGS.PREFABS.AUTOPURIFYWASHSINK.NAME: "<link=\"AUTOPURIFYWASHSINK\">친환경 세면대</link>"

# Strings index [ 6 ]
STRINGS.BUILDINGS.PREFABS.AUTOPURIFYWASHSINK.DESC: "복제체로부터 세균을 제거하고, 만들어진 오염수를 정수하여 재사용합니다."

# Strings index [ 7 ]
STRINGS.BUILDINGS.PREFABS.AUTOPURIFYWASHSINK.EFFECT: "세균에 덮인 복제체는 선택된 방향으로 지나갈 때 세면대를 사용합니다.
모래가 있으면, 오염된 물을 깨끗한 물로 정수합니다. 정수된 물은 재사용합니다.
전력을 공급하면 더 빨리 정수할 수 있고, 보다 효율적으로 사용할 수 있습니다."
```
  
The overall structure is `ID: [Text]`  
  
There are three important points.  
  
The first is text starting with `#`. **This is a comment.**  
  
The second is text surrounded by double quotes.  
In this format, text that spans multiple lines is also surrounded by a pair of double quotation marks.  
  
The third is a tag surrounded by angle brackets(`<`, `>`) or curly brackets(`{`, `}`). This is a reserved text within the game and you must not change this text to anything else when translating.  
```
# Allow
#   1. Move position & translation
SAMPLE1: "Here is my <link=\"AUTOPURIFYWASHSINK\">Text</link>"

# Original -> "이 시설은 보수가 필요하기 전까지 \"{FlushesRemaining}\"명이 이용할 수 있습니다."
#   Same as 1.
SAMPLE2: "\"{FlushesRemaining}\" individuals can use this facility before maintenance is required."

#   2. Tags enclosed in angle brackets can be deleted.
SAMPLE3: "Here is my Text!"


# Disallow
#   1. Change tag
SAMPLE4: "<link=\"HI\">Text</link>"

#   2. In the case of curly bracket tags, deletion is also prohibited.
SAMPLE5: "20 individuals can use this facility before maintenance is required."
```
  
  
### Caution
A backslash (`\`) is combined with one character after it to indicate an escape sequence, so if you want to express a backslash (`\`), you must use the backslash twice.  
  
This applies regardless of whether it is surrounded by a single double quote or three double quotes.  
```
# New Line
SAMPLE1: "\n"

# Backslash
SAMPLE2: "\\"
```
  
The file extension must be a text file (`.txt`).  
  
Additionally, the encoding of the file must be `UTF-8`.  
  

## Download full-example
You can download a working full-example [here](https://drive.google.com/file/d/1a9CeT2PbVGcMfw9k0KuGNlmdD9ntz1Sv/view?usp=sharing).  
  
  