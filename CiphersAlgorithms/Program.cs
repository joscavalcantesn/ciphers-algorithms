using CiphersAlgorithms.Breakers;
using CiphersAlgorithms.Ciphers;

// Running Ciphers
//var caesarCipher = new CaesarCipher();
var vigenereCipher = new VigenereCipher();
//var vernamCipher = new VernamCipher();

//caesarCipher.Run();
vigenereCipher.Run();
//vernamCipher.Run();

// Breaking Vigenere Cipher
var vigenereBreaker = new VigenereBreaker();
vigenereBreaker.Run();