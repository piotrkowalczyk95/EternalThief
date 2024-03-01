using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Threading.Tasks;
using System.Threading;

namespace EternalThief
{
    class SoundManager
    {
        //private static List<SoundEffect> soundEffects = new List<SoundEffect>();
        private static SoundEffect bgMusic;
        private static SoundEffect climbing;
        private static SoundEffect jumping;
        private static SoundEffect running;
        private static SoundEffect running2;
        private static SoundEffect yawn;
        private static SoundEffectInstance soundBgEffectInstance;


        public static void Load(ContentManager content)
        {
            bgMusic = content.Load<SoundEffect>(@"Sounds\bg");
            climbing = content.Load<SoundEffect>(@"Sounds\creak");
            jumping = content.Load<SoundEffect>(@"Sounds\hop");
            running = content.Load<SoundEffect>(@"Sounds\running");
            running2 = content.Load<SoundEffect>(@"Sounds\running2");
            yawn = content.Load<SoundEffect>(@"Sounds\yawn");

        }

        public static void PlayBgMusic()
        {
            soundBgEffectInstance = bgMusic.CreateInstance();
            soundBgEffectInstance.IsLooped = true;
            soundBgEffectInstance.Play();
        }

        public static void PlayHeroJump()
        {
            jumping.Play();
        }

        public static void PlayHeroClimb(bool isPlaying)
        {

            SoundEffectInstance soundEffectInstance = climbing.CreateInstance();
            if (isPlaying == true)
            {
                soundEffectInstance.Play();
            }
            else
            {
                soundEffectInstance.Dispose();
            }

        }

        public static void PlayHeroRun(bool isPlaying)
        {
            SoundEffectInstance soundEffectInstance = running2.CreateInstance();
            if (isPlaying == true)
            {
                soundEffectInstance.Play();
            }
            else
            {
                soundEffectInstance.Dispose();
            }
        }

        public static void PlayHeroIdle()
        {
            SoundEffectInstance soundEffectInstance = yawn.CreateInstance();

            soundEffectInstance.Play();


        }

        internal static void StopMusic()
        {
            soundBgEffectInstance.Dispose();
        }
    }
}