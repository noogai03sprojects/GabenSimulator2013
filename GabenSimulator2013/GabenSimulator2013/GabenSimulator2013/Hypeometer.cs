using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GabenSimulator2013
{
    class Hypeometer
    {
        float Hype = 500;
        float MaxHype = 100;
        float PercentFull = 0;

        float IndicatorCurrent = 0;

        float LerpSpeed = 0.1f;

        Rectangle BaseArea;
        Rectangle BarArea;

        int Height;

        TaskManager Manager;

        Color BaseColor = Color.DimGray;
        Color BarColor = Color.ForestGreen;

        

        public Hypeometer(TaskManager manager, int x, int y, int width, int height, int maxHype)            
        {
            BaseArea = new Rectangle(x, y, width, height);
            Height = height;
            MaxHype = maxHype;
            Manager = manager;
        }

        public void Update()
        {
            //Console.WriteLine(Hype);
            IndicatorCurrent = MathHelper.Lerp(IndicatorCurrent, (Hype / MaxHype) * Height, LerpSpeed);
            PercentFull = (Hype / MaxHype) * 100;

            BarArea = new Rectangle(BaseArea.X, (int)(BaseArea.Y + (Height - (int)IndicatorCurrent)), BaseArea.Width, (int)IndicatorCurrent);

            //Console.WriteLine(PercentFull);
            //BarArea.SetBottom((int)Game1.Instance.ScreenSize.Y);
        }

        public void Tick()
        {
            Hype = 0;
            foreach (Task task in Manager.Tasks)
            {
                Hype += 50;
                Hype += task.PercentComplete;
            }
            //Hype = 250;
            Console.WriteLine(Hype);
            //Console.WriteLine(Hype);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Art.Pixel, BaseArea, BaseColor);
            spriteBatch.Draw(Art.Pixel, BarArea, BarColor);
        }
    }
}
