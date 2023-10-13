# Development guide
## Preparation
The latest `0SuperComicLib.Core.dll`, `1SuperComicLib.eTxt.dll` and `0SuperComicLib.ModONI.dll` are required, of which `0SuperComicLib.ModONI.dll` must be referenced to the project.  
- `0SuperComicLib.Core.dll` (Referenced by `0SuperComicLib.ModONI.dll` and `1SuperComicLib.eTxt.dll`)
- `1SuperComicLib.eTxt.dll` (Referenced by `0SuperComicLib.ModONI.dll`)
- `0SuperComicLib.ModONI.dll`
  
`0SuperComicLib.Core.dll` and `1SuperComicLib.eTxt.dll` are available [here](https://github.com/ekfvoddl3536/0SuperComicLibs/releases), and `0SuperComicLib.ModONI.dll` is available [here](https://github.com/ekfvoddl3536/OniMods/releases).  
You can download all three DLLs [here](https://github.com/ekfvoddl3536/OniMods/releases/latest).  
Or you can get it by subscribing and downloading one of the mods I've made.  
  
Choose the more convenient way.  
  
   
--------
## Define behavior when a mod is loaded
```csharp
using SuperComicLib.ModONI;
//
// ...Omit namespace definition...
//
public class MyMod : KMod.UserMod2
{
    public override void OnLoad(HarmonyLib.Harmony harmony)
    {
        // Maximum number of string keys to use.
        // Although you can specify exactly how much size you want to use,
        // you can also use the FixedCapacity enum to quickly select a power-of-two size.
        
        // Whichever you choose, unlike List<>, it does not auto-grow in size,
        // so an error will occur if the number of keys added becomes larger
        // than the initial size.

        StringKeyList keys = new StringKeyList(FixedCapacity.SZ_16);
        // -or-
        // StringKeyList keys = new StringKeyList(16);

        // All `Add...` methods perform a `id.ToUpper()` operation on the provided `id`.

        // The `Add` method adds one key.
        // This can be useful when you need to add things like tooltips.
        keys.Add("STRINGS.UI.UISIDESCREENS.MYMOD_SLIDER.TOOLTIP")
            .Add("STRINGS.UI.UISIDESCREENS.MYMOD_SLIDER.TITLE");
        
        // `ADD_BUILDINGS` adds ".NAME", ".DESC", and ".EFFECT" under 
        // `"STRINGS.BUILDINGS.PREFABS." + id.ToUpper()` in the order.

        //  When written as follows,
        //  "STRINGS.BUILDINGS.PREFABS.HI.NAME"
        //  "STRINGS.BUILDINGS.PREFABS.HI.DESC"
        //  "STRINGS.BUILDINGS.PREFABS.HI.EFFECT"
        //  are added in order.
        keys.ADD_BUILDINGS("hi");

        // The above code has the same effect as writing the following
        // three lines of code.
        keys.Add("STRINGS.BUILDINGS.PREFABS.HI.NAME")
            .Add("STRINGS.BUILDINGS.PREFABS.HI.DESC")
            .Add("STRINGS.BUILDINGS.PREFABS.HI.EFFECT");
        
        // `ADD_EQUIPMENT` methods similarly to `ADD_BUILDINGS`, 
        // adding in the order of NAME, DESC, EFFECT. 
        // However, the prefix string connected before the `id` is 
        // "STRINGS.EQUIPMENT.PREFABS.".
        keys.ADD_EQUIPMENT("sweater");

        // The above code has the same effect as writing the following
        // three lines of code.
        keys.Add("STRINGS.EQUIPMENT.PREFABS.SWEATER.NAME")
            .Add("STRINGS.EQUIPMENT.PREFABS.SWEATER.DESC")
            .Add("STRINGS.EQUIPMENT.PREFABS.SWEATER.EFFECT");
        
        // This class is a child of `x` and holds the original text to 
        // be used in the mod.
        // The next chapter will teach you how to define this class.
        new SourceTexts().Apply(keys);
    }
}
```  
    
-------
     
## Source texts provider class design
```csharp
using SuperComicLib.ModONI;
//
// ...Omit namespace definition...
//
public class SourceTexts : ModStrings
{
    // This guid is not important as it is only used for identification
    // when a problem occurs.
    // it is recommended, but not required, to provide your mod with a unique guid.
    public SourceTexts() : base(Guid.Empty)
    {
    }

    // Add values in the order of keys added in the previous chapter.
    protected override string[] OnLoadOriginalStrings() 
    {
        // If you need to use a newline character, 
        // be sure to use `System.Environment.NewLine`.
        string newLine = System.Environment.NewLine;

        // Ignoring the duplicate keys added for explanation purposes 
        // in the previous chapter, the values must be generated in the
        // following order:
        return new string[]
        {
            // STRINGS.UI.UISIDESCREENS.MYMOD_SLIDER.TOOLTIP
            "Tooltip!" + newLine +
            "Second line.",
            // STRINGS.UI.UISIDESCREENS.MYMOD_SLIDER.TITLE
            "Title sample",

            // STRINGS.BUILDINGS.PREFABS.HI.NAME
            "HI Name sample",
            // STRINGS.BUILDINGS.PREFABS.HI.DESC
            "HI Desc sample",
            // STRINGS.BUILDINGS.PREFABS.HI.EFFECT
            "HI Effect sample",

            // STRINGS.EQUIPMENT.PREFABS.SWEATER.NAME
            "Sample name",
            // STRINGS.EQUIPMENT.PREFABS.SWEATER.DESC
            "Sample description",
            // STRINGS.EQUIPMENT.PREFABS.SWEATER.EFFECT
            "Sample effect",
        };
    }
}
```
    
If you've written it this way, all steps are now completed, and you're ready to build.  
  
If you'd like to see an actual example without any comments or explanations, check [here](https://github.com/ekfvoddl3536/OniMods/blob/master/src/FastWirelessAutomation_OV2/ModLoad.cs).  
