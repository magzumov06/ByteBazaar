namespace Infrastructure.Helpers;

public  static class PasswordUtil
{
    public static string GenerateRandomPassword(int length = 8)
    {
        const string upperChars = "ABCDEFG";
        const string loweChars = "abcdefg";
        const string numericChars = "1234567890";
        const string specialChars = "-!?";

        var random = new Random();
        var chars = new List<char>();
        chars.Add(upperChars[random.Next(upperChars.Length)]);
        chars.Add(loweChars[random.Next(loweChars.Length)]);
        chars.Add(numericChars[random.Next(numericChars.Length)]);
        chars.Add(specialChars[random.Next(specialChars.Length)]);
        for (int i = chars.Count; i < length; i++)
        {
            var allChars = upperChars + loweChars + numericChars + specialChars;
            chars.Add(allChars[random.Next(allChars.Length)]);
        }

        for (var i = chars.Count-1; i > 0; i--)
        {
            var swapIdex = random.Next(i + 1);
            (chars[i], chars[swapIdex]) = (chars[swapIdex], chars[i]);
        }

        return new string(chars.ToArray());
    }
}