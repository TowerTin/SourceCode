using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.Xna.Framework.Media;


namespace FallaDream
{
    public class FallaDream : Game
    {
        private GraphicsDeviceManager Gm;
      
        private SpriteBatch sprite;
        private int sceneFlg = 0;
        int stageNo =0;
        private SpriteFont font;
        private Scene scene;
        private bool mouseFlg = true ,challengeFlg=false;
        private StartupScreen statupscreen;
        private StageSelect stageselect;
        private Event talkevent;
        private CreateMap map;
        private Enemy enemy;
        private HitBox hitbox;
        private GameOver gameover;
        private GetEnemyDate enemydata;
        public int[,] maptable;
        private List<Enemy> listenemy = new List<Enemy>();
        private Unit unit;
        private List<Unit> listunit = new List<Unit>();
        // 必要な変数を宣言する
        DateTime dtNow = DateTime.Now;
        int unitno = -1;
        int iSecond;
        DateTime w,w1, now;
        int hp = 5;
        int co = 0,enemyco=0,kazu=0,kazu2=0,kazuuni1=0,kazuuni2;
        int[] enemys,enemys2;
        private int Money = 0;
        public int[] mapMoney = { 300, 500, 1000, 1200, 1300, 1500, 2000, 3000 ,5000};
        int stageFlg;
        bool staCliFlg=true;
        private BoundingSphere bsEnemy, bsUnit;
        private Vector3 pos3Enemy, pos3Unit;
        int second = 0;
        Texture2D reng;
        Vector2 pos;
        int muni = 0;
        Random rnd = new Random();
        int randomNumber=0;
        private Song[] song;
        BGM bgm,bgm2;
        SE se;
        int chaUP=1;
        public FallaDream()
        {
            Gm = new GraphicsDeviceManager(this);
            Gm.PreferredBackBufferHeight = 600;
            Gm.PreferredBackBufferWidth = 800;
          
         
        }
        public static void Main(string[] arg)
        {
            using (Game g = new FallaDream())
            {
                g.IsMouseVisible = true;
                g.Run();
            }
        }
        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>("Content/SpriteFont1");
            sprite = new SpriteBatch(GraphicsDevice);
            statupscreen = new StartupScreen(sprite, Gm.GraphicsDevice);
            stageselect = new StageSelect(sprite, Gm.GraphicsDevice);
            talkevent = new Event(sprite, Gm.GraphicsDevice, font);
            gameover = new GameOver(sprite, Gm.GraphicsDevice);
            song = new Song[8];
            for (int i = 0; i < 8; i++)
            {
                song[i] = Content.Load<Song>("Content/bgm"+i);
            }

                hitbox = new HitBox();
            enemydata = new GetEnemyDate();
            bgm = new BGM(song);
            //bgm2 = new BGM(song);
           se = new SE();
          bgm.setBGM(1);
          //bgm2.setBGM(4);
            // 秒 (Second) を取得する
             iSecond = dtNow.Second;

