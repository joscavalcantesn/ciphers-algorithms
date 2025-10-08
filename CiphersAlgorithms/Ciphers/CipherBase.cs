namespace CiphersAlgorithms.Ciphers;

// Interface para cifras que podem ser quebradas
public interface IBreakableCipher<TKey>
{
    IEnumerable<(TKey Key, string Text)> Break(string cipherText);
}

// Interface base para todas as cifras
public interface ICipher<TKey>
{
    string Encrypt(string text, TKey key);
    string Decrypt(string text, TKey key);
    void ValidateKey(TKey key);
}

// Interface para UI de cifras
public interface ICipherUI
{
    void Run();
}

// Classe base abstrata com generics
public abstract class CipherBase<TKey> : ICipher<TKey>, ICipherUI
{
    protected const string Alphabet = "abcdefghijklmnopqrstuvwxyz";

    public abstract string Encrypt(string text, TKey key);
    public abstract string Decrypt(string text, TKey key);
    public abstract void ValidateKey(TKey key);
    public abstract void Run();

    protected static string SanitizeText(string text)
    {
        return text?.ToLower().Trim() ?? string.Empty;
    }

    protected static void ValidateText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Text cannot be null, empty or whitespace only");
        }
    }

    protected static void PrintWelcomeMessage(string cipherName, string version)
    {
        var border = new string('=', 50);
        Console.WriteLine($"\n{border}");
        Console.WriteLine($"    Welcome to {cipherName} {version}!");
        Console.WriteLine($"{border}\n");
    }

    protected static void PrintError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERROR] {message}");
        Console.ResetColor();
    }

    protected static void PrintSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[SUCCESS] {message}");
        Console.ResetColor();
    }

    // Método auxiliar para input seguro
    protected static string GetUserInput(string prompt, bool required = true)
    {
        Console.Write($"{prompt}: ");
        string input = Console.ReadLine() ?? string.Empty;

        if (required && string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentException($"{prompt} is required");
        }

        return input;
    }
}

// Mantém compatibilidade com código existente
public abstract class CipherBase : CipherBase<object>
{
    public override string Encrypt(string text, object key)
    {
        ValidateKey(key);
        return EncryptCore(text, key);
    }

    public override string Decrypt(string text, object key)
    {
        ValidateKey(key);
        return DecryptCore(text, key);
    }

    protected abstract string EncryptCore(string text, object key);
    protected abstract string DecryptCore(string text, object key);
}
