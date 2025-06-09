using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake_Game
{
    class Settings
    {
        // Static variables = variables that don't need to be created as an instance
        // Once they are called from the main class they are accessible throughout the program

        public static int Width { get; set; }
        public static int Height { get; set; }

        public static string directions;


        // Circles are going to be 16x16 px
        public Settings() 
        {
            Width = 16;
            Height = 16;
            directions = "left";
        }


    }
}
