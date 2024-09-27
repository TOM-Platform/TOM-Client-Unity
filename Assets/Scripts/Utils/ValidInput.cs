namespace TOM.Common.Utils
{
    public static class ValidInput
    {
        public static bool isValidInt(string value)
        {
            int i;
            return int.TryParse(value, out i);
        }

        public static bool isValidBool(string boolean)
        {
            bool result;
            return bool.TryParse(boolean, out result);
        }
    }
}
