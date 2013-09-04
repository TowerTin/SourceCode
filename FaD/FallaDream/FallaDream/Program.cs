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
    public class FallaDream : Game
    {
        private GraphicsDeviceManager Gm;
      
        private SpriteBatch sprite;
        private int sceneFlg = 0;
        int stageNo =0;
        private SpriteFont font;
        private Scene scene;
        private bool mouseFlg = true ;
        private StartupScreen statupscreen;
        private StageSelect stageselect;
        private Event talkevent;
        private CreateMap map;
        private Enemy enemy;
        private HitBox hitbox;
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
        public int[] mapMoney = { 300, 500, 1000, 1200, 1300, 1500, 2000, 3000 };
        int stageFlg;
        bool staCliFlg=true;
        private BoundingSphere bsEnemy, bsUnit;
        private Vector3 pos3Enemy, pos3Unit;
        int second = 0;
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
            map = new CreateMap(sprite, Gm.GraphicsDevice);
            hitbox = new HitBox();
            enemydata = new GetEnemyDate();
            // 秒 (Second) を取得する
             iSecond = dtNow.Second;

          

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
                        dtNow = DateTime.Now;
                        now = dtNow;
                        if (now >= w)
                        {
                        
                            w = dtNow.AddSeconds(second);
                            enemy = new Enemy(sprite, Gm.GraphicsDevice, enemys2[co], stageNo);
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
                                    Debug.WriteLine(listenemy.Count);
                                    for (int j = 0; j < kazu-kazu2; j++)
                                    {
                                        listenemy.RemoveAt(0);
                                    }
                                    for (int j = 0; j < kazuuni1; j++)
                                    {
                                        listunit.RemoveAt(0);
                                    }
                                    sceneFlg = 1;
                                }
                            }
                        }
                    
                      
                        for (int i = 0; i < listunit.Count; i++)
                        {
                            listunit[i].UPdate();
                            if (listunit[i].getInterval())
                            {
                                pos3Unit.X = listunit[i].getX();
                                pos3Unit.Y = listunit[i].getY();
                                bsUnit.Center = pos3Unit;
                                bsUnit.Radius = listunit[i].getRNG();
                                for (int j = 0; j < listenemy.Count; j++)
                                {
                                    pos3Enemy.X = listenemy[j].getX();
                                    pos3Enemy.Y = listenemy[j].getY();
                                    bsEnemy.Center = pos3Enemy;
                                    bsEnemy.Radius = 20;
                                    if (hitbox.hitcheck2(bsUnit, bsEnemy))
                                    {
                                        listunit[i].setInterval();
                                        if (listenemy[j].damageHP(listunit[i].getATC()))
                                        {
                                            Money += (int)(listenemy[j].getDRP());
                                            listenemy.RemoveAt(j);
                                            kazu2++;
                                        }
                                        else
                                        {
                                            switch (listunit[i].getUnitNo())
                                            {
                                                case 4:
                                                    listenemy[j].damageSPD();
                                                    break;
                                                case 5:
                                                    int wmo = (int)(listenemy[j].getDRP()) / 10;
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
                    break;

            }
          
            sprite.End();

            base.Draw(gameTime);
        }
        private void mouseChk()
        {
            MouseState state = Mouse.GetState();
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
                                hp = 5;
                                co = 0;
                                kazu = 0;
                                kazu2 = 0;
                                kazuuni1 = 0;

                                enemys = enemydata.getEnemykazu(0);
                                enemys2 = enemydata.getEnemyNo(0);
                                second = enemydata.getSecond(enemys2[0]);
                                Money = mapMoney[0];
 
                            }
                            break;
                        case 1:
                            stageNo = stageselect.MousePressChk(state.X, state.Y);
                            if (stageNo >= 0)
                            {
                                talkevent.setEvent(stageNo);
                                sceneFlg = 2;
                            }
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
                                    
                                    enemys = enemydata.getEnemykazu(stageNo);
                                    enemys2 = enemydata.getEnemyNo(stageNo);
                                    second = enemydata.getSecond(enemys2[0]);
                                    Money = mapMoney[stageNo];
                                    sceneFlg = 3;
                                }
                                else
                                {
                                    talkevent = new Event(sprite, Gm.GraphicsDevice, font);
                                    staCliFlg = true;
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
                                        if (Money >= unit.getCost(unitno))
                                        {
                                            Money = Money - unit.getCost(unitno);
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

                    }
                    mouseFlg = false;
                }
            }
            if (state.LeftButton == ButtonState.Released)
            {
                mouseFlg = true ;
                switch (sceneFlg)
                {
                    case 2:
                        talkevent.MouseReleasChk();
                        break;
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
         }
         return null;
     }
     public int getSecond(int _No)
     {
         if (_No == -1) return 0;

         return stagesec[_No];
           
     }

     
 }


}