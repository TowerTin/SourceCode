﻿using Microsoft.Xna.Framework.Content;
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
        protected readonly List<BoundingBox> ui_player_unit_boundings = new List<BoundingBox>();

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

        public stage_base(Game game) : base(game)
        {
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

        public override void Update(GameTime gameTime)
        {
            var test = test_ui_player_unit();
            if (test.Item1)
                ui_player_unit(test.Item2);

            base.Update(gameTime);
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