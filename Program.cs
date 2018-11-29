using System;
using System.Linq;

namespace Bowling
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(new Game("42 x x x 7/ 42 7/ x -8 xxx").Score());  //179
            Console.WriteLine(new Game("x x x x x x x x x xxx").Score());  //300
            Console.WriteLine(new Game("5/ 5/ 5/ 5/ 5/ 5/ 5/ 5/ 5/ 5/5").Score());  //150
            Console.WriteLine(new Game("-1 3/ x 04 53 4/ -/ x 42 27").Score());  //108
        }
    }
}
