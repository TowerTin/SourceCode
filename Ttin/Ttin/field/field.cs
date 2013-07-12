using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ttin.field
{
    /// <summary>
    /// ステージのフィールドマップの管理構造体
    /// partialで具体的なフィールドのstaticプロパティを持つ
    /// </summary>
    public partial struct field
    {
        /// <summary>
        /// 敵ユニットのルートマップ
        /// 旧mapa
        /// </summary>
        public readonly sbyte[,] enemy_route;

        /// <summary>
        /// マップチップのマップ
        /// 旧maptable
        /// </summary>
        public readonly sbyte[,] map_tips;

        /// <summary>
        /// 自群ユニットの配置マップ（mapaと一つにする）
        /// 旧unimap
        /// </summary>
        public readonly sbyte[,] unit_locate;

        private field(sbyte[,] enemy_route, sbyte[,] map_tips, sbyte[,] unit_locate)
        {
            this.enemy_route = enemy_route;
            this.map_tips = map_tips;
            this.unit_locate = unit_locate;
        }
    }

    /// <summary>
    /// フィールド系ヘルパー
    /// </summary>
    public class helper
    {
        /// <summary>
        /// セルの大きさ
        /// </summary>
        static readonly Vector2 cell_size = new Vector2(40, 40);

        /// <summary>
        /// ゲーム画面におけるフィールド位置までのオフセット
        /// </summary>
        static readonly Vector2 offset = Vector2.Zero;

        /// <summary>
        /// ポインター座標からフィールドのセル座標を得る
        /// </summary>
        /// <param name="pointer_position">ポインター座標</param>
        /// <returns>セル座標</returns>
        static Tuple<int, int> to_field_cell_position(Vector2 pointer_position)
        {
            var p = pointer_position / cell_size + offset;
            return new Tuple<int, int>((int)p.X, (int)p.Y);
        }
    }
}
