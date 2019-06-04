using System;
using System.Collections.Generic;
using System.Text;
using SignalrCommon;

namespace SignalrClient
{
    public static class Helpers
    {
        public static void PrintChatMessage(string sender, string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}] {sender}: {message}");
            Console.ResetColor();
        }

        public static void PrintObjectMessage(string sender, ExampleEntity obj)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}] {sender}: {obj.ToString()}");
            Console.ResetColor();
        }

        public static void ClearCurrentConsoleLine()
        {
            var currentLineCursor = Console.CursorTop - 1;
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}
