using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Media;

namespace FallaDream
{
  
    //マップを生成しマップ画像を表示
    class CreateMap:Data
    {
        private GraphicsDevice g;
        private  SpriteBatch s;
        private Texture2D imgMap, imgStage;
        private Texture2D[] imgIc,imgNo;
        private int[,] maptable;
        private Vector2[] pos;
        private int stageFlg;
        private int Money = 0;
        private int[,] mapUnit;

        public CreateMap( SpriteBatch _s,GraphicsDevice _g)
        {
  
            s = _s;
            g = _g;
            FileStream stream = File.OpenRead("images/map.png");
            imgMap = Texture2D.FromStream(_g, stream);
            pos = new Vector2[7];

            stream = File.OpenRead("images/stage0.png");
            imgStage = Texture2D.FromStream(_g, stream);

            imgIc = new Texture2D[7];
            for (int i = 0; i < 7; i++)
            {
                stream = File.OpenRead("images/UnitIc"+i+".png");
                imgIc[i] = Texture2D.FromStream(_g, stream);

            }

            imgNo = new Texture2D[10];
            for(int i =0;i <10;i++)
            {
                stream = File.OpenRead("images/no"+i+".png");
                imgNo[i] = Texture2D.FromStream(_g, stream);
            }

            pos[0].X = 600;
            pos[0].Y = 200;

            pos[1].X = 700;
            pos[1].Y = 200;

            pos[2].X = 600;
            pos[2].Y = 300;

            pos[3].X = 700;
            pos[3].Y = 300;

            pos[4].X = 600;
            pos[4].Y = 400;

            pos[5].X = 700;
            pos[5].Y = 400;

            pos[6].X = 600;
            pos[6].Y = 500;

            mapUnit = new int[,] { 
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}
        };
            
        }
        public void paint(int no,int stageFlg,int _Money )
        {
            paintmap(no);
            if (no > stageFlg-1)
            {
                paintic(no);
            }
            else
            {
                paintic(stageFlg-1);
            }
           
            paintMoney(Money);
        }
        public void UPMoney(int _Money)
        {
            Money = _Money;
 
        }
        public void paintmap(int no)
        {
           
            switch (no)
            {
                case 0:
                    maptable = mapTable0;
                     
                    break;
                case 1:
                    maptable = mapTable1;
                    break;
                case 2:
                    maptable = mapTable2;
                    break;
                case 3:
                    maptable  = mapTable3;
                    break;
                case 4:
                    maptable = mapTable4;
                    break;
                case 5:
                    maptable = mapTable5;
                    break;
                case 6:
                    maptable = mapTable6;
                    break;
                case 7:
                    maptable = mapTable7;
                    break;

            }
            Vector2 pos;
            Rectangle rect;
            int w, mlow, mcol;
            pos.X = 0;
            pos.Y = 0;
            s.Draw(imgStage, pos, Color.White);
            if (no != 0)
            {
                for (int i = 0; i < maptable.GetLength(0); i++)
                {
                    for (int j = 0; j < maptable.GetLength(1); j++)
                    {
                        pos.X = j * 40;
                        w = maptable[i, j];
                        mlow = w / 16;
                        mcol = w % 16;
                        rect.X = mcol * 40;
                        rect.Y = mlow * 40;
                        rect.Width = 40;
                        rect.Height = 40;
                        s.Draw(imgMap, pos, rect, Color.White);
                    }
                    pos.Y += 40;
                }
            }
        }
        public void paintic(int _stageFlg)
        {
            stageFlg = _stageFlg;
            if (stageFlg >= 6)
            {
                stageFlg = 6;
            }
            for (int i = 0; i <= stageFlg; i++)
            { 
               
                s.Draw(imgIc[i], pos[i], Color.White);
            }
        }
        public void paintMoney(int _money)
        {
            Vector2 pos;
            pos.X = 600;
            pos.Y = 0;
            int w, next = _money, keta = 10000;
            for (int i = 0; i < 5; i++)
            {
              
                    pos.X = 600+(40*i);
                    w = next / keta;

                    s.Draw(imgNo[w], pos, Color.White);
                    next -= w * keta;
                    keta /= 10;
                
               
            }
 
        }
        public int MousePressChk(int x,int y)
        {
            for (int i = 0; i <= stageFlg; i++)
            {
                if (x >= pos[i].X && x < pos[i].X + 100)
                {
                    if (y >= pos[i].Y && y < pos[i].Y + 100)
                    {
                        return i;
                    }
                }
            }

            return -1;
 
        }
        public int mapCheck(int x, int y)
        {
            if (maptable[y / 40, x / 40] == 0)
            {
                if(mapUnit[y/40,x/40] ==-1)
                {
                    mapUnit[y / 40, x / 40] = 0;
                    return 1;
                }
                else if (mapUnit[y / 40, x / 40] == 0)
                {
                    return 2;
                }
            }
            
            return 0;
        }
        public void reset()
        {
            mapUnit = new int[,] { 
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}
        };
        }
    }
    class GameOver
    {
        protected GraphicsDevice g;
      
        protected SpriteBatch s;
        FileStream stream;
          Texture2D imgGameOver;
        Vector2 pos;
        public GameOver(SpriteBatch _s,GraphicsDevice _g)
        {
            s = _s;
            g = _g;
               stream = File.OpenRead("images/GameOver.png");
             imgGameOver = Texture2D.FromStream(g, stream);
            pos.X=0;
            pos.Y =0;
 
        }
        public void paint()
        {
            s.Draw(imgGameOver,pos,Color.White);
        }
     
 
    }
   
    public class BGM
    {
        Song[] song;
        public BGM(Song [] _song)
        {
            song = _song;

            MediaPlayer.IsRepeating = true;  
        }
        public void setBGM(int i)
        {
            //BGMの設定
            //BGMはIDで管理ID=0で停止
            if (i == -1)
            {
                MediaPlayer.Stop();
            }
            else
            {
                MediaPlayer.Play(song[i]);
            }
        }
    }

}
