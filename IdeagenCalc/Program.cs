using System;
using System.Diagnostics.CodeAnalysis;

namespace IdeagenCalc
{
    [ExcludeFromCodeCoverage]
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Please enter expression formula : ");
                    var input = Console.ReadLine();

                    try
                    {
                        var result = Calculator.Calculate(input);
                        Console.WriteLine($"Result : {result.ToString()}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Unhandle Expression.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
