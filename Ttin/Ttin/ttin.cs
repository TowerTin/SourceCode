using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

public class Ttin : Game
{
    private GraphicsDeviceManager Gm;
    // #1 に伴いプロパティpublicにget可能なプロパティに変更
    public SpriteBatch sprite
    {
        get;
        private set;
    }
    private SpriteFont font;

    // #1 コードから意味がわかるように設計を変更
    // 画面中のクリック位置として使われているらしい
    // とりあえず定数のようだから定数メンバーにしとく。
    // ToDo: あとでもっと改善する。
    readonly Vector2 pos1    = new Vector2(1  , 1)
                   , pos2    = new Vector2(600, 0)
                   , posNo   = new Vector2(600, 40)
                   , posUni1 = new Vector2(600, 200)
                   , posUni2 = new Vector2(700, 200)
                   , posUni3 = new Vector2(600, 300)
                   , posUni4 = new Vector2(700, 300)
                   , posUni5 = new Vector2(600, 400)
                   , posUni6 = new Vector2(700, 400)
                   , posUni7 = new Vector2(600, 500)
                   , posG4   = new Vector2(600, 120)
                   , posG42  = new Vector2(700, 120)
                ;
    // #1 これらは変数らしい。使い方が判明次第適切に実装変更する
    Vector2 posG5, posUM, posUM2, posUM3;

    // #1 管理をオブジェクト化する
    private Texture2D Tgazo, gazo2, icnimg;
    public Texture2D uni1, uni2, uni3, uni4, uni5, uni6, uni7, me3, me32, me4, me5;

    // #1 ？
    // update毎にカウントアップするタイミング調整用の変数らしい
    // ToDo: 実装をゲーム時間ベースに変更する
    int st = 0;

    // #2 シーン管理を導入しそちらへ
    private CPU cpu;
    private Unit blast;
    private CreateMap cmap;

    // #1 に伴いとりあえずプロパティにしとく
    // ToDo: あとでフィールドについてはシーン管理とも併せてよりOOP化する
    public int[,] maptable { get; private set; }
    public int[,] mapa { get; private set; }
    public int[,] unimap { get; private set; }

    public Texture2D[] noimg;
    public bool flg = false;
    public bool flg2 = true;
    int unitNo = -1;
    int gold = 1000;
    int[] uniGo = { 0, 100, 120 };
    int[] ke = { 600, 640, 680, 720, 760, 800 };
    int eneLv = 0;
    bool ste = false, icn = false;

    int lvch = -1;
    int lvc = 0;
    int lvx, lvy, rvx, rvy;
    bool lvani = false, lvup = true, lvup2 = true;
    string smess = "";

