using CiphersAlgorithms.Ciphers;

namespace CiphersAlgorithms.Tests.Ciphers;

public class VernamCipherTests
{
    private readonly VernamCipher _vernam = new();

    [Theory]
    [InlineData("hello", "key", "030015070a")]
    [InlineData("attack", "secret", "12111713061f")]
    [InlineData("test", "123", "45574045")]
    [InlineData("message", "password", "1d040000160817")]
    public void Encrypt_ValidInput_ReturnsHexString(string text, string key, string expectedHex)
    {
        // Act
        string result = _vernam.Encrypt(text, key);

        // Assert
        Assert.Equal(expectedHex, result);
    }

    [Theory]
    [InlineData("030015070a", "key", "hello")]
    [InlineData("12111713061f", "secret", "attack")]
    [InlineData("45574045", "123", "test")]
    [InlineData("1d040000160817", "password", "message")]
    public void Decrypt_ValidInput_ReturnsOriginalText(string hexText, string key, string expected)
    {
        // Act
        string result = _vernam.Decrypt(hexText, key);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("hello world", "secretkey")]
    [InlineData("attack at dawn", "lemon")]
    [InlineData("cryptography", "password123")]
    [InlineData("test message with spaces", "key")]
    public void EncryptThenDecrypt_ReturnsOriginalText(string originalText, string key)
    {
        // Act
        string encrypted = _vernam.Encrypt(originalText, key);
        string decrypted = _vernam.Decrypt(encrypted, key);

        // Assert
        Assert.Equal(originalText, decrypted);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void ValidateKey_EmptyOrNullKey_ThrowsArgumentException(string? invalidKey)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => VernamCipher.ValidateKey(invalidKey!));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Encrypt_EmptyOrNullText_ThrowsArgumentException(string? invalidText)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _vernam.Encrypt(invalidText ?? string.Empty, "key"));
    }

    [Theory]
    [InlineData(123)]
    [InlineData(3.14)]
    [InlineData(true)]
    public void Encrypt_InvalidKeyType_ThrowsArgumentException(object invalidKey)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _vernam.Encrypt("hello", invalidKey));
    }

    [Fact]
    public void Decrypt_InvalidHexString_ThrowsArgumentException()
    {
        // Arrange
        string invalidHex = "notvalidhex";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _vernam.Decrypt(invalidHex, "key"));
    }

    [Fact]
    public void Encrypt_Decrypt_WithSpecialCharacters_WorksCorrectly()
    {
        // Arrange
        string text = "hello@world.com!123";
        string key = "secretkey";

        // Act
        string encrypted = _vernam.Encrypt(text, key);
        string decrypted = _vernam.Decrypt(encrypted, key);

        // Assert
        Assert.Equal(text, decrypted);
    }

    [Fact]
    public void Encrypt_Decrypt_WithUnicodeCharacters_WorksCorrectly()
    {
        // Arrange
        string text = "café naïve façade";
        string key = "unicodekey";

        // Act
        string encrypted = _vernam.Encrypt(text, key);
        string decrypted = _vernam.Decrypt(encrypted, key);

        // Assert
        Assert.Equal(text, decrypted);
    }

    [Fact]
    public void Encrypt_Decrypt_WithKeyLongerThanText_WorksCorrectly()
    {
        // Arrange
        string text = "hi";
        string longKey = "verylongkeythatexceedstextlength";

        // Act
        string encrypted = _vernam.Encrypt(text, longKey);
        string decrypted = _vernam.Decrypt(encrypted, longKey);

        // Assert
        Assert.Equal(text, decrypted);
    }

    [Fact]
    public void Encrypt_Decrypt_WithKeyShorterThanText_WorksCorrectly()
    {
        // Arrange
        string text = "this is a long message that requires key repetition";
        string shortKey = "key";

        // Act
        string encrypted = _vernam.Encrypt(text, shortKey);
        string decrypted = _vernam.Decrypt(encrypted, shortKey);

        // Assert
        Assert.Equal(text, decrypted);
    }
}