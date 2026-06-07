# Mage Enhancements

A small [tModLoader](https://tmodloader.net/) mod for Terraria (1.4.4) that adds a
single quality-of-life accessory for mages: the **Sorcerer's Reliquary**.

## The Sorcerer's Reliquary

One accessory that combines the effects of five vanilla mage accessories:

| Component | Effect granted |
| --- | --- |
| Arcane Flower | −8% mana usage, auto Mana Potions, and increased mana regeneration |
| Magnet Flower | −8% mana usage, auto Mana Potions, and increased Star pickup range |
| Celestial Cuffs | +20 maximum mana, mana restored when hit, increased Star pickup range |
| Celestial Emblem | +15% magic damage and increased Star pickup range |
| Mana Cloak | −8% mana usage, auto Mana Potions, and falling stars when hit |

The combined effect set is:

- Reduces mana usage by 8% and automatically uses Mana Potions when needed
- Increased mana regeneration
- Increases maximum mana by 20 and restores mana when damaged
- Increases pickup range for Stars
- Causes stars to fall when you take damage
- 15% increased magic damage

Effects are applied by setting the same `Player` fields vanilla uses
(`manaFlower`, `manaRegenBuff`, `manaMagnet`, `magicCuffs`, `statManaMax2`,
`starCloak`, and magic `GetDamage`), so it behaves identically to the originals.

### Crafting

Crafted at a **Tinkerer's Workbench** from **any 3 of** these 5 accessories:

- Arcane Flower
- Magnet Flower
- Celestial Cuffs
- Celestial Emblem
- Mana Cloak

(Implemented as a recipe group, so any mix of three works — e.g. Celestial Cuffs +
Celestial Emblem + Mana Cloak.)

## Building

This repository *is* the mod source. tModLoader requires the source folder name to
match the mod's internal name, which is `MageEnhancements`.

1. Install tModLoader (1.4.4) via Steam.
2. Clone/copy this repository into your `ModSources` folder so the path is:
   `Documents/My Games/Terraria/tModLoader/ModSources/MageEnhancements`
   (rename the folder to `MageEnhancements` if your clone is named `mage-enhancements`).
3. Launch tModLoader → **Workshop → Develop Mods → Build + Reload**.

## Project layout

```
MageEnhancements.cs                                  Main mod class
MageEnhancements.csproj                              Project file
build.txt / description.txt                          Mod metadata
icon.png                                             Mod browser icon
Common/Systems/RecipeGroupSystem.cs                  "Any 3 of 5" recipe group
Content/Items/Accessories/SorcerersReliquary.cs      The accessory
Content/Items/Accessories/SorcerersReliquary.png     Its sprite
Localization/en-US_Mods.MageEnhancements.hjson       Name + tooltip text
```
