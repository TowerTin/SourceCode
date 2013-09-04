using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

namespace FallaDream
{
    class StartupScreen
    {
        private SpriteBatch s;
        private GraphicsDevice g;
        private Texture2D imgBGI, imgBtn0, imgBtn1;
        private Vector2 posBGI, posBtn0, posBtn1;
        public  StartupScreen(SpriteBatch _s,GraphicsDevice _g)
        {
            s = _s;
            g = _g;
            Stream s0 = File.OpenRead("images/FallaDream.png");
            imgBGI = Texture2D.FromStream(g, s0);
   
        }

       public void drow()
        {
            posBGI.X = 0;
            posBGI.Y = 0;
            s.Draw(imgBGI, posBGI, Color.White);
            posBtn0.X = 275;
           posBtn0.Y = 350;
            s.Draw(imgBtn0, posBtn0, Color.White);
            posBtn1.X = 275;
            posBtn1.Y = 400;
            s.Draw(imgBtn1, posBtn1, Color.White);
        }
       public int  MousePressChk(int x, int y)
       {
           if (x >= posBtn0.X && x < posBtn0.X + 250)
           {
               if (y >= posBtn0.Y && y < posBtn0.Y + 45)
               {
                   return 1;
               }
           }
           if (x >= posBtn1.X && x < posBtn1.X + 250)
           {
               if (y >= posBtn1.Y && y < posBtn1.Y + 45)
               {
                   return 3;
               }
           }
           return 0;
       }
       public  void MouseHoverChk(int x,int y)
        { 
               if (x >= posBtn0.X && x < posBtn0.X + 250)
               {
                if (y >= posBtn0.Y && y < posBtn0.Y + 45)
                {
                    Stream s1 = File.OpenRead("images/storymode1.png");
                    imgBtn0 = Texture2D.FromStream(g, s1);
                }
                else
                {
                    Stream s1 = File.OpenRead("images/storymode0.png");
                    imgBtn0 = Texture2D.FromStream(g, s1);
                }

            }
            else 
            {
                Stream s1 = File.OpenRead("images/storymode0.png");
                imgBtn0 = Texture2D.FromStream(g, s1);
            }

            if (x >= posBtn1.X && x < posBtn1.X + 250)
            {
                if (y >= posBtn1.Y && y < posBtn1.Y + 45)
                {
                    Stream s1 = File.OpenRead("images/challengemode1.png");
                    imgBtn1 = Texture2D.FromStream(g, s1);
                }
                else
                {
                    Stream s1 = File.OpenRead("images/challengemode0.png");
                    imgBtn1 = Texture2D.FromStream(g, s1);
                }

            }
            else
            {
                Stream s1 = File.OpenRead("images/challengemode0.png");
                imgBtn1 = Texture2D.FromStream(g, s1);
            }
           
        
        }
    }
}
