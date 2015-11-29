using Ganter.Algorithm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ganter.Parsers
{
    /// <summary>
    /// A class to parse and pre-process CSV data for formal concept analysis.
    /// </summary>
    public class CsvParser
    {
        /// <summary>
        /// The stream, from which the file should be read.
        /// </summary>
        private Stream FileStream { get; set; }
        /// <summary>
        /// The csv separator character or string.
        /// </summary>
        private string Separator { get; set; }
        /// <summary>
        /// If the data are pre-processed, the true value representative should be stated.
        /// </summary>
        private string TrueValue { get; set; }
        /// <summary>
        /// If the data are pre-processed, the false value representative should be stated.
        /// </summary>
        private string FalseValue { get; set; }

        /// <summary>
        /// During pre-preocessing, default equidistant categorization is formed. This event passes its values, which can be changed.
        /// </summary>
        public event Action<List<Algorithm.Attribute>> OnThresholdsFound;

        /// <summary>
        /// Creates a new csv parser instance.
        /// </summary>
        /// <param name="fileStream">The stream from which the data should be read.</param>
        /// <param name="trueValue">If the data are pre-processed, the true value representative should be stated.</param>
        /// <param name="falseValue">If the data are pre-processed, the false value representative should be stated.</param>
        /// <param name="separator">The csv separator character or string.</param>
        public CsvParser(Stream fileStream, string trueValue, string falseValue, string separator)
        {
            FileStream = fileStream;
            Separator = separator;
            TrueValue = trueValue;
            FalseValue = falseValue;
        }

        /// <summary>
        /// Used for pre-processed data. Parses the data from the Stream according to provided TrueValue, FalseValue and CsvSeparators.
        /// </summary>
        /// <returns>A formal context resulting from parsed data.</returns>
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

        /// <summary>
        /// Changes a string value into its proper boolean representation based on provided TrueValue and FalseValue.
        /// </summary>
        /// <param name="lineValue">The single part of a line separated by CsvSeparator.</param>
        /// <returns>A boolean value based on provided TrueValue and FalseValue.</returns>
        private bool ParseBool(string lineValue)
        {
            lineValue = lineValue.ToLower();
            if (lineValue == FalseValue.ToLower()) return false;
            else if (lineValue == TrueValue.ToLower()) return true;
            else throw new Exception("String does not represent a boolean value.");
        }

        /// <summary>
        /// Creates a set of Ganter attributes from provided set of names.
        /// </summary>
        /// <param name="names">The parsed set of attribute names.</param>
        /// <returns>A set of Ganter attributes from provided set of names.</returns>
        private IEnumerable<Algorithm.Attribute> CreateAttributes(IEnumerable<string> names)
        {
            foreach (string name in names)
                yield return new Algorithm.Attribute() { Name = name };
        }

        /// <summary>
        /// Used for data, which still need pre-processing. Reads the data, parses them and finds the minimum and maximum value.
        /// Default equi-distant categorization is used, to divide the attributes into smaller sub-attributes. This causes the OnThresholdsFound event to be fired.
        /// After that, a new formal context is formed, based on the provided set of items, matrix of values and set of newly created sub-attributes.
        /// </summary>
        /// <returns>A formal context based on equi-distant categorization of provided data.</returns>
        public FormalContext PreprocessData()
        {
            using (TextReader reader = new StreamReader(FileStream))
            {
                string[] lines = reader.ReadLines().ToArray();

                for (int i = 0; i < lines.Length; i++)
                {
                    lines[i] = Regex.Replace(lines[i],
                                    @",(?=[^""]*""(?:[^""]*""[^""]*"")*[^""]*$)",
                                    String.Empty);
                }

                string[] attributeNames = lines[0].Split(new string[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
                List<Algorithm.Attribute> oldAttributes = CreateAttributes(attributeNames.Skip(1)).ToList();

                List<Item> items = new List<Item>();
                int[,] oldMatrix = new int[lines.Length - 1, oldAttributes.Count];

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] lineValues = lines[i].Split(new string[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
                    items.Add(new Item() { Name = lineValues[0] });

                    for (int j = 1; j < lineValues.Length; j++)
                    {
                        int parsedNumber;
                        if (int.TryParse(new string(lineValues[j].ToCharArray().Where(c => char.IsDigit(c)).ToArray()), out parsedNumber))
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

        /// <summary>
        /// Creates a new binary matrix based on provided old attributes, old value matrix and new sub-categorized attributes.
        /// </summary>
        /// <param name="oldAttributes">The old attributes, parsed from the CSV file.</param>
        /// <param name="oldMatrix">The old matrix of values.</param>
        /// <param name="attributes">The newly created set of sub-attributes based on equi-distant division of old attributes.</param>
        /// <returns>A binary matrix serving as the base for a formal context.</returns>
        private bool[,] CreateNewMatrix(List<Algorithm.Attribute> oldAttributes, int[,] oldMatrix, out List<Algorithm.Attribute> attributes)
        {
            attributes = GenerateNewAttributes(oldAttributes).ToList();

            bool[,] result = new bool[oldMatrix.GetLength(0), attributes.Count];

            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < oldMatrix.GetLength(0); j++)
                {
                    result[j, i] = attributes[i].IsInRange(oldMatrix[j, attributes[i].ParentAttribute.LecticPosition]);
                }
            }

            return result;
        }

        /// <summary>
        /// Creates a set of new attributes, based on equi-distant division of old attributes.
        /// </summary>
        /// <param name="oldAttributes">The set of old attributes, parsed from the CSV file.</param>
        /// <returns>A set of new attributes, based on equi-distant division of old attributes.</returns>
        private IEnumerable<Algorithm.Attribute> GenerateNewAttributes(List<Algorithm.Attribute> oldAttributes)
        {
            for (int i = 0; i < oldAttributes.Count; i++)
            {
                oldAttributes[i].LecticPosition = i;
                for (int start = oldAttributes[i].Min; start <= oldAttributes[i].Max; start += oldAttributes[i].Step)
                {
                    yield return new Algorithm.Attribute()
                    {
                        Name = string.Format("{0}[{1}-{2}]", oldAttributes[i].Name, start, start + oldAttributes[i].Step),
                        Min = start,
                        Max = start + oldAttributes[i].Step,
                        ParentAttribute = oldAttributes[i]
                    };
                }
            }
        }
    }
}
