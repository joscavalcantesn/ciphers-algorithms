// Original Author: Matheus Ribeiro - Python Version
// GitHub: https://github.com/MatheusRibeiro443
// Converted to C# by: Josias Cavalcante
// Conversion Date: 23/09/2025
// Original Repository: https://github.com/MatheusRibeiro443/Ciphers

using System.Text;

namespace CiphersAlgorithms.Breakers;

public class VigenereBreaker
{
    // Portuguese letter frequency table (from the original Python code)
    private static readonly Dictionary<char, decimal> PortugueseFrequency = new()
    {
        {'a', 0.1396m}, {'b', 0.0100m}, {'c', 0.0401m}, {'d', 0.0497m}, {'e', 0.1205m},
        {'f', 0.0100m}, {'g', 0.0120m}, {'h', 0.0080m}, {'i', 0.0584m}, {'j', 0.0040m},
        {'k', 0.0010m}, {'l', 0.0305m}, {'m', 0.0462m}, {'n', 0.0483m}, {'o', 0.1073m},
        {'p', 0.0251m}, {'q', 0.0090m}, {'r', 0.0707m}, {'s', 0.0778m}, {'t', 0.0442m},
        {'u', 0.0460m}, {'v', 0.0130m}, {'w', 0.0005m}, {'x', 0.0030m}, {'y', 0.0005m},
        {'z', 0.0040m}
    };

    private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";

    /// <summary>
    /// Attempts to break the Vigenère cipher by testing different key lengths and using frequency analysis
    /// </summary>
    /// <param name="cipherText">The encrypted text to break</param>
    /// <param name="minKeyLength">Minimum key length to test</param>
    /// <param name="maxKeyLength">Maximum key length to test</param>
    /// <returns>Dictionary with key length as key and possible key as value</returns>
    public Dictionary<int, string> BreakCipher(string cipherText, int minKeyLength, int maxKeyLength)
    {
        ValidateInput(cipherText, minKeyLength, maxKeyLength);

        var results = new Dictionary<int, string>();
        string cleanText = CleanText(cipherText);

        if (string.IsNullOrEmpty(cleanText))
            return results;

        for (int keyLength = minKeyLength; keyLength <= maxKeyLength; keyLength++)
        {
            string possibleKey = FindKeyForLength(cleanText, keyLength);
            results[keyLength] = possibleKey;
        }

        return results;
    }

    /// <summary>
    /// Interactive method to run the Vigenère breaker with console input/output
    /// </summary>
    public void Run()
    {
        PrintWelcomeMessage();

        Console.Write("Text: ");
        string text = Console.ReadLine()?.ToLower() ?? string.Empty;

        Console.Write("Min. Range Key: ");
        if (!int.TryParse(Console.ReadLine(), out int minKeyLength))
        {
            Console.WriteLine("***Invalid minimum key length***");
            return;
        }

        Console.Write("Max. Range Key: ");
        if (!int.TryParse(Console.ReadLine(), out int maxKeyLength))
        {
            Console.WriteLine("***Invalid maximum key length***");
            return;
        }

        try
        {
            var results = BreakCipher(text, minKeyLength, maxKeyLength);

            foreach (var (keyLength, possibleKey) in results)
            {
                Console.WriteLine($"{keyLength} - {possibleKey}");
            }

            Console.WriteLine($"{new string('=', 25)} END {new string('=', 25)}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"***{ex.Message}***");
        }
    }

    private static void ValidateInput(string text, int minKeyLength, int maxKeyLength)
    {
        if (string.IsNullOrEmpty(text))
            throw new ArgumentException("Text cannot be null or empty");

        if (minKeyLength <= 0)
            throw new ArgumentException("Minimum key length must be greater than 0");

        if (maxKeyLength < minKeyLength)
            throw new ArgumentException("Maximum key length must be greater than or equal to minimum key length");
    }

    private static string CleanText(string text)
    {
        var sb = new StringBuilder();
        foreach (char ch in text.ToLower())
        {
            if (Alphabet.Contains(ch))
                sb.Append(ch);
        }
        return sb.ToString();
    }

    private string FindKeyForLength(string cleanText, int keyLength)
    {
        var columns = SplitIntoColumns(cleanText, keyLength);
        var key = new StringBuilder();

        foreach (string column in columns)
        {
            char bestKey = FindBestCaesarKey(column);
            key.Append(bestKey);
        }

        return key.ToString();
    }

    private static List<string> SplitIntoColumns(string text, int keyLength)
    {
        var columns = new List<string>();

        for (int i = 0; i < keyLength; i++)
        {
            var column = new StringBuilder();
            for (int j = i; j < text.Length; j += keyLength)
            {
                column.Append(text[j]);
            }
            columns.Add(column.ToString());
        }

        return columns;
    }

    private char FindBestCaesarKey(string column)
    {
        if (string.IsNullOrEmpty(column))
            return 'a';

        decimal minChiSquare = decimal.MaxValue;
        int bestKey = 0;

        // Test all possible Caesar shifts (0-25)
        for (int k = 0; k < 26; k++)
        {
            string decryptedColumn = ApplyCaesarShift(column, k);
            decimal chiSquare = CalculateChiSquare(decryptedColumn);

            if (chiSquare < minChiSquare)
            {
                minChiSquare = chiSquare;
                bestKey = k;
            }
        }

        return Alphabet[bestKey];
    }

    private static string ApplyCaesarShift(string text, int shift)
    {
        var result = new StringBuilder();

        foreach (char ch in text)
        {
            int index = Alphabet.IndexOf(ch);
            int newIndex = (index - shift + 26) % 26;
            result.Append(Alphabet[newIndex]);
        }

        return result.ToString();
    }

    private static decimal CalculateChiSquare(string text)
    {
        if (string.IsNullOrEmpty(text))
            return decimal.MaxValue;

        // Count letter frequencies
        var letterCount = new Dictionary<char, int>();
        foreach (char letter in Alphabet)
        {
            letterCount[letter] = 0;
        }

        foreach (char ch in text)
        {
            if (letterCount.ContainsKey(ch))
                letterCount[ch]++;
        }

        // Calculate chi-square statistic
        decimal chiSquare = 0;
        decimal textLength = text.Length;

        foreach (char letter in Alphabet)
        {
            decimal observed = letterCount[letter];
            decimal expected = PortugueseFrequency[letter] * textLength;

            if (expected > 0)
            {
                chiSquare += (observed - expected) * (observed - expected) / expected;
            }
        }

        return chiSquare;
    }

    private static void PrintWelcomeMessage()
    {
        Console.WriteLine($"\n{new string('>', 10)} Welcome to Vigenère Break v1.0! {new string('<', 10)}");
    }
}