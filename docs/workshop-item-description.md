# Steam Workshop Item title and description
**Last Published to Workshop**: 12/10/2025 (PST)    

## AdvancedGenerators_OV3
### Title
```plaintext
[Vanilla + DLC] Advanced Generators+ V2.0
```

### Description
```plaintext
[h1] Mod Translation [/h1]
[url=https://steamcommunity.com/sharedfiles/filedetails/?id=2785073113] SuperComic's MODS Translation - EN/JP/CH (AUTO) [/url]

[h1] Generator Added [/h1]
[olist]
[*] Thermoelectric Generator
[*] Naphtha Generator
[*] Eco-friendly Natural Gas generator
[*] Refined Carbon Generator
[*] Nuclear Fusion Plant
[/olist]

[h1] Thermoelectric Generator [/h1]
When only the wires are connected, they absorb the heat around them and produce electricity.

[b] Power Generation: [/b] 300w
[b] Heat Generation: [/b] -148.0kDTU
[b] Operating Temperature: [/b] 253.15 to 473.15 Kelvin


[h1] Naphtha Generator [/h1]
It consumes naphtha and O2, producing CO2 and power.
[b] Since oxygen is obtained from around the machine, the machine must be surrounded by oxygen. [/b]

[b] Power Generation: [/b] 2200w
[b] Heat Generation: [/b] 16.0kDTU
[b] Consumption: [/b]  Naphtha 1000g/s, Oxygen 100g/s
[b] Production: [/b] CO2 40g/s


[h1] Eco-friendly Natural Gas generator [/h1]
It consumes natural gas, O2 and filtration media to produce CO2, water and power.
[b] Since oxygen is obtained from around the machine, the machine must be surrounded by oxygen. [/b]
[b] CO2 is exported to the gas pipe. [/b]

[b] Power Generation: [/b] 800w
[b] Heat Generation: [/b] 10.0kDTU
[b] Consumption: [/b]  Methane 100g/s, Oxygen 100g/s, Filter 70g/s
[b] Production: [/b] Clean water 70g/s, CO2 30g/s


[h1] Refined Carbon Generator [/h1]
It consumes refined carbon to produce electricity very efficiently.

[b] Power Generation: [/b] 1200w
[b] Heat Generation: [/b] 4.0kDTU
[b] Consumption: [/b]  Carbone 1000g/s
[b] Production: [/b] CO2 20g/s (348.15 Kelvin)


[h1] Nuclear Fusion Plant [/h1]
It is powerful, safe, and produces a huge amount of electricity.

[b] Power Generation: [/b] 580500000w (580.5 Megawatt)
[b] Consumption: [/b]  Liquid Hydrogen 2g/s
[b] Production: [/b] Helium 0.99355g/s (9773.15 Kelvin)



[h1] Other [/h1]
[b] Github [/b]: [url=https://github.com/ekfvoddl3536/OniMods] here [/url]
```

---

## EcoFriendlyToilet_OV3
### Title
```plaintext
[Vanilla + DLC] Eco-friendly Toilet & Farm+ V3.0
```

### Description
```plaintext
[h1] Mod Translation [/h1]
[url=https://steamcommunity.com/sharedfiles/filedetails/?id=2785073113] SuperComic's MODS Translation - EN/JP/CH (AUTO) [/url]

[h1] New buildings [/h1]
[list]
[*] Eco-friendly toilet
[*] Eco-friendly Washbasin
[*] Wild Farm Tile
[*] Wild Flower Pot
[*] Wild Hanging Pot
[/list]

[h1] Details [/h1]
[h1] Eco-friendly toilet [/h1]
[olist]
[*] Can be used 24 times.
[*] Create Fertilizer 9.0 Kg per use.
[/olist]

[h1] Eco-friendly Washbasin (polluted water filtration sink) [/h1]
[b] This is a very special sink. [/b]

Minimum requirements for operation:
[olist]
[*] Water 5 Kg
[/olist]

Minimum requirements for filtration:
[olist]
[*] Polluted Water 1000 mg/s
[*] Filtration Medium 2500 mg/s
[/olist]

[b] Minimum requirements for turbo filtration speed: [/b]
[olist]
[*] [b] Power 60w [/b]
[*] Polluted Water 1250 g/s
[*] Filtration Medium 1000 g/s
[/olist]

[h1] Wild Farm Tile [/h1]
Grow crops wild.

Crops planted here follow the wild growth cycle and therefore have a very slow growth rate.

However, by planting plants here, you can meet the requirements of a "park" or "nature reserve".

[h1] Wild Flower Pot [/h1]
It works similarly to the Wild Farm Tile, but you can only plant decorative plants on it.

[h1] Wild Hanging Pot [/h1]
It works similarly to the Wild Farm Tile, but you can only plant decorative plants on it.


[h1] Other [/h1]
[b] Github [/b]: [url=https://github.com/ekfvoddl3536/OniMods] here [/url]

```

---

## FastWirelessAutomation_OV3
### Title
```plaintext
[Vanilla + DLC] Fast Wireless Automation+ V2.3
```

