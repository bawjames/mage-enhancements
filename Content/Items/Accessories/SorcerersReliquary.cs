using MageEnhancements.Common.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MageEnhancements.Content.Items.Accessories
{
	// The Sorcerer's Reliquary: a single accessory that rolls together the effects of
	// the Arcane Flower, Magnet Flower, Celestial Cuffs, Celestial Emblem and Mana Cloak.
	// Each effect below reproduces a vanilla accessory by setting the same Player fields
	// that vanilla sets in Player.VanillaUpdateAccessory, so behaviour (including how it
	// interacts with other accessories) matches the originals exactly.
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
			// Light purple, matching the Celestial Emblem tier of its components.
			Item.rare = ItemRarityID.LightPurple;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			// Arcane Flower / Magnet Flower / Mana Cloak share the Mana Flower component:
			// reduces mana usage by 8% and auto-consumes mana potions when out of mana.
			player.manaFlower = true;

			// Arcane Flower's Mana Regeneration Band component: faster mana regeneration.
			player.manaRegenBuff = true;

			// Magnet Flower / Celestial Cuffs / Celestial Emblem share the Celestial Magnet
			// component: greatly increased pickup range for fallen Stars.
			player.manaMagnet = true;

			// Celestial Cuffs' Magic Cuffs component: restores mana when the wearer is hit,
			// and increases maximum mana by 20.
			player.magicCuffs = true;
			player.statManaMax2 += 20;

			// Mana Cloak's Star Cloak component: drops damaging stars when the wearer is hit.
			player.starCloak = true;

			// Celestial Emblem's emblem component: 15% increased magic damage.
			player.GetDamage(DamageClass.Magic) += 0.15f;
		}

		public override void AddRecipes()
		{
			// Any 3 of the five component accessories, combined at a Tinkerer's Workbench.
			CreateRecipe()
				.AddRecipeGroup(RecipeGroupSystem.ManaAccessoryGroup, 3)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}
