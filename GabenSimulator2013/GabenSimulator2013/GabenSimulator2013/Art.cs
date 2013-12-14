using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace GabenSimulator2013
{
    static class Art
    {
        public static SpriteFont Font;

        public static Texture2D Pixel;

        public static Texture2D AddTask;
        public static Texture2D TaskFrame;

        public static void LoadContent(ContentManager Content)
        {
            Font = Content.Load<SpriteFont>("Font");

            Pixel = new Texture2D(GameRoot.Instance.GraphicsDevice, 1, 1);
            Color[] data = new Color[1];
            data[0] = Color.White;
            Pixel.SetData<Color>(data);

            AddTask = Content.Load<Texture2D>("AddNewTask");
            TaskFrame = Content.Load<Texture2D>("frame");
            Console.WriteLine(Art.Font.LineSpacing);
        }
    }
}
