# Scaling Cantrips: Kingmaker
This is a backport of the <a href="https://github.com/RealityMachina/Scaling-Cantrips">Scaling-Cantrips</a> mod for Wrath of the Righteous to Kingmaker.

## Features
- All damage cantrips (Ray of Frost, Acid Splash, etc) increase the number of damage dice used for every 2 caster levels (configurable).
- Adds additional cantrips from the WoTR mod (Firebolt, Jolting Grasp, Divine Zap, Unholy Zap)
- Cantrip Expert feats for adding spellcasting ability modifiers to damage rolls.

## Missing Features
- Any fixes related to item interactions
- A way to disable the mod for a save file and safely uninstall

## WIP: Call of the Wild Support
While this mod does not require Call Of The Wild, a desired feature of mine is to make sure this mod is supported since its one of the most popular Kingmaker mods that adds a slew of new content. Currently the support for this mod mainly extends to the base classes added by it.

If you notice a part of CallOfTheWild is not supported by this mod please feel free to check the Issues section and open an enhancement!

## How to Install
- Download and install <a href="https://github.com/newman55/unity-mod-manager">Unity Mod Manager</a> with at least version 0.25.4.0
- Setup UMM to find your Pathfinder Kingmaker install
- Download ScalingCantripsKM.zip
- Install the mod by draggin the zip file into UMM under the Mods tab 

## Uninstallation
Unfortunately given the nature of modifying database blueprints in the game there is no easy way to uninstall the mod once its been saved to your file, you have been warned. If you are ok with this then you can simply right click the mod in UMM and click "Uninstall".

## Thanks
- RealityMachina's <a href="https://github.com/RealityMachina/Scaling-Cantrips">ScalingCantrips</a> for the original mod where much of this code was taken or referenced.
- Holic75's <a href="https://github.com/Holic75/KingmakerRebalance">Call of the Wild</a> provided useful helper methods and insight onto how modding Kingmaker even works.
- Hsinyu's <a href="https://www.nexusmods.com/pathfinderkingmaker/mods/106">Data Viewer</a> was vital for looking up database ids for the stuff not shared with WoTR
- My cat for never leaving me alone.