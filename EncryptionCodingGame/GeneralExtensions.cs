namespace EncryptionCodingGame
{
    public static class GeneralExtensions
    {
        public static T[] GetValues<T>(this T[] array, int startIndex, int length)
        {
            startIndex %= array.Length;
            length = length > array.Length ? array.Length : length;

            var subset = new T[length];

            for (int i = 0; i < length; i++)
            {
                subset[i] = array[startIndex + i];
            }
            return subset;
        }
    }
}
