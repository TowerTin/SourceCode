using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

namespace Ttin.unit_manager
{

    /// <summary>
    /// プレイヤーユニットを管理
    /// #1 Todo: シーンとしての管理、個々のユニットのUpdate、Drawがごちゃ混ぜな状態を綺麗にする。
    /// </summary>
    // #1 ToDo: 管理だけならば Drawable ではなく単なるGameComponent化する
    class player_unit_manager : DrawableGameComponent
    {
        // #1 これはなに？変数名を見た人が発狂するコードを書いてはいけません！
        const int BLASTSU = 225;
        public int[] x, y, mapNo, inter, dam, ren, lv, unit;
        sbyte[,] map;
        public bool[] ani;
        Vector2[] pos;
        int uni, no;
        int[,] unisyu;
        int[] dame = { 0, 2, 10 };
        int[] renge = { 0, 40, 80 };

        int[] rengg = { 0, 40, 80, 120, 160, 200 };
        int[, ,] unitst = 
        {/*[ユニットの種類,Lv,パラメータ{コスト,攻撃,レンジ,インターバル}*/
        /*ルーシー*/{{100,10,1,100},{50,17,1,100},{80,28,1,100},{100,39,1,100},{500,150,1,80}},
        /*パーリィ*/{{100,9,2,100},{80,18,2,100},{100,32,2,100},{150,50,2,100},{500,100,3,100}},
        /*ティガー*/{{550,80,2,1000},{100,95,2,800},{150,120,3,600},{150,150,3,400},{550,255,4,400}},
        /*イズミ*/{{240,10,3,80},{50,17,3,80},{50,22,3,80},{100,35,4,80},{160,50,5,60}},
        /*オーズ*/{{300,5,2,120},{100,15,3,120},{100,25,3,120},{100,35,3,120},{400,45,3,100}},
        /*ヒコヨシ*/{{200,1,1,200},{100,4,1,150},{150,8,1,100},{300,16,2,100},{350,32,2,80}},
        /*キャラリオ*/{{1000,100,-1,2000},{1000,200,-1,2000},{1000,400,-1,1500},{1000,800,-1,1500},{1000,1600,-1,1000}},
        };

        int cnt = 0;
        private Texture2D[] TBlast;
        private SpriteBatch sprite;

