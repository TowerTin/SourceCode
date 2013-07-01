using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
//using System.Drawing;
//using System.Windows.Forms;
public class Scene0 : Game
{
    public static void Main(string[] arg)
    {
        using (Game g = new Scene0())
        {
            g.IsMouseVisible = true;
            g.Run();
        }
    }
    private GraphicsDeviceManager Gm;
    private SpriteBatch sprite;
    private  IOS ios;
    public string name = "";
    public string mozi="";
    private Texture2D Tsprite, Tsprite1;
    private Texture2D g1,g2,g3,g4,g5,g6,g7,g8,bgi;
    private ContentManager Cm;
    Vector2 pos,pos1;
    private SpriteFont font, font1;
    bool flg = true;
    bool mas = false;
    int con = 0;
    public Scene0()
    {
        Gm = new GraphicsDeviceManager(this);
        Gm.PreferredBackBufferWidth = 800;
        Gm.PreferredBackBufferHeight = 600;
        Cm = new ContentManager(Services);

       
    }

    protected override void LoadContent()
    { 
        font = Content.Load<SpriteFont>("Content/MS20");
        sprite = new SpriteBatch(GraphicsDevice);
         ios = new IOS(Gm.GraphicsDevice, sprite,font );
        //using (Stream stream = File.OpenRead("hb0.png"))
        //    bgi = Texture2D.FromStream(GraphicsDevice, stream);
        //using (Stream stream = File.OpenRead("masamne0.png"))
        //    g1  = Texture2D.FromStream(GraphicsDevice, stream);
        //using (Stream stream = File.OpenRead("lu0.png"))
        //    g2 = Texture2D.FromStream(GraphicsDevice, stream);
        //using (Stream stream = File.OpenRead("lu1.png"))
        //    g3 = Texture2D.FromStream(GraphicsDevice, stream);
        //using (Stream stream = File.OpenRead("lu3.jpg"))
        //    g4 = Texture2D.FromStream(GraphicsDevice, stream);
        //Tsprite = Cm.Load<Texture2D>("Content/sakura");
        //Tsprite = Content.Load<Texture2D>("Content/sakura");
        //Tsprite1 = Content.Load<Texture2D>("Content/sasuke");
        
      
       
        //font1 = Content.Load<SpriteFont>("Content/SpriteFont2");
        base.LoadContent();
    }
    protected override void Update(GameTime gameTime)
    {
          MouseState state = Mouse.GetState();
          if (state.LeftButton == ButtonState.Pressed)
          {
              using (Stream stream = File.OpenRead("masamne0.png"))
                  g1 = Texture2D.FromStream(GraphicsDevice, stream);
              if (flg)
              {
                  //con++;
                  //mas = true;
                  //if (con >= 5)
                  //    mas = false;

                  ios.getmess();
                  flg = false;
              }
          }
          if (state.LeftButton == ButtonState.Released)
          {
              if (!flg)
              {
                  flg = true ;
              }
          }
        base.Update(gameTime);
    }
    protected override void Draw(GameTime gameTime)
    {
       
        GraphicsDevice.Clear(Color.White);
        sprite.Begin();
    
        //pos.X = 0;
        //pos.Y = 0;
        //sprite.Draw(bgi , pos, Color.White);
        pos.X = 0;
        pos.Y = 100;
        if(mas)
        sprite.Draw(g1, pos, Color.White);
        
        
          ios.paint();
       
        sprite.End();

        base.Draw(gameTime);
    }
}

