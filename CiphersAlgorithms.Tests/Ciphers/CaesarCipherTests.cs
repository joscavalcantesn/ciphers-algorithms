using CiphersAlgorithms.Ciphers;

namespace CiphersAlgorithms.Tests.Ciphers;

public class CaesarCipherTests
{
    private readonly CaesarCipher _caesar = new();

    [Theory]
    [InlineData("hello world", 3, "khoor zruog")]
    [InlineData("attack at dawn", 5, "fyyfhp fy ifbs")]
    [InlineData("programming is fun", 7, "wyvnyhttpun pz mbu")]
    [InlineData("test@123.com", 4, "xiwx@123.gsq")]
    [InlineData("HELLO WORLD", 3, "khoor zruog")]
    public void Encrypt_ValidInput_ReturnsEncryptedText(string text, int key, string expected)
    {
        // Act
        string result = _caesar.Encrypt(text, key);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("khoor zruog", 3, "hello world")]
    [InlineData("fyyfhp fy ifbs", 5, "attack at dawn")]
    [InlineData("wyvnyhttpun pz mbu", 7, "programming is fun")]
    [InlineData("xiwx@123.gsq", 4, "test@123.com")]
    public void Decrypt_ValidInput_ReturnsDecryptedText(string text, int key, string expected)
    {
        // Act
        string result = _caesar.Decrypt(text, key);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("hello world", 3)]
    [InlineData("attack at dawn", 5)]
    [InlineData("secret message", 13)]
    public void EncryptThenDecrypt_ReturnsOriginalText(string originalText, int key)
    {
        // Act
        string encrypted = _caesar.Encrypt(originalText, key);
        string decrypted = _caesar.Decrypt(encrypted, key);

        // Assert
        Assert.Equal(originalText.ToLower().Trim(), decrypted);
    }

    [Fact]
    public void Break_EncryptedText_ContainsOriginalText()
    {
        // Arrange
        string originalText = "hello world";
        int originalKey = 3;
        string encryptedText = _caesar.Encrypt(originalText, originalKey);

        // Act
        var possibilities = _caesar.Break(encryptedText);

        // Assert
        Assert.Contains(possibilities, p => p.Text == originalText.ToLower().Trim());
        Assert.Contains(possibilities, p => p.Key == originalKey);
    }

    [Fact]
    public void Break_ReturnsAll26Possibilities()
    {
        // Arrange
        string text = "test message";

        // Act
        var possibilities = _caesar.Break(text);

        // Assert
        Assert.Equal(26, possibilities.Count());
        Assert.All(possibilities, p => Assert.InRange(p.Key, 1, 26));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Encrypt_EmptyOrWhitespaceText_ThrowsArgumentException(string invalidText)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _caesar.Encrypt(invalidText, 3));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(27)]
    [InlineData(100)]
    public void Encrypt_InvalidKey_ThrowsArgumentOutOfRangeException(int invalidKey)
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => _caesar.Encrypt("hello", invalidKey));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(13)]
    [InlineData(26)]
    public void ValidateKey_ValidKey_DoesNotThrow(int validKey)
    {
        // Act & Assert
        var exception = Record.Exception(() => _caesar.ValidateKey(validKey));
        Assert.Null(exception);
    }
}