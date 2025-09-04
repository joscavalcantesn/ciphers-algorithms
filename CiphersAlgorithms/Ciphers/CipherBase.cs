namespace CiphersAlgorithms.Ciphers;

public abstract class CipherBase
{
    protected const string Alphabet = "abcdefghijklmnopqrstuvwxyz";

    public abstract string Encrypt(string text, object key);
    public abstract string Decrypt(string text, object key);
    public abstract void Run();

    protected static string SanitizeText(string text)
    {
        return text?.ToLower() ?? string.Empty;
    }

    protected static void ValidateText(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            throw new ArgumentException("Text cannot be null or empty");
        }
    }

    protected static void PrintWelcomeMessage(string cipherName, string version)
    {
        Console.WriteLine($"\n{new string('>', 10)} Welcome to {cipherName} {version}! {new string('<', 10)}");
    }
}
