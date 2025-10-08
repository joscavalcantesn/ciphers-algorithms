// Original Author: Matheus Ribeiro - Python Version
// GitHub: https://github.com/MatheusRibeiro443
// Converted to C# by: Josias Cavalcante
// Conversion Date: 10/09/2025
// Original Repository: https://github.com/MatheusRibeiro443/Ciphers

using System.Text;
using CiphersAlgorithms.Common;

namespace CiphersAlgorithms.Ciphers;

public class VernamCipher : CipherBase<string>
{
    public override string Encrypt(string text, string key)
    {
        ValidateKey(key);
        return ProcessVernam(text, key, CipherMode.Encrypt);
    }

    public override string Decrypt(string text, string key)
    {
        ValidateKey(key);
        return ProcessVernam(text, key, CipherMode.Decrypt);
    }

    public override void ValidateKey(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null, empty or whitespace only");
        }
    }

    private static string ProcessVernam(string text, string key, CipherMode mode)
    {
        if (mode == CipherMode.Decrypt)
        {
            text = ConvertHexToString(text);
        }
        else
        {
            ValidateText(text);
        }

        var result = new StringBuilder(text.Length);
        int keyPosition = 0;

        foreach (char ch in text)
        {
            char keyChar = key[keyPosition % key.Length];
            char processedChar = (char)(ch ^ keyChar);
            result.Append(processedChar);
            keyPosition++;
        }

        return mode == CipherMode.Encrypt 
            ? ConvertStringToHex(result.ToString())
            : result.ToString();
    }

    private static string ConvertHexToString(string hexString)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(hexString))
            {
                throw new ArgumentException("Hex string cannot be null or empty");
            }

            // Remove any whitespace and convert to lowercase
            hexString = hexString.Replace(" ", "").ToLower();
            
            // Validate hex string format
            if (hexString.Length % 2 != 0 || !IsValidHexString(hexString))
            {
                throw new FormatException("Invalid hex string format");
            }

            byte[] bytes = Convert.FromHexString(hexString);
            return Encoding.UTF8.GetString(bytes);
        }
        catch (FormatException ex)
        {
            throw new ArgumentException("Invalid hex string for decryption", ex);
        }
    }

    private static string ConvertStringToHex(string text)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        return Convert.ToHexString(bytes).ToLower();
    }

    private static bool IsValidHexString(string hex)
    {
        return hex.All(c => char.IsDigit(c) || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F'));
    }

    public override void Run()
    {
        try
        {
            PrintWelcomeMessage("Vernam Cipher", "v2.0");

            string text = GetUserInput("Text");
            string key = GetUserInput("Key");
            string action = GetUserInput("Action (enc/dec)").ToLower();

            ValidateKey(key);

            string result = action switch
            {
                "enc" => Encrypt(text, key),
                "dec" => Decrypt(text, key),
                _ => throw new ArgumentException("Invalid action. Use 'enc' or 'dec'")
            };

            Console.WriteLine($"\nResult: {result}");
            PrintSuccess($"{(action == "enc" ? "Encryption" : "Decryption")} completed!");
        }
        catch (ArgumentException ex)
        {
            PrintError(ex.Message);
        }
        catch (Exception ex)
        {
            PrintError($"Unexpected error: {ex.Message}");
        }
    }
}