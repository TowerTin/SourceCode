using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

namespace Ttin
{

    public class Ttin : Game
    {
        public GraphicsDeviceManager graphic_device_manager { get; private set; }
        public SpriteBatch sprite_batch { get; private set; }
        public SpriteFont sprite_font { get; private set; }

        public input_manager input_manager { get; private set; }

        public Ttin()
        {
            graphic_device_manager = new GraphicsDeviceManager(this);
            graphic_device_manager.PreferredBackBufferHeight = 600;
            graphic_device_manager.PreferredBackBufferWidth = 800;
        }

        protected override void Initialize()
        {
            sprite_batch = new SpriteBatch(GraphicsDevice);
            sprite_font = Content.Load<SpriteFont>("Content/MS20");

            input_manager = new input_manager();
            Components.Add(input_manager);

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (var c in Components)
                if (c.GetType() is IUpdateable)
                    ((IUpdateable)c).Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            sprite_batch.Begin();

            foreach (var c in Components)
                if(c.GetType() is IDrawable)
                    ((IDrawable)c).Draw(gameTime);

            sprite_batch.End();

            base.Draw(gameTime);
        }

    }
}