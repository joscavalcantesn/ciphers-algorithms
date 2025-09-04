// Original Author: Matheus Ribeiro - Python Version
// GitHub: https://github.com/MatheusRibeiro443
// Converted to C# by: Josias Cavalcante
// Conversion Date: 04/09/2025
// Original Repository: https://github.com/MatheusRibeiro443/Ciphers

using System.Text;

namespace CiphersAlgorithms.Ciphers;

public class CaesarCipher : CipherBase
{
    public override string Encrypt(string text, object key)
    {
        if (key is not int intKey) throw new ArgumentException("Key must be an integer");
        return ProcessCaesar(text, intKey, "enc");
    }

    public override string Decrypt(string text, object key)
    {
        if (key is not int intKey) throw new ArgumentException("Key must be an integer");
        return ProcessCaesar(text, intKey, "dec");
    }

    public List<(int Key, string Text)> Break(string text)
    {
        var results = new List<(int, string)>();

        for (int currentKey = 1; currentKey <= 26; currentKey++)
        {
            string decryptedText = ProcessCaesar(text, currentKey, "dec");
            results.Add((currentKey, decryptedText));
        }

        return results;
    }

    private static string ProcessCaesar(string text, int key, string mode)
    {
        text = SanitizeText(text);
        ValidateText(text);

        StringBuilder result = new();

        foreach (char ch in text)
        {
            if (Alphabet.Contains(ch))
            {
                int index = Alphabet.IndexOf(ch);

                index = mode == "enc"
                    ? (index + key) % 26
                    : (index - key) % 26;

                if (index < 0) index += 26;

                result.Append(Alphabet[index]);
            }
            else
            {
                result.Append(ch);
            }
        }

        return result.ToString();
    }

    public override void Run()
    {
        PrintWelcomeMessage("Caesar Cipher", "v1.0");

        Console.Write("Text: ");
        string text = Console.ReadLine()?.ToLower() ?? string.Empty;

        Console.Write("Mode (enc/dec/break): ");
        string mode = Console.ReadLine() ?? string.Empty;

        int key = 0;

        if (mode != "break")
        {
            Console.Write("Key(1-26): ");
            int.TryParse(Console.ReadLine(), out key);
        }

        if (mode == "break")
        {
            var results = Break(text);
            foreach (var (resultKey, resultText) in results)
            {
                Console.WriteLine($"Key: {resultKey:00} - * {resultText} *");
            }
        }
        else if (mode == "enc")
        {
            string result = Encrypt(text, key);
            Console.WriteLine(result);
        }
        else if (mode == "dec")
        {
            string result = Decrypt(text, key);
            Console.WriteLine(result);
        }
        else
        {
            Console.WriteLine("***mode error***");
        }
    }
}