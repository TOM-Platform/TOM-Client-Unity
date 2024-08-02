internal class ValidInput
{
    internal static bool isValidInt(string value)
    {
        int i;
        return int.TryParse(value, out i);
    }

    internal static bool isValidBool(string boolean)
    {
        bool result;
        return bool.TryParse(boolean, out result);
    }
}