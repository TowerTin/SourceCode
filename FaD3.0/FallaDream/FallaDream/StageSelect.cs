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
    class StageSelect
    {
        private SpriteBatch s;
        private GraphicsDevice g;
        private Texture2D imgBGI;
        private Texture2D[] imgStage; 
        private Vector2 posBGI;
        private Vector2[] posBtn;
        private int stageFlg = 7;
        public StageSelect(SpriteBatch _s, GraphicsDevice _g)
        {
            s = _s;
            g = _g;
            imgStage = new Texture2D [8];
            posBtn = new Vector2[8];
            Stream s0 = File.OpenRead("Content/images/bgi0.png");
            imgBGI = Texture2D.FromStream(g, s0);
            for (int i = 0; i < 8; i++)
            {

                s0 = File.OpenRead("Content/images/" + i + "syoub0.png");
                    imgStage[i] = Texture2D.FromStream(g, s0);
                
            }
          
        }
        public void drow()
        {
            posBGI.X = 0;
            posBGI.Y = 0;
            s.Draw(imgBGI, posBGI, Color.White);
            for (int i = 0; i < 8; i++)
            {  
                if (i <= stageFlg)
                {
                posBtn[i].X = 185;
                posBtn[i].Y = 75*i;
                s.Draw(imgStage[i], posBtn[i], Color.White);
                }
            }
          
        }
        public int MousePressChk(int x, int y)
        {
            for (int i = 0; i < stageFlg+1; i++)
            {
                if (x >= posBtn[i].X && x < posBtn[i].X + 430)
                {
                    if (y >= posBtn[i].Y && y < posBtn[i].Y + 75)
                    {
                        //if (stageFlg < (i + 1))
                        //{
                        //    stageFlg = (i + 1)%7;
                        //}
                        return i;
                    }

                }
                
            }
            return -1;
        }
        public void MouseHoverChk(int x, int y)
        {
            for (int i = 0; i < stageFlg +1; i++)
            {
                if (x >= posBtn[i].X && x < posBtn[i].X + 430)
                {
                    if (y >= posBtn[i].Y && y < posBtn[i].Y + 75)
                    {
                        Stream s1 = File.OpenRead("Content/images/" + i + "syoub1.png");
                        imgStage[i] = Texture2D.FromStream(g, s1);
                    }
                    else
                    {
                        Stream s1 = File.OpenRead("Content/images/" + i + "syoub0.png");
                        imgStage[i] = Texture2D.FromStream(g, s1);
                    }

                }
                else
                {
                    Stream s1 = File.OpenRead("Content/images/" + i + "syoub0.png");
                    imgStage[i] = Texture2D.FromStream(g, s1);
                }
            }
        }
        public int getstageFlg()
        {
            return stageFlg;
        }
        public void nextstage(int _stageFlg,int _stageNo)
        {
            if (_stageFlg == _stageNo)
            {
                if(stageFlg <=7)
                stageFlg++;
            }
 
        }
    }
}
