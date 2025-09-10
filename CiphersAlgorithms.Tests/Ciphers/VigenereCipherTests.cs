using CiphersAlgorithms.Ciphers;

namespace CiphersAlgorithms.Tests.Ciphers;

public class VigenereCipherTests
{
    private readonly VigenereCipher _vigenere = new();

    [Theory]
    [InlineData("attack at dawn", "lemon", "lxfopv ef rnhr")]
    [InlineData("hello world", "key", "rijvs uyvjn")]
    [InlineData("cryptography is cool", "secret", "uvagxhyvcglr aw efse")]
    [InlineData("test@mail.com", "code", "vsvx@oolp.ecp")]
    public void Encrypt_ValidInput_ReturnsEncryptedText(string text, string key, string expected)
    {
        // Act
        string result = _vigenere.Encrypt(text, key);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("lxfopv ef rnhr", "lemon", "attack at dawn")]
    [InlineData("rijvs uyvjn", "key", "hello world")]
    [InlineData("uvagxhyvcglr aw efse", "secret", "cryptography is cool")]
    [InlineData("vsvx@oolp.ecp", "code", "test@mail.com")]
    public void Decrypt_ValidInput_ReturnsDecryptedText(string text, string key, string expected)
    {
        // Act
        string result = _vigenere.Decrypt(text, key);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("hello world", "key")]
    [InlineData("attack at dawn", "lemon")]
    [InlineData("secret message", "password")]
    public void EncryptThenDecrypt_ReturnsOriginalText(string originalText, string key)
    {
        // Act
        string encrypted = _vigenere.Encrypt(originalText, key);
        string decrypted = _vigenere.Decrypt(encrypted, key);

        // Assert
        Assert.Equal(originalText.ToLower(), decrypted);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void ValidateKey_EmptyOrNullKey_ThrowsArgumentException(string? invalidKey)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => VigenereCipher.ValidateKey(invalidKey ?? string.Empty));
    }

    [Theory]
    [InlineData("key123")]
    [InlineData("invalid key!")]
    [InlineData("12345")]
    [InlineData("test@key")]
    public void ValidateKey_KeyWithNonLetters_ThrowsArgumentException(string invalidKey)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => VigenereCipher.ValidateKey(invalidKey));
    }

    [Theory]
    [InlineData("validkey")]
    [InlineData("LEMON")]
    [InlineData("Secret")]
    [InlineData("a")] // Chave de um caractere
    [InlineData("verylongkeythatworks")] // Chave longa
    public void ValidateKey_ValidKey_DoesNotThrow(string validKey)
    {
        // Act & Assert (não deve lançar exceção)
        var exception = Record.Exception(() => VigenereCipher.ValidateKey(validKey));
        Assert.Null(exception);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Encrypt_EmptyOrNullText_ThrowsArgumentException(string? invalidText)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _vigenere.Encrypt(invalidText ?? string.Empty, "key"));
    }

    [Theory]
    [InlineData(123)]
    [InlineData(3.14)]
    [InlineData(true)]
    public void Encrypt_InvalidKeyType_ThrowsArgumentException(object invalidKey)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _vigenere.Encrypt("hello", invalidKey));
    }

    [Fact]
    public void Encrypt_Decrypt_WithKeyLongerThanText_WorksCorrectly()
    {
        // Arrange
        string text = "hi";
        string longKey = "verylongkey";

        // Act
        string encrypted = _vigenere.Encrypt(text, longKey);
        string decrypted = _vigenere.Decrypt(encrypted, longKey);

        // Assert
        Assert.Equal(text, decrypted);
    }
}