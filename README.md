# Scaling Cantrips: Kingmaker
This is a backport of the <a href="https://github.com/RealityMachina/Scaling-Cantrips">Scaling-Cantrips</a> mod for Wrath of the Righteous to Kingmaker. It should be able to work with both existing saves and new games, however once installed to a save it cannot be removed.

## Features
All damage cantrips (and Virtue) will now scale with your highest caster level. By default this adds additional dice to damage rolls every 2 caster levels to a maximum of 6 dice, however you can configure this in the UMM options menu. Additional cantrips have also been added (Firebolt, Jolting Grasp, Divine Zap, and Unholy Zap) to help fill in the gaps and desire for more damage cantrips and emulate what exists in 5th edition. Finally new feats in the form of Cantrip Expert have been added allowing you to add an ability score modifier to your cantrip damage rolls.

## Call of the Wild Support
This mod does not require Call of the Wild, however if you have that mod installed it should work seemlessly with it adding the new cantrips to Call of the Wild's classes.

If there is another mod that adds classes you would like supported feel free to make a PR or open an Enhancement.

## How to Install
- Download and install <a href="https://github.com/newman55/unity-mod-manager">Unity Mod Manager</a> with at least version 0.25.4.0
- Download ScalingCantripsKM.zip
- Install the mod by draggin the ScalingCantripsKM-Releasezip file into UMM under the Mods tab
- Once installed UMM should alert you to any new versions of the mod.

## Uninstallation
- You can uninstall from the Unity Mod Manager, however because this mod adds new blueprints to the DB your saves that used the mod will break. 
- You can effectively turn the mod off by disabling each cantrip in the UMM Options menu or simply reducing "Max Dice" to 1, thus restoring original functionality.

## Thanks
- RealityMachina's <a href="https://github.com/RealityMachina/Scaling-Cantrips">ScalingCantrips</a> for the original mod where much of this code was taken or referenced.
- Holic75's <a href="https://github.com/Holic75/KingmakerRebalance">Call of the Wild</a> provided useful helper methods and insight onto how modding Kingmaker even works.
- Hsinyu's <a href="https://www.nexusmods.com/pathfinderkingmaker/mods/106">Data Viewer</a> was vital for looking up database ids for the stuff not shared with WoTR
- My cat for never leaving me alone.