class IOS
{
    public GraphicsDevice g;
    public SpriteBatch s, sprite;
    public string[] mess;
    private SpriteFont font;
    int i = 0;
    int j = 0;
    Texture2D bgi, imgL, imgR, Tsprite;
    string name = "", mozi = "";
    Vector2 posB, posL, posR, posM,posM2, posN, pos1;
    bool lvisi=false, rvisi=false;
   public  IOS(GraphicsDevice _g,SpriteBatch _s,SpriteFont _f)
    {
        sprite = new SpriteBatch(_g);
        sprite = _s;
        g = _g;
        font = _f;
        posB.X = 0;
        posB.Y = 0;
        posL.X = 0;
        posL.Y = 50;
        posR.X = 550;
        posR.Y = 50;
        Stream stream1 = File.OpenRead("sowaku.png");
        Tsprite = Texture2D.FromStream(g, stream1);
        Stream stream2 = File.OpenRead("hb0.png");
        bgi = Texture2D.FromStream(g, stream2);
        //s = new SpriteBatch(g );
        s = _s;
        mess = new string[200];
        // StreamReader の新しいインスタンスを生成する
        System.IO.StreamReader cReader = (
            new System.IO.StreamReader("Hoge.txt", System.Text.Encoding.Default)
        );

        // 読み込んだ結果をすべて格納するための変数を宣言する
        string stResult = string.Empty;

        // 読み込みできる文字がなくなるまで繰り返す
        while (cReader.Peek() >= 0)
        {
            // ファイルを 1 行ずつ読み込む
            string stBuffer = cReader.ReadLine();
            // 読み込んだものを追加で格納する


            stResult += stBuffer + System.Environment.NewLine;
            //Debug.WriteLine(stBuffer);
            mess[i] = stBuffer;
            i++;
        }

        // cReader を閉じる (正しくは オブジェクトの破棄を保証する を参照)
        cReader.Close();

        // 結果を表示する
    
    }
    public void   getmess()
    {
        string command;
        bool next=true;
        while (next)
        {
            command = mess[j];
            switch (command)
            {
                case "bgi":
                    j++;
                    setBGI();
                    break;

                case "warp":
                    eventwarp();
                    next = false;
                    break;

                case "leftin":
                    j++;
                    charaLin();
                    break;
                case "leftout":
                    j++;
                    lvisi = false;
                    break;
                case "rightin":
                    j++;
                    charaRin();
                    break;
                case "rightout":
                    j++;
                    rvisi = false;
                    break;
                case "doublein":
                    j++;
                    charaDin();
                    break;
                case "doubleout":
                    j++;
                    lvisi = false;
                    rvisi = false;
                    break;

                default:
                    eventmess();
                    next = false;
                    break;

            }
        }
      
    }
    public void paint()
    {
        sprite.Draw(bgi, posB, Color.White);
        if(lvisi)
        sprite.Draw(imgL , posL , Color.White);
        if (rvisi)
        sprite.Draw(imgR, posR, Color.White);
        pos1.X = 10;
        pos1.Y = 340;
        sprite.Draw(Tsprite, pos1, Color.White);
        posN.X = 100;
        posN.Y = 360;
        sprite.DrawString(font, name, posN, Color.Black);
        //posM2.X = 50;
        //posM2.Y = 350;
        //sprite.DrawString(font, name, posM, Color.Black);
        posM.X = 50;
        posM.Y = 400;
        sprite.DrawString(font, mozi, posM, Color.Black);
    }
   
    public void setBGI()
    {
        //背景の設定
        Stream stream = File.OpenRead(mess[j++] + ".png");
        bgi  = Texture2D.FromStream(g, stream);
        //pos.X = 0;
        //pos.Y = 0;
        //sprite.Draw(img, pos, Color.White);
    }
    public void eventwarp()
    {
        //ワープの効果
        Stream stream = File.OpenRead("pikaan.png");
        bgi = Texture2D.FromStream(g, stream);
        name = "";
        mozi = "ピカーーーン！！！";
        lvisi = false;
        rvisi = false;
        j++;

    }
    public void charaLin()
    {
        //左側にキャラを出す
        lvisi = true;
        Stream stream = File.OpenRead(mess[j++] + ".png");    
        imgL  = Texture2D.FromStream(g, stream);
    }
    public void charaRin()
    {
        //右側にキャラを出す
        rvisi = true;
        Stream stream = File.OpenRead(mess[j++] + ".png");
        imgR = Texture2D.FromStream(g, stream);
    }
    public void charaDin()
    {
        //左右同時
        lvisi = true;
        Stream stream1 = File.OpenRead(mess[j++] + ".png");
        imgL = Texture2D.FromStream(g, stream1);
        rvisi = true;
        Stream stream2 = File.OpenRead(mess[j++] + ".png");
        imgR = Texture2D.FromStream(g, stream2);
    }
    public void startstage()
    {
        //ステージの開始？
    }
    public void eventmess()
    {
        //メッセージ表示
        name = mess[j++];
        mozi = mess[j++];
    }

}


