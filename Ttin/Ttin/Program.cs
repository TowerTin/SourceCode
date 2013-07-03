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

    // #1 エントリーポイント分離
    public class Main_
    {
        public static void Main(string[] arg)
        {
            using (Game g = new Ttin())
            {
                g.IsMouseVisible = true;
                g.Run();
            }
        }
    }

}