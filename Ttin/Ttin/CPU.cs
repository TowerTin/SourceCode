using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

//敵ユニットの設定
class CPU
{
    const int BLASTSU = 255;
    public int[] x, y, mapNo, x1, y1, hp, lv, sped, kougeki, cont;
    
    //リストにチャレンジしたときの残骸
    List<int> eX = new List<int>();
    List<int> eY = new List<int>();
    List<int> emapNo = new List<int>();
    List<int> eX1 = new List<int>();
    List<int> eY1 = new List<int>();
    List<Vector2> epos = new List<Vector2>();

    int[,] enemystat =
    {  
    /*{HP,攻撃力,移動速度,画像No. }  */       
    {   100,1,5,0},                    
    {   20,1,2,0},       
    {   40,1,3,0},
    { 1000,5,1,1}          
    };
    int[] gname;
    int[] gname2;

    Vector2[] pos;
    private Vector3 pos3, poss;
    public HitBox hb;
    private Texture2D[] TBlast;
    private SpriteBatch sprite;
    BoundingSphere bs1, bs2;
    BoundingBox box1;
    int life = 5;
    int[,] map;
    int gold = 0;
    public int[] swh;
    public CPU(GraphicsDevice g, SpriteBatch _sprite, int[,] _m, int eneNo)
    {
        pos = new Vector2[BLASTSU];

        x = new int[BLASTSU];
        y = new int[BLASTSU];
        x1 = new int[BLASTSU];
        y1 = new int[BLASTSU];
        swh = new int[BLASTSU];
        hp = new int[BLASTSU];
        mapNo = new int[BLASTSU];
        lv = new int[BLASTSU];
        sped = new int[BLASTSU];
        kougeki = new int[BLASTSU];
        cont = new int[BLASTSU];
        hb = new HitBox();
        map = _m;

        TBlast = new Texture2D[64];
        gname = new int[BLASTSU];
        gname2 = new int[BLASTSU];
        for (int i = 0; i < BLASTSU; i++)
        {
            //初期化
            gname[i] = 0;
            gname2[i] = 0;
            pos[i].X = -30;
            pos[i].Y = 88;
            x1[i] = 1;
            y1[i] = 0;
            swh[i] = 0;
            mapNo[i] = -1;
            hp[i] = enemystat[eneNo, 0];
            lv[i] = 99;
            cont[i] = 10;
            sped[i] = 0;
        }

        for (int i = 0; i < 64; i++)
        {
            //敵ユニットの画像の取得
            if (i < 32)
            {
                using (Stream stream = File.OpenRead("img/Tr" + i + ".png"))
                {
                    TBlast[i] = Texture2D.FromStream(g, stream);

                }
            }
            else if (i < 64)
            {
                using (Stream stream = File.OpenRead("img/en" + (i - 32) + ".png"))
                {
                    TBlast[i] = Texture2D.FromStream(g, stream);

                }
            }
        }
        sprite = _sprite;
    }

    //敵ユニットの生成？
    public void setCPU(int _x, int _y, int en)
    {
        for (int i = 0; i < BLASTSU; i++)
        {
            if (mapNo[i] == -1)
            {
                eX.Add(_x);
                eY.Add(_y);
                x[i] = _x;
                y[i] = _y;
                hp[i] = enemystat[en, 0];
                kougeki[i] = enemystat[en, 1];
                sped[i] = enemystat[en, 2];
                emapNo.Add(0);
                mapNo[i] = 0;
                break;
            }
        }
    }
    int cnt = 0;

    //アニメーションの更新（移動）
    public void animNoUpdate()
    {
        cnt++;
        if (cnt < 5) return;
        cnt = 0;
        for (int i = 0; i < BLASTSU; i++)
        {
            if (mapNo[i] != -1)
            {
                mapNo[i]++;
                if (mapNo[i] >= 7) mapNo[i] = 0;
            }
        }
    }

