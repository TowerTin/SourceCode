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
    }

}