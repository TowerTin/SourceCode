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
}
