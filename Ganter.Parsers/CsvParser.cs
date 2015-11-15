using Ganter.Algorithm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ganter.Parsers
{
    public class CsvParser
    {
        private Stream FileStream { get; set; }
        private char Separator { get; set; }

        public CsvParser(Stream fileStream, char separator = ';')
        {
            FileStream = fileStream;
            Separator = separator;
        }

        public FormalContext ParseContext()
        {
            using (TextReader reader = new StreamReader(FileStream))
            {
                string[] lines = reader.ReadLines().ToArray();

                string[] attributeNames = lines[0].Split(Separator);
                List<Algorithm.Attribute> attributes = CreateAttributes(attributeNames.Skip(1)).ToList();
                List<Item> items = new List<Item>();
                bool[,] matrix = new bool[lines.Length - 1, attributes.Count];

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] lineValues = lines[i].Split(Separator);
                    items.Add(new Item() { Name = lineValues[0] });

                    for (int j = 1; j < lineValues.Length; j++)
                    {
                        matrix[i - 1, j - 1] = ParseBool(lineValues[j]);
                    }
                }

                return new FormalContext(attributes, items, matrix, true);
            }
        }

        private bool ParseBool(string lineValue)
        {
            lineValue = lineValue.ToLower();

            // TODO: pridat hodnoty do konfiguracie
            if (lineValue == "0" || lineValue == "false" || lineValue == "ne") return false;
            else if (lineValue == "1" || lineValue == "true" || lineValue == "ano") return true;
            else throw new Exception("String does not represent a boolean value.");
        }

        private IEnumerable<Algorithm.Attribute> CreateAttributes(IEnumerable<string> names)
        {
            foreach (string name in names)
                yield return new Algorithm.Attribute() { Name = name };
        }
    }
}
