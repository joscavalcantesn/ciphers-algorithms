using CiphersAlgorithms.Common;

namespace CiphersAlgorithms.Utilities;

/// <summary>
/// Utility methods for converting between UI strings and enums
/// </summary>
public static class ActionConverter
{
    private static readonly Dictionary<string, CipherMode> StringToCipherMode = new()
    {
        { "enc", CipherMode.Encrypt },
        { "encrypt", CipherMode.Encrypt },
        { "encryption", CipherMode.Encrypt },
        { "dec", CipherMode.Decrypt },
        { "decrypt", CipherMode.Decrypt },
        { "decryption", CipherMode.Decrypt }
    };

    private static readonly Dictionary<string, CipherEnums.UIAction> StringToUIAction = new()
    {
        { "enc", CipherEnums.UIAction.Encrypt },
        { "encrypt", CipherEnums.UIAction.Encrypt },
        { "dec", CipherEnums.UIAction.Decrypt },
        { "decrypt", CipherEnums.UIAction.Decrypt },
        { "break", CipherEnums.UIAction.Break },
        { "quit", CipherEnums.UIAction.Quit },
        { "q", CipherEnums.UIAction.Quit },
        { "exit", CipherEnums.UIAction.Quit }
    };

    /// <summary>
    /// Converts a string action to CipherMode
    /// </summary>
    public static CipherMode ToCipherMode(string action)
    {
        if (string.IsNullOrWhiteSpace(action))
            throw new ArgumentException("Action cannot be null or empty");

        var normalizedAction = action.ToLower().Trim();
        
        if (!StringToCipherMode.TryGetValue(normalizedAction, out var mode))
        {
            var validActions = string.Join(", ", StringToCipherMode.Keys);
            throw new ArgumentException($"Invalid action '{action}'. Valid actions: {validActions}");
        }

        return mode;
    }

    /// <summary>
    /// Converts a string action to UIAction
    /// </summary>
    public static CipherEnums.UIAction ToUIAction(string action)
    {
        if (string.IsNullOrWhiteSpace(action))
            throw new ArgumentException("Action cannot be null or empty");

        var normalizedAction = action.ToLower().Trim();
        
        if (!StringToUIAction.TryGetValue(normalizedAction, out var uiAction))
        {
            var validActions = string.Join(", ", StringToUIAction.Keys);
            throw new ArgumentException($"Invalid action '{action}'. Valid actions: {validActions}");
        }

        return uiAction;
    }

    /// <summary>
    /// Checks if a string is a valid cipher mode action
    /// </summary>
    public static bool IsValidCipherMode(string action)
    {
        if (string.IsNullOrWhiteSpace(action))
            return false;

        return StringToCipherMode.ContainsKey(action.ToLower().Trim());
    }

    /// <summary>
    /// Checks if a string is a valid UI action
    /// </summary>
    public static bool IsValidUIAction(string action)
    {
        if (string.IsNullOrWhiteSpace(action))
            return false;

        return StringToUIAction.ContainsKey(action.ToLower().Trim());
    }

    /// <summary>
    /// Gets all valid cipher mode strings
    /// </summary>
    public static IEnumerable<string> GetValidCipherModeStrings()
    {
        return StringToCipherMode.Keys;
    }

    /// <summary>
    /// Gets all valid UI action strings
    /// </summary>
    public static IEnumerable<string> GetValidUIActionStrings()
    {
        return StringToUIAction.Keys;
    }
}