    public Ttin()
    {
        Gm = new GraphicsDeviceManager(this);
        Gm.PreferredBackBufferHeight = 600;
        Gm.PreferredBackBufferWidth = 800;

        //敵ユニットのルートマップ
        mapa = new int[,] { 
        {98,98,98,98,98,98,98,98,98,98,98,98,98,98,98},
        {98,99,99,99,99,99,99,99,99,99,99,99,99,99,98},
        {51,50,49,48,47,46,45,43,42,41,40,39,38,99,98},
        {98,99,99,99,99,99,99,99,99,99,99,99,37,99,98},
        {98,98,98,98,98,98,98,98,98,98,98,98,36,99,98},
        {98,98,98,98,98,98,98,98,98,98,98,98,35,99,98},
        {98,99,99,99,99,99,99,99,99,99,99,99,34,99,98},
        {98,99,17,18,19,20,21,22,23,24,99,99,33,99,98},
        {98,99,16,99,99,99,99,99,99,25,99,99,32,99,98},
        {98,99,15,99,98,98,98,98,98,26,99,99,31,99,98},
        {98,99,14,99,99,99,99,99,99,27,28,29,30,99,98}, 
        {98,99,13,99,99,99,99,99,99,99,99,99,99,99,98},
        {98,99,12,11,10, 9, 8, 7, 6, 5, 4, 3, 2,99,98},
        {98,99,99,99,99,99,99,99,99,99,99,99, 1,99,98},
        {98,98,98,98,98,98,98,98,98,98,98,98, 0,98,98}
        };

        //マップチップのマップ
        maptable = new int[,] { 
        {49,49,49,49,49,49,49,49,49,49,49,49,49,49,49},
        {17,17,17,17,17,17,17,17,17,17,17,17,17,17,49},
        { 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,17,49},
        {17,17,17,17,17,17,17,17,17,17,17,17, 7,17,49},
        {17,17,17,17,17,17,17,17,17,17,17,17, 7,17,49},
        {49,49,49,49,49,49,49,49,49,49,49,17, 7,17,49},
        {49,49,49,49,49,49,49,49,49,49,49,17, 7,17,49},
        {49,49,17,17,17,17,17,17,17,17,17,17, 7,17,49},
        {49,49,17, 7, 7, 7, 7, 7, 7, 7, 7,17, 7,17,49},
        {49,49,17, 7,17,17,17,17,17,17, 7,17, 7,17,49},
        {49,49,17, 7,17,49,49,49,49,17, 7, 7, 7,17,49}, 
        {49,49,17, 7,17,17,17,17,17,17,17,17,17,17,49},
        {49,49,17, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,17,49},
        {49,49,17,17,17,17,17,17,17,17,17,17, 7,17,49},
        {49,49,49,49,49,49,49,49,49,49,49,49, 7,49,49}
        };

        //自群ユニットの配置マップ（mapaと一つにする）
        unimap = new int[,] { 
        {49,49,49,49,49,49,49,49,49,49,49,49,49,49,49},
        {99,99,99,99,99,99,99,99,99,99,99,99,99,99,49},
        { 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,99,49},
        {99,99,99,99,99,99,99,99,99,99,99,99, 7,99,49},
        {99,99,99,99,99,99,99,99,99,99,99,99, 7,99,49},
        {49,49,49,49,49,49,49,49,49,49,49,99, 7,99,49},
        {49,49,49,49,49,49,49,49,49,49,49,99, 7,99,49},
        {49,49,99,99,99,99,99,99,99,99,99,99, 7,99,49},
        {49,49,99, 7, 7, 7, 7, 7, 7, 7, 7,99, 7,99,49},
        {49,49,99, 7,99,99,99,99,99,99, 7,99, 7,99,49},
        {49,49,99, 7,99,49,49,49,49,99, 7, 7, 7,99,49}, 
        {49,49,99, 7,99,99,99,99,99,99,99,99,99,99,49},
        {49,49,99, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,99,49},
        {49,49,99,99,99,99,99,99,99,99,99,99, 7,99,49},
        {49,49,49,49,49,49,49,49,49,49,49,49, 7,49,49}
        };

        noimg = new Texture2D[10];
    }

    protected override void Initialize()
    {
        sprite = new SpriteBatch(GraphicsDevice);
        font = Content.Load<SpriteFont>("Content/MS20");

        cpu = new CPU(Gm.GraphicsDevice, sprite, mapa, eneLv);
        // #1 ctorパラメーターを抜本的に変更
        blast = new Unit(this);
        cmap = new CreateMap(Gm.GraphicsDevice, sprite);
        base.Initialize();
    }

    // #2 シーンシステムごとにする
    protected override void LoadContent()
    {
        Tgazo = Content.Load<Texture2D>("Content/sampgame");
        gazo2 = Content.Load<Texture2D>("Content/sampgame2");

        for (int i = 0; i < 10; i++)
        {
            using (Stream stream = File.OpenRead("img/no" + i + ".png"))
            {
                noimg[i] = Texture2D.FromStream(GraphicsDevice, stream);
            }
        }

        Stream s1 = File.OpenRead("img/luM.png");
        uni1 = Texture2D.FromStream(GraphicsDevice, s1);
        Stream s11 = File.OpenRead("img/luM.png");
        uni3 = Texture2D.FromStream(GraphicsDevice, s11);
        Stream s12 = File.OpenRead("img/luM.png");
        uni4 = Texture2D.FromStream(GraphicsDevice, s12);
        Stream s13 = File.OpenRead("img/luM.png");
        uni5 = Texture2D.FromStream(GraphicsDevice, s13);
        Stream s14 = File.OpenRead("img/luM.png");
        uni6 = Texture2D.FromStream(GraphicsDevice, s14);
        Stream s15 = File.OpenRead("img/luM.png");
        uni7 = Texture2D.FromStream(GraphicsDevice, s15);

        Stream s2 = File.OpenRead("img/boM.png");
        uni2 = Texture2D.FromStream(GraphicsDevice, s2);
        Stream s4 = File.OpenRead("img/lvup.png");
        me3 = Texture2D.FromStream(GraphicsDevice, s4);
        Stream s42 = File.OpenRead("img/sale.png");
        me32 = Texture2D.FromStream(GraphicsDevice, s42);
        Stream s5 = File.OpenRead("img/lvup2.png");
        me4 = Texture2D.FromStream(GraphicsDevice, s5);
        Stream s6 = File.OpenRead("img/unitmen.png");
        me5 = Texture2D.FromStream(GraphicsDevice, s6);
        base.LoadContent();
    }

