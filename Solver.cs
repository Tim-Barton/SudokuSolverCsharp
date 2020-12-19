﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace SudokuSolver
{
    class Solver
    {
        GridSpace[,] grid = new GridSpace[9, 9];

        Queue<WorkItem> workQueue = new Queue<WorkItem>();


        public Solver()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    grid[i, j] = new GridSpace();
                }
            }
        }

        private void SetValue(int row, int column, int value)
        {
            if (row < 9 && column < 9)
            {
                grid[row, column].value = value;
                workQueue.Enqueue(new WorkItem(row, column, value));
            }

        }

        public void PrintOutput()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(grid[i, j].value + ",");
                }
                Console.WriteLine();
            }

        }


        public bool IsSolved()
        {
            foreach (GridSpace gridSpace in grid)
            {
                if (!gridSpace.HasValue())
                {
                    return false;
                }
            }
            return true;
        }

        public void Solve()
        {
            WorkItem workItem;
            while (workQueue.TryDequeue(out workItem))
            {
                if (!IsSolved())
                {
                    //clear the row
                    for (int column = 0; column < 9; column++)
                    {
                        if (grid[workItem.Row, column].DiscardOption(workItem.Value))
                        {
                            workQueue.Enqueue(new WorkItem(workItem.Row, column, workItem.Value));
                        }
                    }
                    //clear the column
                    for (int row = 0; row < 9; row++)
                    {
                        if (grid[row, workItem.Column].DiscardOption(workItem.Value))
                        {
                            workQueue.Enqueue(new WorkItem(row, workItem.Column, workItem.Value));
                        }
                    }
                    int x = workItem.Column / 3;
                    int y = workItem.Row / 3;
                    //clear the square
                    for (int row = y * 3; row < (y * 3) + 3; row++)
                    {
                        for (int column = x * 3; column < (x * 3) + 3; column++)
                        {
                            if (grid[row, column].DiscardOption(workItem.Value))
                            {
                                workQueue.Enqueue(new WorkItem(row, column, workItem.Value));
                            }
                        }

                    }
                }

            }
        }

        public static Solver ParseFromFile(string filename)
        {
            Solver solver = new Solver();
            using (TextFieldParser parser = new TextFieldParser(filename))
            {
                parser.SetDelimiters(",");
                int i = 0;
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    int j = 0;
                    foreach (string field in fields)
                    {
                        if (!String.IsNullOrEmpty(field))
                        {
                            solver.SetValue(i, j, int.Parse(field));
                        }
                        j++;
                    }
                    i++;
                }
            }
            return solver;
        }
    }
}
