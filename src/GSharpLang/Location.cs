﻿namespace GSharpLang
{
    public class Location
    {
        public int Line { get; private set; }
        public int Column { get; private set; }
        
        public Location(int line, int column)
        {
            Line = line;
            Column = column;
        }

        public void IncrementColumn()
        {
            Column++;
        }

        public void IncrementLine()
        {
            Column = 1;
            Line++;
        }
    }
}
