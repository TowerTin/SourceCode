using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;


//マップを生成しマップ画像を表示
class CreateMap
{
    private GraphicsDevice g;
    public SpriteBatch s;
    private Texture2D map;

    public CreateMap(GraphicsDevice _g, SpriteBatch _s)
    {

        g = _g;
        s = _s;
        FileStream stream = File.OpenRead("img/mapTin.png");
        map = Texture2D.FromStream(_g, stream);

    }
    public void paintmap(int[,] maptable)
    {

        Vector2 pos;
        Rectangle rect;
        int w, mlow, mcol;
        pos.X = 0;
        pos.Y = 0;

        for (int i = 0; i < maptable.GetLength(0); i++)
        {
            for (int j = 0; j < maptable.GetLength(1); j++)
            {
                pos.X = j * 40;
                w = maptable[i, j];
                mlow = w / 16;
                mcol = w % 16;
                rect.X = mcol * 40;
                rect.Y = mlow * 40;
                rect.Width = 40;
                rect.Height = 40;
                s.Draw(map, pos, rect, Color.White);
            }
            pos.Y += 40;
        }
    }
}