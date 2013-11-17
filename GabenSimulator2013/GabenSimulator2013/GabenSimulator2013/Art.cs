using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GabenSimulator2013
{
    static class Art
    {
        public static SpriteFont Font;

        public static void LoadContent(ContentManager Content)
        {
            Font = Content.Load<SpriteFont>("Font");
        }
    }
}
