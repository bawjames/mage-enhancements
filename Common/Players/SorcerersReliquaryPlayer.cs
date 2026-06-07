using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MageEnhancements.Common.Players
{
	// Reproduces the Mana Cloak's Star Cloak component ("stars fall when you take damage").
	// tModLoader's vanilla Player has no simple starCloak flag, so instead of relying on a
	// version-specific hurt hook we detect damage by watching statLife across frames in the
	// rock-stable PostUpdate hook, then rain down Hallow Stars (ProjectileID.HallowStar, the
	// same projectile the vanilla Star Cloak / Star Veil use).
	public class SorcerersReliquaryPlayer : ModPlayer
	{
		// Set each frame by SorcerersReliquary.UpdateAccessory while the accessory is worn.
		public bool reliquaryStarCloak;

		private int previousLife;
		private int starCooldown;

		public override void ResetEffects()
		{
			reliquaryStarCloak = false;
		}

		public override void PostUpdate()
		{
			if (starCooldown > 0)
				starCooldown--;

			// statLife has already been reduced by Player.Hurt earlier this frame, so a drop
			// since last frame means the player just took damage.
			bool tookDamage = Player.statLife < previousLife;

			// Only the local player spawns the stars; Projectile.NewProjectile syncs them.
			if (reliquaryStarCloak && tookDamage && !Player.dead && starCooldown == 0
				&& Player.whoAmI == Main.myPlayer)
			{
				SpawnStars();
				starCooldown = 40; // ~0.66s, so damage-over-time effects don't spam stars
			}

			previousLife = Player.statLife;
		}

		private void SpawnStars()
		{
			var source = Player.GetSource_FromThis("SorcerersReliquary_StarCloak");
			const int starDamage = 40;
			const float starKnockback = 4f;

			// Three stars rain down from above and converge near the player, like the Star Cloak.
			for (int i = 0; i < 3; i++)
			{
				Vector2 spawnPos = Player.Center + new Vector2(Main.rand.Next(-360, 361), -700f - Main.rand.Next(0, 200));
				Vector2 targetPos = Player.Center + new Vector2(Main.rand.Next(-120, 121), 0f);

				Vector2 velocity = targetPos - spawnPos;
				velocity.Normalize();
				velocity *= 13f;

				int proj = Projectile.NewProjectile(source, spawnPos, velocity, ProjectileID.HallowStar, starDamage, starKnockback, Main.myPlayer);

				// Make sure the star damages enemies (and not the player) regardless of defaults.
				if (proj >= 0 && proj < Main.maxProjectiles)
				{
					Main.projectile[proj].friendly = true;
					Main.projectile[proj].hostile = false;
				}
			}
		}
	}
}
