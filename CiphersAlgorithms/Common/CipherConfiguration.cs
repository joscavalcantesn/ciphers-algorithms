namespace CiphersAlgorithms.Common;

/// <summary>
/// Configuration settings for cipher algorithms
/// </summary>
public static class CipherConfiguration
{
    public const string DefaultAlphabet = "abcdefghijklmnopqrstuvwxyz";
    public const int CaesarMinKey = 1;
    public const int CaesarMaxKey = 26;
    
    // Console colors for different message types
    public static class Colors
    {
        public static readonly ConsoleColor Error = ConsoleColor.Red;
        public static readonly ConsoleColor Success = ConsoleColor.Green;
        public static readonly ConsoleColor Warning = ConsoleColor.Yellow;
        public static readonly ConsoleColor Info = ConsoleColor.Cyan;
    }
    
    // UI Messages
    public static class Messages
    {
        public const string InvalidMode = "Invalid mode selected";
        public const string KeyRequired = "Key is required";
        public const string TextRequired = "Text is required";
        public const string OperationCompleted = "Operation completed successfully";
        
        // Cipher-specific messages
        public static string GetModeMessage(CipherMode mode) => mode switch
        {
            CipherMode.Encrypt => "Encryption completed successfully!",
            CipherMode.Decrypt => "Decryption completed successfully!",
            _ => OperationCompleted
        };
        
        public static string GetCipherName(CipherEnums.CipherType cipher) => cipher switch
        {
            CipherEnums.CipherType.Caesar => "Caesar Cipher",
            CipherEnums.CipherType.Vigenere => "Vigenère Cipher",
            CipherEnums.CipherType.Vernam => "Vernam Cipher",
            _ => "Unknown Cipher"
        };
    }
}