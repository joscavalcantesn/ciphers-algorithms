// Original Author: Matheus Ribeiro - Python Version
// GitHub: https://github.com/MatheusRibeiro443
// Converted to C# by: Josias Cavalcante
// Conversion Date: 04/09/2025
// Original Repository: https://github.com/MatheusRibeiro443/Ciphers

using System.Text;
using CiphersAlgorithms.Common;
using CiphersAlgorithms.Utilities;

namespace CiphersAlgorithms.Ciphers;

public class VigenereCipher : CipherBase<string>
{
    public override string Encrypt(string text, string key)
    {
        ValidateKey(key);
        return ProcessVigenere(text, key, CipherMode.Encrypt);
    }

    public override string Decrypt(string text, string key)
    {
        ValidateKey(key);
        return ProcessVigenere(text, key, CipherMode.Decrypt);
    }

    public override void ValidateKey(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null, empty or whitespace only");
        }

        if (key.Any(ch => !char.IsLetter(ch)))
        {
            throw new ArgumentException("Key must contain only letters");
        }
    }

    private static string ProcessVigenere(string text, string key, CipherMode mode)
    {
        text = SanitizeText(text);
        ValidateText(text);
        key = key.ToLower();

        var result = new StringBuilder(text.Length);
        int keyPosition = 0;

        foreach (char currentChar in text)
        {
            if (!Alphabet.Contains(currentChar))
            {
                result.Append(currentChar);
                continue;
            }

            char keyChar = key[keyPosition % key.Length];
            int keyIndex = Alphabet.IndexOf(keyChar);
            int textIndex = Alphabet.IndexOf(currentChar);

            int newIndex = mode == CipherMode.Encrypt
                ? (textIndex + keyIndex) % 26
                : (textIndex - keyIndex + 26) % 26;

            result.Append(Alphabet[newIndex]);
            keyPosition++;
        }

        return result.ToString();
    }

    public override void Run()
    {
        try
        {
            PrintWelcomeMessage(
                CipherConfiguration.Messages.GetCipherName(CipherEnums.CipherType.Vigenere),
                "v2.0"
            );

            string text = GetUserInput("Text").ToLower();
            string key = GetUserInput("Key").ToLower();
            string actionString = GetUserInput("Action (enc/dec)").ToLower();

            ValidateKey(key);

            // Usando o ActionConverter para converter string em enum
            var mode = ActionConverter.ToCipherMode(actionString);

            string result = mode == CipherMode.Encrypt
                ? Encrypt(text, key)
                : Decrypt(text, key);

            Console.WriteLine($"\nResult: {result}");
            PrintSuccess(CipherConfiguration.Messages.GetModeMessage(mode));
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