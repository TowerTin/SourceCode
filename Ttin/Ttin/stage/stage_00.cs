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
        : stage_base
    {
        /// <summary>
        /// 背景画像のリソース名
        /// 文字列を指定さえすれば基底クラスがよしなに
        /// </summary>
        protected override string background_resource
        { get { return "stage/00/background.png"; } }

        // #1 Ttinから移動。
        // ToDo: 謎変数。解読次第適切に対処。
        Vector2 posG5  // 右側のよくわからん
              , posUM3 // 右側のよくわからん
              ;

        // #1 Ttinから移動。
        // ToDo: 謎変数。解読次第適切に対処。
        private Texture2D icnimg; // 右側のアイコンかマウスオーバーの時のアイコン

        // UIのプレイヤーユニットの絵
        protected Texture2D[] ui_player_unit_images = new Texture2D[(int)unit_manager.player_unit.last];

        // #1 UIばくはつの絵
        protected Texture2D ui_bomb_image;

        // #1 UIレベルアップの絵
        protected Texture2D ui_level_up_image;
        // #1 UIレベルアップの絵、その２
        protected Texture2D ui_level_up_2_image;

        // #1 UI売り出しの絵
        protected Texture2D ui_sale_image;

        // #1 UIプレイヤーユニットのチップスの絵
        protected Texture2D ui_unit_tips_image;

        public field.field field { get; private set; }

        // #1 Ttinから移動。
        // ToDo: 謎変数。解読次第適切に対処。
        public Texture2D[] noimg; // 敵とプレイヤーユニットのアニメーション

        // #1 Ttinから移動。
        // ToDo: 謎変数。解読次第適切に対処。
        int unitNo = -1;
        int gold = 1000;
        int[] uniGo = { 0, 100, 120 };
        int[] ke = { 600, 640, 680, 720, 760, 800 };

        // #1 Ttinから移動。
        // ToDo: 謎変数。解読次第適切に対処。
        // 敵ユニットのインデックス
        public int eneLv { get { return _eneLv; } private set { _eneLv = value; } }
        int _eneLv = 0;

        // #1 Ttinから移動。
        // ToDo: 謎変数。解読次第適切に対処。
        bool ste = false // わかんない
           , icn = false // わかんない（あいこんがでるかでないか？）
           ;

        // #1 Ttinから移動。
        // ToDo: 謎変数。解読次第適切に対処。
        int lvch = -1; // レベルチェック。用途不明
        int lvc = 0; // わかんない
        int lvx, lvy, rvx, rvy; // LVはレベルアップの選択肢のXY、RVはリリース
        bool lvani = false, lvup = true, lvup2 = true; // lvup/lvup2はレベルアップ処理の連続処理防止の為のフラグ
        string smess = ""; // 出てるユニットにカーソルオーバーで次までのポイントを表示するメッセージ

        public stage_00(Game game) : base(game) { }

        protected override void LoadContent()
        {
            base.LoadContent();

            noimg = new Texture2D[10];

            for (int i = 0; i < 10; i++)
            {
                using (Stream stream = File.OpenRead("img/no" + i + ".png"))
                {
                    noimg[i] = Texture2D.FromStream(GraphicsDevice, stream);
                }
            }

            ui_player_unit_images.Add
                ( unit_manager.player_unit.Lucy
                , system.helper.load_from_tmp_file(GraphicsDevice, "img/LuM.png")
                );

            // ToDo: ui_player_unit_imagesにLucy以外の画像もロードする

            ui_bomb_image = system.helper.load_from_tmp_file(GraphicsDevice, "img/boM.png");
            ui_level_up_image = system.helper.load_from_tmp_file(GraphicsDevice, "img/lvup.png");
            ui_level_up_2_image = system.helper.load_from_tmp_file(GraphicsDevice, "img/lvup2.png");
            ui_sale_image = system.helper.load_from_tmp_file(GraphicsDevice, "img/sale.png");
            ui_unit_tips_image = system.helper.load_from_tmp_file(GraphicsDevice, "img/unitmen.png");

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (input_manager.button1_pressed)
                unitNo = 1;

            if (input_manager.button2_pressed)
                unitNo = 2;

            // #1 Ttinから移動。
            // ToDo: 何をしているか未解読な関数。解読して適切に対処する。
            mousePressChk();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // #1 Ttinから移動。
            // ToDo: 未解読。解読して適切に対処する。
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
        }

        // #1 Ttinから移動。
        // ToDo: 未解読。解読して適切に対処する。
        void mousePressChk()
        {
            // #1 ToDo: この処理はユニット制御クラスのUpdateで行うべき
            if (player_unit_manager.uniste(input_manager.pointer_position) != "")
            {
                ste = true;
                smess = player_unit_manager.uniste(input_manager.pointer_position);

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
                    if (gold >= player_unit_manager.lv0(unitNo))
                        if (player_unit_manager.setBlast(input_manager.pointer_position, unitNo))
                        {
                            var unit_position = input_manager.pointer_position / 40;
                            ++field.unit_locate[(int)unit_position.Y, (int)unit_position.X];
                            gold -= player_unit_manager.lv0(unitNo);
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
                            if (gold >= player_unit_manager.lvnextcost(lvx, lvy))
                            {
                                if (player_unit_manager.lvup(lvx, lvy))
                                {
                                    gold -= player_unit_manager.lvcost(lvx, lvy);
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
                    if (gold >= player_unit_manager.lvnextcost(rvx, rvy))
                        if (player_unit_manager.lvup(rvx, rvy))
                        {
                            gold -= player_unit_manager.lvcost(rvx, rvy);
                            posG5.X = (rvx / 40) * 40;
                            posG5.Y = (rvy / 40) * 40;
                            lvani = true;
                            lvup2 = false;
                        }

            if (input_manager.button1_pressed)
                if (input_manager.pointer_position.X >= posG42.X && input_manager.pointer_position.X < posG42.X + 100)
                    if (input_manager.pointer_position.Y >= posG42.Y && input_manager.pointer_position.Y < posG42.Y + 40)
                    {
                        player_unit_manager.unitexit(lvx, lvy);
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
                if (input_manager.pointer_position.X < 600 && input_manager.pointer_position.Y < 600)
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
