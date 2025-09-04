// Original Author: Matheus Ribeiro - Python Version
// GitHub: https://github.com/MatheusRibeiro443
// Converted to C# by: Josias Cavalcante
// Conversion Date: 04/09/2025
// Original Repository: https://github.com/MatheusRibeiro443/Ciphers

using System.Text;

namespace CiphersAlgorithms.Ciphers;

public class VigenereCipher : CipherBase
{
    public override string Encrypt(string text, object key)
    {
        if (key is not string stringKey) throw new ArgumentException("Key must be a string");
        ValidateKey(stringKey);
        return ProcessVigenere(text, stringKey, "enc");
    }

    public override string Decrypt(string text, object key)
    {
        if (key is not string stringKey) throw new ArgumentException("Key must be a string");
        ValidateKey(stringKey);
        return ProcessVigenere(text, stringKey, "dec");
    }

    public static void ValidateKey(string key)
    {
        if (string.IsNullOrEmpty(key) || key.Any(ch => !char.IsLetter(ch)))
        {
            throw new ArgumentException("Key must contain only letters");
        }
    }

    private static string ProcessVigenere(string text, string key, string action)
    {
        text = SanitizeText(text);
        ValidateText(text);
        key = key.ToLower();

        StringBuilder result = new();
        int keyPosition = 0;

        for (int i = 0; i < text.Length; i++)
        {
            char currentChar = text[i];

            if (!Alphabet.Contains(currentChar))
            {
                result.Append(currentChar);
                continue;
            }

            char keyChar = key[keyPosition % key.Length];
            int keyIndex = Alphabet.IndexOf(keyChar);
            int textIndex = Alphabet.IndexOf(currentChar);

            int newIndex;
            if (action == "enc")
            {
                newIndex = (textIndex + keyIndex) % 26;
            }
            else
            {
                newIndex = (textIndex - keyIndex + 26) % 26;
            }

            result.Append(Alphabet[newIndex]);
            keyPosition++;
        }

        return result.ToString();
    }

    public override void Run()
    {
        PrintWelcomeMessage("Vigenére Cipher", "v1.0");

        Console.Write("Text: ");
        string text = Console.ReadLine()?.ToLower() ?? string.Empty;

        Console.Write("Key: ");
        string key = Console.ReadLine()?.ToLower() ?? string.Empty;

        try
        {
            ValidateKey(key);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"***{ex.Message}***");
            return;
        }

        Console.Write("Action (enc/dec): ");
        string action = Console.ReadLine() ?? string.Empty;

        try
        {
            string result = action == "enc"
                ? Encrypt(text, key)
                : action == "dec"
                    ? Decrypt(text, key)
                    : throw new ArgumentException("Invalid action");

            Console.WriteLine(result);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"***{ex.Message}***");
        }
    }
}