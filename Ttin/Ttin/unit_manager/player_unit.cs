using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

namespace Ttin.unit_manager
{
    /// <summary>
    /// プレイヤーユニット
    /// int互換、必要に応じてIDやインデックスに対応
    /// </summary>
    public enum player_unit : int
    {
        Lucy
      , Paley
      , Tiger
      , Izumi
      , Oz
      , Hikoyoshi
      , Carario

      , none = int.MinValue
      , last = Carario
      , end  = last + 1
    }

    public abstract class player_unit_base : DrawableGameComponent
    {
        public virtual string name { get; }

        public virtual float cost { get; }
        public virtual float attack { get; }
        public virtual float range { get; }
        public virtual float interval { get; }

        public int level { get; set; }

        /// <summary>
        /// レベルを変数とした四次関数の特解を得る
        /// パラメーター計算用。
        /// ToDo: #1着手時点での設定数値を関数で再現する為に必要だが、本来はもっと美しい関数を用いるべきでは。
        /// </summary>
        /// <param name="a">4乗項の定数</param>
        /// <param name="b">3乗項の定数</param>
        /// <param name="c">2乗項の定数</param>
        /// <param name="d">1乗項の定数</param>
        /// <param name="e">0乗項の定数</param>
        /// <returns></returns>
        protected float calc_quartic(double a, double b, double c, double d, double e)
        {
            return (float)
                (
                    a * Math.Pow(level, 4)
                  + b * Math.Pow(level, 3)
                  + c * Math.Pow(level, 2)
                  + d * level
                  + e
                )
                ;
        }

        /// <summary>
        /// レベルを変数とした一次関数の特解を得る
        /// パラメーター計算用。
        /// </summary>
        /// <param name="a">1乗項の定数</param>
        /// <param name="b">0乗項の定数</param>
        protected float calc_linear(double a, double b)
        {
            return (float)
                (
                    a * level
                  + b
                )
                ;
        }

        /// <summary>
        /// レベルを変数とした一次関数の特解を得る
        /// パラメーター計算用。a * exp( b * level )
        /// </summary>
        /// <param name="a">乗算定数</param>
        /// <param name="b">指数定数</param>
        /// <returns></returns>
        protected float calc_exp(double a, double b)
        { return (float)(a * Math.Exp(b * level)); }
    }

    public class Lucy : player_unit_base
    {
        public override string name { get { return "ルーシー"; } }
        public override float cost { get { return calc_quartic(20, -215, 830, -1335, 800); } }
        public override float attack { get { return calc_quartic(4.3333, -44, 157.67, -223, 115); } }
        public override float range { get { return 1; } }
        public override float interval { get { return calc_quartic(-0.8333, 8.3333, -29.167, 41.667, 80); } }
    }

    public class Paley : player_unit_base
    {
        public override string name { get { return "パーリィ"; } }
        public override float cost { get { return calc_quartic(11.667, -118.33, 438.33, -681.67, 450); } }
        public override float attack { get { return calc_quartic(1.2083, -12.25, 45.792, -60.75, 35); } }
        public override float range { get { return calc_quartic(0.0417, -0.4167, 1.4583, -2.0833, 3); } }
        public override float interval { get { return 100; } }
    }

    public class Tiger : player_unit_base
    {
        public override string name { get { return "ティガー"; } }
        public override float cost { get { return calc_quartic(41.667,-508.33,2258.3,-4291.7,3050); } }
        public override float attack { get { return calc_quartic(3.125, -32.083, 119.37, 165.42, 155); } }
        public override float range { get { return calc_quartic(0.1667, -2, 8.3333, -13.5, 9); } }
        public override float interval { get { return calc_quartic(8.3333, -83.333, 291.67, -616.67, 1400); } }
    }

    public class Izumi : player_unit_base
    {
        public override string name { get { return "イズミ"; } }
        public override float cost { get { return calc_quartic(4.1667, -65, 380.83, -940, 860); } }
        public override float attack { get { return calc_quartic(-0.6667, 8.3333, -34.333, 61.667, -25); } }
        public override float range { get { return calc_quartic(-0.0833, 1, -3.9167, 6, 3e-11); } }
        public override float interval { get { return calc_quartic(-0.8333, 8.3333, -29.167, 41.667, 60); } }
    }

    public class Oz : player_unit_base
    {
        public override string name { get { return "オーズ"; } }
        public override float cost { get { return calc_quartic(20.833, -241.67, 1029.2, -1908.3, 1400); } }
        public override float attack { get { return calc_linear(10, -5); } }
        public override float range { get { return calc_quartic(-0.0417, 0.5833, -2.9583, 6.4167, -2); } }
        public override float interval { get { return calc_quartic(-0.8333, 8.3333, -29.167, 41.667, 100); } }
    }

    public class Hikoyoshi : player_unit_base
    {
        public override string name { get { return "ヒコヨシ"; } }
        public override float cost { get { return calc_quartic(-6.25, 54.167, -93.75, -104.17, 350); } }
        public override float attack { get { return calc_quartic(0.0417, 0.0833, -1.0417, 4.9167, -3); } }
        public override float range { get { return calc_quartic(-0.125, 1.4167, -5.375, 8.0833, -3); } }
        public override float interval { get { return calc_quartic(-5, 58.333, -225, 291.67, 80); } }
    }

    public class Carario : player_unit_base
    {
        public override string name { get { return "キャラリオ"; } }
        public override float cost { get { return 1000; } }
        public override float attack { get { return calc_exp(50,0.693); } }
        public override float range { get { return -1; } }
        public override float interval { get { return (float)(2000.0 - 500.0 * (Math.Floor((-1.0 + level) / 2.0))); } }
    }
}
