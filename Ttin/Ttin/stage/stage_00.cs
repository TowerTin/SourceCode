using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

namespace Ttin.stage
{


    /// <summary>
    /// stage_00
    /// ToDo: #2 でシーンシステムを実装したならばシーン既定からの派生ないしインターフェース実装を行う
    /// </summary>
    public class stage_00
        : system.scene_base_prototype
    {
        // #1 Ttinから移動。マウスクリックアクションの座標データ。
        // ToDo: よりよい実装にリファクタリングする
        readonly Vector2 pos1 = new Vector2(1, 1)
                       , pos2 = new Vector2(600, 0)
                       , posNo = new Vector2(600, 40)
                       , posUni1 = new Vector2(600, 200)
                       , posUni2 = new Vector2(700, 200)
                       , posUni3 = new Vector2(600, 300)
                       , posUni4 = new Vector2(700, 300)
                       , posUni5 = new Vector2(600, 400)
                       , posUni6 = new Vector2(700, 400)
                       , posUni7 = new Vector2(600, 500)
                       , posG4 = new Vector2(600, 120)
                       , posG42 = new Vector2(700, 120)
                    ;

        // #1 Ttinから移動。
        // ToDo: 謎変数。解読次第適切に対処。
        Vector2 posG5
              , posUM3
              ;

        // #1 Ttinから移動。
        // ToDo: 謎変数。解読次第適切に対処。
        private Texture2D Tgazo, gazo2, icnimg;
        public Texture2D uni1, uni2, uni3, uni4, uni5, uni6, uni7, me3, me32, me4, me5;

        public field.field field { get; private set; }

        // #1 Ttinから移動。
        // ToDo: 挙動未整理。解読次第適切に対処。
        unit_manager.enemmy_unit_manager enemmy_unit_manager;

        // #1 Ttinから移動。
        // ToDo: 挙動未整理。解読次第適切に対処。
        unit_manager.player_unit_manager player_unit_manager;

        // #1 Ttinから移動。
        // ToDo: 挙動未整理。解読次第適切に対処。
        CreateMap cmap;

        // #1 Ttinから移動。
        // ToDo: 謎変数。解読次第適切に対処。
        public Texture2D[] noimg;

        // #1 Ttinから移動。
        // ToDo: 謎変数。解読次第適切に対処。
        public bool flg = false;
        public bool flg2 = true;

        // #1 Ttinから移動。
        // ToDo: 謎変数。解読次第適切に対処。
        int unitNo = -1;
        int gold = 1000;
        int[] uniGo = { 0, 100, 120 };
        int[] ke = { 600, 640, 680, 720, 760, 800 };

        // #1 Ttinから移動。
        // ToDo: 謎変数。解読次第適切に対処。
        public int eneLv { get { return _eneLv; } private set { _eneLv = value; } }
        int _eneLv = 0;

        // #1 Ttinから移動。
        // ToDo: 謎変数。解読次第適切に対処。
        bool ste = false, icn = false;

        // #1 Ttinから移動。
        // ToDo: 謎変数。解読次第適切に対処。
        int lvch = -1;
        int lvc = 0;
        int lvx, lvy, rvx, rvy;
        bool lvani = false, lvup = true, lvup2 = true;
        string smess = "";

        public override void Initialize()
        {
            base.Initialize();

            // #1 フィールマップのfieldクラス化による整理
            field = field.field.Stage_00;

            enemmy_unit_manager = new unit_manager.enemmy_unit_manager(Game);
            components.Add(enemmy_unit_manager);

            player_unit_manager = new unit_manager.player_unit_manager(Game);
            components.Add(player_unit_manager);

            cmap = new CreateMap(graphic_device_manager.GraphicsDevice, sprite_batch);

        }

        protected override void LoadContent()
        {
            base.LoadContent();

            noimg = new Texture2D[10];
            Tgazo = content_manager.Load<Texture2D>("Content/sampgame");
            gazo2 = content_manager.Load<Texture2D>("Content/sampgame2");

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

        }

        public override void Update(GameTime gameTime)
        {
            if (input_manager.button1_pressed)
                unitNo = 1;

            if (input_manager.button2_pressed)
                unitNo = 2;

            // #1 Ttinから移動。
            // ToDo: 何をしているか未解読な関数。解読して適切に対処する。
            mousePressChk();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            sprite_batch.Draw(Tgazo, pos1, Color.White);

            // #1 Ttinから移動。
            // ToDo: 未解読。解読して適切に対処する。
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

        // #1 Ttinから移動。
        // ToDo: 未解読。解読して適切に対処する。
        void mousePressChk()
        {
            // #1 ToDo: この処理はユニット制御クラスのUpdateで行うべき
            if (blast.uniste(input_manager.pointer_position) != "")
            {
                ste = true;
                smess = blast.uniste(input_manager.pointer_position);

            }
            else
                ste = false;

            // #1 ToDo: icnchが何なのかまるで読み取れないので解読次第対処する
            if (icnch(input_manager.pointer_position))
                icn = true;
            else
                icn = false;

            if (input_manager.button1_pressed)
                if (unitNo >= 0)
                    if (gold >= blast.lv0(unitNo))
                        if (blast.setBlast(input_manager.pointer_position, unitNo))
                        {
                            var unit_position = input_manager.pointer_position / 40;
                            ++field.unit_locate[(int)unit_position.Y, (int)unit_position.X];
                            gold -= blast.lv0(unitNo);
                        }

            if (input_manager.button1_released)
                if (!lvup)
                    lvup = true;

            if (input_manager.button1_pressed)
            {

                if (input_manager.pointer_position.X >= posUni1.X && input_manager.pointer_position.X < posUni1.X + 100)
                {
                    if (input_manager.pointer_position.Y >= posUni1.Y && input_manager.pointer_position.Y < posUni1.Y + 100)
                    {
                        unitNo = 0;
                    }
                }

                if (input_manager.pointer_position.X >= posUni2.X && input_manager.pointer_position.X < posUni2.X + 100)
                {
                    if (input_manager.pointer_position.Y >= posUni2.Y && input_manager.pointer_position.Y < posUni2.Y + 100)
                    {
                        unitNo = 1;
                    }

                }

                if (input_manager.pointer_position.X >= posG4.X && input_manager.pointer_position.X < posG4.X + 100)
                {
                    if (input_manager.pointer_position.Y >= posG4.Y && input_manager.pointer_position.Y < posG4.Y + 40)
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

            if (input_manager.button2_released)
                if (!lvup2)
                    lvup2 = true;

            if (input_manager.button2_pressed)
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

            if (input_manager.button1_pressed)
                if (input_manager.pointer_position..X >= posG42.X && input_manager.pointer_position..X < posG42.X + 100)
                    if (input_manager.pointer_position.Y >= posG42.Y && input_manager.pointer_position.Y < posG42.Y + 40)
                    {
                        blast.unitexit(lvx, lvy);
                    }

            if (input_manager.button1_pressed)
                if (flg2)
                {
                    flg = true;
                    flg2 = false;
                }

            if (input_manager.button1_pressed)
                if (input_manager.pointer_position.X < 600 && input_manager.pointer_position.Y < 600)
                {
                    lvx = (int)input_manager.pointer_position.X;
                    lvy = (int)input_manager.pointer_position.Y;
                    rvx = (int)input_manager.pointer_position.X;
                    rvy = (int)input_manager.pointer_position.Y;
                }

            if (input_manager.button2_pressed)
                if (input_manager.pointer_position.X < 600 && input_manager.pointer_position..Y < 600)
                {
                    lvx = (int)input_manager.pointer_position.X;
                    lvy = (int)input_manager.pointer_position.Y;
                    rvx = (int)input_manager.pointer_position.X;
                    rvy = (int)input_manager.pointer_position.Y;
                }
        }

        // #1 一時的な措置
        /// <summary>
        /// icnchのVector2ラッパー
        /// </summary>
        /// <param name="pointer_position">ポインター座標</param>
        /// <returns>？ #1 要・元解読</returns>
        public bool icnch(Vector2 pointer_position)
        { return icnch((int)pointer_position.X, (int)pointer_position.Y); }

        // #1 Ttinから移動。
        // ToDo: 未解読。解読して適切に対処する。
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
}
