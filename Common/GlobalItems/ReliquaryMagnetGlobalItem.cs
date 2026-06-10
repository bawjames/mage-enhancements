using MageEnhancements.Content.Items.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MageEnhancements.Common.GlobalItems
{
	// Doubles the Celestial Magnet's star pickup range for Sorcerer's Reliquary wearers.
	// Vanilla adds Item.manaGrabRange once for mana stars when manaMagnet is set (see
	// Player.GetItemGrabRange / GetItemGrabRange2), so we add a second helping here — making
	// the reliquary's magnet reach twice the normal bonus, reflecting the two Celestial
	// Magnets consumed to craft it. The reliquary still sets manaMagnet itself, which provides
	// the first helping.
	public class ReliquaryMagnetGlobalItem : GlobalItem
	{
		public override void GrabRange(Item item, Player player, ref int grabRange)
		{
			if (IsManaStar(item.type) && HasReliquaryEquipped(player))
				grabRange += Item.manaGrabRange;
		}

		// The mana-restoring stars that the Celestial Magnet (manaMagnet) attracts.
		private static bool IsManaStar(int type)
			=> type == ItemID.Star || type == ItemID.SoulCake || type == ItemID.SugarPlum
				|| type == ItemID.ManaCloakStar;

		private static bool HasReliquaryEquipped(Player player)
		{
			int reliquaryType = ModContent.ItemType<SorcerersReliquary>();
			for (int i = 3; i <= 9; i++)
			{
				Item accessory = player.armor[i];
				if (!accessory.IsAir && accessory.type == reliquaryType)
					return true;
			}

			return false;
		}
	}
}
