using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day3
{
    internal class Number
    {
        public int Y { get; set; }

        public int StartX { get; set; }

        public int EndX { get; set; }

        public int Value { get; set; }

        /// <summary>
        /// Represent the result of a scan of adjacent lines by a Number
        /// The gears ids may be part of an incomplete gear (as in with only one element)
        /// </summary>
        /// <param name="HasAdjacentSymbol"></param>
        /// <param name="GearsIds"></param>
        public record ScanReport (bool HasAdjacentSymbol, IEnumerable<string> GearsIds);

        /// <summary>
        /// Scan adjacent lines to find symbol and/or potential gear
        /// Doesn't check if found gear are connected to two number, only search for posible gear
        /// </summary>
        /// <param name="closeLines"></param>
        /// <returns></returns>
        public ScanReport ScanLines(IEnumerable<Line> closeLines)
        {
            bool hasSymbol = false;
            List<string> gears = [];


            foreach (Line line in closeLines)
            {

                if (line.Y == Y)
                {
                    //As we are scanning the line where the number is present, we only need to check the char before and the one after
                    char? previousChar = StartX > 0 ? line.Value[StartX - 1] : null;
                    char? nextChar = EndX < line.XMax ? line.Value[EndX + 1] : null;

                    if (previousChar != null && IsSymbol(previousChar.Value))
                    {
                        hasSymbol = true;
                        if (previousChar == '*')
                        {
                            gears.Add($"{line.Y}-{StartX - 1}");
                        }
                    }

                    if (nextChar != null && IsSymbol(nextChar.Value))
                    {
                        hasSymbol = true;
                        if (nextChar == '*')
                        {
                            gears.Add($"{line.Y}-{EndX + 1}");
                        }
                    }
                }
                else if( line.Y == Y + 1 || line.Y == Y - 1)
                {
                    //General case : we need to get a substring containing only the characters adjacent to the number and check them
                    var minX = StartX > 0 ? StartX - 1 : StartX;
                    var maxX = EndX < line.XMax ? EndX + 1 : EndX;
                    var lenght = maxX - minX;
                    var inspectedSub = line.Value.Substring(minX, lenght + 1);

                    for (int i = 0; i < inspectedSub.Length; i++)
                    {
                        if (IsSymbol(inspectedSub[i]))
                        {
                            hasSymbol = true;
                            if (inspectedSub[i] == '*')
                            {
                                gears.Add($"{line.Y}-{minX + i}");
                            }
                        }
                    }
                }
            }

            return new ScanReport(hasSymbol, gears);
        }

        private static bool IsSymbol(char c)
        {
            return !char.IsDigit(c) && c != '.';
        }
    }
}
