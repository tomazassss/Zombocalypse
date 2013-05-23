using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using XRPGLibrary.TileEngine;
using XRPGLibrary.SpriteClasses;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace ZombocalypseRevised.Components.Actors
{
    public class Zombie : Enemy
    {


        #region Constructor Region

        public Zombie(Texture2D moveSprite, Texture2D deathSprite, Texture2D hitSprite, Texture2D attackSprite,
            Vector2 position, Game game, Camera cam) 
        : base(moveSprite, deathSprite, hitSprite, attackSprite, position, game, cam)
        {
            Id = 1;
            Damage = 50f;
            EnemyAttack = new AudioEngine(@"Content\Audio\ZombieSounds\ZombieAttack.xgs");
            EnemyAttackWaveBank = new WaveBank(EnemyAttack, @"Content\Audio\ZombieSounds\ZombieAttackWaveBank.xwb");
            EnemyAttackSoundBank = new SoundBank(EnemyAttack, @"Content\Audio\ZombieSounds\ZombieAttackSoundBank.xsb");
            DeathCue = "Zombie_Long_Death";
            AttackCue = "Zombie_Attack";
            HitCue = "Zombie_Hit";
        }

        #endregion

        #region Method Region

        public override void Update(GameTime gameTime, Player player)
        {
            base.Update(gameTime, player);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        #endregion
    }
}