    // #1 OOP化
    protected override void Update(GameTime gameTime)
    {
        KeyboardState state = Keyboard.GetState();

        if (state[Keys.D1] == KeyState.Down)
            unitNo = 1;

        if (state[Keys.D2] == KeyState.Down)
            unitNo = 2;

        mousePressChk();

        //敵ユニットを出現させる
        if (st == 30)
        {
            cpu.setCPU(1, 1, eneLv);
            st = 0;
        }
        else
            st++;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);
        sprite.Begin();
        sprite.Draw(Tgazo, pos1, Color.White);
        foreach (IDrawable c in Components)
            c.Draw(gameTime);
        sprite.End();

        // #1 OOPリファクタリング
        // この内容はOOP化してComponentsに放り込んでどうにかする
        /*
        if (flg)
        {
            cmap.paintmap(maptable);
            sprite.Draw(gazo2, pos2, Color.White);
            int w, keta = 10000, next = gold;
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    posNo.X = ke[i];
                    w = next / keta;

                    sprite.Draw(noimg[w], posNo, Color.White);
                    next -= w * keta;
                    keta /= 10;
                    //posG.X += 40;
                }
                catch
                {
                    Debug.WriteLine("金のエラーあとで直す");
                }
            }
            blast.paintBlast();
            gold += cpu.enHP() + cpu.getgold();
            if (gold >= 99999)
            {
                gold = 99999;
            }
            if (cpu.paintCPU(blast, eneLv))
            {
                Stream s3 = File.OpenRead("img/gover.png");
                Tgazo = Texture2D.FromStream(GraphicsDevice, s3);
                flg = false;
            }
            sprite.Draw(uni1, posUni1, Color.White);
            sprite.Draw(uni2, posUni2, Color.White);
            sprite.Draw(uni3, posUni3, Color.White);
            sprite.Draw(uni4, posUni4, Color.White);
            sprite.Draw(uni5, posUni5, Color.White);
            sprite.Draw(uni6, posUni6, Color.White);
            sprite.Draw(uni7, posUni7, Color.White);

            sprite.Draw(me3, posG4, Color.White);
            sprite.Draw(me32, posG42, Color.White);
        }
        else
        {
            sprite.Draw(Tgazo, pos1, Color.White);

        }
        if (lvani)
        {
            if (lvc <= 10)
            {
                lvc++;
                sprite.Draw(me4, posG5, Color.White);
            }
            else
            {
                lvc = 0;
                lvani = false;
            }
        }
        if (ste)
        {
            sprite.Draw(me5, posUM, Color.White);
            posUM2.X = posUM.X + 10;
            posUM2.Y = posUM.Y + 10;
            sprite.DrawString(font, smess, posUM2, Color.Black);
        }
        if (icn)
        {
            sprite.Draw(icnimg, posUM3, Color.White);
        }
        */

