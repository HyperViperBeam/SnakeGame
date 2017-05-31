using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SnakeGame
{
    public class Snake
    {
        List<Vector2> snakeList = new List<Vector2>();
        List<Color> partColor = new List<Color>();
        public int Facing = 2; // 0= Down, 1= right, 2= up, 3= left \\
        public int Count = 0;
        public Boolean isAlive = true;
        public int DrawSize = 21;
        Random rnd = new Random();
        int speed = 0;
        public SoundEffect effect;
        

        public Snake(int x, int y, int size, List<Color> colorScheme)
        {

            partColor = colorScheme;
            speed = size;
            int temp = size;
            snakeList.Add(new Vector2(x, y));
            snakeList.Add(new Vector2(x, y + size));
            size += temp; ;
            snakeList.Add(new Vector2(x, y + size));
            size += temp; ;
            snakeList.Add(new Vector2(x, y + size));
            size += temp; ;
            snakeList.Add(new Vector2(x, y + size));
            Count = 5;
        }

        public void AddBlock()
        {
            snakeList.Add(new Vector2(snakeList[snakeList.Count - 1].X, snakeList[snakeList.Count - 1].Y));
            Count++;
        }
        public void Update()
        {
            if (isAlive)
            {

                Count = snakeList.Count;

                for (int i = snakeList.Count - 1; i > 0; i--)
                {
                    snakeList[i] = snakeList[i - 1];

                }

                if (Facing == 0)
                    snakeList[0] = new Vector2(snakeList[0].X, (snakeList[0].Y) + speed);
                if (Facing == 1)
                    snakeList[0] = new Vector2((snakeList[0].X + speed), (snakeList[0].Y));
                if (Facing == 2)
                    snakeList[0] = new Vector2(snakeList[0].X, (snakeList[0].Y) - speed);
                if (Facing == 3)
                    snakeList[0] = new Vector2((snakeList[0].X - speed), (snakeList[0].Y));
            }
            else
            {
                snakeList.Clear();
                int temp2 = speed;
                int temp = temp2;
                snakeList.Add(new Vector2(400, 200));
                snakeList.Add(new Vector2(400, 200 + temp2));
                temp2 += temp; ;
                snakeList.Add(new Vector2(400, 200 + temp2));
                temp2 += temp; ;
                snakeList.Add(new Vector2(400, 200 + temp2));
                temp2 += temp; ;
                snakeList.Add(new Vector2(400, 200 + temp2));
                isAlive = true;
                Facing = 0;
                Count = 5;


            }

        }
        //Single Enemy Snake
        public Boolean CheckCollisions(GameWindow wind)
        {
            Rectangle Front = new Rectangle((int)snakeList[0].X, (int)snakeList[0].Y, 20, 20);
            for (int i = 1; i < snakeList.Count; i++)
            {
                Rectangle Body = new Rectangle((int)snakeList[i].X, (int)snakeList[i].Y, 20, 20);
                if (Front.Intersects(Body)) return true;

            }

            int X = (int)snakeList[0].X;
            int Y = (int)snakeList[0].Y;

            if (X > wind.ClientBounds.Width) snakeList[0] = new Vector2(0, snakeList[0].Y);
            if (X < 0) snakeList[0] = new Vector2(wind.ClientBounds.Width, snakeList[0].Y);
            if (Y > wind.ClientBounds.Height) snakeList[0] = new Vector2(snakeList[0].X, 0);
            if (Y < 0) snakeList[0] = new Vector2(snakeList[0].X, wind.ClientBounds.Height);



            return false;
        }

        public Rectangle getFront()
        {
            Rectangle Front = new Rectangle((int)snakeList[0].X, (int)snakeList[0].Y, 20, 20);
            return Front;
        }
        public Boolean DidEatPellet(Rectangle Pellet)
        {

            Rectangle Front = new Rectangle((int)snakeList[0].X, (int)snakeList[0].Y, 20, 20);
            if (Pellet.Intersects(Front))
            {
                AddBlock();
                effect.Play();
                return true;
            }
            else return false;

    }
    
        public Color getDrawColor(int part)
        {
            if (part >= partColor.Count())
                return partColor[partColor.Count() - 1];
            return partColor[part];
        }
        public void SetSpeed(int NewSpeed)
        {
            speed = NewSpeed;
        }
        public Vector2 GetInstance(int X)
        {
            return snakeList[X];
        }

    }
}