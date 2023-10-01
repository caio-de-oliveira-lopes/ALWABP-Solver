﻿using ALWABP.Domain;
using System.Numerics;

namespace ALWABP
{
    public static class Program
    {
        private static readonly string Warning = "\nPlease Follow The Structure => '<InputFileName> <InputFilePath> <OutputFilePath(OPTIONAL)>'"; 
        static void Main(string[] args)
        {
            string inputFileName;
            if (args.Length > 0)
                inputFileName = args[0];
            else
            {
                Console.WriteLine($"No Input File Name Was Informed.{Warning}");
                return;
            }

            string inputFileDirectory;
            if (args.Length > 1)
                inputFileDirectory = args[1];
            else
            {
                Console.WriteLine($"No Input File Path Was Informed.{Warning}");
                return;
            }

            string outputFilePath;
            if (args.Length > 2)
                outputFilePath = args[2];
            else
                outputFilePath = Directory.GetCurrentDirectory();

            if (!Directory.Exists(outputFilePath))
                Directory.CreateDirectory(outputFilePath);

            ALWABPInstance instance = new(inputFileName, inputFileDirectory, outputFilePath);
        }
    }
}