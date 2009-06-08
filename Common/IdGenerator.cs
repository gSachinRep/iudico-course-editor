namespace FireFly.CourseEditor.Common
{
    using System;
    using System.IO;
    using System.Text;

    public static class IdGenerator
    {
        /// <summary>
        /// Checks is specified char is valid for identifier.
        /// </summary>
        /// <param name="c">Char to test</param>
        /// <returns>Returns true if char can be use for identifier and false otherwise</returns>
        public static bool ValidCharForId(char c)
        {
            return char.IsLetterOrDigit(c);
        }

        /// <summary>
        /// Generates identifier using prototype and validator delegate.
        /// </summary>
        /// <param name="prototype">Prototype for id should be generated</param>
        /// <param name="validator">Delegate to test is generated id is acceptable</param>
        /// <returns>Returns generated identifier</returns>
        public static string GenerateValidId([NotNull]string prototype, [NotNull]Predicate<string> validator)
        {
            string result = GenerateId(prototype);
            if (!validator(result))
            {
                result += '_';
                string temp;
                int i = 1;
                do
                {
                    temp = result + (i++);
                } while (!validator(temp));

                return temp;
            }
            else
                return result;
        }

        /// <summary>
        /// Generates unique file name using prototype, extension for specified directory
        /// </summary>
        /// <param name="prototype">Prototype for generated file</param>
        /// <param name="extension">Extension for generated file with needed dots</param>
        /// <param name="dir">Location for generated file</param>
        /// <returns>Name of generated file with extension but full path</returns>
        public static string GenerateUniqueFileName([NotNull]string prototype, [NotNull]string extension, [NotNull]string dir)
        {
            return GenerateValidId(prototype, id => !File.Exists(Path.Combine(dir, id + extension)));
        }

        /// <summary>
        /// Generates straightforward kind of identifier using specified prototype
        /// </summary>
        /// <param name="prototype">Prototype for id should be generated</param>
        /// <returns>Generated identifier</returns>
        public static string GenerateId([NotNull]string prototype)
        {
            var result = new StringBuilder();
            foreach (var c in prototype)
            {
                result.Append(ValidCharForId(c) ? c : '_');
            }
            return result.ToString();
        }
    }
}