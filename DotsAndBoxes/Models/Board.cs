using DotsAndBoxes.Common.CommonModels;
using DotsAndBoxes.Extensions;
using DotsAndBoxes.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DotsAndBoxes.Core.Models
{
    //Board
    public class Board:Dictionary<Dot, int>
    {
        public Board(int dimension)
        {
            int crosses;
            if (dimension < 2)
                throw new Exception("Not valid number");
            //Fill board with dots
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {

                    if (i == 0 || i == dimension)
                    {
                        if (j == 0 || j == dimension)
                            crosses = 2;
                        else
                            crosses = 3;
                    }
                    else
                    {
                        if (j == 0 || j == dimension)
                            crosses = 3;
                        else
                            crosses = 4;
                        
                    }
                    //Each position at the board contains a dot(x,y) and depending on that position the times it
                    //can be crossed. The possible cross scenarios vary from 2 to 4.
                    this.Add(new Dot { X = i, Y = j }, crosses);
                }
            }
            Squares = dimension^2;
        }
        //Max possible squares, used to predict the score
        public int Squares { get; private set; }
        //The lines that have currently been drawn
        public List<Line> DrawnLines { get; private set; }
        

        //Draw line logic. Checks if the dots are valid in order to draw line
        private bool DrawLine(Dot starterDot, Dot endingDot)
        {
            //Checks dot cross availiability
            if (!(this[starterDot]==0||this[endingDot]==0))
            {
                //Check if the dots are in the same line or row
                if ((Math.Abs(starterDot.X - endingDot.X) == 1 || Math.Abs(starterDot.Y - endingDot.Y) == 1) &&
                    !(Math.Abs(starterDot.X - endingDot.X) == 1 && Math.Abs(starterDot.Y - endingDot.Y) == 1))
                {
                    Line line = new Line { StartDot = starterDot, EndDot = endingDot };
                    //for foes we have yet to consider about (antiyagni)
                    if (!DrawnLines.Contains(line))
                    {
                        DrawnLines.Add(line);
                        this[starterDot]--;
                        this[endingDot]--;
                        //CheckBox(line);
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }

       


        //check if a box has been drawn
        //This is accomplished by checking if at least one of the parallel lines is contained in the DrawnLines
        //For each case check the existance of the necessary vertical lines 
        public Score CheckBox(Line newLine)
        {
            if (!DrawLine(newLine.StartDot, newLine.EndDot))
                return Score.Invalid;
            var score = Score.NoScore;
            var parallelLines = newLine.GetParallelLines();
            //Checks box's existence based on the first parallel
            if (DrawnLines.Contains(parallelLines.Item1))
            {
                if (DrawnLines.Contains(newLine.GetVerticalLines(parallelLines.Item1).Item1)&&
                    DrawnLines.Contains(newLine.GetVerticalLines(parallelLines.Item1).Item2))
                {
                    score = Score.Single;
                }
            }
            //Checks box's existence based on the second parallel
            if (DrawnLines.Contains(parallelLines.Item2))
            {
                if (DrawnLines.Contains(newLine.GetVerticalLines(parallelLines.Item2).Item1)&&
                    DrawnLines.Contains(newLine.GetVerticalLines(parallelLines.Item2).Item2))
                {
                    score = score == Score.NoScore ? Score.Single : Score.Double;
                }
            }
            return score;
        }
        

      
    }
}
