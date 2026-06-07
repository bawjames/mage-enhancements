using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MageEnhancements.Common.Systems
{
	// Registers the recipe group that lets the Sorcerer's Reliquary be crafted from
	// "any 3 of" the five mana accessories it combines. Using a recipe group (rather
	// than five separate recipes) means the player can mix and match the components
	// freely, exactly like vanilla "Any Wood"/"Any Iron Bar" recipes.
	public class RecipeGroupSystem : ModSystem
	{
		// Fully-qualified name used both when registering the group and when a recipe
		// references it. The "ModName:" prefix keeps it from colliding with other mods.
		public const string ManaAccessoryGroup = "MageEnhancements:ManaAccessories";

		public override void AddRecipeGroups()
		{
			// "LegacyMisc.37" is the vanilla word "Any", so this displays as
			// "Any Mana Accessory" in the crafting UI and Recipe Browser.
			var group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} Mana Accessory",
				ItemID.ArcaneFlower,
				ItemID.MagnetFlower,
				ItemID.CelestialCuffs,
				ItemID.CelestialEmblem,
				ItemID.ManaCloak);

			RecipeGroup.RegisterGroup(ManaAccessoryGroup, group);
		}
	}
}
