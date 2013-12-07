using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GabenSimulator2013
{   
    public delegate void HypeDelegate();

    class Hypeometer
    {
        public struct HypeEvent
        {
            public string Name, Description;
            public float TriggerValue;
            public HypeDelegate Function;

            public HypeEvent(string name, string description, float triggerValue, HypeDelegate func)
            {
                Name = name;
                Description = description;
                TriggerValue = triggerValue;
                Function = func;
            }
        }
        float Hype = 0;
        float MaxHype = 100;
        float PercentFull = 0;

        float IndicatorCurrent = 0;

        float LerpSpeed = 0.1f;

        Rectangle BaseArea;
        Rectangle BarArea;

        int Height;

        TaskManager Manager;

        Color BaseColor = Color.DimGray;
        Color BarColor = new Color(92, 185, 128);

        public static Hypeometer Instance;

        public List<HypeEvent> Events = new List<HypeEvent>();
        public List<Rectangle> EventLines = new List<Rectangle>();

        public Hypeometer(TaskManager manager, int x, int y, int width, int height, int maxHype)            
        {
            BaseArea = new Rectangle(x, y, width, height);
            Height = height;
            MaxHype = maxHype;
            Manager = manager;
            Instance = this;
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
            //Hype = 0;
            foreach (Task task in Manager.Tasks)
            {
                //Hype += 50;
                //Hype += task.PercentComplete;
            }
            foreach (HypeEvent _event in Events)
            {
                if (Hype > _event.TriggerValue)
                {
                    if (_event.Function != null)
                        _event.Function();
                }
            }
            //Hype = 250;
            //Console.WriteLine(Hype);
            //Console.WriteLine(Hype);
        }

        public void AddHype(int hype)
        {
            Hype += hype;
        }

        public void AddEvent(HypeEvent _event)
        {
            Events.Add(_event);
            Rectangle rect = new Rectangle(BaseArea.X, (int)(BaseArea.Y + (Height - (_event.TriggerValue / MaxHype) * Height)), BaseArea.Width, 2);
            EventLines.Add(rect);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Art.Pixel, BaseArea, BaseColor);
            spriteBatch.Draw(Art.Pixel, BarArea, BarColor);
            foreach (Rectangle rect in EventLines)
            {
                spriteBatch.Draw(Art.Pixel, rect, Color.Red);
            }
        }
    }
}
