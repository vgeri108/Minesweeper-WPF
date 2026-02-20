using System;
using System.Collections.Generic;
using System.Media;
using System.Text;

namespace Minesweeper_WPF
{
    internal class Sounds
    {
        public static SoundPlayer Start => new SoundPlayer($"Assets/Themes/{Configuration.CurrentTheme}/{Appearance.Images.ImageNames["Start"]}");
        public static SoundPlayer Click => new SoundPlayer($"Assets/Themes/{Configuration.CurrentTheme}/{Appearance.Images.ImageNames["Click"]}");
    }
}
