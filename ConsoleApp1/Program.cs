using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите белую фигуру в начальной позиции");
            //string figure = Console.ReadLine();
            //List<string> whiteFigurePositions = new List<string>();
            //whiteFigurePositions.Add(figure);
            //List<string> blackFigurePositions = new List<string>();
            Inputer inputer = new Inputer();
            bool gameIsCountining = true;
            while (gameIsCountining)
            {
                Console.WriteLine("Введите ход...");
                string hod = Console.ReadLine();
                gameIsCountining = inputer.MakeHod(hod);
            }
        }
        
    }
    enum Name
    {
        Pown,
        Rock,
        Knife,
        Bishop,
        Queen,
        King,
    }
    struct Vector2
    {
        public int hor { get; private set; }
        public int vert { get; private set; }
        public Vector2(int hor, int vert)
        {
            this.hor = hor;
            this.vert = vert;
        }
    }
    struct Hod
    {
        public Name nameOfFigure { get; private set; }
        public Vector2 startField { get; private set; }
        public Vector2 endField { get; private set; }
        public Name pownBecomeTo { get; private set; }
        public Hod(Name nameOfFigure, Vector2 startField, Vector2 endField, Name pownBecomeTo=Name.Pown)
        {
            this.nameOfFigure = nameOfFigure;
            this.startField = startField;
            this.endField = endField;
            this.pownBecomeTo = pownBecomeTo;
        }
    }
}
