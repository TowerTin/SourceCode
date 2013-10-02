using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using System.Media;

namespace FallaDream
{
    class Event
    {
        private SpriteFont font;
        private int no;
        private List<string> serif;
        private SpriteBatch s;
        private GraphicsDevice g;
        private Texture2D imgBGI,imgLeft0,imgLeft1,imgRight0,imgRight1,imgFrame;
        private Vector2 posBGI, posLeft0, posLeft1, posRight0, posRight1, posFrame,posName,posTalk;
        private bool leftFlg0, leftFlg1, rightFlg0, rightFlg1, mouseFlg = true,endFlg = true;
        private string name="", talk="";
        private bool backFlg0= true, backFlg1= true;
        
        public Event(SpriteBatch _s, GraphicsDevice _g,SpriteFont _font)
        {
            s = _s;
            g = _g;
            font = _font;

            posBGI.X = 0;
            posBGI.Y = 0;
            posLeft0.X = 0;
            posLeft0.Y = 10;
            posLeft1.X = 60;
            posLeft1.Y = 10;
            posRight0.X = 480;
            posRight0.Y = 10;
            posRight1.X = 520;
            posRight1.Y = 10;
            posFrame.X = 10;
            posFrame.Y = 340;
            posName.X = 100;
            posName.Y = 355;
            posTalk.X = 50;
            posTalk.Y = 400;

            serif = new List<string>();
            Stream stream;
            stream = File.OpenRead("Content/images/hb1.png");
            imgBGI = Texture2D.FromStream(g, stream);

            stream = File.OpenRead("Content/images/sowaku.png");
             imgFrame = Texture2D.FromStream(g, stream);
            
        }
        public void setEvent(int _no) 
        {
            Stream stream;
            stream = File.OpenRead("Content/images/hb1.png");
            imgBGI = Texture2D.FromStream(g, stream);

            stream = File.OpenRead("Content/images/sowaku.png");
            imgFrame = Texture2D.FromStream(g, stream);

            serif.Clear();
            

            System.IO.StreamReader cReader = (
           new System.IO.StreamReader("Content/story/FaDstage" + _no + ".txt", System.Text.Encoding.Default)
       );

            // 読み込んだ結果をすべて格納するための変数を宣言する
            string stResult = string.Empty;
            string stBuffer = string.Empty;

            // 読み込みできる文字がなくなるまで繰り返す
            while (cReader.Peek() >= 0)
            {
                // ファイルを 1 行ずつ読み込む
                 stBuffer = cReader.ReadLine();
                // 読み込んだものを追加で格納する
                stResult += stBuffer + System.Environment.NewLine;
                serif.Add(stBuffer);
            }

            // cReader を閉じる (正しくは オブジェクトの破棄を保証する を参照)
            cReader.Close();
        }

        public void setLise()
        {
            name = "";
            talk = "";
            leftFlg0 = false;
            leftFlg1 = false;
            rightFlg0 = false;
            rightFlg1 = false;
 
        }

        public void drow()
        {
            s.Draw(imgBGI, posBGI, Color.White);
            if (backFlg0)
            {
                if (leftFlg0)
                    s.Draw(imgLeft0, posLeft0, Color.White);
                if (leftFlg1)
                    s.Draw(imgLeft1, posLeft1, Color.White);
            }
            else 
            {
                if (leftFlg1)
                    s.Draw(imgLeft1, posLeft1, Color.White);
                if (leftFlg0)
                    s.Draw(imgLeft0, posLeft0, Color.White);
                
            }
            if (backFlg1)
            {
                if (rightFlg0)
                    s.Draw(imgRight0, posRight0, Color.White);
                if (rightFlg1)
                    s.Draw(imgRight1, posRight1, Color.White);
            }
            else
            {  
                if (rightFlg1)
                    s.Draw(imgRight1, posRight1, Color.White);
                if (rightFlg0)
                    s.Draw(imgRight0, posRight0, Color.White);
              
            }


            s.Draw(imgFrame, posFrame, Color.White);

            s.DrawString(font, name, posName, Color.Black);

            s.DrawString(font, talk, posTalk, Color.Black);
        }
        public bool  MousePressChk()
        {
            if (mouseFlg)
            {
                if (endFlg)
                {
                    getEVENT();
                    mouseFlg = false;
                }
                else 
                {
                    return true;
                }
            }
            return false;
        }

        public void MouseReleasChk()
        {
            mouseFlg = true;
           
        }