        base.Draw(gameTime);
    }

    // #1 内容確認次第だけどifがミートソース過ぎるので設計から見直す
    private void mousePressChk()
    {
        MouseState state = Mouse.GetState();
        posUM.X = state.X;
        posUM.Y = state.Y;
        if (blast.uniste(state.X, state.Y) != "")
        {
            ste = true;
            smess = blast.uniste(state.X, state.Y);

        }
        else
            ste = false;

        if (icnch(state.X, state.Y))
            icn = true;
        else
            icn = false;

        if (state.LeftButton == ButtonState.Pressed)
            if (unitNo >= 0)
                if (gold >= blast.lv0(unitNo))
                    if (blast.setBlast(state.X, state.Y, unitNo))
                    {
                        unimap[state.Y / 40, state.X / 40] += 1;
                        gold -= blast.lv0(unitNo);
                    }

        if (state.LeftButton == ButtonState.Released)
            if (!lvup)
                lvup = true;

        if (state.LeftButton == ButtonState.Pressed)
        {

            if (state.X >= posUni1.X && state.X < posUni1.X + 100)
            {
                if (state.Y >= posUni1.Y && state.Y < posUni1.Y + 100)
                {
                    unitNo = 0;
                }
            }

            if (state.X >= posUni2.X && state.X < posUni2.X + 100)
            {
                if (state.Y >= posUni2.Y && state.Y < posUni2.Y + 100)
                {
                    unitNo = 1;
                }

            }

            if (state.X >= posG4.X && state.X < posG4.X + 100)
            {
                if (state.Y >= posG4.Y && state.Y < posG4.Y + 40)
                {
                    if (lvup)
                    {
                        if (gold >= blast.lvnextcost(lvx, lvy))
                        {
                            if (blast.lvup(lvx, lvy))
                            {
                                gold -= blast.lvcost(lvx, lvy);
                                posG5.X = (lvx / 40) * 40;
                                posG5.Y = (lvy / 40) * 40;
                                lvani = true;
                                lvup = false;
                            }
                        }
                    }

                }

            }

        }

        if (state.RightButton == ButtonState.Released)
            if (!lvup2)
                lvup2 = true;

        if (state.RightButton == ButtonState.Pressed)
            if (lvup2)
                if (gold >= blast.lvnextcost(rvx, rvy))
                    if (blast.lvup(rvx, rvy))
                    {
                        gold -= blast.lvcost(rvx, rvy);
                        posG5.X = (rvx / 40) * 40;
                        posG5.Y = (rvy / 40) * 40;
                        lvani = true;
                        lvup2 = false;
                    }

        if (state.LeftButton == ButtonState.Pressed)
            if (state.X >= posG42.X && state.X < posG42.X + 100)
                if (state.Y >= posG42.Y && state.Y < posG42.Y + 40)
                {
                    blast.unitexit(lvx, lvy);
                }

        if (state.LeftButton == ButtonState.Pressed)
            if (flg2)
            {
                flg = true;
                flg2 = false;
            }

        if (state.LeftButton == ButtonState.Pressed)
            if (state.X < 600 && state.Y < 600)
            {
                lvx = state.X;
                lvy = state.Y;
                rvx = state.X;
                rvy = state.Y;
            }

        if (state.RightButton == ButtonState.Pressed)
            if (state.X < 600 && state.Y < 600)
            {
                lvx = state.X;
                lvy = state.Y;
                rvx = state.X;
                rvy = state.Y;
            }
    }

    // #1 内容確認次第だけどifがミートソース過ぎるので設計から見直す
    public bool icnch(int _x, int _y)
    {
        if (_x >= posUni1.X && _x < posUni1.X + 100)
        {
            if (_y >= posUni1.Y && _y < posUni1.Y + 100)
            {
                Stream icn1 = File.OpenRead("img/Luicn.png");
                icnimg = Texture2D.FromStream(GraphicsDevice, icn1);
                posUM3.X = _x;
                posUM3.Y = _y;
                return true;
            }
        }
        if (_x >= posUni2.X && _x < posUni2.X + 100)
        {
            if (_y >= posUni2.Y && _y < posUni2.Y + 100)
            {
                Stream icn1 = File.OpenRead("img/Paicn.png");
                icnimg = Texture2D.FromStream(GraphicsDevice, icn1);
                posUM3.X = _x - 100;
                posUM3.Y = _y;
                return true;
            }
        }
        if (_x >= posUni3.X && _x < posUni3.X + 100)
        {
            if (_y >= posUni3.Y && _y < posUni3.Y + 100)
            {
                Stream icn1 = File.OpenRead("img/Tiicn.png");
                icnimg = Texture2D.FromStream(GraphicsDevice, icn1);
                posUM3.X = _x;
                posUM3.Y = _y;
                return true;
            }
        }
        if (_x >= posUni4.X && _x < posUni4.X + 100)
        {
            if (_y >= posUni4.Y && _y < posUni4.Y + 100)
            {
                Stream icn1 = File.OpenRead("img/Izicn.png");
                icnimg = Texture2D.FromStream(GraphicsDevice, icn1);
                posUM3.X = _x - 100;
                posUM3.Y = _y;
                return true;
            }
        }
        if (_x >= posUni5.X && _x < posUni5.X + 100)
        {
            if (_y >= posUni5.Y && _y < posUni5.Y + 100)
            {
                Stream icn1 = File.OpenRead("img/Ozicn.png");
                icnimg = Texture2D.FromStream(GraphicsDevice, icn1);
                posUM3.X = _x;
                posUM3.Y = _y - 100;
                return true;
            }
        }
        if (_x >= posUni6.X && _x < posUni6.X + 100)
        {
            if (_y >= posUni6.Y && _y < posUni6.Y + 100)
            {
                Stream icn1 = File.OpenRead("img/Hiicn.png");
                icnimg = Texture2D.FromStream(GraphicsDevice, icn1);
                posUM3.X = _x - 100;
                posUM3.Y = _y - 100;
                return true;
            }
        }
        if (_x >= posUni7.X && _x < posUni7.X + 100)
        {
            if (_y >= posUni7.Y && _y < posUni7.Y + 100)
            {
                Stream icn1 = File.OpenRead("img/Caicn.png");
                icnimg = Texture2D.FromStream(GraphicsDevice, icn1);
                posUM3.X = _x;
                posUM3.Y = _y - 100;
                return true;
            }
        }
        return false;
    }

}