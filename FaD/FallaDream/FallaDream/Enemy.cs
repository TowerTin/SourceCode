using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using System.Media;

namespace FallaDream
{
    class Enemy:Data
    {
        private GraphicsDevice g;
        private SpriteBatch s;
        int HP, ATC, SPD, No, DRP, x, y, stageFlg;
        int[,] map;
        Texture2D imgEnemy;
        Vector2 pos;
       

        public Enemy(SpriteBatch _s, GraphicsDevice _g, int _enemyNo, int mapno)
        {
            s = _s;
            g = _g;
            //map = _map;
          
            HP = enemystat[_enemyNo, 0];
            ATC = enemystat[_enemyNo, 1];
            SPD = enemystat[_enemyNo, 2];
            DRP = enemystat[_enemyNo, 3];
            No = enemystat[_enemyNo, 4];

            FileStream stream = File.OpenRead("images/enemy"+No+".png");
            imgEnemy = Texture2D.FromStream(_g, stream);
          
            
            switch (mapno)
            {
                case 0:
                    pos.X = mapStartpos0[1];
                    pos.Y = mapStartpos0[2];
                    map = mapEnemyRoute0;
                    break;
                case 1:
                    pos.X = mapStartpos1[1];
                    pos.Y = mapStartpos1[2];
                    map = mapEnemyRoute1;
                    break;
                case 2:
                    pos.X = mapStartpos2[1];
                    pos.Y = mapStartpos2[2];
                    x = 0;
                    y = -SPD;
                    map = mapEnemyRoute2;
                    break;
                case 3:
                    pos.X = mapStartpos3[1];
                    pos.Y = mapStartpos3[2];
                    map = mapEnemyRoute3;
                    break;
                case 4:
                    pos.X = mapStartpos4[1];
                    pos.Y = mapStartpos4[2];
                    map = mapEnemyRoute4;
                    break;
                case 5:
                    pos.X = mapStartpos5[1];
                    pos.Y = mapStartpos5[2];
                    map = mapEnemyRoute5;
                    break;
                case 6:
                    pos.X = mapStartpos6[1];
                    pos.Y = mapStartpos6[2];
                    map = mapEnemyRoute6;
                    break;
                case 7:
                    pos.X = mapStartpos7[1];
                    pos.Y = mapStartpos7[2];
                    map = mapEnemyRoute7;
                    break;
            }
            
        }

        public int getHP()
        {
            return HP;
        }
        public int getATC()
        {
            return ATC;
        }
        public int getSPD()
        {
            return SPD;
        }
        public int getDRP()
        {
            return DRP;
        }
        public int getNo()
        {
            return No;
        }

        public bool damageHP(int _dmg)
        {
            HP -= _dmg;

            if (HP <= 0) 
            {
                return true;
            }
            return false;
 
        }
        public void damageSPD()
        {
            if (SPD > 1)
            {
                SPD -= 1;
            }

        }
        public bool enemymove()
        {
            int ax = ((int)pos.Y / 40);
            int ay = ((int)pos.X / 40);
            int lv = map[ax, ay];
          
            if (lv == 0)
            {
                x = 0;
                y = 0;
                return true;
            }
            if (ax > 0)
            {
                if (lv > map[ax, ay + 1])
                {
                    x = SPD;
                    y = 0;
                    pos.X += x;
                    pos.Y += y;
                    return false;
                }
                if (lv > map[ax + 1, ay])
                {
                    x = 0;
                    y = SPD;
                    pos.X += x;
                    pos.Y += y;
                    return false;
                }
            }
            if (ay > 0 && ax >0)
            {
                if (lv > map[ax, ay - 1])
                {
                    x = -SPD;
                    y = 0;
                    pos.X += x;
                    pos.Y += y;
                    return false;
                }
                if (lv > map[ax - 1, ay])
                {
                    x = 0;
                    y = -SPD;
                    pos.X += x;
                    pos.Y += y;
                    return false;
                }
            }

            return false;
        }
        public void paint()
        {
            //if (enemymove())
            //{
               
                s.Draw(imgEnemy, pos, Color.White);
            //}
 
        }
        public float getX()
        {
            return pos.X;
        }
        public float getY()
        {
            return pos.Y;
        }
    }
}
