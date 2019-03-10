using DotsAndBoxes.Common.CommonModels;
using DotsAndBoxes.Extensions;
using DotsAndBoxes.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DotsAndBoxes.Core.Models
{
    //Board
    public class Board
    {
        public Board(int dimension)
        {
            int crosses;
            if (dimension < 2)
                throw new Exception("Not valid number");
            Points = new List<BoardPoint>();
            //Fill board with dots
            for (int i = 1; i <= dimension; i++)
            {
                for (int j = 1; j <= dimension; j++)
                {

                    if (i == 1 || i == dimension)
                    {
                        if (j == 0 || j == dimension)
                            crosses = 2;
                        else
                            crosses = 3;
                    }
                    else
                    {
                        if (j == 1 || j == dimension)
                            crosses = 3;
                        else
                            crosses = 4;

                    }
                    //Each position at the board contains a dot(x,y) and depending on that position the times it
                    //can be crossed. The possible cross scenarios vary from 2 to 4.
                    Points.Add(new BoardPoint { Dot = new Dot { X = i, Y = j }, Crosses = crosses });
                   
                }
            }
            DrawnLines = new List<Line>();
            
            _dimension = dimension;
            var s = dimension - 1;
            Squares = s*s;

        }
        public int Moves { get; set; }
        private int _dimension;
        //Max possible squares, used to predict the score
        public int Squares { get; private set; }
        //The lines that have currently been drawn
        public List<Line> DrawnLines { get; private set; }
        //The "coordinates"
        public List<BoardPoint> Points { get; set; }

        //Draw line logic. Checks if the dots are valid in order to draw line
        private bool DrawLine(Dot starterDot, Dot endingDot)
        {
            //Checks if the dots are within the board's bounds
            if (!IsWithinBounds(starterDot) && IsWithinBounds(endingDot))
                return false;
            //Checks dot cross availiability
            var startpoint = Points.SingleOrDefault(p => p.Dot.Equals(starterDot));
            var endPoint = Points.SingleOrDefault(p => p.Dot.Equals(endingDot));
            if (!(startpoint.Crosses == 0||endPoint.Crosses==0))
            {
                //Check if the dots are in the same line or row
                if ((Math.Abs(starterDot.X - endingDot.X) == 1 || Math.Abs(starterDot.Y - endingDot.Y) == 1) &&
                    !(Math.Abs(starterDot.X - endingDot.X) == 1 && Math.Abs(starterDot.Y - endingDot.Y) == 1))
                {
                    Line line = new Line { StartDot = starterDot, EndDot = endingDot };
                    //for foes we have yet to consider about (antiyagni)
                    if (!DrawnLines.Any(l=>l.Equals(line)||l.Equals(line.AltLine())))
                    {
                        DrawnLines.Add(line);
                        Points.SingleOrDefault(p=>p.Dot.Equals(starterDot)).Crosses--;
                        Points.SingleOrDefault(p=>p.Dot.Equals(endingDot)).Crosses--;
                        
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
            if (DrawnLines.Any(l=>l.Equals(parallelLines.Item1))||DrawnLines.Any(l=>l.Equals(parallelLines.Item1.AltLine())))
            {
                if (DrawnLines.Any(l=>l.Equals(newLine.GetVerticalLines(parallelLines.Item1).Item1)|| l.Equals(newLine.GetVerticalLines(parallelLines.Item1).Item1.AltLine())) &&
                    DrawnLines.Any(l=>l.Equals(newLine.GetVerticalLines(parallelLines.Item1).Item2)|| l.Equals(newLine.GetVerticalLines(parallelLines.Item1).Item2.AltLine())))
                {
                    score = Score.Single;
                    Squares--;
                }
            }
            //Checks box's existence based on the second parallel
            if (DrawnLines.Any(l=>l.Equals(parallelLines.Item2)||l.Equals(parallelLines.Item2.AltLine())))
            {
                if (DrawnLines.Any(l=>l.Equals(newLine.GetVerticalLines(parallelLines.Item2).Item1)||l.Equals(newLine.GetVerticalLines(parallelLines.Item2).Item1.AltLine()))&&
                    DrawnLines.Any(l=>l.Equals(newLine.GetVerticalLines(parallelLines.Item2).Item2)||l.Equals(newLine.GetVerticalLines(parallelLines.Item2).Item2.AltLine())))
                {
                    score = score == Score.NoScore ? Score.Single : Score.Double;
                    Squares--;
                }
            }
            return score;
        }
        private bool IsWithinBounds(Dot dot)
        {
            if (dot.X > _dimension || dot.Y > _dimension)
                return false;
            else
                return true;
        }
        



    }
}
