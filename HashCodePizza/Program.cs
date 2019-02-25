using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCodePizza
{
    class Program
    {
        static int numberOfRows;
        static int numberOfColumns;
        static int minIngredient;
        static int maxIngredient;

        static string[,] arrayPizza;
        static bool[,] arraySliced;

        static List<Rectangle> dimensionsList = new List<Rectangle>();


        static void Main(string[] args)
        {
            List<string> output = new List<string>();
            ReadFile(@"c:/example.in");

            dimensionsList = new List<Rectangle>();
            GeneratePossibleRectangleDimensions(minIngredient, maxIngredient);
            dimensionsList.Reverse();

            //main loop
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    foreach (Rectangle rectangle in dimensionsList)
                    {
                        if (CheckIfSliceIsPossible(i, j, rectangle.dimensionA, rectangle.dimensionB) && CheckIngredients(i, j, rectangle.dimensionA, rectangle.dimensionB))
                        {
                            Slice(i, j, rectangle.dimensionA, rectangle.dimensionB);
                            string s = i + " " + j + " " + (i - 1 + rectangle.dimensionA) + " " + (j - 1 + rectangle.dimensionB);
                            output.Add(s);
                        }
                    }
                }
            }

            Console.WriteLine("Result: " + CalculateScore());

            using (TextWriter tw = new StreamWriter("output.txt"))
            {
                tw.WriteLine(output.Count);
                foreach (String s in output)
                     tw.WriteLine(s);
            }
        }


        static int CalculateScore()
        {
            int count = 0;
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    if (arraySliced[i, j] == true)
                    {
                        count++;
                    }
                }
            }

            return count;
        }


        struct Rectangle
        {
            public int dimensionA;
            public int dimensionB;

            public Rectangle(int a, int b)
            {
                dimensionA = a;
                dimensionB = b;
            }
        };

        static void GeneratePossibleRectangleDimensions(int L, int H)
        {
            for (int i = 1; i <= H; i++)
            {
                for (int j = 1; j <= H; j++)
                {
                    if ((i * j >= 2 * L) && (i * j <= H))
                    {
                        dimensionsList.Add(new Rectangle(i, j));
                    }
                }
            }
        }

        static void PrintDimensions()
        {
            foreach (Rectangle dim in dimensionsList)
            {
                Console.WriteLine(dim.dimensionA + " " + dim.dimensionB);
            }
        }


        static void ReadFile(string filePath)
        {
            String[] lines = File.ReadAllLines(filePath);

            string[] inputParameter = lines[0].Split(' ');
            numberOfRows = Convert.ToInt32(inputParameter[0]);
            numberOfColumns = Convert.ToInt32(inputParameter[1]);
            minIngredient = Convert.ToInt32(inputParameter[2]);
            maxIngredient = Convert.ToInt32(inputParameter[3]);

            arrayPizza = new string[numberOfRows, numberOfColumns];
            arraySliced = new bool[numberOfRows, numberOfColumns];

            int x = 0;

            foreach (string line in lines)
            {
                if (line != lines[0])
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        arrayPizza[x, i] = line[i].ToString();
                        //Console.Write(arrayPizza[x, i]);
                    }
                    //Console.WriteLine();
                    x++;
                }
            }
        }


        static bool CheckIfSliceIsPossible(int indexX, int indexY, int dimensionA, int dimensionB)
        {
            for (int i = indexX; i < indexX + dimensionA; i++)
            {
                for (int j = indexY; j < indexY + dimensionB; j++)
                {
                    if (i >= numberOfRows || j >= numberOfColumns)
                    {
                        return false;
                    }
                    else if (arraySliced[i, j] == true)
                    {
                        return false;
                    }

                }
            }
            return true;
        }

        static bool CheckIngredients(int indexX, int indexY, int dimensionA, int dimensionB)
        {
            int countT = 0;
            int countM = 0;

            for (int i = indexX; i < indexX + dimensionA; i++)
            {
                for (int j = indexY; j < indexY + dimensionB; j++)
                {
                    if (arrayPizza[i, j] == "M") countM++;
                    else countT++;
                }
            }

            if (countM >= minIngredient && countT >= minIngredient) return true;
            else return false;
        }

        static void Slice(int indexX, int indexY, int dimensionA, int dimensionB)
        {
            for (int i = indexX; i < indexX + dimensionA; i++)
            {
                for (int j = indexY; j < indexY + dimensionB; j++)
                {
                    arraySliced[i, j] = true;
                }
            }
        }









    }
}