        public void getEVENT()
        {
            string command;

            bool next = true;

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
                        next = false;
                        setWARP();
                        break;

                    case "BGI":
                        //setBGI(int.Parse(com[1]));
                        setBGI(0);
                        break;

                    case "BGM":
                        //setBGM(int.Parse(com[1]));
                        setBGM(0);
                        break;

                    case "SE":
                        //setSE(int.Parse(com[1]));
                        setSE(0);
                        break;
                    case "STOP":
                        next = false;
                        break;
                    case "END":
                        //serif.RemoveAt(0);
                        next = false;
                        endFlg = false;
                        break;
                    case"BACK":
                        setBACK(com);
                        break;
                    default:
                        setMESS(com);
                        next = false;
                        break;
                }
            }

        }
        public void setBACK(string[] com)
        {
            if (com[1] == "L") 
            {
                backFlg0 = !backFlg0;
            }
            else if (com[1] == "R")
            {
                backFlg1 = !backFlg1;
            }
 

        }



       
        public void setMESS(string[] com)
        {
            name = com[0];
            if (com.Length < 3)
            {
                talk = com[1];
            }
            else 
            {
                talk = com[1] + "\r\n" + com[2];
            }
        }

        public void setSE(int i)
        {
            //効果音の設定
            //効果音はIDで管理ID=0で停止
            //string a = "janp";

            //後で位置を修正
            //SoundPlayer player = new SoundPlayer(@"C:\Users\minami\Desktop\XNA\Scene0\Scene0Content\" + a + ".wav");
            //if (player != null)
            //    stopSE();

            //player.Play();

        }
        public void setBGM(int i)
        {
            //BGMの設定
            //BGMはIDで管理ID=0で停止
            if (i == -1)
            {
                MediaPlayer.Stop();
            }
            else
            {
                //MediaPlayer.Play(song[0]);
            }
        }

        public void setBGI(int i)
        {
            //BGIの設定
            //BGIはIDで管理ID=0で黒
            //Stream stream = File.OpenRead("bgi" + i + ".png");
            Stream stream = File.OpenRead("Content/images/hb1.png");
            imgBGI = Texture2D.FromStream(g, stream);

        }
        public void setWARP()
        {
            //ワープの効果
            Stream stream = File.OpenRead("Content/images/pikaan.png");
            imgBGI = Texture2D.FromStream(g, stream);
            name = "";
            talk = "ピカーーーン！！！";
            
            leftFlg0 = false;
            leftFlg1 = false;
            rightFlg0 = false;
            rightFlg1 = false;
        }
        public void setIN(string[] com)
        {
            if (com[1].Equals("L"))
            {
                var charEM1 = com[2].Split(':');
                leftFlg0 = true;
                Stream stream1 = File.OpenRead("Content/images/" + com[2] + ".png");
                //Stream stream1 = File.OpenRead("images/Izumi1.png");
                imgLeft0 = Texture2D.FromStream(g, stream1);
                if (com.Length > 3)
                {
                    var charEM2 = com[3].Split(':');
                    leftFlg1 = true;
                    Stream stream2 = File.OpenRead("Content/images/" + com[3] + ".png");
                    //Stream stream2 = File.OpenRead("images/Izumi0.png");
                    imgLeft1 = Texture2D.FromStream(g, stream2);
                }
            }
            else if (com[1].Equals("R"))
            {
                var charEM1 = com[2].Split(':');
                rightFlg0 = true;
                Stream stream1 = File.OpenRead("Content/images/" + com[2] + ".png");
               // Stream stream1 = File.OpenRead("images/Lucy0.png");
                imgRight0 = Texture2D.FromStream(g, stream1);
                if (com.Length > 3)
                {
                    var charEM2 = com[3].Split(':');
                    rightFlg1 = true;
                    // Stream stream2 = File.OpenRead(charEM2[0] + charEM2[1] + ".png");
                    Stream stream2 = File.OpenRead("Content/images/" + com[3] + ".png");
                    imgRight1 = Texture2D.FromStream(g, stream2);
                }
            }
        }
        public void setOUT(string[] com)
        {
            if (com[1].Equals("L"))
            {
                leftFlg0 = false;
                leftFlg1 = false;
            }
            else if (com[1].Equals("R"))
            {
                rightFlg0 = false;
                rightFlg1 = false;
            }
            else if (com[1].Equals("ALL"))
            {
                leftFlg0 = false;
                leftFlg1 = false;
                rightFlg0 = false;
                rightFlg1 = false;
            }
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
            //if (player != null)
            //{
            //    player.Stop();
            //    player.Dispose();
            //    player = null;
            //}
        }
        public void END()
        {
            endFlg = false;
            
        }
        
    }
}