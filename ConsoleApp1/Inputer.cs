using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Inputer
    {
        private Desk desk;
        public Inputer()
        {
            desk = new Desk();
            StartovayaRasstanovka();
        }
        public bool MakeHod(string hod)
        {
            Hod rassifrovaniyHod = RasshifrovkaHoda(hod);
            return desk.MakeHod(rassifrovaniyHod);
        }
        public void RasstanovkaFigur(List<string> whiteStartPositions, List<string> blackStartPositions)
        {
            //Расставляем фигуры на доске в соответствии с изначальной позицией;
            foreach (string whiteFigure in whiteStartPositions)
            {
                Hod informationAboutFigure = RasshifrovkaHoda(whiteFigure);
                Figure figure = new Figure(true, informationAboutFigure.nameOfFigure, informationAboutFigure.endField.hor, informationAboutFigure.endField.vert);
                desk.AddFigure(figure, informationAboutFigure.endField.hor, informationAboutFigure.endField.vert);
            }
            foreach (string blackFigure in blackStartPositions)
            {
                Hod informationAboutFigure = RasshifrovkaHoda(blackFigure);
                Figure figure = new Figure(false, informationAboutFigure.nameOfFigure, informationAboutFigure.endField.hor, informationAboutFigure.endField.vert);
                desk.AddFigure(figure, informationAboutFigure.endField.hor, informationAboutFigure.endField.vert);
            }
        }

        public Hod RasshifrovkaHoda(string hod)
        {
            //Компоненты для возвращаемого значения
            Name nameOfFigure;
            Vector2 startField = new Vector2(8, 8);
            Vector2 endField;
            Name pownBecomeTo = Name.Pown;

            if (hod == "0-0")
            {
                nameOfFigure = Name.King;
                if (desk.nowWhiteAreGoing)
                {
                    startField = new Vector2(0, 4);
                    endField = new Vector2(0, 6);
                }
                else
                {
                    startField = new Vector2(7, 4);
                    endField = new Vector2(7, 6);
                }
                return new Hod(nameOfFigure, startField, endField);
            }
            if (hod == "0-0-0")
            {
                nameOfFigure = Name.King;
                if (desk.nowWhiteAreGoing)
                {
                    startField = new Vector2(0, 4);
                    endField = new Vector2(0, 2);
                }
                else
                {
                    startField = new Vector2(7, 4);
                    endField = new Vector2(7, 2);
                }
                return new Hod(nameOfFigure, startField, endField);
            }
            char[] chars = hod.ToCharArray();

            //Узнаём название фигуры;
            nameOfFigure = FigureByLetter(chars[0], chars[1]);

            //Конечное поле хода;
            char vertChar = chars[0];
            char horChar = chars[0];
            int vertEndField;
            int horEndField;
            if (nameOfFigure != Name.King && nameOfFigure != Name.Pown)
            {
                if (CharIsLetter(chars[2]))
                {
                    if (CharIsLetter(chars[1]))
                    {
                        //Две фигуры на одно поле...Кef7
                        vertChar = chars[2];
                        horChar = chars[3];
                        startField = new Vector2(8, LetterToChislo(chars[1]));
                        //Если [8] горизонталь, то значит две фигуры претендуют на одно полю и Desk придётся искать нужную;
                    }
                    if (CharIsChislo(chars[1]))
                    {
                        //Две фигуры на одно поле...Л1ф5
                        vertChar = chars[2];
                        horChar = chars[3];
                        startField = new Vector2(Convert.ToInt32(chars[1].ToString()) - 1, 8);
                        //Если [8] веротикаль, то значит две фигуры претендуют на одно полю и Desk придётся искать нужную;
                    }
                }
                else
                {
                    vertChar = chars[1];
                    horChar = chars[2];
                }
            }
            else if (nameOfFigure == Name.King)
            {
                vertChar = chars[2];
                horChar = chars[3];
            }
            else
            {
                //Более сложный алгоритм для пешки;
                if (chars.Length == 3)
                {
                    pownBecomeTo = FigureByLetter(chars[2]);
                }
                if (CharIsLetter(chars[1]))
                {
                    //Взятие пешкой...
                    startField = new Vector2(8, LetterToChislo(chars[0]));
                    endField = new Vector2(8, LetterToChislo(chars[1]));
                    return new Hod(nameOfFigure, startField, endField, pownBecomeTo);
                }
                else
                {
                    //Ход пешкой вперёд...
                    vertChar = chars[0];
                    horChar = chars[1];
                }
            }


            horEndField = Convert.ToInt32(horChar.ToString()) - 1;

            //Перевод букв в цифры;
            vertEndField = LetterToChislo(vertChar);

            endField = new Vector2(horEndField, vertEndField);
            Hod output = new Hod(nameOfFigure, startField, endField, pownBecomeTo);
            return output;
        }
        private Name FigureByLetter(char letter1, char letter2 = 'н')
        {
            if (letter1 == 'К' && letter2 == 'р')
                return Name.King;
            else if (letter1 == 'Ф')
                return Name.Queen;
            else if (letter1 == 'Л')
                return Name.Rock;
            else if (letter1 == 'К')
                return Name.Knife;
            else if (letter1 == 'С')
                return Name.Bishop;
            else
                return Name.Pown;
        }
        private bool CharIsLetter(char c)
        {
            if (c == 'a' || c == 'b' || c == 'c' || c == 'd' || c == 'e' || c == 'f' || c == 'g' || c == 'h')
                return true;
            else
                return false;
        }
        private bool CharIsChislo(char c)
        {
            if (c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8')
                return true;
            else
                return false;
        }
        private int LetterToChislo(char c)
        {
            if (c == 'a')
                return 0;
            else if (c == 'b')
                return 1;
            else if (c == 'c')
                return 2;
            else if (c == 'd')
                return 3;
            else if (c == 'e')
                return 4;
            else if (c == 'f')
                return 5;
            else if (c == 'g')
                return 6;
            else
                return 7;
        }
        private void StartovayaRasstanovka()
        {
            List<string> whiteFigures = new List<string>();
            List<string> blackFigures = new List<string>();
            whiteFigures.Add("Лa1");
            whiteFigures.Add("Лh1");
            whiteFigures.Add("Кb1");
            whiteFigures.Add("Кg1");
            whiteFigures.Add("Сc1");
            whiteFigures.Add("Сf1");
            whiteFigures.Add("Фd1");
            whiteFigures.Add("Крe1");

            whiteFigures.Add("a2");
            whiteFigures.Add("b2");
            whiteFigures.Add("c2");
            whiteFigures.Add("d2");
            whiteFigures.Add("e2");
            whiteFigures.Add("f2");
            whiteFigures.Add("g2");
            whiteFigures.Add("h2");


            blackFigures.Add("Лa8");
            blackFigures.Add("Лh8");
            blackFigures.Add("Кb8");
            blackFigures.Add("Кg8");
            blackFigures.Add("Сc8");
            blackFigures.Add("Сf8");
            blackFigures.Add("Фd8");
            blackFigures.Add("Крe8");

            blackFigures.Add("a7");
            blackFigures.Add("b7");
            blackFigures.Add("c7");
            blackFigures.Add("d7");
            blackFigures.Add("e7");
            blackFigures.Add("f7");
            blackFigures.Add("g7");
            blackFigures.Add("h7");


            //whiteFigures.Add("a7");
            //blackFigures.Add("b8");

            RasstanovkaFigur(whiteFigures, blackFigures);
        }
    }
}
