using System;

namespace PPOL_Twilio.Test
{
    class Program
    {
        public static void Main (string[] args)
        {
            Console.WriteLine("Welcome to out testing function!");
            Console.WriteLine("1) CSV Import Test");
            Console.WriteLine("2) Recall Test");
            Console.WriteLine("3) Send Events Test");
            Console.Write("Please enter what you want to do: ");
            var user = Console.ReadLine();
            switch (int.Parse(user))
            {
                case 1:
                    CSVTest.Test();
                    break;
                case 2:
                    RecallTest.Test();
                    break;
                case 3:
                    SentEventsTest.Test();
                    break;
            }
        }
    }
}
