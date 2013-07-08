using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;


//ユニットと敵の当たり判定
class HitBox
{
    public HitBox()
    {
        //コンストラクタ
    }
    public bool hitcheck(BoundingSphere sphere, BoundingBox box)
    {
        //ユニット（キャラリオ専用）と敵の当たり判定
        //○と□での当たり判定
        return sphere.Intersects(box);
    }

    public bool hitcheck2(BoundingSphere sphere1, BoundingSphere sphere2)
    {
        //ユニット（他）と敵の当たり判定
        //○と○での当たり判定
        return sphere1.Intersects(sphere2);
    }
}
