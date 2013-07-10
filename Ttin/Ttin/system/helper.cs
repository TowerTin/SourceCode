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
    /// ヘルパー機能群
    /// </summary>
    public class helper
    {
        // #1 HitBox.cs から移行
        // オーバロード化
        // ToDo: 用途を確認次第さらにリファクタリング
        public static bool hitcheck(BoundingSphere a, BoundingSphere b)
        { return a.Intersects(b); }
        public static bool hitcheck(BoundingSphere a, BoundingBox b)
        { return a.Intersects(b); }

        /// <summary>
        /// Texture2Dをファイルからロード
        /// </summary>
        /// <param name="graphic_device">GraphicsDevice</param>
        /// <param name="filename">ファイルパス</param>
        /// <returns></returns>
        public static Texture2D load_from_file(GraphicsDevice graphics_device, string filename)
        {
            var s = File.OpenRead(filename);
            var t = Texture2D.FromStream(graphics_device, s);
            return t;
        }

#if DEBUG

        /// <summary>
        /// Texture2Dを一時ファイル置き場からロード
        /// ※開発用
        /// </summary>
        /// <param name="graphics_device">GraphicsDevice</param>
        /// <param name="tmp_filename">一時ファイル置き場内でのファイルパス</param>
        /// <returns></returns>
        public static Texture2D load_from_tmp_file(GraphicsDevice graphics_device, string tmp_filename)
        { return load_from_file(graphics_device, tmp_path + "/" + tmp_filename); }

        /// <summary>
        /// 実行ファイルから見た一時ファイル置き場のパス
        /// ※開発用
        /// </summary>
        const string tmp_path = "../../tmp";

#endif

    }

}