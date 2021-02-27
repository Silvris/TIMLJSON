using System;
using System.IO;
using System.Text.Json;
using TIMLJSON.Classes;

namespace TIMLJSON
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args[0] == "-j")
            {
                Console.WriteLine("Converting from Timl to Json");
                BinaryReader infile = new BinaryReader(new FileStream(args[1],FileMode.Open));
                TIML mTiml = new TIML(infile);
                infile.Close();
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,

                    
                };
                string jsonString = JsonSerializer.Serialize<object>(mTiml,options);
                StreamWriter outfile = new StreamWriter(new FileStream(args[1] + ".json", FileMode.OpenOrCreate));
                outfile.Write(jsonString);
                outfile.Close();
            }
            else if(args[0] == "-t")
            {
                Console.WriteLine("Converting from Json to Timl");
                string jsonString = File.ReadAllText(args[1]);
                TIML nTiml = JsonSerializer.Deserialize<TIML>(jsonString);
                BinaryWriter outFile = new BinaryWriter(new FileStream(args[1].Replace(".timl.json", "") + ".timl", FileMode.Create));
                nTiml.Export(outFile);
            }
            else if (args[0] == "-h")
            {
                Console.WriteLine("///////////////////////////////////////");
                Console.WriteLine("//              TIMLJSON             //");
                Console.WriteLine("// -j xxx - Convert timl to json     //");
                Console.WriteLine("// -t xxx - Convert json to timl     //");
                Console.WriteLine("// -h - View help for the program    //");
                Console.WriteLine("///////////////////////////////////////");
            }
            else
            {
                Console.Write("Invalid Args: ");
                Console.Write(args[0]);
                Console.WriteLine(args[1]);
                Console.WriteLine("Use \"-h\" to see instructions");
            }
        }
    }
}
