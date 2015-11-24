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
        private string Separator { get; set; }
        private string TrueValue { get; set; }
        private string FalseValue { get; set; }

        public event Action<List<Algorithm.Attribute>> OnThresholdsFound;

        public CsvParser(Stream fileStream, string trueValue, string falseValue, string separator)
        {
            FileStream = fileStream;
            Separator = separator;
            TrueValue = trueValue;
            FalseValue = falseValue;
        }

        public FormalContext ParseContext()
        {
            using (TextReader reader = new StreamReader(FileStream))
            {
                string[] lines = reader.ReadLines().ToArray();

                string[] attributeNames = lines[0].Split(new string[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
                List<Algorithm.Attribute> attributes = CreateAttributes(attributeNames.Skip(1)).ToList();
                List<Item> items = new List<Item>();
                bool[,] matrix = new bool[lines.Length - 1, attributes.Count];

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] lineValues = lines[i].Split(new string[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
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
            if (lineValue == FalseValue.ToLower()) return false;
            else if (lineValue == TrueValue.ToLower()) return true;
            else throw new Exception("String does not represent a boolean value.");
        }

        private IEnumerable<Algorithm.Attribute> CreateAttributes(IEnumerable<string> names)
        {
            foreach (string name in names)
                yield return new Algorithm.Attribute() { Name = name };
        }

        public FormalContext PreprocessData()
        {
            using (TextReader reader = new StreamReader(FileStream))
            {
                string[] lines = reader.ReadLines().ToArray();

                string[] attributeNames = lines[0].Split(new string[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
                List<Algorithm.Attribute> oldAttributes = CreateAttributes(attributeNames.Skip(1)).ToList();

                List<Item> items = new List<Item>();
                decimal[,] oldMatrix = new decimal[lines.Length - 1, oldAttributes.Count];

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] lineValues = lines[i].Split(new string[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
                    items.Add(new Item() { Name = lineValues[0] });
                    
                    for (int j = 1; j < lineValues.Length; j++)
                    {
                        decimal parsedNumber;
                        if (decimal.TryParse(new string(lineValues[j].ToCharArray().Where(c => char.IsDigit(c)).ToArray()), out parsedNumber))
                        {
                            oldMatrix[i - 1, j - 1] = parsedNumber;

                            if (oldAttributes[j - 1].Max < parsedNumber)
                                oldAttributes[j - 1].Max = parsedNumber;

                            if (oldAttributes[j - 1].Min > parsedNumber)
                                oldAttributes[j - 1].Min = parsedNumber;
                        }
                        else throw new InvalidCastException("Non-numeric string found, where number was expected, line " + i);
                    }
                }

                List<Algorithm.Attribute> attributes;

                if (OnThresholdsFound != null)
                    OnThresholdsFound(oldAttributes);

                bool[,] matrix = CreateNewMatrix(oldAttributes, oldMatrix, out attributes);

                return new FormalContext(attributes, items, matrix, true);
            }
        }

        private bool[,] CreateNewMatrix(List<Algorithm.Attribute> oldAttributes, decimal[,] oldMatrix, out List<Algorithm.Attribute> attributes)
        {
            attributes = new List<Algorithm.Attribute>();

            for(int i = 0; i < oldAttributes.Count; i++)
            {
                oldAttributes[i].LecticPosition = i;
                attributes.AddRange(GenerateNewAttributes(oldAttributes[i]));
            }

            bool[,] result = new bool[oldMatrix.GetLength(0), attributes.Count];

            for(int i =0; i < attributes.Count; i++)
            {
                for(int j =0; j < oldMatrix.GetLength(0); j++)
                {
                    result[j, i] = attributes[i].IsInRange(oldMatrix[j, attributes[i].ParentAttribute.LecticPosition);
                }
            }

            return result;
        }

        private IEnumerable<Algorithm.Attribute> GenerateNewAttributes(Algorithm.Attribute oldAttribute)
        {
            for (decimal start = oldAttribute.Min; start < oldAttribute.Max; start += oldAttribute.Step)
            {
                yield return new Algorithm.Attribute()
                    {
                        Name = string.Format("{0}[{1}-{2}]", oldAttribute.Name, start, start + oldAttribute.Step - 1),
                        Min = start,
                        Max = start + oldAttribute.Step - 1,
                        ParentAttribute = oldAttribute
                    };
            }
        }
    }
}