             Stream s = File.OpenRead("Content/images/reng1.png");
             reng = Texture2D.FromStream(Gm.GraphicsDevice, s);

        }
        protected override void Update(GameTime gameTime)
        {
           
            mouseChk();
            switch (sceneFlg)
            {
                case 3:
                    map.UPMoney(Money);
                    if (enemys2[co] != -1)
                    {
                        if (enemys2[co] == -2)
                        {
                            co = 0;
                            chaUP++;
                        }
                        dtNow = DateTime.Now;
                        now = dtNow;
                       
                        if (now >= w)
                        {
                            w = dtNow.AddSeconds(second);
                            enemy = new Enemy(sprite, Gm.GraphicsDevice, enemys2[co], stageNo,chaUP);
                            kazu++;
                            listenemy.Add(enemy);
                            if (enemyco >= enemys[co])
                            {
                                co++;
                                second = enemydata.getSecond(enemys2[co]);
                                
                                enemyco = 0;
                            }
                            enemyco++;
                        }
                    }
                    else if(kazu ==kazu2)
                    {
                        for (int j = 0; j < kazuuni1; j++)
                        {
                            listunit.RemoveAt(0);
                        }
                        stageselect.nextstage(stageFlg, stageNo);
                        talkevent.setEvent(stageNo+8);
                        staCliFlg = false;
                        bgm.setBGM(6);
                        
                        sceneFlg = 2;
                    }
                       
                    for (int i = 0; i < listenemy.Count; i++)
                        {
                            if (listenemy[i].enemymove())
                            {
                                hp -= listenemy[i].getATC();
                                listenemy.RemoveAt(i);
                                kazu--;
                               
                                if (hp <= 0)
                                {
                                    co = 4;
                                 
                                    for (int j = 0; j < kazu-kazu2; j++)
                                    {
                                        listenemy.RemoveAt(0);
                                    }
                                    for (int j = 0; j < kazuuni1; j++)
                                    {
                                        listunit.RemoveAt(0);
                                    }
  
                                    bgm.setBGM(7);
                                    sceneFlg = 4;
                                }
                            }
                        }
                  
                      
                        for (int i = 0; i < listunit.Count; i++)
                        {
                            listunit[i].UPdate();
                            randomNumber = rnd.Next(4);
                            if (listunit[i].getInterval())
                            {
                                pos3Unit.X = listunit[i].getX();
                                pos3Unit.Y = listunit[i].getY();
                                bsUnit.Center = pos3Unit;
                                bsUnit.Radius = listunit[i].getRNG();
                                for (int j = 0; j < listenemy.Count ; j++)
                                {
                                    int lex=0, ley=0, no=0;
                                    pos3Enemy.X = listenemy[j].getX();
                                    pos3Enemy.Y = listenemy[j].getY();
                                    bsEnemy.Center = pos3Enemy;
                                    bsEnemy.Radius = 20;
                                    if (hitbox.hitcheck2(bsUnit, bsEnemy))
                                    {
                                        //if (listunit[i].getUnitNo() != 6)
                                        //{
                                        //    listunit[i].setInterval();
                                        //}
                                        //else 
                                        //{ 

                                        //}
                                        se.setSE();
                                        listunit[i].setInterval();
                                        if (listunit[i].getUnitNo() == 6)
                                        {
                                           
                                           BoundingBox  box1 = new BoundingBox();
                                           
                                           muni = (++muni%4);

                                           switch (muni)
                                            {
                                                case 0:
                                                    box1.Min = new Vector3(listunit[i].getX()+10,listunit[i].getY()-600,0);
                                                    box1.Max = new Vector3(listunit[i].getX() + 30, listunit[i].getY(), 0);
                                                    lex = (int)listunit[i].getX();
                                                    ley = (int)listunit[i].getY() - 600;
                                                    no=0;
                                                    break;
                                                case 1:
                                                    box1.Min = new Vector3(listunit[i].getX() + 40, listunit[i].getY()-10, 0);
                                                    box1.Max = new Vector3(listunit[i].getX()+640,listunit[i].getY ()+10,0);
                                                    lex = (int)listunit[i].getX() + 40;
                                                    ley = (int)listunit[i].getY();
                                                    no=1;
                                                    break;
                                                case 2:
                                                    box1.Min = new Vector3(listunit[i].getX()+10, listunit[i].getY()+40, 0);
                                                    box1.Max = new Vector3(listunit[i].getX() + 30, listunit[i].getY() + 640, 0);
                                                    lex = (int)listunit[i].getX();
                                                    ley = (int)listunit[i].getY()+40;
                                                    no=0;
                                                    break;
                                                case 3:
                                                    box1.Min = new Vector3(listunit[i].getX()-600, listunit[i].getY()-10 , 0);
                                                    box1.Max = new Vector3(listunit[i].getX(), listunit[i].getY() + 10, 0);
                                                     lex = (int)listunit[i].getX() -600;
                                                    ley = (int)listunit[i].getY();
                                                    no=1;
                                                    break;


                                            }

                                            for (int k = 0; k < listenemy.Count; k++)
                                            {
                                                pos3Enemy.X = listenemy[k].getX();
                                                pos3Enemy.Y = listenemy[k].getY();
                                                bsEnemy.Center = pos3Enemy;
                                                bsEnemy.Radius = 20;
                                              
                                            
                                                if (hitbox.hitcheck(bsEnemy, box1))
                                                {
                                                    listunit[i].le(no, lex, ley);
                                                    if (listenemy[k].damageHP(listunit[i].getATC()))
                                                    {
                                                       
                                                        Money += (int)(listenemy[k].getDRP());
                                                        listenemy.RemoveAt(k);
                                                        k--;
                                                        //kazu--;
                                                        kazu2++;
                                                    }

                                                }
 
                                            }
                                            
                                        }
                                       else if (listenemy[j].damageHP(listunit[i].getATC()))
                                        {
                                            Money += (int)(listenemy[j].getDRP());
                                            listenemy.RemoveAt(j);
                                            kazu2++;
                                                //kazu--;
                                        }
                                        else
                                        {
                                            switch (listunit[i].getUnitNo())
                                            {
                                                case 4:
                                                    listenemy[j].damageSPD(listunit[i].getLv());
                                                    break;
                                                case 5:
                                                    int wmo = (int)(listenemy[j].getDRP()) / (10-listunit[i].getLv());
                                                    if (wmo <= 1)
                                                    {
                                                        wmo = 1;
                                                    }
                                                    Money += wmo;
                                                    break;
                                            }

                                        }
                                    }

                                }
                            }
                        }
                        break;
            }
          
            base.Update(gameTime);

        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            sprite.Begin();
            switch (sceneFlg)
            {
                case 0:
                    statupscreen.drow();
                    break;
                case 1:
                    stageselect.drow();
                    break;
                case 2:
                    talkevent.drow();
                    break;
                case 3:
                     stageFlg = stageselect.getstageFlg();
                     
                    map.paint(stageNo, stageFlg, Money);
                    for (int i = 0; i < listenemy.Count; i++)
                    {
                        listenemy[i].paint();
                    }
                    for (int i = 0; i < listunit.Count; i++)
                    {
                        listunit[i].paint();
                    }
                    //sprite.Draw(reng, pos, Color.White);
                    break;
                case 4:
                    gameover.paint();
                    break;

                    
            }
          
            sprite.End();

            base.Draw(gameTime);
        }
        private void mouseChk()
        {
           
            MouseState state = Mouse.GetState();
            pos.X = state.X;
            pos.Y = state.Y;
            

            if (state.X >= 0 && state.X <= 800 && state .Y >=0 && state .Y <=600)
            {
              
             
                switch (sceneFlg)
                {
                    case 0:
                        statupscreen.MouseHoverChk(state.X, state.Y);
                        break;
                    case 1:
                        stageselect.MouseHoverChk(state.X, state.Y);
                        break;
                }

                if (state.LeftButton == ButtonState.Pressed)
                {
                    if (mouseFlg)
                    {
                        switch (sceneFlg)
                        {
                            case 0:
                                sceneFlg = statupscreen.MousePressChk(state.X, state.Y);
                                if (sceneFlg == 3)
                                {
                                    stageNo = rnd.Next(stageFlg);
                                    dtNow = DateTime.Now;
                                    hp = 5;
                                    co = 0;
                                    kazu = 0;
                                    kazu2 = 0;
                                    kazuuni1 = 0;
                                    map = new CreateMap(sprite, Gm.GraphicsDevice);
                                    enemys = enemydata.getEnemykazu(8);
                                    enemys2 = enemydata.getEnemyNo(8);
                                    second = enemydata.getSecond(enemys2[0]);
                                    Money = mapMoney[8];
                                    challengeFlg = true;
                                    int[] aaa = { 3, 3, 5, 5, 4, 4, 2, 2 };
                                    bgm.setBGM(aaa[stageNo]);
                                    w = dtNow.AddSeconds(5);
                                }
                         
                                break;
                            case 1:
                                stageNo = stageselect.MousePressChk(state.X, state.Y);
                                if (stageNo >= 0)
                                {
                                    talkevent.setEvent(stageNo);
                                    sceneFlg = 2;
                                }
                                bgm.setBGM(6);
                                break;
                            case 2:
                                if (talkevent.MousePressChk())
                                {
                                    if (staCliFlg)
                                    {
                                        talkevent = new Event(sprite, Gm.GraphicsDevice, font);
                                        //enemy = new Enemy(sprite, Gm.GraphicsDevice, 0, 0, 0, 80);
                                        //listenemy.Add(enemy ); 
                                        hp = 5;
                                        co = 0;
                                        kazu = 0;
                                        kazu2 = 0;
                                        kazuuni1 = 0;
                                        map = new CreateMap(sprite, Gm.GraphicsDevice);
                                        enemys = enemydata.getEnemykazu(stageNo);
                                        enemys2 = enemydata.getEnemyNo(stageNo);
                                        second = enemydata.getSecond(enemys2[0]);
                                        Money = mapMoney[stageNo];
                                        sceneFlg = 3;
                                        int[] aaa = {3,3,5,5,4,4,2,2};
                                        bgm.setBGM(aaa[stageNo]);
                                        dtNow = DateTime.Now;
                                        w = dtNow.AddSeconds(5);
                                    }
                                    else
                                    {
                                        talkevent = new Event(sprite, Gm.GraphicsDevice, font);
                                        staCliFlg = true;
                                        bgm.setBGM(1);
                                        sceneFlg = 1;
                                    }
                                }
                                
                                break;
                            case 3:

                                if (state.X > 600)
                                {

                                    unitno = map.MousePressChk(state.X, state.Y);
                                }
                                else
                                {
                                    if (unitno != -1)
                                    {
                                        int ch = map.mapCheck(state.X, state.Y);
                                        if (ch == 1)
                                        {
                                            unit = new Unit(sprite, Gm.GraphicsDevice, unitno, state.X, state.Y);
                                            if (Money >= unit.getCost())
                                            {
                                                Money = Money - unit.getCost();
                                                listunit.Add(unit);
                                                kazuuni1++;
                                            }
                                        }
                                        else if (ch == 2)
                                        {
                                            for (int i = 0; i < listunit.Count; i++)
                                            {
                                                if (listunit[i].getX() <= state.X && state.X < listunit[i].getX() + 40)
                                                {

                                                    if (listunit[i].getY() <= state.Y && state.Y < listunit[i].getY() + 40)
                                                    {

                                                        if (Money >= listunit[i].getLVUPCost())
                                                        {

                                                            Money = Money - listunit[i].getLVUPCost();

                                                            listunit[i].getLvUP();

                                                        }


                                                    }


                                                }


                                            }


                                        }
                                    }
                                }
                            
                                break;
                            case 4:
                                bgm.setBGM(1);
                                if (challengeFlg)
                                {
                                    sceneFlg = 0;
                                }
                                else
                                {
 
                                    sceneFlg = 1;
                                }
                               
                                break;

                        }
                        mouseFlg = false;
                    }
                }
                if (state.RightButton == ButtonState.Pressed)
                {
                  
                    switch (sceneFlg)
                    {
                        case 1:
                            sceneFlg = 0;
                            break;
                        case 2:
                            talkevent.setLise();
                             hp = 5;     
                             co = 0;
                             kazu = 0;
                             kazu2 = 0;
                             kazuuni1 = 0;
                             map = new CreateMap(sprite, Gm.GraphicsDevice);
                             enemys = enemydata.getEnemykazu(stageNo);
                             enemys2 = enemydata.getEnemyNo(stageNo);
                             second = enemydata.getSecond(enemys2[0]);
                             Money = mapMoney[stageNo];
                             sceneFlg = 3;
                             int[] aaa = {3,3,5,5,4,4,2,2};
                             bgm.setBGM(aaa[stageNo]);
                             dtNow = DateTime.Now;
                             w = dtNow.AddSeconds(5);
                             break;
                        case 3:
                             //int ch = map.mapCheck(state.X, state.Y);
                             for (int i = 0; i < listunit.Count; i++)
                             {
                                 if (listunit[i].getX() <= state.X && state.X < listunit[i].getX() + 40)
                                 {

                                     if (listunit[i].getY() <= state.Y && state.Y < listunit[i].getY() + 40)
                                     {
                                         Money = Money +( listunit[i].getCost())/2;
                                         map.maptetai(state.X, state.Y);
                                         listunit.RemoveAt(i);
                                           kazuuni1--;
                                     }
                                 }

                             }
                             break;
                    }
                }
                if (state.LeftButton == ButtonState.Released)
                {
                    mouseFlg = true;
                    switch (sceneFlg)
                    {
                        case 2:
                            talkevent.MouseReleasChk();
                            break;
                    }
                }
            }
        }
            }



