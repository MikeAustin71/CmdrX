using System;
using System.Threading;

namespace ExecuteCommand
{
    public class Program
    {
        static void Main(string[] args)
        {
            string command = String.Empty;

            // Write to the console.
            Console.Write("Enter the command you wish to execute: ");

            // Get the command you wish to execute.
            command = Console.ReadLine().Trim();

            // Execute the command synchronously.
            ExecuteCmd exe = new ExecuteCmd();
            exe.ExecuteCommandSync(command);

            // Execute the command asynchronously.
            exe.ExecuteCommandAsync(command);

            // Your' done !!!
            Console.WriteLine("\nDone !");
            Console.ReadLine();
        }
    }

    
}
