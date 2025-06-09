using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_Game
{
    class Circle
    {
        public int X { get; set; } 
        public int Y { get; set; }
        // like variables but they give you more control on how to use them, and how they get called from the class

        // Constructor
        // Each time a new instance is created of this class, it needs to know what to do with properties, (X, Y)
        // properties are public so they will be accessible for us from another class that creates an instance of circle.
        public Circle() 
        {
            X = 0;
            Y = 0;
        }
    }
}