        // public Unit(GraphicsDevice g, SpriteBatch _sprite, int[,] _map)
        public player_unit_manager(Game game)
            : base(game)
        {
            var ttin = game as Ttin;

            // #1 に伴いとりあえずは元のコードからの改変を最小限に抑える一時的な措置として
            var g = ttin.GraphicsDevice;
            var _sprite = ttin.sprite_batch;
            var _map = ttin.field.unit_locate;

            TBlast = new Texture2D[20];
            for (int i = 0; i < 15; i++)
            {
                if (i < 5)
                {
                    using (Stream stream = File.OpenRead("img/lu" + i + ".png"))
                    {
                        TBlast[i] = Texture2D.FromStream(g, stream);
                    }
                }
                if (i >= 5 && i < 10)
                {
                    using (Stream stream = File.OpenRead("img/B" + (i - 5) + ".png"))
                    {
                        TBlast[i] = Texture2D.FromStream(g, stream);
                    }
                }
                if (i >= 10)
                {
                    using (Stream stream = File.OpenRead("img/B" + (i - 5) + ".png"))
                    {
                        TBlast[i] = Texture2D.FromStream(g, stream);
                    }
                }
            }
            sprite = _sprite;
            pos = new Vector2[BLASTSU];
            x = new int[BLASTSU];
            y = new int[BLASTSU];
            mapNo = new int[BLASTSU];
            inter = new int[BLASTSU];
            ani = new bool[BLASTSU];
            dam = new int[BLASTSU];
            ren = new int[BLASTSU];
            lv = new int[BLASTSU];
            unit = new int[BLASTSU];
            map = _map;

            //配置したユニットの種類を記憶
            unisyu = new int[,]
            {
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

            no = 0;
            for (int i = 0; i < BLASTSU; i++)
            {
                mapNo[i] = -1;
                inter[i] = 0;
                lv[i] = 0;
                ani[i] = false;
            }
        }

        // #1 一時的な措置として定義
        /// <summary>
        /// setBlastをVector2形式のポインター座標から呼ぶラッパー
        /// </summary>
        /// <param name="pointer_position">ポインター座標</param>
        /// <param name="_uni">？ #1 元のsetBlast関数から後ほど解読する必要あり</param>
        /// <returns>？ #1 元のsetBlast関数から後ほど解読する必要あり</returns>
        public bool setBlast(Vector2 pointer_position, int _uni)
        { return setBlast((int)pointer_position.X, (int)pointer_position.Y, _uni); }

        //自群ユニットの配置
        public bool setBlast(int _x, int _y, int _uni)
        {
            uni = _uni;
            if (uni == -1)
            {
                return false;
            }
            no = _uni * 5;

            if (_x < 600 && _y < 600 && _x > 0 && _y > 0)
            {

                for (int i = 0; i < BLASTSU; i++)
                {

                    if (mapNo[i] == -1)
                    {
                        unit[i] = uni;
                        if (unisyu[_x / 40, _y / 40] == -1)
                        {
                            unisyu[_x / 40, _y / 40] = no;
                        }
                        if (map[_y / 40, _x / 40] == 99)
                        {
                            {
                                dam[i] = unitst[unit[i], lv[i], 1];
                                ren[i] = rengg[unitst[unit[i], lv[i], 2]];
                                x[i] = (_x / 40) * 40;
                                y[i] = (_y / 40) * 40;
                                mapNo[i] = 0;
                                return true;
                            }

                        }
                    }
                }
            }
            return false;
        }


        //アニメーションの更新、攻撃のエフェクト
        public void animNoUpdate()
        {
            cnt++;
            if (cnt < 5) return;
            cnt = 0;
            for (int i = 0; i < BLASTSU; i++)
            {
                if (ani[i])
                {
                    if (mapNo[i] != -1)
                    {
                        mapNo[i]++;
                        if (mapNo[i] >= 4 + unisyu[x[i] / 40, y[i] / 40])
                        {
                            ani[i] = false;
                            mapNo[i] = 0;
                        }
                    }
                }
            }
        }

        //自群ユニットの画像表示
        public void paintBlast()
        {

            for (int i = 0; i < BLASTSU; i++)
            {

                if (mapNo[i] != -1)
                {
                    pos[i].X = x[i];
                    pos[i].Y = y[i];
                    sprite.Draw(TBlast[mapNo[i] + unisyu[x[i] / 40, y[i] / 40]], pos[i], Color.White);
                }


            }
            animNoUpdate();
        }

        // #1 一時的な措置としてunisteとやらのVector2ラッパー
        /// <summary>
        /// unisteのVector2ラッパー
        /// </summary>
        /// <param name="pointer_position">ポインター座標</param>
        /// <returns>？ #1 元のunisteを解読する必要あり</returns>
        public string uniste(Vector2 pointer_position)
        { return uniste((int)pointer_position.X, (int)pointer_position.Y); }

        //マウスカーソルが重なったユニットの次の強化コストなどを表示する
        public string uniste(int _x, int _y)
        {
            int x = _x, y = _y;
            string steit = "";

            for (int i = 0; i < BLASTSU; i++)
            {
                if (pos[i].X <= x && x < pos[i].X + 40)
                {
                    if (pos[i].Y <= y && y < pos[i].Y + 40)
                    {
                        if (lv[i] < 4)
                        {
                            steit = "Next->" + unitst[unit[i], lv[i] + 1, 0] + "pt";
                        }
                        else
                        {
                            steit = "LvMAX!!";
                        }
                    }
                }
            }
            return steit;
        }
        public int renReng(int i)
        {
            //ren[i] = rengg[unitst[unit[i], lv[i], 2]];

            return ren[i];
        }

        //選択した自群ユニットの攻撃力の取得
        public int damegi(int i)
        {
            return dam[i];
        }

        //自群のX座標取得？
        public int rx(int _x)
        {
            return (int)pos[_x].X;
        }

        //自群のY座標取得？
        public int ry(int _y)
        {
            return (int)pos[_y].Y;
        }

        //自群ユニットのインターバルを減らす
        public void turn(int i)
        {
            inter[i] -= 1;
        }

        //i番目のユニットの種類の取得
        public int getuni(int i)
        {
            return unit[i];
        }

        //自群ユニットの数取得
        public int suu()
        {
            return BLASTSU;
        }
        //各自群ユニットが次の攻撃可能になるまでの待機時間
        public bool interval(int i)
        {
            if (inter[i] <= 0)
            {
                inter[i] = unitst[unit[i], lv[i], 3];
                ani[i] = true;
                return true;
            }
            else
            {
                //inter[i] -= 1;
            }

            return false;
        }

        //クリックした自群ユニットの強化処理
        public bool lvup(int x, int y)
        {
            for (int i = 0; i < BLASTSU; i++)
            {
                if (pos[i].X <= x && x < pos[i].X + 40)
                {
                    if (pos[i].Y <= y && y < pos[i].Y + 40)
                    {
                        if (lv[i] < 4)
                        {
                            lv[i] += 1;
                            dam[i] = unitst[unit[i], lv[i], 1];
                            ren[i] = rengg[unitst[unit[i], lv[i], 2]];
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return false;

        }

        //クリックした自群ユニットのコスト取得
        public int lvcost(int x, int y)
        {
            for (int i = 0; i < BLASTSU; i++)
            {
                if (pos[i].X <= x && x < pos[i].X + 40)
                {
                    if (pos[i].X <= x && x < pos[i].X + 40)
                    {
                        return unitst[unit[i], lv[i], 0]; ;
                    }
                }
            }
            return 0;
        }

        //クリックした自群ユニットの次の強化に必要なコスト取得
        public int lvnextcost(int x, int y)
        {
            for (int i = 0; i < BLASTSU; i++)
            {
                if (pos[i].X <= x && x < pos[i].X + 40)
                {
                    if (pos[i].X <= x && x < pos[i].X + 40)
                    {
                        if (lv[i] < 4)
                        {
                            return unitst[unit[i], lv[i] + 1, 0];

                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            return 0;
        }

        //ユニットの退却
        public void unitexit(int x, int y)
        {
            for (int i = 0; i < BLASTSU; i++)
            {
                if (pos[i].X <= x && x < pos[i].X + 40)
                {
                    if (pos[i].Y <= y && y < pos[i].Y + 40)
                    {
                        unisyu[y / 40, x / 40] = -1;
                        dam[i] = 0;
                        ren[i] = 0;
                        mapNo[i] = -1;
                        inter[i] = 0;
                        lv[i] = 0;
                        ani[i] = false;

                    }
                }
            }
        }

        //レベルを初期化する
        public int lv0(int uni)
        {
            return unitst[uni, 0, 0]; ;
        }

    }
}