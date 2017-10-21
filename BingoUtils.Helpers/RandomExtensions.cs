using System;

namespace BingoUtils.Helpers
{
    public static class RandomExtensions
    {
        // By: Matt Howells / StackOverflow Community
        // At: http://stackoverflow.com/a/110570/5686352
        /// <summary>
        /// Shuffles and array
        /// </summary>
        /// <typeparam name="T">The type of data into the array</typeparam>
        /// <param name="rng">The random object to be used</param>
        /// <param name="array">The array to be shuffled</param>
        /// <exception cref="OverflowException" />
        /// <exception cref="ArgumentOutOfRangeException" />
        public static void Shuffle<T>(this Random rng, T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }
}
