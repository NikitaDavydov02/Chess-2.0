using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Figure
    {
        public bool white { get; private set; }
        public Name name { get; private set; }
        public bool lineinaya { get; private set; }
        public List<Vector2> beatenFields { get; private set; }

        public int hor { get; private set; }

        public int vert { get; private set; }
        public Figure(bool white, Name name, int hor, int vert)
        {
            this.white = white;
            this.name = name;
            this.hor = hor;
            this.vert = vert;
            beatenFields = new List<Vector2>();
            if (name == Name.Bishop)
            {
                lineinaya = true;
                beatenFields.Add(new Vector2(1,1));
                beatenFields.Add(new Vector2(1,-1));
                beatenFields.Add(new Vector2(-1,1));
                beatenFields.Add(new Vector2(-1,-1));
            }
            else if (name == Name.King)
            {
                lineinaya = false;
                beatenFields.Add(new Vector2(1,1));
                beatenFields.Add(new Vector2(1,0));
                beatenFields.Add(new Vector2(1, -1));
                beatenFields.Add(new Vector2(0, 1));
                beatenFields.Add(new Vector2(0, -1));
                beatenFields.Add(new Vector2(-1, 1));
                beatenFields.Add(new Vector2(-1, 0));
                beatenFields.Add(new Vector2(-1,-1));
            }
            else if (name == Name.Knife)
            {
                lineinaya = false;
                beatenFields.Add(new Vector2(1, 2));
                beatenFields.Add(new Vector2(2, 1));
                beatenFields.Add(new Vector2(-1, 2));
                beatenFields.Add(new Vector2(-2, 1));
                beatenFields.Add(new Vector2(-1, -2));
                beatenFields.Add(new Vector2(-2, -1));
                beatenFields.Add(new Vector2(1, -2));
                beatenFields.Add(new Vector2(2, -1));
            }
            else if (name == Name.Pown)
            {
                lineinaya = false;
            }
            else if (name == Name.Queen)
            {
                lineinaya = true;
                beatenFields.Add(new Vector2(1, 1));
                beatenFields.Add(new Vector2(1, -1));
                beatenFields.Add(new Vector2(-1, 1));
                beatenFields.Add(new Vector2(-1, -1));
                beatenFields.Add(new Vector2(0, 1));
                beatenFields.Add(new Vector2(0, -1));
                beatenFields.Add(new Vector2(1, 0));
                beatenFields.Add(new Vector2(-1, 0));
            }
            else
            {
                lineinaya = true;
                beatenFields.Add(new Vector2(0, 1));
                beatenFields.Add(new Vector2(0, -1));
                beatenFields.Add(new Vector2(1, 0));
                beatenFields.Add(new Vector2(-1, 0));
            }
        }

    }
}
