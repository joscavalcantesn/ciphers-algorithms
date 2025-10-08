using CiphersAlgorithms.Ciphers;
using CiphersAlgorithms.Common;

namespace CiphersAlgorithms.Factories;

/// <summary>
/// Factory for creating cipher instances
/// </summary>
public static class CipherFactory
{
    private static readonly Dictionary<string, Func<ICipherUI>> CipherCreators = new()
    {
        { "caesar", () => new CaesarCipher() },
        { "vigenere", () => new VigenereCipher() },
        { "vernam", () => new VernamCipher() }
    };

    private static readonly Dictionary<CipherEnums.CipherType, string> CipherTypeToString = new()
    {
        { CipherEnums.CipherType.Caesar, "caesar" },
        { CipherEnums.CipherType.Vigenere, "vigenere" },
        { CipherEnums.CipherType.Vernam, "vernam" }
    };
    
    /// <summary>
    /// Gets all available cipher names
    /// </summary>
    public static IEnumerable<string> GetAvailableCiphers()
    {
        return CipherCreators.Keys;
    }
    
    /// <summary>
    /// Gets all available cipher types
    /// </summary>
    public static IEnumerable<CipherEnums.CipherType> GetAvailableCipherTypes()
    {
        return CipherTypeToString.Keys;
    }
    
    /// <summary>
    /// Creates a cipher instance by name
    /// </summary>
    public static ICipherUI CreateCipher(string cipherName)
    {
        if (string.IsNullOrWhiteSpace(cipherName))
            throw new ArgumentException("Cipher name cannot be null or empty");
            
        var normalizedName = cipherName.ToLower().Trim();
        
        if (!CipherCreators.TryGetValue(normalizedName, out var creator))
        {
            var available = string.Join(", ", GetAvailableCiphers());
            throw new ArgumentException($"Unknown cipher '{cipherName}'. Available ciphers: {available}");
        }
        
        return creator();
    }
    
    /// <summary>
    /// Creates a cipher instance by type
    /// </summary>
    public static ICipherUI CreateCipher(CipherEnums.CipherType cipherType)
    {
        if (!CipherTypeToString.TryGetValue(cipherType, out var cipherName))
        {
            throw new ArgumentException($"Unsupported cipher type: {cipherType}");
        }
        
        return CreateCipher(cipherName);
    }
    
    /// <summary>
    /// Checks if a cipher is available
    /// </summary>
    public static bool IsAvailable(string cipherName)
    {
        if (string.IsNullOrWhiteSpace(cipherName))
            return false;
            
        return CipherCreators.ContainsKey(cipherName.ToLower().Trim());
    }
    
    /// <summary>
    /// Checks if a cipher type is available
    /// </summary>
    public static bool IsAvailable(CipherEnums.CipherType cipherType)
    {
        return CipherTypeToString.ContainsKey(cipherType);
    }
}