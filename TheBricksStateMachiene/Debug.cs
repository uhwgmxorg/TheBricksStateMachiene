using System;

namespace TheBricksStateMachiene
{

    /// <summary>
    /// Unity Random Proxy Class
    /// </summary>
    public class Random
    {

        private static System.Random _random = new System.Random();

        /// <summary>
        /// Range
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Range(int min, int max)
        {
            return (int)RandomDouble(min, max - 1, 0);
        }

        /// <summary>
        /// RandomDouble
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="deci"></param>
        /// <returns></returns>
        private static double RandomDouble(double min, double max, int deci)
        {
            double d;
            d = _random.NextDouble() * (max - min) + min;
            return Math.Round(d, deci);
        }
    }

    /// <summary>
    /// Unity Debug Proxy Class
    /// </summary>
    public class Debug
    {
        static public void Log(Object obj)
        {
            System.Diagnostics.Debug.WriteLine(obj);
        }
    }
}
