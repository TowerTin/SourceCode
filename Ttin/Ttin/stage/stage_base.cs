using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace Ttin.stage
{

    /// <summary>
    /// ステージ抽象型
    /// </summary>
    public abstract class stage_base
        : system.scene_base_prototype
    {
        /// <summary>
        /// プレイヤーユニットUI用の画像リソース名
        /// </summary>
        protected const string ui_player_unit_resource = "stage/player_unit_ui.png";

        /// <summary>
        /// 背景画像リソース名
        /// </summary>
        protected virtual string background_resource { get { throw new NotImplementedException(); } }

        /// <summary>
        /// ステージシーン開始時のタイトル（背景）表示時間
        /// </summary>
        protected virtual TimeSpan show_title_time { get { return TimeSpan.FromSeconds(3); } }

        protected readonly List<BoundingBox> ui_player_unit_boundings = new List<BoundingBox>();

        protected Texture2D background_image;
        protected Texture2D ui_player_unit_background_image;

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

        // #1 stage_00から分離。マウスクリックアクションの座標データ。
        // ToDo: よりよい実装にリファクタリングする
        protected readonly Vector2
            pos1 = new Vector2(1, 1)
          , pos2 = new Vector2(600, 0)
          , posNo = new Vector2(600, 40)
          , posG4 = new Vector2(600, 120)
          , posG42 = new Vector2(700, 120)
          ;
        
        // #1 stage_00から分離。
        // ToDo: 挙動未整理。解読次第適切に対処。
        protected unit_manager.enemmy_unit_manager enemmy_unit_manager;

        // #1 stage_00から分離。
        // ToDo: 挙動未整理。解読次第適切に対処。
        protected unit_manager.player_unit_manager player_unit_manager;

        // #1 stage_00から分離。
        // ToDo: 挙動未整理。解読次第適切に対処。
        //protected CreateMap cmap;

        protected TimeSpan scene_time { get; private set; }

        public stage_base(Game game) : base(game)
        {
            scene_time = TimeSpan.Zero;
            generate_ui_player_unit_boundings();
        }

        /// <summary>
        /// プレイヤーユニットのUIの反応領域をバウンディングボックスで準備
        /// (600,200),(700,200),(600,300)...
        /// </summary>
        void generate_ui_player_unit_boundings()
        {
            ui_player_unit_boundings.Clear();

            var number_of_unit = 7;
            float base_x = 600f, base_y = 200f;
            var bb_size = new Vector3(100, 100, 0);

            for (var n = 0; n < number_of_unit; ++n)
            {
                var bb_min = new Vector3(base_x + (n % 2 == 0 ? 0 : bb_size.X), base_y + bb_size.Y * (n >> 1), 0);
                ui_player_unit_boundings.Add(new BoundingBox(bb_min, bb_min + bb_size));
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            //field = field.field.Stage_00;
            //components.Add(field);

            enemmy_unit_manager = new unit_manager.enemmy_unit_manager(Game);
            components.Add(enemmy_unit_manager);

            player_unit_manager = new unit_manager.player_unit_manager(Game);
            components.Add(player_unit_manager);

            //cmap = new CreateMap(graphic_device_manager.GraphicsDevice, sprite_batch);

        }

        protected override void LoadContent()
        {
            base.LoadContent();

            background_image = content.Load<Texture2D>(background_resource);
            ui_player_unit_background_image = content.Load<Texture2D>(ui_player_unit_resource);

            ui_player_unit_images[(int)unit_manager.player_unit.Lucy]
                = system.helper.load_from_tmp_file(GraphicsDevice, "img/LuM.png");

            // ToDo: ui_player_unit_imagesにLucy以外の画像もロードする

            ui_bomb_image = system.helper.load_from_tmp_file(GraphicsDevice, "img/boM.png");
            ui_level_up_image = system.helper.load_from_tmp_file(GraphicsDevice, "img/lvup.png");
            ui_level_up_2_image = system.helper.load_from_tmp_file(GraphicsDevice, "img/lvup2.png");
            ui_sale_image = system.helper.load_from_tmp_file(GraphicsDevice, "img/sale.png");
            ui_unit_tips_image = system.helper.load_from_tmp_file(GraphicsDevice, "img/unitmen.png");

        }

        public override void Update(GameTime gameTime)
        {
            scene_time += gameTime.ElapsedGameTime;

            // タイトル画面の表示状態
            if (scene_time < show_title_time)
            {
                // タイトル画面スキップ
                if (input_manager.button1_pressed)
                    scene_time = show_title_time;
                return;
            }

            var t = test_ui_player_unit();
            if (t.Item1)
                ui_player_unit(t.Item2);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // 背景
            sprite_batch.Draw(background_image, Vector2.Zero, Color.White);

            // プレイヤーユニットUI
            sprite_batch.Draw(ui_player_unit_background_image, new Vector2(600, 200), Color.White);

            base.Draw(gameTime);
        }

        /// <summary>
        /// プレイヤーユニットのUIが発動すべきかテストする
        /// </summary>
        /// <returns>Item1: 発動すべきか否か、Item2: 発動すべきインデックス</returns>
        Tuple<bool, int> test_ui_player_unit()
        {
            if (input_manager.button1_pressed)
            {
                var p = new Vector3(input_manager.pointer_position, 0);
                for (var n = 0; n < ui_player_unit_boundings.Count; ++n)
                    if (ui_player_unit_boundings[n].Contains(p) == ContainmentType.Contains)
                        return new Tuple<bool, int>(true, n);
            }
            return new Tuple<bool, int>(false, int.MinValue);
        }

        /// <summary>
        /// UIによるプレイヤーユニット選択
        /// </summary>
        /// <param name="index">プレイヤーユニットID</param>
        void ui_player_unit(int index)
        { player_unit_manager.ui_active_unit = (unit_manager.player_unit)index; }

    }
}
