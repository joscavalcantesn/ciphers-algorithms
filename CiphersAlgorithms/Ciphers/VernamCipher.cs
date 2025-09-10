// Original Author: Matheus Ribeiro - Python Version
// GitHub: https://github.com/MatheusRibeiro443
// Converted to C# by: Josias Cavalcante
// Conversion Date: 10/09/2025
// Original Repository: https://github.com/MatheusRibeiro443/Ciphers

using System.Text;

namespace CiphersAlgorithms.Ciphers;

public class VernamCipher : CipherBase
{
    public override string Encrypt(string text, object key)
    {
        if (key is not string stringKey) throw new ArgumentException("Key must be a string");
        ValidateKey(stringKey);
        return ProcessVernam(text, stringKey, "enc");
    }

    public override string Decrypt(string text, object key)
    {
        if (key is not string stringKey) throw new ArgumentException("Key must be a string");
        ValidateKey(stringKey);
        return ProcessVernam(text, stringKey, "dec");
    }

    public static void ValidateKey(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("Key cannot be null or empty");
        }
    }

    private static string ProcessVernam(string text, string key, string action)
    {
        ValidateText(text);

        if (action == "dec")
        {
            text = HexToString(text);
        }

        StringBuilder result = new();
        int keyPosition = 0;

        foreach (char ch in text)
        {
            char keyChar = key[keyPosition % key.Length];
            char processedChar = (char)(ch ^ keyChar);
            result.Append(processedChar);
            keyPosition++;
        }

        string processedText = result.ToString();

        if (action == "enc")
        {
            processedText = StringToHex(processedText);
        }

        return processedText;
    }

    private static string StringToHex(string text)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        return Convert.ToHexStringLower(bytes);
    }

    private static string HexToString(string hex)
    {
        try
        {
            if (hex.Length % 2 != 0)
            {
                throw new ArgumentException("Invalid hex string length");
            }

            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                string hexByte = hex.Substring(i * 2, 2);
                bytes[i] = Convert.ToByte(hexByte, 16);
            }
            return Encoding.UTF8.GetString(bytes);
        }
        catch
        {
            throw new ArgumentException("Invalid hex string for decryption");
        }
    }

    public override void Run()
    {
        PrintWelcomeMessage("Vernam Cipher", "v1.0");

        Console.Write("Text: ");
        string text = Console.ReadLine() ?? string.Empty;

        Console.Write("Key: ");
        string key = Console.ReadLine() ?? string.Empty;

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