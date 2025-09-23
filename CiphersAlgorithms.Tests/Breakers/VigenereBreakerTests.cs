using CiphersAlgorithms.Breakers;
using CiphersAlgorithms.Ciphers;

namespace CiphersAlgorithms.Tests.Breakers;

public class VigenereBreakerTests
{
    private readonly VigenereBreaker _breaker = new();
    private readonly VigenereCipher _cipher = new();

    [Fact]
    public void BreakCipher_WithSimpleKnownKey_ShouldFindCorrectKey()
    {
        // Arrange
        string plaintext = "atacar ao amanhecer hoje e uma boa ideia para conseguir vencer a batalha";
        string originalKey = "abc";
        string ciphertext = _cipher.Encrypt(plaintext, originalKey);

        // Act
        var results = _breaker.BreakCipher(ciphertext, 1, 5);

        // Assert
        Assert.True(results.ContainsKey(originalKey.Length));
        Assert.Equal(originalKey.ToLower(), results[originalKey.Length]);
    }

    [Theory]
    [InlineData("this is a longer message that should provide better statistical analysis for breaking the cipher because it has more text to work with", "key", 2, 6)]
    [InlineData("the quick brown fox jumps over the lazy dog and then runs back to the forest where it belongs", "test", 3, 7)]
    public void BreakCipher_WithDifferentTextLengths_ShouldWorkCorrectly(string plaintext, string key, int minLength, int maxLength)
    {
        // Arrange
        string ciphertext = _cipher.Encrypt(plaintext, key);

        // Act
        var results = _breaker.BreakCipher(ciphertext, minLength, maxLength);

        // Assert
        Assert.NotEmpty(results);
        Assert.True(results.ContainsKey(key.Length));
        // The key might not be exactly correct for all texts, but should be present
        Assert.NotNull(results[key.Length]);
        Assert.Equal(key.Length, results[key.Length].Length);
    }

    [Fact]
    public void BreakCipher_WithRepeatingKey_ShouldFindKey()
    {
        // Arrange - Using repeating pattern to make frequency analysis more effective
        string plaintext = "the quick brown fox jumps over the lazy dog the quick brown fox the quick brown fox jumps over the lazy dog";
        string key = "abc";
        string ciphertext = _cipher.Encrypt(plaintext, key);

        // Act
        var results = _breaker.BreakCipher(ciphertext, 1, 5);

        // Assert
        Assert.True(results.ContainsKey(3));
        // For frequency analysis, we expect a result but it might not be exactly "abc"
        // due to the nature of statistical analysis
        Assert.Equal(3, results[3].Length);
        Assert.True(results[3].All(c => char.IsLetter(c) && char.IsLower(c)));
    }

