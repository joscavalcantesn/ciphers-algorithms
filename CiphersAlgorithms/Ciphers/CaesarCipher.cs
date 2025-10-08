// Original Author: Matheus Ribeiro - Python Version
// GitHub: https://github.com/MatheusRibeiro443
// Converted to C# by: Josias Cavalcante
// Conversion Date: 04/09/2025
// Original Repository: https://github.com/MatheusRibeiro443/Ciphers

using System.Text;
using CiphersAlgorithms.Common;

namespace CiphersAlgorithms.Ciphers;

public class CaesarCipher : CipherBase<int>, IBreakableCipher<int>
{
    private const int MaxKey = 26;
    private const int MinKey = 1;

    public override string Encrypt(string text, int key)
    {
        ValidateKey(key);
        return ProcessCaesar(text, key, CipherMode.Encrypt);
    }

    public override string Decrypt(string text, int key)
    {
        ValidateKey(key);
        return ProcessCaesar(text, key, CipherMode.Decrypt);
    }

    public override void ValidateKey(int key)
    {
        if (key < MinKey || key > MaxKey)
        {
            throw new ArgumentOutOfRangeException(nameof(key),
                $"Key must be between {MinKey} and {MaxKey}");
        }
    }

    public IEnumerable<(int Key, string Text)> Break(string cipherText)
    {
        var results = new List<(int, string)>();

        for (int currentKey = MinKey; currentKey <= MaxKey; currentKey++)
        {
            try
            {
                string decryptedText = ProcessCaesar(cipherText, currentKey, CipherMode.Decrypt);
                results.Add((currentKey, decryptedText));
            }
            catch (ArgumentException)
            {
                // Skip invalid keys
                continue;
            }
        }

        return results;
    }

    private static string ProcessCaesar(string text, int key, CipherMode mode)
    {
        text = SanitizeText(text);
        ValidateText(text);

        var result = new StringBuilder(text.Length);

        foreach (char ch in text)
        {
            if (Alphabet.Contains(ch))
            {
                int index = Alphabet.IndexOf(ch);

                index = mode == CipherMode.Encrypt
                    ? (index + key) % MaxKey
                    : (index - key + MaxKey) % MaxKey;

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
        try
        {
            PrintWelcomeMessage("Caesar Cipher", "v2.0");

            string text = GetUserInput("Text").ToLower();
            string mode = GetUserInput("Mode (enc/dec/break)").ToLower();

            switch (mode)
            {
                case "break":
                    RunBreakMode(text);
                    break;
                case "enc":
                case "dec":
                    RunCipherMode(text, mode);
                    break;
                default:
                    PrintError("Invalid mode. Use 'enc', 'dec', or 'break'");
                    break;
            }
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

    private void RunBreakMode(string text)
    {
        Console.WriteLine("\n[BREAKING] Breaking Caesar Cipher...\n");

        var results = Break(text);
        foreach (var (key, decryptedText) in results)
        {
            Console.WriteLine($"Key {key:00}: {decryptedText}");
        }

        PrintSuccess("Break analysis completed!");
    }

    private void RunCipherMode(string text, string mode)
    {
        int key = int.Parse(GetUserInput($"Key ({MinKey}-{MaxKey})"));

        string result = mode == "enc"
            ? Encrypt(text, key)
            : Decrypt(text, key);

        Console.WriteLine($"\nResult: {result}");
        PrintSuccess($"{(mode == "enc" ? "Encryption" : "Decryption")} completed!");
    }
}