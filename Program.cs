using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace akinator
{
    internal class Program
    {
        class Node
        {
            public string ID { get; set; }
            public string Text { get; set; }
            public string Yes { get; set; }
            public string No { get; set; }
            public bool IsAnswer { get; set; }
        }

        static void Main(string[] args)
        {
            string filePath = "aki.data";

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Soubor 'aki.data' nebyl nalezen!");
                return;
            }


            var lines = File.ReadAllLines(filePath);
            var nodes = lines
                .Skip(1)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Split(';'))
                .ToDictionary(
                    f => f[0],
                    f => new Node
                    {
                        ID = f[0],
                        Text = f[1],
                        Yes = f.Length > 2 ? f[2] : "",
                        No = f.Length > 3 ? f[3] : "",
                        IsAnswer = f.Length > 4 && f[4] == "1"
                    }
                );

            if (!nodes.ContainsKey("1"))
            {
                Console.WriteLine("Chyba: uzel s ID '1' nebyl nalezen!");
                return;
            }

            Node current = nodes["1"];


            while (true)
            {
                if (current.IsAnswer)
                {
                    Console.WriteLine();
                    Console.WriteLine($"✅ Myslíš si: {current.Text}!");
                    Console.WriteLine();
                    break;
                }

                Console.WriteLine(current.Text + " (ano/ne)");
                string? odpoved = Console.ReadLine()?.Trim().ToLower();

                while (odpoved != "ano" && odpoved != "ne")
                {
                    Console.WriteLine("Zadej prosím 'ano' nebo 'ne'.");
                    odpoved = Console.ReadLine()?.Trim().ToLower();
                }

                if (odpoved == "ano")
                {
                    if (!nodes.ContainsKey(current.Yes))
                    {
                        Console.WriteLine("Chyba: chybí větev 'ano'.");
                        return;
                    }
                    current = nodes[current.Yes];
                }
                else
                {
                    if (!nodes.ContainsKey(current.No))
                    {
                        Console.WriteLine("Chyba: chybí větev 'ne'.");
                        return;
                    }
                    current = nodes[current.No];
                }
            }

            Console.WriteLine("Konec hry.");
        }
    }
}