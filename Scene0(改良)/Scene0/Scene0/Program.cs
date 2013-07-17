using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Media;

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
    private ContentManager Cm;
    private SpriteFont font;
    bool flg = true;
    bool mas = true ;
    public Song[] song;
    

    public Scene0()
    {
        Gm = new GraphicsDeviceManager(this);
        Gm.PreferredBackBufferWidth = 800;
        Gm.PreferredBackBufferHeight = 600;
        Cm = new ContentManager(Services);
        song = new Song[10];
    }

    protected override void LoadContent()
    {
        //BGMの取得　改良予定
        song[0] = Content.Load<Song>("Content/GLORIA");
        font = Content.Load<SpriteFont>("Content/MS20");
        sprite = new SpriteBatch(GraphicsDevice); 
        ios = new IOS(Gm.GraphicsDevice, sprite,font ,song ,"Hoge.txt");
        base.LoadContent();
    }
    protected override void Update(GameTime gameTime)
    {
          MouseState state = Mouse.GetState();
          if (state.LeftButton == ButtonState.Pressed)
          {
              if (flg)
              {
                  ios.getEVENT();
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
    public List<string> serif; 
    private SpriteFont font;
    Texture2D imgL1, imgL2, imgR1, imgR2;
    Texture2D bgi,fram;
    string name = "", mozi = "";
    Vector2 posBGI, posL1, posL2, posR1,posR2, posMOZI, posNAME, posFRAME;
    bool visL1 = false, visL2 = false , visR1 = false,visR2= false ;
    private System.Media.SoundPlayer player = null;

    
   public Song[] song;     
   public  IOS(GraphicsDevice _g,SpriteBatch _s,SpriteFont _f,Song[] _song,string title)
    {
       
        sprite = new SpriteBatch(_g);
        song = _song;
        sprite = _s;
        g = _g;
        font = _f;
        posBGI.X = 0;
        posBGI.Y = 0;
        posL1.X = 60;
        posL1.Y = 10;
        posL2.X = 0;
        posL2.Y = 10;
        posR1.X = 480;
        posR1.Y = 10;
        posR2.X = 520;
        posR2.Y = 10;
        posFRAME.X = 10;
        posFRAME.Y = 340;
        posNAME.X = 100;
        posNAME.Y = 360;
        posMOZI.X = 50;
        posMOZI.Y = 400;
        Stream stream1 = File.OpenRead("sowaku.png");
        fram = Texture2D.FromStream(g, stream1);

        
       Stream stream = File.OpenRead("hb0.png"); 
       bgi = Texture2D.FromStream(g, stream);
      
        
       s = _s; 
       serif = new List<string>();
       mess = new string[200];
        // StreamReader の新しいインスタンスを生成する
        System.IO.StreamReader cReader = (
            new System.IO.StreamReader(title, System.Text.Encoding.Default)
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
            serif.Add(stBuffer);
        }

        // cReader を閉じる (正しくは オブジェクトの破棄を保証する を参照)
        cReader.Close();

        // 結果を表示する
    
    }
    public void   getEVENT()
    {
        string command;

        bool next=true;

        while (next)
        {
            command = getNext();
            var com = command.Split(',');
            switch (com[0])
            {
                case "IN":
                    setIN(com);
                    break;

                case "OUT":
                    setOUT(com);
                    break;

                case "WARP":
                    setWARP();
                    break;

                case "BGI":
                    setBGI(int.Parse(com[1]));
                    break;

                case "BGM":
                    setBGM(int.Parse(com[1]));
                    break;

                case "SE":
                    setSE(int.Parse(com[1]));
                    break;
                case "STOP":
                    next = false;
                    break;
                default:
                    setMESS(com);
                    next = false;
                    break;
            }
        }
      
    }
    public void paint()
    {
        sprite.Draw(bgi, posBGI, Color.White);
        
        if (visL2)
            sprite.Draw(imgL2, posL2, Color.White);
        if (visR2)
            sprite.Draw(imgR2, posR2, Color.White);
        if (visL1)
            sprite.Draw(imgL1, posL1, Color.White);
        if (visR1)
            sprite.Draw(imgR1, posR1, Color.White);
        sprite.Draw(fram, posFRAME, Color.White);
        
        sprite.DrawString(font, name, posNAME, Color.Black);
      
        sprite.DrawString(font, mozi, posMOZI, Color.Black);
    }
    public void setMESS(string[] com)
    {
        name = com[0];
        mozi = com[1];
    }

    public void setSE(int i)
    {
       //効果音の設定
       //効果音はIDで管理ID=0で停止
        string a = "janp";

        //後で位置を修正
        SoundPlayer player = new SoundPlayer(@"C:\Users\minami\Desktop\XNA\Scene0\Scene0Content\"+a+".wav");
        if (player != null)
            stopSE();

        player.Play();

    }
    public void setBGM(int  i)
    {
        //BGMの設定
        //BGMはIDで管理ID=0で停止
        if (i == -1)
        {
            MediaPlayer.Stop();
        }
        else
        {
            MediaPlayer.Play(song[0]);
        }
    }
   
    public void setBGI(int i)
    {
        //BGIの設定
        //BGIはIDで管理ID=0で黒
        Stream stream = File.OpenRead("bgi" + i + ".png");
        bgi = Texture2D.FromStream(g, stream);

    }
    public void setWARP()
    {
        //ワープの効果
        Stream stream = File.OpenRead("pikaan.png");
        bgi = Texture2D.FromStream(g, stream);
        name = "";
        mozi = "ピカーーーン！！！";
        visL1 = false;
        visL2 = false;
        visR1 = false;
        visR2 = false;
    }
    public void setIN(string[] com)
    {
        if (com[1].Equals("L"))
        {
            var charEM1 = com[2].Split(':');
            visL1 = true;
            Stream stream1 = File.OpenRead(""+charEM1[0] + charEM1[1] + ".png");
            imgL1 = Texture2D.FromStream(g, stream1);
            if (com.Length >3)
            {
                var charEM2 = com[3].Split(':');
                visL2 = true;
                Stream stream2 = File.OpenRead(charEM2[0] + charEM2[1] + ".png");
                imgL2 = Texture2D.FromStream(g, stream2);
            }
        }
        else if (com[1].Equals("R"))
        {
            var charEM1 = com[2].Split(':');
            visR1 = true;
            Stream stream1 = File.OpenRead(charEM1[0] + charEM1[1] + ".png");
            imgR1 = Texture2D.FromStream(g, stream1);
            if (com[3].Length > 3)
            {
                var charEM2 = com[3].Split(':');
                visR2 = true;
                Stream stream2 = File.OpenRead(charEM2[0] + charEM2[1] + ".png");
                imgR2 = Texture2D.FromStream(g, stream2);
            }
        }
    }
    public void setOUT(string[] com) 
    {
        if (com[1].Equals("L")) 
        {
            visL1 = false;
            visL2 = false;
        }
        else if(com[1].Equals("R")) 
        {
            visR1 = false;
            visR2 = false;
        }
        else if (com[1].Equals("ALL"))
        {
            visL1 = false;
            visL2 = false;
            visR1 = false;
            visR2 = false;
        }
    }
    public void StartStage()
    {
        //ステージの開始？
    }
    public string getNext()
    {
        string nextmessage = serif[0];
        serif.RemoveAt(0);
        return nextmessage;
    }
    public string getCommand()
    {
        string nextmessage = serif[0];
        return nextmessage;
    }
    private void stopSE()
    {
        if (player != null)
        {
            player.Stop();
            player.Dispose();
            player = null;
        }
    }

}






