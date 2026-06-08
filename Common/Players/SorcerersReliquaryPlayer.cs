using MageEnhancements.Content.Items.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MageEnhancements.Common.Players
{
	// Gives the Sorcerer's Reliquary a 25% chance to shrug off Mana Sickness when a mana
	// potion is used. Vanilla applies Mana Sickness (BuffID.ManaSickness) in
	// Player.ApplyLifeAndOrMana whenever a mana-restoring item is consumed (manual use or the
	// Mana Flower's auto-use), and there's no hook at that point. So we watch the buff's
	// remaining time: a jump upward means a potion just (re)applied it, and we roll then.
	//
	// This runs in PreUpdateBuffs, before the buff is processed into the magic-damage penalty,
	// so a successful roll avoids the penalty entirely. We detect the equipped reliquary by
	// scanning the accessory slots rather than a flag from UpdateAccessory, because that pass
	// runs later in the update than PreUpdateBuffs (and ResetEffects would clear a flag first).
	public class SorcerersReliquaryPlayer : ModPlayer
	{
		private const float AvoidChance = 0.25f;

		private int previousManaSicknessTime;

		public override void PreUpdateBuffs()
		{
			int index = Player.FindBuffIndex(BuffID.ManaSickness);
			int currentTime = index >= 0 ? Player.buffTime[index] : 0;

			// A rise in the remaining time means a mana potion was just used.
			if (Player.whoAmI == Main.myPlayer
				&& currentTime > previousManaSicknessTime
				&& IsReliquaryEquipped()
				&& Main.rand.NextFloat() < AvoidChance)
			{
				Player.ClearBuff(BuffID.ManaSickness);
				currentTime = 0;
			}

			previousManaSicknessTime = currentTime;
		}

		private bool IsReliquaryEquipped()
		{
			int type = ModContent.ItemType<SorcerersReliquary>();
			for (int i = 3; i <= 9; i++)
			{
				Item accessory = Player.armor[i];
				if (!accessory.IsAir && accessory.type == type)
					return true;
			}

			return false;
		}
	}
}