//ユニットと敵の当たり判定
 class HitBox
{
     public HitBox()
     {
         //コンストラクタ
     }
    public bool hitcheck(BoundingSphere sphere, BoundingBox box)
    {
        //ユニット（キャラリオ専用）と敵の当たり判定
        //○と□での当たり判定
        return sphere.Intersects(box);
    }

    public bool hitcheck2(BoundingSphere sphere1, BoundingSphere sphere2)
    {
        //ユニット（他）と敵の当たり判定
        //○と○での当たり判定
        return sphere1.Intersects(sphere2);
    }
}
 class GetEnemyDate:Data
 {
     public int[] getEnemyNo(int _No)
     {
         switch(_No)
         {
             case 0:
                 return enemyNo0;
             case 1:
                 return enemyNo1;
             case 2:
                 return enemyNo2;
             case 3:
                 return enemyNo3;
             case 4:
                 return enemyNo4;
             case 5:
                 return enemyNo5;
             case 6:
                 return enemyNo6;
             case 7:
                 return enemyNo7;    
             case 8:
                 return enemyNoC;
         }
         return null;
     }
     public int[] getEnemykazu(int _No)
     {
         switch (_No)
         {
             case 0:
                 return enemykazu0;
             case 1:
                 return enemykazu1;
             case 2:
                 return enemykazu2;
             case 3:
                 return enemykazu3;
             case 4:
                 return enemykazu4;
             case 5:
                 return enemykazu5;
             case 6:
                 return enemykazu6;
             case 7:
                 return enemykazu7;
             case 8:
                 return enemykazuC;
         }
         return null;
     }
     public int getSecond(int _No)
     {
         if (_No <= -1) return 0;

         return stagesec[_No];
           
     }

     
 }


}