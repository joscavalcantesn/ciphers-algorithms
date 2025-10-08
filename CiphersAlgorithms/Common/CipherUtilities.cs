using System.Text;

namespace CiphersAlgorithms.Common;

/// <summary>
/// Utility methods for cipher operations
/// </summary>
public static class CipherUtilities
{
    /// <summary>
    /// Validates if text contains only valid characters for cipher operations
    /// </summary>
    public static bool IsValidCipherText(string text, string allowedChars = CipherConfiguration.DefaultAlphabet)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;

        return text.ToLower().All(c => allowedChars.Contains(c) || char.IsWhiteSpace(c) || char.IsPunctuation(c));
    }

    /// <summary>
    /// Calculates the Index of Coincidence for a given text
    /// Useful for cryptanalysis
    /// </summary>
    public static double CalculateIndexOfCoincidence(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0;

        text = text.ToLower().Where(char.IsLetter).Aggregate(new StringBuilder(), (sb, c) => sb.Append(c)).ToString();

        if (text.Length < 2)
            return 0;

        var frequencies = new int[26];
        foreach (char c in text)
        {
            if (c >= 'a' && c <= 'z')
                frequencies[c - 'a']++;
        }

        double ic = 0;
        int n = text.Length;

        for (int i = 0; i < 26; i++)
        {
            ic += frequencies[i] * (frequencies[i] - 1);
        }

        return ic / (n * (n - 1));
    }

    /// <summary>
    /// Calculates frequency analysis of characters in text
    /// </summary>
    public static Dictionary<char, double> CalculateFrequencyAnalysis(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return [];

        text = text.ToLower();
        var letterCount = text.Count(char.IsLetter);

        if (letterCount == 0)
            return [];

        return text
            .Where(char.IsLetter)
            .GroupBy(c => c)
            .ToDictionary(g => g.Key, g => (double)g.Count() / letterCount);
    }

    /// <summary>
    /// Estimates if text is likely English based on frequency analysis
    /// </summary>
    public static bool IsLikelyEnglish(string text, double threshold = 0.04)
    {
        var ic = CalculateIndexOfCoincidence(text);
        return ic >= threshold && ic <= 0.08; // English IC is around 0.067
    }
}