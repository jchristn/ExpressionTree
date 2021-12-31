﻿using System;
using System.IO;
using System.Text;
using ExpressionTree;

namespace Test.NewtonsoftJson
{
    internal class Program
    {
        static string FILE1 = "./ex1.json";
        static string FILE2 = "./ex2.json";
        static string FILE3 = "./ex3.json";
        static string FILE4 = "./ex4.json";
        static string FILE5 = "./ex5.json";
        static string FILE6 = "./ex6.json";

        static void Main(string[] args)
        {
            string ex1str = File.ReadAllText(FILE1);
            string ex2str = File.ReadAllText(FILE2);
            string ex3str = File.ReadAllText(FILE3);
            string ex4str = File.ReadAllText(FILE4);
            string ex5str = File.ReadAllText(FILE5);
            string ex6str = File.ReadAllText(FILE6);

            Console.WriteLine("--- " + FILE1 + " ---");
            Console.WriteLine(ex1str);
            Expr ex1 = SerializationHelper.DeserializeJson<Expr>(ex1str);
            Console.WriteLine(ex1.ToString());
            Console.WriteLine("");

            Console.WriteLine("--- " + FILE2 + " ---");
            Console.WriteLine(ex2str);
            Expr ex2 = SerializationHelper.DeserializeJson<Expr>(ex2str);
            Console.WriteLine(ex2.ToString());
            Console.WriteLine("");

            Console.WriteLine("--- " + FILE3 + " ---");
            Console.WriteLine(ex3str);
            Expr ex3 = SerializationHelper.DeserializeJson<Expr>(ex3str);
            Console.WriteLine(ex3.ToString());
            Console.WriteLine("");

            Console.WriteLine("--- " + FILE4 + " ---");
            Console.WriteLine(ex4str);
            Expr ex4 = SerializationHelper.DeserializeJson<Expr>(ex4str);
            Console.WriteLine(ex4.ToString());
            Console.WriteLine("");

            Console.WriteLine("--- " + FILE5 + " ---");
            Console.WriteLine(ex5str);
            Expr ex5 = SerializationHelper.DeserializeJson<Expr>(ex5str);
            Console.WriteLine(ex5.ToString());
            Console.WriteLine("");

            Console.WriteLine("--- " + FILE6 + " ---");
            Console.WriteLine(ex6str);
            Expr ex6 = SerializationHelper.DeserializeJson<Expr>(ex6str);
            Console.WriteLine(ex6.ToString());
            Console.WriteLine("");
        }
    }
}