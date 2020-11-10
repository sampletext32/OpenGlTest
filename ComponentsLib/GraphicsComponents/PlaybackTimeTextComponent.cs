using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace ComponentsLib
{
    public class PlaybackTimeTextComponent : TextComponent
    {
        public MusicComponent MusicComponent { get; set; }

        public PlaybackTimeTextComponent(uint locX, uint locY, uint sizeX, uint sizeY) : base(locX, locY, sizeX, sizeY)
        {
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Update(float dt)
        {
            if (IsInited)
            {
                var milliseconds = (uint)(MusicComponent.TimeSeconds * 1000);
                var seconds = milliseconds / 1000;
                var minutes = seconds / 60 % 60;
                var hours = seconds / 3600;
                milliseconds %= 1000;
                seconds %= 60;

                Text = $"{hours:00}:{minutes:00}:{seconds:00}:{milliseconds:0000}";
                UpdateRequired = true;
            }
            base.Update(dt);
        }
    }
}
