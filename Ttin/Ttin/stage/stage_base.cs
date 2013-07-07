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
    /// ステージ抽象型
    /// </summary>
    public abstract class stage_base
        : system.scene_base_prototype
    {
        // #1 stage_00から分離。マウスクリックアクションの座標データ。
        // ToDo: よりよい実装にリファクタリングする
        protected readonly Vector2
            pos1 = new Vector2(1, 1)
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

        // #1 stage_00から分離。
        // ToDo: 挙動未整理。解読次第適切に対処。
        protected unit_manager.enemmy_unit_manager enemmy_unit_manager;

        // #1 stage_00から分離。
        // ToDo: 挙動未整理。解読次第適切に対処。
        protected unit_manager.player_unit_manager player_unit_manager;

        // #1 stage_00から分離。
        // ToDo: 挙動未整理。解読次第適切に対処。
        //protected CreateMap cmap;

        public stage_base(Game game) : base(game) { }

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

    }
}
