using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ttin
{
    /// <summary>
    /// ユーザーからのUI入力状態を保持する構造体
    /// ToDo: マウス以外の方法でもポインター移動に対応する
    /// </summary>
    public class input_manager : IUpdateable, IGameComponent
    {
        /// <summary>
        /// 内部用マウス状態
        /// </summary>
        MouseState mouse_state;

        /// <summary>
        /// 内部用キーボード状態
        /// </summary>
        LinkedList<KeyboardState> keyboard_states = new LinkedList<KeyboardState>();

        /// <summary>
        /// 内部用ゲームパッド状態
        /// </summary>
        GamePadState gamepad_state;

        /// <summary>
        /// ポインター（マウス）座標
        /// #1 旧 Ttin.posUM の役割
        /// </summary>
        public Vector2 pointer_position { get { return new Vector2(mouse_state.X, mouse_state.Y); } }

        /// <summary>
        /// ゲームとしてのボタン1が押されているか
        /// サポートする事になればキーボード、ゲームパッドとも複合的に処理する
        /// </summary>
        public bool button1_pressed
        {
            get
            {
                return mouse_state.LeftButton == ButtonState.Pressed
                    || keyboard_is_pressed(Keys.D1)
                    || gamepad_state.Buttons.A == ButtonState.Pressed
                    ;
            }
        }

        /// <summary>
        /// ボタン2が押されているか
        /// </summary>
        public bool button2_pressed
        {
            get
            {
                return mouse_state.RightButton == ButtonState.Pressed
                    || keyboard_is_pressed(Keys.D2)
                    || gamepad_state.Buttons.B == ButtonState.Pressed
                    ;
            }
        }
        
        /// <summary>
        ///  ボタン3が押されているか
        /// </summary>
        public bool button3_pressed
        {
            get
            {
                return mouse_state.MiddleButton == ButtonState.Pressed
                    || keyboard_is_pressed(Keys.D3)
                    || gamepad_state.Buttons.X == ButtonState.Pressed
                    ;
            }
        }

        /// <summary>
        ///  ボタン4が押されているか
        /// </summary>
        public bool button4_pressed
        {
            get
            {
                return mouse_state.XButton1 == ButtonState.Pressed
                    || keyboard_is_pressed(Keys.D4)
                    || gamepad_state.Buttons.Y == ButtonState.Pressed
                    ;
            }
        }

        /// <summary>
        ///  ボタン5が押されているか
        /// </summary>
        public bool button5_pressed
        {
            get
            {
                return mouse_state.XButton2 == ButtonState.Pressed
                    || keyboard_is_pressed(Keys.D5)
                    || gamepad_state.Buttons.BigButton == ButtonState.Pressed
                    ;
            }
        }

        /// <summary>
        /// ボタン1が押されていないか
        /// #1 既存コードへの互換性の為に定義したが、単純に否定を取ればよいだけなので実際問題わざわざここでプロパティを定義すべきか疑問
        /// </summary>
        public bool button1_released { get { return !button1_pressed; } }
        /// <summary>
        /// ボタン2が押されていないか
        /// </summary>
        public bool button2_released { get { return !button2_pressed; } }
        /// <summary>
        /// ボタン3が押されていないか
        /// </summary>
        public bool button3_released { get { return !button3_pressed; } }
        /// <summary>
        /// ボタン4が押されていないか
        /// </summary>
        public bool button4_released { get { return !button4_pressed; } }
        /// <summary>
        /// ボタン5が押されていないか
        /// </summary>
        public bool button5_released { get { return !button5_pressed; } }

        public bool Enabled { get; private set; }
        public int UpdateOrder { get; private set; }

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public input_manager()
        { Initialize(); }

        public void Initialize()
        {
            Enabled = true;
            UpdateOrder = -10;

            initialize_keyboard_states();
        }

        void initialize_keyboard_states()
        {
            keyboard_states.Clear();
            var s = Keyboard.GetState();
            keyboard_states.AddLast(s);
            keyboard_states.AddLast(s);
        }

        /// <summary>
        /// 1つ前の状態と現在の状態から押された直後か否かを判定するヘルパー関数
        /// </summary>
        /// <param name="key">キー</param>
        /// <returns>押されていればtrue</returns>
        bool keyboard_is_pressed(Keys key)
        {
            return keyboard_states.Last.Value.IsKeyUp(key)
                && keyboard_states.First.Value.IsKeyDown(key);
        }

        public void Update(GameTime gameTime)
        {
            update_gamepad();
            update_keyboard();
            update_mouse();
        }

        /// <summary>
        /// ゲームパッドによる操作の実装
        /// 1台目のゲームパッドにのみ対応
        /// </summary>
        void update_gamepad()
        { gamepad_state = GamePad.GetState(PlayerIndex.One); }

        /// <summary>
        /// キーボードによる操作の実装
        /// </summary>
        void update_keyboard()
        { keyboard_states.AddLast(Keyboard.GetState()); }

        /// <summary>
        /// マウスによる操作の実装
        /// </summary>
        void update_mouse()
        { mouse_state = Mouse.GetState(); }

    }

}
