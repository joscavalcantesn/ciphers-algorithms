namespace CiphersAlgorithms.Common;

/// <summary>
/// Enum representing cipher operation modes
/// </summary>
public enum CipherMode
{
    Encrypt,
    Decrypt
}

/// <summary>
/// Common enums and constants for cipher operations
/// </summary>
public static class CipherEnums
{
    /// <summary>
    /// Supported cipher algorithms
    /// </summary>
    public enum CipherType
    {
        Caesar,
        Vigenere,
        Vernam
    }

    /// <summary>
    /// User interface actions
    /// </summary>
    public enum UIAction
    {
        Encrypt,
        Decrypt,
        Break,
        Quit
    }
}