    //敵ユニットの画像の描画（移動後、当たり判定付）
    public bool paintCPU(Unit bl, int en)
    {

        for (int i = 0; i < BLASTSU; i++)
        {
            if (mapNo[i] != -1)
            {
                //移動先の判断
                switch (mapch((int)pos[i].X, (int)pos[i].Y, map))
                {
                    case 0:
                        gname[i] = 0;
                        pos[i].X = -30;
                        pos[i].Y = 80;
                        x1[i] = sped[i];
                        y1[i] = 0;
                        mapNo[i] = -1;
                        life -= kougeki[i];
                        if (life <= 0)
                        {
                            return true;
                        }
                        break;
                    case 1:
                        gname[i] = 0;
                        x1[i] = sped[i];
                        y1[i] = 0;
                        pos[i].X += x1[i];
                        pos[i].Y += y1[i];
                        break;

                    case 2:
                        gname[i] = 8;
                        x1[i] = 0;
                        y1[i] = sped[i];
                        pos[i].X += x1[i];
                        pos[i].Y += y1[i];
                        break;
                    case 3:
                        gname[i] = 16;
                        x1[i] = -sped[i];
                        y1[i] = 0;
                        pos[i].X += x1[i];
                        pos[i].Y += y1[i];
                        break;
                    case 4:
                        gname[i] = 24;
                        x1[i] = 0;
                        y1[i] = -sped[i];
                        pos[i].X += x1[i];
                        pos[i].Y += y1[i];
                        break;

                    case -1:
                        pos[i].X += x1[i];
                        pos[i].Y += y1[i];
                        break;
                }

                //当たり判定
                if (mapNo[i] != -1)
                    sprite.Draw(TBlast[mapNo[i] + gname[i] + gname2[i]], pos[i], Color.White);
                //判定する敵ユニットの座標の取得
                pos3.X = pos[i].X;
                pos3.Y = pos[i].Y;
                bs1.Center = pos3;
                bs1.Radius = 20;

                for (int ii = 0; ii < bl.suu(); ii++)
                {
                    //判定する自群ユニットの座標の取得
                    poss.X = bl.rx(ii);
                    poss.Y = bl.ry(ii);

                    //当たり判定
                    //『キャラリオ』のみ別の範囲での判定
                    if (bl.getuni(ii) == 7)
                    {
                        //未実装
                        //tiger(bl.rx(ii), bl.ry(ii), getmuki());
                        //bl.turn(ii);
                        //if (bl.interval(ii))
                        //{
                        //    for (int i2 = 0; i2 < BLASTSU; i2++)
                        //    {
                        //        pos3.X = pos[i2].X;
                        //        pos3.Y = pos[i2].Y;
                        //        bs1.Center = pos3;
                        //        bs1.Radius = 20;
                        //        if (hb.hitcheck(bs1, box1))
                        //        {
                        //            hp[i2] -= 100;
                        //        }
                        //    }
                        //}
                    }
                    else
                    {
                        poss.X = bl.rx(ii);
                        poss.Y = bl.ry(ii);
                        bs2.Center = poss;
                        bs2.Radius = bl.renReng(ii);
                        bl.turn(ii);


                        if (hb.hitcheck2(bs1, bs2))
                        {
                            if (bl.interval(ii))
                            {
                                //自群ユニットが『オーズ』だった場合
                                if (bl.getuni(ii) == 4)
                                {
                                    //攻撃した相手のスピードを1段階遅くする
                                    if (sped[i] > 1)
                                        sped[i] -= 1;
                                }
                                //自群ユニットが『ヒコヨシ』だった場合
                                if (bl.getuni(ii) == 5)
                                {
                                    //攻撃前に相手の残りHPの1/100をptとして奪う（1～100）
                                    gold += ((hp[i] / 100) % 100) + 1;
                                }

                                //ダメージを与える
                                hp[i] -= bl.damegi(ii);

                            }
                        }
                    }
                }
            }
        }
        animNoUpdate();
        return false;
    }

    //『キャラリオ』用
    public void tiger(int _x, int _y, int muki)
    {
        if (muki == 0)
        {
            box1.Min = new Vector3(_x, _y, 0);
            box1.Max = new Vector3(600, _y + 20, 0);
        }
        if (muki == 1)
        {
            //box1.Min = new Vector3(0, _x, 0);
            //box1.Max = new Vector3(_x, _y + 20, 0);
        }
        if (muki == 2)
        {
            box1.Min = new Vector3(0, _y, 0);
            box1.Max = new Vector3(_x, _y + 20, 0);
        }
        if (muki == 3)
        {
            //    box1.Min = new Vector3(0, _x, 0);
            //    box1.Max = new Vector3(_y, _y + 20, 0);
        }
    }

    //攻撃する時に敵の方向を取得
    public int getmuki()
    {
        //0右,1下,2左,3上
        int muki1, muki2;
        int chx, chy;
        if (pos3.X < poss.X)
        {
            muki1 = 2;
            chx = (int)(poss.X - pos3.X);
        }
        else
        {
            muki1 = 0;
            chx = (int)(pos3.X - poss.X);
        }
        if (pos3.Y < poss.Y)
        {
            muki2 = 3;
            chy = (int)(poss.Y - pos3.Y);
        }
        else
        {
            muki2 = 1;
            chy = (int)(pos3.Y - poss.Y);
        }
        if (chx < chy)
        {
            return muki1;
        }
        else
        {
            return muki2;
        }

    }

    //goldに貯めていたptを反映させる
    public int getgold()
    {
        int g2 = gold;
        gold = 0;
        return g2;
    }
    int nHP = 10, co = 0;

    //敵ユニットをが倒されたときの処理
    public int enHP()
    {
        int go = 0;
        for (int i = 0; i < BLASTSU; i++)
        {
            {

                if (hp[i] <= 0)
                {
                    go += 10;
                    if (gname2[i] == 0)
                    {
                        gname2[i] = 32;
                    }
                    else
                    {
                        gname2[i] = 0;
                    }
                    gname[i] = 0;
                    pos[i].X = -30;
                    pos[i].Y = 80;
                    x1[i] = 1;
                    y1[i] = 0;
                    mapNo[i] = -1;
                    hp[i] = nHP;
                    co++;
                    if (co >= 30)
                    {
                        nHP += 10;
                        co = 0;
                    }

                }
            }
        }
        return go;
    }

    //敵ユニットの移動ルートを調べる
    public int mapch(int x1, int y1, int[,] map)
    {
        int ax = ((y1) / 40);
        int ay = ((x1) / 40);

        int ax2 = ((y1 + 40) / 40);
        int ay2 = ((x1 + 40) / 40);

        int lv = map[ax, ay];
        int lv2 = map[ax, ay];

        if (lv <= 0)
        {
            return 0;
        }
        if (ax > 0)
        {
            if (lv > map[ax, ay + 1])
            {
                return 1;
            }
            if (lv > map[ax + 1, ay])
            {
                return 2;
            }
        }
        if (ay > 0)
        {
            if (lv > map[ax, ay - 1])
            {
                return 3;
            }
            if (lv > map[ax - 1, ay])
            {
                return 4;
            }
        }

        return -1;
    }
}