### Description
```plaintext
[h1] Mod Translation [/h1]
[url=https://steamcommunity.com/sharedfiles/filedetails/?id=2785073113] SuperComic's MODS Translation - EN/JP/CH (AUTO) [/url]

[h1]Description[/h1]
Normal channels change output according to the last input.
Channels 0 to 127 are used as OR channels.

[olist]
[*] [b]This does not check the status every 200ms, every second.[/b]
[*] By default, the last input signal is prioritized. (Exclude OR channels)
[*] The number of channels supported is 1024
[*] Use less computer resources.
[*] Very efficient system structure.
[/olist]

[h1] Compare [/h1]
[b]N[/b] = number of devices on the channel
[table]
[tr]
[th][/th]
[th]Wireless Automation[/th]
[th]THIS MOD[/th]
[/tr]
[tr]
[td]Channels[/td]
[td]100[/td]
[td]1024[/td]
[/tr]
[tr]
[td]Workload per second[/td]
[td]N * 5 ~ N * 500[/td]
[td]0 ([b]NEVER[/b])[/td]
[/tr]
[tr]
[td]Workload when changing signal[/td]
[td]N * 100[/td]
[td]N[/td]
[/tr]
[tr]
[td]Speed[/td]
[td]0.2s[/td]
[td]Almost [b]Immediately[/b][/td]
[/tr]
[/table]

[h1]Requires[/h1]
[list]
[*] [b]Power 5w[/b]
[*] Computing research
[/list]

[QnA]
Q1. What is "OR Channel"
A1. OR channel, the inactive signal is output only when the input signals of all connected signal transmitters are inactive.

[h1]Other[/h1]
source code: [url=https://github.com/ekfvoddl3536/OniMods]Github[/url]

```

---

## SupportPackage_OV3
### Title
```plaintext
[Vanilla + DLC] Support Package+ V2.2.1
```

### Description
```plaintext
[h1] Mod Translation [/h1]
[url=https://steamcommunity.com/sharedfiles/filedetails/?id=2785073113] SuperComic's MODS Translation - EN/JP/CH (AUTO) [/url]

[h1] Add New Buildings [/h1]
[olist]
[*] Tuning electrolyzer 2.0
[*] 2 in 1 Electric grill
[*] Polluted water sterilizer
[*] Magic tile
[*] Mineral deoxidizer
[*] Water synthesizer
[*] CO2 Decomposer
[/olist]

[h1] Add New Equipments [/h1]
[olist]
[*] Super Suit
[*] Legendary Super Suit
[*] Small Backpack
[*] Modern Backpack
[*] Legendary Super Backpack
[/olist]

[h1] Research [/h1]
[b] No research is needed [/b]
[b] Everything is unlocked [/b]

[h1] Mod Summary [/h1]
This mod is full of elements that will make the game easier.
Let's take a look at the newly added 6 buildings and 1 tile.

[h1] Tuning electrolyzer 2.0 [/h1]
It is an improved version of vanilla electrolyte. The gas pipe from which hydrogen will be discharged must be connected.
[b]Requires:[/b]
[list]
[*] Power: 90w
[*] Liquid Input
[*] Gas Output
[*] Water 1000g/s
[/list]

[h1] 2 in 1 Electric grill [/h1]
It is a combination version of an electric grill and a refrigerator. Of course, it has much better performance than vanilla refrigerators.
Up to 450KG of food can be stored in a freezer at -20grad.
However, this freezer only stores food made in this building.
[b]Requires:[/b]
[list]
[*] Power: 90w
[/list]

[h1] Polluted water sterilizer [/h1]
This building simply kills all bacteria in contaminated water.
[b]Requires:[/b]
[list]
[*] Power: 90w
[*] Liquid Input
[*] Liquid Output
[*] Polluted water 5000g/s
[*] Filtration medium 1000g/s
[/list]

[h1] Magic tile [/h1]
A tile that does not block liquid flow, but blocks gas flow.
[i]Be careful when using it as liquids dripping from top to bottom will cause the gas under this tile to pass through the tile.[/i]
The recommended layout is to divide the space left and right based on tiles.

[h1] Mineral deoxidizer [/h1]
A machine that extracts oxygen from microorganisms present in the soil.
[b]Requires:[/b]
[list]
[*] Power: 240w
[*] Dirt 4000g/s
[/list]

[h1] Water synthesizer [/h1]
A machine that synthesizes oxygen and hydrogen into water.
The machine collects oxygen by itself, so input only hydrogen through the gas pipe.
[b]Requires:[/b]
[list]
[*] Power: 1200w
[*] Gas Input ([b]only hydrogen[/b])
[*] Liquid Output
[*] Hydrogen 500g/s
[*] Oxygen 250g/s
[/list]

[h1] CO2 Decomposer [/h1]
Splits carbon dioxide into oxygen and coal.
[b]Requires:[/b]
[list]
[*] Power: 60w
[*] Liquid Input
[*] Liquid Output
[*] Water 10g/s
[*] CarbonDioxide 750g/s
[/list]

[h1] Super Suit [/h1]
[b] Effects: [/b]
[list]
[*] Decor +120
[/list]

[h1] Legendary Super Suit [/h1]
[b] Effects: [/b]
[list]
[*] Decor +9999
[/list]

[h1] Small Backpack [/h1]
[b] Effects: [/b]
[list]
[*] Carray amount +100kg (+220.5lb)
[*] Decor -10
[/list]

[h1] Modern Backpack [/h1]
[b] Effects: [/b]
[list]
[*] Carray amount +400kg (+882.0lb)
[*] Athletics -2
[*] Decor -10
[/list]

[h1] Legendary Super Backpack [/h1]
[b] Effects: [/b]
[list]
[*] Carray amount +4800kg (+10584.0lb)
[*] Athletics -4
[*] Decor -10
[/list]

[h1] Other [/h1]
The source code can be found on my github. [url=https://github.com/ekfvoddl3536/OniMods]link[/url]

```

