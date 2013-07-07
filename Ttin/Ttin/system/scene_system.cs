using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

namespace Ttin.system
{
    /// <summary>
    /// #2 で実装予定のシーンシステムの#1に必要最小限の試作。
    /// </summary>
    class scene_base_prototype
        : DrawableGameComponent
    {
        protected Ttin game { get { return Game as Ttin; } }
        protected GameComponentCollection components { get { return game.Components; } }
        protected ContentManager content_manager { get { return game.Content; } }
        protected GraphicsDeviceManager graphic_device_manager { get { return game.graphic_device_manager; } }
        protected SpriteBatch sprite_batch { get { return game.sprite_batch; } }
        protected input_manager input_manager { get { return game.input_manager; } }

        public override void Initialize()
        {
            base.Initialize();
            UpdateOrder = -1;
        }

    }
}
