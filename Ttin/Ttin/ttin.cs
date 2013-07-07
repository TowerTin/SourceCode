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
        public const string title = "Fall a Dream";
        public const int width_in_pixels = 800;
        public const int height_in_pixels = 600;
        const string content_sprite_font = "Content/MS20";
        const Type first_scene = typeof(stage.stage_00);

        public GraphicsDeviceManager graphic_device_manager { get; private set; }
        public SpriteBatch sprite_batch { get; private set; }
        public SpriteFont sprite_font { get; private set; }

        public system.input_manager input_manager { get; private set; }

        public Ttin()
        {
            graphic_device_manager = new GraphicsDeviceManager(this);
            graphic_device_manager.PreferredBackBufferWidth = width_in_pixels;
            graphic_device_manager.PreferredBackBufferHeight = height_in_pixels;
        }

        protected override void Initialize()
        {
            sprite_batch = new SpriteBatch(GraphicsDevice);

            reset_scene(Activator.CreateInstance(first_scene) as system.scene_base_prototype);

            Window.AllowUserResizing = false;
            Window.Title = title;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            sprite_font = Content.Load<SpriteFont>(content_sprite_font);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (var c in Components)
                if (c.GetType() is IUpdateable)
                    ((IUpdateable)c).Update(gameTime);

            if (input_manager.exit_pressed)
                Exit();

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

        void reset_scene(system.scene_base_prototype scene)
        {
            Components.Clear();

            input_manager = new system.input_manager();
            Components.Add(input_manager);
            Components.Add(scene);
        }

    }
}