# Super Comic's Oxygen Not Included Mods

## Introduce

### Mods
* [Advanced Generator OV3.0](./src/AdvancedGenerators_OV3/)
* [Eco Friendly Toilet OV3.0](./src/EcoFriendlyToilet_OV3/)
* [Fast Wireless Automation OV3.0](./src/FastWirelessAutomation_OV3/)
* [Support Package OV3.0](./src/SupportPackage_OV3/)


# Building

## What you need
1. **.NET SDK** — recommended version **10.0**  
   Install it from the [official download page](https://aka.ms/dotnet/download).
2. **(Optional)** PowerShell 7.4 or newer  
   Helpful if you want to run the setup script with `pwsh`. [Install instruction](https://aka.ms/install-powershell)

---

## Step by step

1. **Clone the repository**

   Open a terminal and run:
   ```
   git clone https://github.com/ekfvoddl3536/OniMods
   ```

2. **Go into the project folder**

   ```
   cd ./OniMods
   ```

3. **Run the setup script**

   If you have PowerShell Core (`pwsh`):
   ```
   pwsh ./build/setup.ps1 -init
   ```

   Or if you're using Windows PowerShell:
   ```
   ./build/setup.ps1 -init
   ```

4. **(Optional) Tell the script where your game is installed**

   If needed, you can specify a game directory:
   ```
   pwsh ./build/setup.ps1 -init -gamedir "D:\My Game Folder\common\OxygenNotIncluded"
   ```

   If the script cannot finish everything automatically, follow the [manual steps](./docs/manual-environment-setup.md).

5. **Build the project**

   ```
   dotnet build -c Release
   ```

6. **Check the final output**

   ```
   cd ./.ldenv/.modsdir/Dev
   ```
   This folder contains the built files.

---

## Quick notes & troubleshooting

- Make sure you run all commands from the root of the repository.
- If `pwsh` is not found:
  - On Windows: try running the script in Windows PowerShell instead.
  - On macOS/Linux: install PowerShell Core or [perform manual setup](./docs/manual-environment-setup.md).
- If `dotnet` is not recognized, reinstall the .NET SDK and restart the terminal.

