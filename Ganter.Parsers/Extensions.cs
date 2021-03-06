﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ganter.Parsers
{
    public static class Extensions
    {
        /// <summary>
        /// Reads all lines of a document.
        /// </summary>
        /// <param name="reader">The stream to get data from.</param>
        /// <returns>Set of read lines.</returns>
        public static IEnumerable<string> ReadLines(this TextReader reader)
        {
            string line = null;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }
    }
}
