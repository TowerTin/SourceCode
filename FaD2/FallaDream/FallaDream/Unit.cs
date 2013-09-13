using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using System;
namespace FallaDream
{
    class Unit:Data
    {
        private SpriteBatch s;
        private GraphicsDevice g;
        private int unitNo, x, y,i=0;
        private Vector2 pos,posle;
        private Texture2D imgUnit,imgle;
        private FileStream stream;
        private int ATC, CST, ITR, RNG, Lv;
        DateTime dtNow = DateTime.Now,interval ; 
        private bool flgUpdate= false;
        bool leflg = false;
        DateTime intrvar = DateTime.Now;

        public Unit(SpriteBatch _s, GraphicsDevice _g, int _unitNo,int _x,int _y)
        {
            s = _s;
            g = _g;
            unitNo = _unitNo;
            x = _x;
            y = _y;
            Lv = 0;
            CST = unitst[unitNo, Lv, 0];
            ATC = unitst[unitNo, Lv, 1];
            RNG =range[ unitst[unitNo, Lv, 2]];
            ITR = unitst[unitNo, Lv, 3];
            setInterval();
           
            
            switch (unitNo)
            {
                case 0:
                    stream = File.OpenRead("images/unitLucy0.png");
                    imgUnit = Texture2D.FromStream(g, stream);
                    break;
                case 1:
                    stream = File.OpenRead("images/unitPaley0.png");
                    imgUnit = Texture2D.FromStream(g, stream);
                    break;
                case 2:
                    stream = File.OpenRead("images/unitTiger0.png");
                    imgUnit = Texture2D.FromStream(g, stream);
                    break;
                case 3:
                    stream = File.OpenRead("images/unitIzumi0.png");
                    imgUnit = Texture2D.FromStream(g, stream);
                    break;
                case 4:
                    stream = File.OpenRead("images/unitOz0.png");
                    imgUnit = Texture2D.FromStream(g, stream);
                    break;
                case 5:
                    stream = File.OpenRead("images/unitHikoyoshi0.png");
                    imgUnit = Texture2D.FromStream(g, stream);
                    break;
                case 6:
                    stream = File.OpenRead("images/unitCarario0.png");
                    imgUnit = Texture2D.FromStream(g, stream);
                    break;
               
            }
            
 
        }
        public void getLvUP()
        {
            if (Lv < 4)
            {
                Lv++;
                CST = unitst[unitNo, Lv, 0];
                ATC = unitst[unitNo, Lv, 1];
                RNG = range[unitst[unitNo, Lv, 2]];
                ITR = unitst[unitNo, Lv, 3];
            }
            
        }
        public int getLVUPCost()
        {
            if (Lv < 4)
            {
                return unitst[unitNo, Lv + 1, 0];
            }
            return 0;
        }
       
        
        public int getCost(int _unitNo)
        {
            return unitst[_unitNo,0,0];
        }
        public void paint()
        {
            pos.X = (x / 40) * 40;
            pos.Y = (y / 40) * 40;
            s.Draw(imgUnit, pos, Color.White);
            if (leflg)
            {
                s.Draw(imgle, posle, Color.White);
                
                if (dtNow >= intrvar)
                {
                    intrvar = dtNow.AddMilliseconds(1000);
                    leflg = false;
                    
                }
            }

        }
        public float getX()
        {
            return pos.X;
        }
        public float getY()
        {
            return pos.Y;
        }
        public int getRNG()
        {
            return RNG;
        }
        public int getATC()
        {
            flgUpdate = true;
            return ATC;
        }
        public int getUnitNo()
        {
            return unitNo;
        }
        public bool getInterval()
        {
            dtNow = DateTime.Now;
            if (dtNow >= interval)
            {
                return true;
            }
            return false;
        }
        public void setInterval()
        {
            dtNow = DateTime.Now;
            interval = dtNow.AddMilliseconds(ITR);
        }
        public void UPdate()
        {
            if (flgUpdate)
            {
                switch (unitNo)
                {
                    case 0:
                        stream = File.OpenRead("images/unitLucy"+1+".png");
                        imgUnit = Texture2D.FromStream(g, stream);
                        break;
                    case 1:
                        stream = File.OpenRead("images/unitPaley" + 1 + ".png");
                        imgUnit = Texture2D.FromStream(g, stream);
                        break;
                    case 2:
                        stream = File.OpenRead("images/unitTiger" + 1 + ".png");
                        imgUnit = Texture2D.FromStream(g, stream);
                        break;
                    case 3:
                        stream = File.OpenRead("images/unitIzumi" + 1 + ".png");
                        imgUnit = Texture2D.FromStream(g, stream);
                        break;
                    case 4:
                        stream = File.OpenRead("images/unitOz" + 1 + ".png");
                        imgUnit = Texture2D.FromStream(g, stream);
                        break;
                    case 5:
                        stream = File.OpenRead("images/unitHikoyoshi" + 1 + ".png");
                        imgUnit = Texture2D.FromStream(g, stream);
                        break;
                    case 6:
                        stream = File.OpenRead("images/unitCarario" + 1 + ".png");
                        imgUnit = Texture2D.FromStream(g, stream);
                        break;
                }
                i++;
                if (i >= 8)
                {
                    flgUpdate = false;
                    i = 0;

                }

            }
            else 
            {
                switch (unitNo)
                {
                    case 0:
                        stream = File.OpenRead("images/unitLucy0.png");
                        imgUnit = Texture2D.FromStream(g, stream);
                        break;
                    case 1:
                        stream = File.OpenRead("images/unitPaley0.png");
                        imgUnit = Texture2D.FromStream(g, stream);
                        break;
                    case 2:
                        stream = File.OpenRead("images/unitTiger0.png");
                        imgUnit = Texture2D.FromStream(g, stream);
                        break;
                    case 3:
                        stream = File.OpenRead("images/unitIzumi0.png");
                        imgUnit = Texture2D.FromStream(g, stream);
                        break;
                    case 4:
                        stream = File.OpenRead("images/unitOz0.png");
                        imgUnit = Texture2D.FromStream(g, stream);
                        break;
                    case 5:
                        stream = File.OpenRead("images/unitHikoyoshi0.png");
                        imgUnit = Texture2D.FromStream(g, stream);
                        break;
                    case 6:
                        stream = File.OpenRead("images/unitCarario0.png");
                        imgUnit = Texture2D.FromStream(g, stream);
                        break;

                }
            }
 
        }
        public void le(int no,int x,int y)
        {
            posle.X = x;
            posle.Y = y;
            if (no == 1)
            {

                stream = File.OpenRead("images/laser0.png");
                imgle = Texture2D.FromStream(g, stream);
            }
            else
            {
                stream = File.OpenRead("images/laser1.png");
                imgle = Texture2D.FromStream(g, stream);
            }
            leflg = true;
            intrvar = dtNow.AddMilliseconds(500);
        }
       
            
    }
}
