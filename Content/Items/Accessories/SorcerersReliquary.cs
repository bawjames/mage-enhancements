using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MageEnhancements.Content.Items.Accessories
{
	// The Sorcerer's Reliquary: one accessory with the combined (idempotent) effects of the
	// Arcane Flower, Magnet Flower, Celestial Cuffs, Celestial Emblem and Mana Cloak. Every
	// effect is reproduced by setting the same Player fields vanilla sets in
	// Player.ApplyEquipFunctional, so behaviour matches the originals exactly.
	public class SorcerersReliquary : ModItem
	{
		public override void SetStaticDefaults()
		{
			// One copy is enough to unlock it in Journey Mode's research.
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.accessory = true;
			Item.value = Item.sellPrice(gold: 5);
			Item.rare = ItemRarityID.LightPurple;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			// --- Idempotent flags ---
			// Vanilla sets each of these booleans once no matter how many accessories provide
			// them, so they never stack. Always applied.
			player.manaFlower = true;   // automatically uses Mana Potions when out of mana
			player.manaMagnet = true;   // increased Star pickup range (Celestial Magnet)
			player.magicCuffs = true;   // restores mana when hit (Magic Cuffs)

			// Falling stars when hit (Mana Cloak). Vanilla drives this from a single shared
			// starCloakItem field whose hurt proc fires once, so it cannot stack with a worn
			// Mana Cloak (or any other Star Cloak accessory). We select the Mana Cloak variant.
			player.starCloakItem = Item;
			player.starCloakItem_manaCloakOverrideItem = Item;

			// --- Additive effects ---
			// These add a number, so they WOULD double up if the component that also provides
			// them is equipped. Each is withheld in that case so the total matches one accessory.

			// -8% mana cost (provided by the Arcane Flower, Magnet Flower and Mana Cloak).
			if (!IsWearingComponent(player, ItemID.ArcaneFlower)
				&& !IsWearingComponent(player, ItemID.MagnetFlower)
				&& !IsWearingComponent(player, ItemID.ManaCloak))
				player.manaCost -= 0.08f;

			// Reduced enemy aggression (Arcane Flower).
			if (!IsWearingComponent(player, ItemID.ArcaneFlower))
				player.aggro -= 400;

			// +20 maximum mana (Celestial Cuffs).
			if (!IsWearingComponent(player, ItemID.CelestialCuffs))
				player.statManaMax2 += 20;

			// 15% increased magic damage (Celestial Emblem).
			if (!IsWearingComponent(player, ItemID.CelestialEmblem))
				player.GetDamage(DamageClass.Magic) += 0.15f;

			// +8% magic critical strike chance — a small bonus none of the five components give.
			player.GetCritChance(DamageClass.Magic) += 8f;
		}

		// True if the player has the given item equipped in a functional (non-vanity) accessory
		// slot. Vanilla functional accessory slots are armor[3..9] (5 base + Demon Heart + Master).
		private static bool IsWearingComponent(Player player, int itemType)
		{
			for (int i = 3; i <= 9; i++)
			{
				Item accessory = player.armor[i];
				if (!accessory.IsAir && accessory.type == itemType)
					return true;
			}

			return false;
		}

		public override void AddRecipes()
		{
			// "Any 3 of the 5, all different." A recipe group with a required count of 3 would
			// let the player craft with 3 identical accessories, so instead we register every
			// distinct trio (C(5,3) = 10), each requiring one of three different accessories.
			// The crafting menu only lists recipes you currently have the ingredients for, so
			// in practice you see just the one matching the three accessories you're holding.
			int[] components =
			{
				ItemID.ArcaneFlower,
				ItemID.MagnetFlower,
				ItemID.CelestialCuffs,
				ItemID.CelestialEmblem,
				ItemID.ManaCloak,
			};

			for (int a = 0; a < components.Length; a++)
				for (int b = a + 1; b < components.Length; b++)
					for (int c = b + 1; c < components.Length; c++)
					{
						CreateRecipe()
							.AddIngredient(components[a])
							.AddIngredient(components[b])
							.AddIngredient(components[c])
							.AddTile(TileID.TinkerersWorkbench)
							.Register();
					}
		}
	}
}
