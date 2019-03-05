using DotsAndBoxes.Common.CommonModels;
using System;

namespace DotsAndBoxes.Extensions
{
    public static class LineExtensions
    {
        public static Line AltLine(this Line line)
        {
            return new Line { StartDot = line.EndDot, EndDot = line.StartDot };
        }
        public static Tuple<Line,Line> GetParallelLines(this Line newLine)
        {
            
            if (newLine.Axis == Axis.Horizontal)
            {
                return new Tuple<Line, Line>(new Line
                {
                    StartDot = new Dot { X = newLine.StartDot.X - 1, Y = newLine.StartDot.Y },
                    EndDot = new Dot { X = newLine.EndDot.X - 1, Y = newLine.EndDot.Y }
                },
                new Line
                {
                    StartDot = new Dot { X = newLine.StartDot.X + 1, Y = newLine.StartDot.Y },
                    EndDot = new Dot { X = newLine.EndDot.X + 1, Y = newLine.EndDot.Y }
                }
                );
            }
            else
            {
                return new Tuple<Line, Line>(new Line
                {
                    StartDot = new Dot { X = newLine.StartDot.X, Y = newLine.StartDot.Y - 1 },
                    EndDot = new Dot { X = newLine.EndDot.X, Y = newLine.EndDot.Y - 1 }
                },
                new Line
                {
                    StartDot = new Dot { X = newLine.StartDot.X, Y = newLine.StartDot.Y + 1 },
                    EndDot = new Dot { X = newLine.EndDot.X, Y = newLine.EndDot.Y + 1 }
                }
                );
            }
        }
        //get vertical lines based on a certain parallel
        public static Tuple<Line, Line> GetVerticalLines(this Line line, Line parallelLine)
        {
            return new Tuple<Line, Line>(new Line
            {
                StartDot = line.StartDot, EndDot = parallelLine.StartDot
            },
            new Line
            {
                StartDot = line.EndDot, EndDot = parallelLine.EndDot
            });

        }
    }
}
