using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Desk
    {
        private Figure[,] desk;
        public bool nowWhiteAreGoing { get; private set; }
        private List<Figure> whiteFigures;
        private List<Figure> blackFigures;
        private Figure whiteKing;
        private Figure blackKing;
        private Figure pownWhichCanBeTakenOnProhod;

        public Desk()
        {
            nowWhiteAreGoing = true;
            desk = new Figure[8, 8];
            for (int hor = 0; hor < 8; hor++)
                for (int vert = 0; vert < 8; vert++)
                    desk[hor, vert] = null;
            whiteFigures = new List<Figure>();
            blackFigures = new List<Figure>();
            pownWhichCanBeTakenOnProhod = null;
            //RasstanovkaFigur(whiteStartPositions, blackStartPositions);
            //StartovayaRasstanovka();
        }
        public bool MakeHod(Hod informationAboutHod)
        {
            Hod fullInformationAboutHod = SearchFigure(informationAboutHod);
            if (fullInformationAboutHod.startField.hor == 8 || fullInformationAboutHod.startField.vert == 8 || fullInformationAboutHod.endField.hor == 8 || fullInformationAboutHod.endField.hor == 8)
                return false;
            if (fullInformationAboutHod.nameOfFigure != Name.Knife)
            {
                if (CheckIfEstPregrady(fullInformationAboutHod.startField, fullInformationAboutHod.endField))
                    return false; //Ход физически невозможен;
            }
            //Пробуем сделать ход
            Figure endFieldFigure = null;
            if (desk[fullInformationAboutHod.endField.hor, fullInformationAboutHod.endField.vert] != null)
                endFieldFigure = desk[fullInformationAboutHod.endField.hor, fullInformationAboutHod.endField.vert];
            desk[fullInformationAboutHod.endField.hor, fullInformationAboutHod.endField.vert] = desk[fullInformationAboutHod.startField.hor, fullInformationAboutHod.startField.vert];
            desk[fullInformationAboutHod.startField.hor, fullInformationAboutHod.startField.vert] = null;

            //Проверяем правила;
            if (nowWhiteAreGoing)
            {
                //Не под шахом ли король?
                if (PoleBito(false, whiteKing.hor, whiteKing.vert))
                {
                    ComeBackHod(endFieldFigure, fullInformationAboutHod);
                    return false;
                }
                //Рокеровка не через шах
                if(fullInformationAboutHod.nameOfFigure==Name.King && fullInformationAboutHod.startField.hor==0 && fullInformationAboutHod.startField.vert==4 && fullInformationAboutHod.endField.hor == 0 && fullInformationAboutHod.endField.vert == 6)
                {
                    if(PoleBito(false,0,5)|| PoleBito(false, 0, 4))
                    {
                        ComeBackHod(endFieldFigure, fullInformationAboutHod);
                        return false;
                    }
                }
            }
            else
            {
                if (PoleBito(true, blackKing.hor, blackKing.vert))
                {
                    ComeBackHod(endFieldFigure, fullInformationAboutHod);
                    return false;
                }
                if (fullInformationAboutHod.nameOfFigure == Name.King && fullInformationAboutHod.startField.hor == 7 && fullInformationAboutHod.startField.vert == 4 && fullInformationAboutHod.endField.hor == 7 && fullInformationAboutHod.endField.vert == 6)
                {
                    if (PoleBito(true, 7, 5) || PoleBito(true, 7, 4))
                    {
                        ComeBackHod(endFieldFigure, fullInformationAboutHod);
                        return false;
                    }
                }
            }

            //Ходящая сторона поставила мат?
            if (CheckMateFor(!nowWhiteAreGoing))
            {
                Console.WriteLine("Мат!");
            }
            else
            {
                nowWhiteAreGoing = !nowWhiteAreGoing;
            }



            return true;
        }

        private bool CheckMateFor(bool white)
        {
            return false;
        }
        private void ComeBackHod(Figure endFieldFigure, Hod fullInformationAboutHod)
        {
            desk[fullInformationAboutHod.startField.hor, fullInformationAboutHod.startField.vert] = desk[fullInformationAboutHod.endField.hor, fullInformationAboutHod.endField.vert];
            if (endFieldFigure != null)
                desk[fullInformationAboutHod.endField.hor, fullInformationAboutHod.endField.vert] = endFieldFigure;
            else
                desk[fullInformationAboutHod.endField.hor, fullInformationAboutHod.endField.vert] = null;
        }
        private bool PoleBito(bool whiteBiutChernih, int hor, int vert)
        {
            //Этот метод учитывает и оппозицию королей;
            //Будем проверять диагонали (на слонов, ферзей и королей)
            List<Vector2> checkLinesVectors = new List<Vector2>();
            checkLinesVectors.Add(new Vector2(1, 1));
            checkLinesVectors.Add(new Vector2(1, -1));
            checkLinesVectors.Add(new Vector2(-1, 1));
            checkLinesVectors.Add(new Vector2(-1, -1));
            foreach(Vector2 vector in checkLinesVectors)
            {
                for(int k = 1; k <= 7; k++)
                {
                    int checkHor = hor + vector.hor * k;
                    int checkVert = vert + vector.vert * k;
                    if(checkHor>=0 && checkHor<=7 && checkVert >= 0 && checkVert <= 7)
                    {
                        if(desk[checkHor, checkVert] != null)
                        {
                            if(desk[checkHor, checkVert].name == Name.Bishop || desk[checkHor, checkVert].name == Name.Queen || (desk[checkHor, checkVert].name == Name.King && k == 1))
                            {
                                if ((whiteBiutChernih && desk[checkHor, checkVert].white) || (!whiteBiutChernih && !desk[checkHor, checkVert].white))
                                    return true;
                            }
                        }
                    }
                }
            }
            checkLinesVectors.Clear();
            //Теперь проверяем вертикали на ладей, королей и ферзей
            checkLinesVectors.Add(new Vector2(0, 1));
            checkLinesVectors.Add(new Vector2(0, -1));
            checkLinesVectors.Add(new Vector2(1, 0));
            checkLinesVectors.Add(new Vector2(-1, 0));
            foreach (Vector2 vector in checkLinesVectors)
            {
                for (int k = 1; k <= 7; k++)
                {
                    int checkHor = hor + vector.hor * k;
                    int checkVert = vert + vector.vert * k;
                    if (checkHor >= 0 && checkHor <= 7 && checkVert >= 0 && checkVert <= 7)
                    {
                        if (desk[checkHor, checkVert] != null)
                        {
                            if (desk[checkHor, checkVert].name == Name.Rock || desk[checkHor, checkVert].name == Name.Queen || (desk[checkHor, checkVert].name == Name.King && k == 1))
                            {
                                if ((whiteBiutChernih && desk[checkHor, checkVert].white) || (!whiteBiutChernih && !desk[checkHor, checkVert].white))
                                    return true;
                            }
                        }
                    }
                }
            }
            checkLinesVectors.Clear();
            //Теперь проверяем коней;
            checkLinesVectors.Add(new Vector2(1, 2));
            checkLinesVectors.Add(new Vector2(2, 1));
            checkLinesVectors.Add(new Vector2(-1, 2));
            checkLinesVectors.Add(new Vector2(-2, 1));
            checkLinesVectors.Add(new Vector2(-1, -2));
            checkLinesVectors.Add(new Vector2(-2, -1));
            checkLinesVectors.Add(new Vector2(1, -2));
            checkLinesVectors.Add(new Vector2(2, -1));
            foreach (Vector2 vector in checkLinesVectors)
            {
                int checkHor = hor + vector.hor;
                int checkVert = vert + vector.vert;
                if (checkHor >= 0 && checkHor <= 7 && checkVert >= 0 && checkVert <= 7)
                {
                    if (desk[checkHor, checkVert] != null)
                    {
                        if (desk[checkHor, checkVert].name == Name.King)
                        {
                            if ((whiteBiutChernih && desk[checkHor, checkVert].white) || (!whiteBiutChernih && !desk[checkHor, checkVert].white))
                                return true;
                        }
                    }
                }
            }
            //Теперь проверяем пешек;
            if (whiteBiutChernih)
            {
                if(hor!= 0)
                {
                    if (desk[hor - 1, vert + 1] != null)
                        if (desk[hor - 1, vert + 1].name == Name.Pown && desk[hor - 1, vert + 1].white)
                            return true;
                    if (desk[hor - 1, vert - 1] != null)
                        if (desk[hor - 1, vert - 1].name == Name.Pown && desk[hor - 1, vert - 1].white)
                            return true;
                }
            }
            else
            {
                if (hor != 7)
                {
                    if (desk[hor + 1, vert + 1] != null)
                        if (desk[hor + 1, vert + 1].name == Name.Pown && desk[hor + 1, vert + 1].white)
                            return true;
                    if (desk[hor + 1, vert - 1] != null)
                        if (desk[hor + 1, vert - 1].name == Name.Pown && desk[hor + 1, vert - 1].white)
                            return true;
                }
            }
            return false;
        }
        private Hod SearchFigure(Hod informationAboutHod)
        {
            //Метод должен уже полностью сформировать ВСЮ информацию о ходе
            if (nowWhiteAreGoing)
            {
                //Рокировка?
                //startField = new Vector2(0, 4);
                //endField = new Vector2(0, 6);
                if (informationAboutHod.nameOfFigure==Name.King&&informationAboutHod.startField.hor==0&&informationAboutHod.startField.vert==4&& informationAboutHod.endField.hor == 0 && (informationAboutHod.endField.vert == 6|| informationAboutHod.endField.vert == 2))
                {
                    return informationAboutHod;
                }
                //Ищем пешку?
                if (informationAboutHod.nameOfFigure == Name.Pown)
                {
                    //Конечное поле известно?
                    if(informationAboutHod.endField.hor!=8&& informationAboutHod.endField.vert != 8)
                    {
                        //Проверяем поле на один назад
                        if(desk[informationAboutHod.endField.hor-1, informationAboutHod.endField.vert] != null)
                        {
                            //Проверяем мы нашли белую пешку?
                            if (desk[informationAboutHod.endField.hor - 1, informationAboutHod.endField.vert].name == Name.Pown && desk[informationAboutHod.endField.hor - 1, informationAboutHod.endField.vert].white)
                                //Возвращаем белую пешку
                                return new Hod(Name.Pown, new Vector2(informationAboutHod.endField.hor - 1, informationAboutHod.endField.vert), informationAboutHod.endField, informationAboutHod.pownBecomeTo);
                        }
                        //Проверяем поле на два назад
                        if(desk[informationAboutHod.endField.hor - 2, informationAboutHod.endField.vert] != null)
                        {
                            //Проверяем мы нашли белую пешку?
                            if (desk[informationAboutHod.endField.hor - 2, informationAboutHod.endField.vert].name == Name.Pown && desk[informationAboutHod.endField.hor - 2, informationAboutHod.endField.vert].white)
                                //Возвращаем белую пешку
                                return new Hod(Name.Pown, new Vector2(informationAboutHod.endField.hor - 2, informationAboutHod.endField.vert), informationAboutHod.endField, informationAboutHod.pownBecomeTo);
                        }
                    }
                    else
                    {
                        //Конечное поле неизвестно, взятие
                        //Прочёсываем всю начальную вертикаль
                        for(int hor = 0; hor < 7; hor++)
                        {
                            if(desk[hor, informationAboutHod.startField.vert] != null)
                            {
                                //Убеждаемся, что пешка белая
                                if (desk[hor, informationAboutHod.startField.vert].white)
                                {
                                    //Убеждаемся, что эта пешка может бить
                                    if (desk[hor + 1, informationAboutHod.endField.vert] != null)
                                        if (!desk[hor + 1, informationAboutHod.endField.vert].white)
                                            return new Hod(Name.Pown, new Vector2(hor, informationAboutHod.startField.vert), new Vector2(hor + 1, informationAboutHod.endField.vert), informationAboutHod.pownBecomeTo);
                                    //Или если пешка может произвести взятие на проходе;
                                    if(desk[hor, informationAboutHod.endField.vert]==pownWhichCanBeTakenOnProhod && pownWhichCanBeTakenOnProhod!=null)
                                        return new Hod(Name.Pown, new Vector2(hor, informationAboutHod.startField.vert), new Vector2(hor + 1, informationAboutHod.endField.vert), informationAboutHod.pownBecomeTo);
                                }
                            }
                        }
                    }
                }
                else
                {
                    //Ищем не пешку;
                    //Создаём тестовую фигуру;
                    Figure testFigure = new Figure(true, informationAboutHod.nameOfFigure, informationAboutHod.endField.hor, informationAboutHod.endField.vert);
                    //Смотрим, куда она может ходить
                    List<Vector2> beatenFields = testFigure.beatenFields;

                    int maxK = 1;
                    //Фигура линейная?
                    if (testFigure.lineinaya)
                        maxK = 7;
                    //Домножаем на коэффициент...
                    for (int k = 1; k <= maxK; k++)
                    {
                        //Каждое направление хордов фигуры...
                        foreach (Vector2 edinichnieVector in beatenFields)
                        {
                            int hor = k * edinichnieVector.hor + informationAboutHod.endField.hor;
                            int vert = k * edinichnieVector.vert + informationAboutHod.endField.vert;
                            //И проверяем, укладываемя ли в доску.
                            if (hor >= 0 && hor <= 7 && vert >= 0 && vert <= 7)
                            {
                                //На этом поле стоит фигура?
                                if (desk[hor, vert] != null)
                                {
                                    //Фигура подходит;
                                    if (desk[hor, vert].name == informationAboutHod.nameOfFigure && desk[hor, vert].white)
                                    {
                                        //Если начальное поле ytизвестно, то сразу возвращаем фигуру
                                        if (informationAboutHod.startField.hor == 8 && informationAboutHod.startField.vert == 8)
                                            return new Hod(informationAboutHod.nameOfFigure, new Vector2(hor, vert), informationAboutHod.endField, informationAboutHod.pownBecomeTo);
                                        else if (informationAboutHod.startField.hor == 8)
                                        {
                                            //Неизвеста горизонталь начального поля, но известна вертикаль
                                            //Проверяем стоит ли претендент на этой вертикали и тогда возвращаем его
                                            if (vert == informationAboutHod.startField.vert)
                                                return new Hod(informationAboutHod.nameOfFigure, new Vector2(hor, vert), informationAboutHod.endField, informationAboutHod.pownBecomeTo);
                                        }
                                        else if (informationAboutHod.startField.vert == 8)
                                        {
                                            //Неизвеста вертикаль начального поля, но известна горизонталь
                                            //Проверяем стоит ли претендент на этой горизонтали и тогда возвращаем его
                                            if (hor == informationAboutHod.startField.hor)
                                                return new Hod(informationAboutHod.nameOfFigure, new Vector2(hor, vert), informationAboutHod.endField, informationAboutHod.pownBecomeTo);
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            else
            {
                //Ходят чёрные, но всё почти также, как у белых...
                //Рокировка?
                //startField = new Vector2(7, 4);
                //endField = new Vector2(7, 6);
                if (informationAboutHod.nameOfFigure == Name.King && informationAboutHod.startField.hor == 7 && informationAboutHod.startField.vert == 4 && informationAboutHod.endField.hor == 7 && (informationAboutHod.endField.vert == 6 || informationAboutHod.endField.vert == 2))
                {
                    return informationAboutHod;
                }

                List<Figure> pretendenty = new List<Figure>();
                //Ищем пешку?
                if (informationAboutHod.nameOfFigure == Name.Pown)
                {
                    //Конечное поле известно?
                    if (informationAboutHod.endField.hor != 8 && informationAboutHod.endField.vert != 8)
                    {
                        //Проверяем поле на один назад
                        if (desk[informationAboutHod.endField.hor + 1, informationAboutHod.endField.vert] != null)
                        {
                            //Проверяем мы нашли чёрную пешку?
                            if (desk[informationAboutHod.endField.hor + 1, informationAboutHod.endField.vert].name == Name.Pown && !desk[informationAboutHod.endField.hor + 1, informationAboutHod.endField.vert].white)
                                //Возвращаем чёрную пешку
                                return new Hod(Name.Pown, new Vector2(informationAboutHod.endField.hor + 1, informationAboutHod.endField.vert), informationAboutHod.endField, informationAboutHod.pownBecomeTo);
                        }
                        //Проверяем поле на два назад
                        if (desk[informationAboutHod.endField.hor + 2, informationAboutHod.endField.vert] != null)
                        {
                            //Проверяем мы нашли чёрную пешку?
                            if (desk[informationAboutHod.endField.hor + 2, informationAboutHod.endField.vert].name == Name.Pown && !desk[informationAboutHod.endField.hor + 2, informationAboutHod.endField.vert].white)
                                //Возвращаем чёрную пешку
                                return new Hod(Name.Pown, new Vector2(informationAboutHod.endField.hor + 2, informationAboutHod.endField.vert), informationAboutHod.endField, informationAboutHod.pownBecomeTo);
                        }
                    }
                    else
                    {
                        //Конечное поле неизвестно, взятие
                        //Прочёсываем всю начальную вертикаль
                        for (int hor = 0; hor < 7; hor++)
                        {
                            if (desk[hor, informationAboutHod.startField.vert] != null)
                            {
                                //Убеждаемся, что пешка чёрная
                                if (!desk[hor, informationAboutHod.startField.vert].white)
                                {
                                    //Убеждаемся, что эта пешка может бить
                                    if (desk[hor - 1, informationAboutHod.endField.vert] != null)
                                        if (desk[hor - 1, informationAboutHod.endField.vert].white)
                                            return new Hod(Name.Pown, new Vector2(hor, informationAboutHod.startField.vert), new Vector2(hor - 1, informationAboutHod.endField.vert), informationAboutHod.pownBecomeTo);
                                    //Или если пешка может произвести взятие на проходе;
                                    if (desk[hor, informationAboutHod.endField.vert] == pownWhichCanBeTakenOnProhod && pownWhichCanBeTakenOnProhod != null)
                                        return new Hod(Name.Pown, new Vector2(hor, informationAboutHod.startField.vert), new Vector2(hor - 1, informationAboutHod.endField.vert), informationAboutHod.pownBecomeTo);
                                }
                            }
                        }
                    }
                }
                else
                {
                    //Ищем не пешку;
                    //Создаём тестовую фигуру;
                    Figure testFigure = new Figure(true, informationAboutHod.nameOfFigure, informationAboutHod.endField.hor, informationAboutHod.endField.vert);
                    //Смотрим, куда она может ходить
                    List<Vector2> beatenFields = testFigure.beatenFields;

                    int maxK = 1;
                    //Фигура линейная?
                    if (testFigure.lineinaya)
                        maxK = 7;
                    //Домножаем на коэффициент...
                    for (int k = 1; k <= maxK; k++)
                    {
                        //Каждое направление хордов фигуры...
                        foreach (Vector2 edinichnieVector in beatenFields)
                        {
                            int hor = k * edinichnieVector.hor + informationAboutHod.endField.hor;
                            int vert = k * edinichnieVector.vert + informationAboutHod.endField.vert;
                            //И проверяем, укладываемя ли в доску.
                            if (hor >= 0 && hor <= 7 && vert >= 0 && vert <= 7)
                            {
                                //На этом поле стоит фигура?
                                if (desk[hor, vert] != null)
                                {
                                    //Фигура подходит по названию и цвету?
                                    if (desk[hor, vert].name == informationAboutHod.nameOfFigure && !desk[hor, vert].white)
                                    {
                                        //Если конечное поле известно, то сразу возвращаем фигуру
                                        if (informationAboutHod.startField.hor != 8 && informationAboutHod.startField.vert != 8)
                                            return new Hod(informationAboutHod.nameOfFigure, new Vector2(hor, vert), informationAboutHod.endField, informationAboutHod.pownBecomeTo);
                                        else if (informationAboutHod.startField.hor == 8)
                                        {
                                            //Неизвеста горизонталь начального поля, но известна вертикаль
                                            //Проверяем стоит ли претендент на этой вертикали и тогда возвращаем его
                                            if (vert == informationAboutHod.startField.vert)
                                                return new Hod(informationAboutHod.nameOfFigure, new Vector2(hor, vert), informationAboutHod.endField, informationAboutHod.pownBecomeTo);
                                        }
                                        else if (informationAboutHod.startField.vert == 8)
                                        {
                                            //Неизвеста вертикаль начального поля, но известна горизонталь
                                            //Проверяем стоит ли претендент на этой горизонтали и тогда возвращаем его
                                            if (hor == informationAboutHod.startField.hor)
                                                return new Hod(informationAboutHod.nameOfFigure, new Vector2(hor, vert), informationAboutHod.endField, informationAboutHod.pownBecomeTo);
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            //Если совсем запутался...
            return new Hod(Name.Pown, new Vector2(8, 8), new Vector2(8, 8), Name.Pown);
        }
        private bool CheckIfEstPregrady(Vector2 startField, Vector2 endField)
        {
            int deltaHor;
            int deltaVert;
            int lengthOfHod;
            //Единичный венктор разности вертикалей;
            if (endField.hor > startField.hor)
                deltaHor = 1;
            else if (endField.hor == startField.hor)
                deltaHor = 0;
            else
                deltaHor = -1;
            //Единичный вектор разности горизонталей;
            if (endField.vert > startField.vert)
                deltaVert = 1;
            else if (endField.vert == startField.vert)
                deltaVert = 0;
            else
                deltaVert = -1;
            //Определение длины хода;
            if (deltaHor != 0)
                lengthOfHod = Math.Abs(endField.hor - startField.hor);
            else if (deltaVert != 0)
                lengthOfHod = Math.Abs(endField.vert - startField.vert);
            else
                lengthOfHod = 0;

            //Проверяем поля между начальным и конечными полями
            for(int n = 1; n < lengthOfHod; n++)
            {
                int checkFieldHor = startField.hor + deltaHor * n;
                int checkFieldVert = startField.vert + deltaVert * n;
                if (desk[checkFieldHor, checkFieldVert] != null)
                    return true; //Есть преграда;
            }
            //Проверяем конечное поле
            if (nowWhiteAreGoing)
            {
                if (desk[endField.hor, endField.vert] != null)
                    if (desk[endField.hor, endField.vert].white || (!desk[endField.hor, endField.vert].white && desk[endField.hor, endField.vert].name == Name.King))
                        return true;//Нельзя пойти на поле, где стоит свой, или съесть короля
            }
            else
            {
                if (desk[endField.hor, endField.vert] != null)
                    if (!desk[endField.hor, endField.vert].white || (desk[endField.hor, endField.vert].white && desk[endField.hor, endField.vert].name == Name.King))
                        return true;//Нельзя пойти на поле, где стоит свой, или съесть короля
            }
            return false;
        }
        public void AddFigure(Figure figure, int hor, int vert)
        {
            desk[hor, vert] = figure;
            if (figure.white)
            {
                whiteFigures.Add(figure);
                if (figure.name == Name.King)
                    whiteKing = figure;
            }
            else
            {
                blackFigures.Add(figure);
                if (figure.name == Name.King)
                    blackKing = figure;
            }
        }
    }
}
