using DotsAndBoxes.Common.CommonModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace DotsAndBoxes.Core.Models
{
    //Board
    public class Board:Dictionary<Dot, int>
    {
        public Board(int dimension)
        {
            int crosses;
            if (dimension > 2)
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
                    //be crossed. The possible cross scenarios vary from 2 to 4.
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
        public bool DrawLine(Dot starterDot, Dot endingDot)
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


    }
}
