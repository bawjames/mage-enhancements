# Mage Enhancements

A small [tModLoader](https://tmodloader.net/) mod for Terraria (1.4.4) that adds a
single quality-of-life accessory for mages: the **Sorcerer's Reliquary**.

## The Sorcerer's Reliquary

One accessory that combines the effects of five vanilla mage accessories:

| Component | Effect granted |
| --- | --- |
| Arcane Flower | −8% mana usage, auto Mana Potions, and reduced enemy aggression |
| Magnet Flower | −8% mana usage, auto Mana Potions, and increased Star pickup range |
| Celestial Cuffs | +20 maximum mana, mana restored when hit, increased Star pickup range |
| Celestial Emblem | +15% magic damage and increased Star pickup range |
| Mana Cloak | −8% mana usage, auto Mana Potions, and falling stars when hit |

The combined (de-duplicated) effect set is:

- Reduces mana usage by ~15% and automatically uses Mana Potions when needed
- Increases maximum mana by 20 and restores mana when damaged
- 15% increased magic damage
- Greatly increases pickup range for Stars
- Causes stars to fall when you take damage
- Reduces enemy aggression

Effects are applied by setting the same `Player` fields vanilla sets in
`Player.ApplyEquipFunctional` (verified against the decompiled 1.4.4 source):
`manaFlower`, `manaCost`, `aggro -= 400`, `manaMagnet`, `magicCuffs`,
`statManaMax2 += 20`, magic `GetDamage`, and the `starCloakItem` fields (which drive
the vanilla Star Cloak proc — using the Mana Cloak variant), so they behave like the
originals.

Two effects are **doubled** to reflect that crafting the reliquary consumes two Mana
Flowers and two Celestial Magnets: the mana-cost reduction is the Mana Flower's −8%
applied twice multiplicatively (×0.92² ≈ −15.4%), and `ReliquaryMagnetGlobalItem` adds a
second `Item.manaGrabRange` to the mana-star pickup range.

**No stacking with its components.** If you wear the reliquary alongside any of its
five source accessories, the bonuses don't double up. The flag effects (auto-potion,
star pickup, mana-on-hit) can't stack in vanilla; the falling stars use a single
shared `starCloakItem` field so they fire once; and the additive effects are withheld
when the matching component is also worn — the −8% mana cost (vs. Arcane/Magnet/Mana
Cloak), the +20 max mana (vs. Celestial Cuffs), the +15% magic damage (vs. Celestial
Emblem), and the reduced aggro (vs. Arcane Flower).

### Crafting

Crafted at a **Tinkerer's Workbench** from **any 3 different** of these 5 accessories:

- Arcane Flower
- Magnet Flower
- Celestial Cuffs
- Celestial Emblem
- Mana Cloak

This is implemented as every distinct trio (C(5,3) = 10 recipes), so the recipe
always requires three *different* accessories — you can't use three of the same one.
The crafting menu only lists recipes you currently hold the ingredients for, so you
typically see just the one matching your three accessories.

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
Content/Items/Accessories/SorcerersReliquary.cs      The accessory
Content/Items/Accessories/SorcerersReliquary.png     Its sprite
Common/GlobalItems/ReliquaryMagnetGlobalItem.cs      Doubles the star pickup range
Localization/en-US_Mods.MageEnhancements.hjson       Name + tooltip text
_gen_sprites.py                                       Generates the sprite + icon
```