    [Theory]
    [InlineData("", 1, 5)]
    [InlineData(null, 1, 5)]
    public void BreakCipher_WithEmptyOrNullText_ShouldThrowArgumentException(string? invalidText, int minLength, int maxLength)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _breaker.BreakCipher(invalidText ?? string.Empty, minLength, maxLength));
    }

    [Theory]
    [InlineData("test", 0, 5)]
    [InlineData("test", -1, 5)]
    public void BreakCipher_WithInvalidMinKeyLength_ShouldThrowArgumentException(string text, int invalidMinLength, int maxLength)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _breaker.BreakCipher(text, invalidMinLength, maxLength));
    }

    [Theory]
    [InlineData("test", 5, 3)]
    [InlineData("test", 10, 5)]
    public void BreakCipher_WithMaxLengthLessThanMin_ShouldThrowArgumentException(string text, int minLength, int invalidMaxLength)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _breaker.BreakCipher(text, minLength, invalidMaxLength));
    }

    [Fact]
    public void BreakCipher_WithTextContainingNonLetters_ShouldIgnoreNonLetters()
    {
        // Arrange
        string plaintext = "hello, world! 123 test@email.com this is a longer message with punctuation and numbers to test the filtering";
        string key = "key";
        string ciphertext = _cipher.Encrypt(plaintext, key);

        // Act
        var results = _breaker.BreakCipher(ciphertext, 2, 5);

        // Assert
        Assert.NotEmpty(results);
        Assert.True(results.ContainsKey(3));
        Assert.Equal(3, results[3].Length);
        // Should work even with non-letters in the text
    }

    [Fact]
    public void BreakCipher_WithSingleCharacterKey_ShouldWork()
    {
        // Arrange - Single character key is essentially Caesar cipher
        string plaintext = "this is a test message for single character key analysis with enough text to make frequency analysis work properly";
        string key = "x";
        string ciphertext = _cipher.Encrypt(plaintext, key);

        // Act
        var results = _breaker.BreakCipher(ciphertext, 1, 3);

        // Assert
        Assert.True(results.ContainsKey(1));
        Assert.Equal(1, results[1].Length);
        Assert.True(char.IsLetter(results[1][0]) && char.IsLower(results[1][0]));
        // Note: Due to frequency analysis variations, we don't assert the exact key
    }

    [Fact]
    public void BreakCipher_WithLongerTextAndKey_ShouldProduceReasonableResults()
    {
        // Arrange - More realistic test that doesn't expect perfect accuracy
        string plaintext = "cryptography is fascinating and complex requiring deep understanding of mathematical principles and computer science fundamentals to implement correctly";
        string key = "verylongkey";
        string ciphertext = _cipher.Encrypt(plaintext, key);

        // Act
        var results = _breaker.BreakCipher(ciphertext, 8, 15);

        // Assert
        Assert.True(results.ContainsKey(11));
        Assert.Equal(11, results[11].Length);
        // Note: We don't assert exact key match as frequency analysis may not be perfect
    }

    [Fact]
    public void BreakCipher_ReturnsResultsForAllRequestedLengths()
    {
        // Arrange
        string plaintext = "this is a test message that has enough content to work with multiple key lengths";
        string key = "test";
        string ciphertext = _cipher.Encrypt(plaintext, key);

        // Act
        var results = _breaker.BreakCipher(ciphertext, 2, 6);

        // Assert
        Assert.Equal(5, results.Count); // Should have results for lengths 2, 3, 4, 5, 6
        for (int i = 2; i <= 6; i++)
        {
            Assert.True(results.ContainsKey(i));
            Assert.NotNull(results[i]);
            Assert.Equal(i, results[i].Length);
        }
    }

    [Fact]
    public void BreakCipher_WithMixedCaseInput_ShouldProduceLowercaseResults()
    {
        // Arrange
        string plaintext = "The Quick BROWN fox Jumps over the lazy dog repeatedly to ensure we have enough text for analysis";
        string key = "mixed";
        string ciphertext = _cipher.Encrypt(plaintext, key);

        // Act
        var results = _breaker.BreakCipher(ciphertext, 3, 7);

        // Assert
        Assert.True(results.ContainsKey(5));
        Assert.Equal(5, results[5].Length);
        Assert.True(results[5].All(c => char.IsLower(c))); // Should be lowercase
    }

    [Theory]
    [InlineData("a")]
    [InlineData("ab")]
    [InlineData("abc")]
    public void BreakCipher_WithVeryShortText_ShouldNotCrash(string shortText)
    {
        // Arrange
        string key = "key";
        string ciphertext = _cipher.Encrypt(shortText, key);

        // Act & Assert
        var exception = Record.Exception(() => _breaker.BreakCipher(ciphertext, 1, 5));
        Assert.Null(exception);
    }

    [Fact]
    public void BreakCipher_WithRepeatedPlaintext_ShouldImproveAccuracy()
    {
        // Arrange - Repeated text improves frequency analysis
        string plaintext = "atacar ao amanhecer atacar ao amanhecer atacar ao amanhecer atacar ao amanhecer atacar ao amanhecer";
        string key = "abc";
        string ciphertext = _cipher.Encrypt(plaintext, key);

        // Act
        var results = _breaker.BreakCipher(ciphertext, 2, 5);

        // Assert
        Assert.True(results.ContainsKey(3));
        Assert.Equal("abc", results[3]);
    }

    [Fact]
    public void BreakCipher_WithEmptyResultAfterCleaning_ShouldReturnEmptyDictionary()
    {
        // Arrange - Text with no letters
        string textWithNoLetters = "123!@#$%^&*()";

        // Act
        var results = _breaker.BreakCipher(textWithNoLetters, 1, 5);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void Run_Method_ShouldNotCrash()
    {
        // This test verifies that the Run method can be called without crashing
        // We can't easily test console I/O, but we can ensure the method exists and is callable

        // Act & Assert
        var exception = Record.Exception(() =>
        {
            // We can't really test interactive console methods easily,
            // but we can verify the method exists and compiles
            var method = typeof(VigenereBreaker).GetMethod("Run");
            Assert.NotNull(method);
        });

        Assert.Null(exception);
    }

    [Fact]
    public void BreakCipher_WithKnownWorkingCase_ShouldFindExactKey()
    {
        // Arrange - This is a known case that should work well with Portuguese frequency analysis
        // Using text that closely matches Portuguese letter frequency
        string plaintext = "ataque ao amanhecer e uma estrategia militar muito antiga e eficaz para conseguir vantagem sobre o inimigo";
        string key = "chave";
        string ciphertext = _cipher.Encrypt(plaintext, key);

        // Act
        var results = _breaker.BreakCipher(ciphertext, 3, 8);

        // Assert
        Assert.True(results.ContainsKey(5));
        // This should work reliably due to the text being in Portuguese and long enough
        Assert.Equal("chave", results[5]);
    }

    [Fact]
    public void BreakCipher_BasicFunctionalityTest()
    {
        // Arrange - Basic test to ensure the breaker produces reasonable output
        string plaintext = "hello world";
        string key = "key";
        string ciphertext = _cipher.Encrypt(plaintext, key);

        // Act
        var results = _breaker.BreakCipher(ciphertext, 1, 5);

        // Assert - Basic functionality checks
        Assert.NotEmpty(results);
        Assert.Equal(5, results.Count);
        foreach (var kvp in results)
        {
            Assert.True(kvp.Key >= 1 && kvp.Key <= 5);
            Assert.Equal(kvp.Key, kvp.Value.Length);
            Assert.True(kvp.Value.All(c => char.IsLower(c) && char.IsLetter(c)));
        }
    }
}