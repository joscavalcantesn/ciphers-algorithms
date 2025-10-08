using CiphersAlgorithms.Breakers;
using CiphersAlgorithms.Common;
using CiphersAlgorithms.Factories;
using CiphersAlgorithms.Utilities;

try
{
    ShowWelcomeMessage();

    while (true)
    {
        ShowMainMenu();
        var choice = GetUserChoice();

        if (IsQuitAction(choice))
        {
            ShowGoodbyeMessage();
            break;
        }

        HandleUserChoice(choice);

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }
}
catch (Exception ex)
{
    Console.ForegroundColor = CipherConfiguration.Colors.Error;
    Console.WriteLine($"[ERROR] Unexpected error: {ex.Message}");
    Console.ResetColor();
}

void ShowWelcomeMessage()
{
    Console.Clear();
    var border = new string('=', 60);
    Console.ForegroundColor = CipherConfiguration.Colors.Info;
    Console.WriteLine(border);
    Console.WriteLine("    CIPHER ALGORITHMS - C# Version 2.0");
    Console.WriteLine("    Converted from Python by Josias Cavalcante");
    Console.WriteLine("    Original by Matheus Ribeiro");
    Console.WriteLine(border);
    Console.ResetColor();
    Console.WriteLine();
}

void ShowMainMenu()
{
    Console.WriteLine("Main Menu:");
    Console.WriteLine($"1. {CipherConfiguration.Messages.GetCipherName(CipherEnums.CipherType.Caesar)}");
    Console.WriteLine($"2. {CipherConfiguration.Messages.GetCipherName(CipherEnums.CipherType.Vigenere)}");
    Console.WriteLine($"3. {CipherConfiguration.Messages.GetCipherName(CipherEnums.CipherType.Vernam)}");
    Console.WriteLine("4. Vigenere Breaker");
    Console.WriteLine("Q. Quit");
    Console.WriteLine();
}

string GetUserChoice()
{
    Console.Write("Choose an option: ");
    return Console.ReadLine()?.ToLower().Trim() ?? string.Empty;
}

bool IsQuitAction(string choice)
{
    return ActionConverter.IsValidUIAction(choice) &&
           ActionConverter.ToUIAction(choice) == CipherEnums.UIAction.Quit;
}

void HandleUserChoice(string choice)
{
    try
    {
        switch (choice)
        {
            case "1":
                RunCipher(CipherEnums.CipherType.Caesar);
                break;
            case "2":
                RunCipher(CipherEnums.CipherType.Vigenere);
                break;
            case "3":
                RunCipher(CipherEnums.CipherType.Vernam);
                break;
            case "4":
                RunVigenereBreaker();
                break;
            default:
                Console.ForegroundColor = CipherConfiguration.Colors.Warning;
                Console.WriteLine("[WARNING] Invalid option. Please try again.");
                Console.ResetColor();
                break;
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = CipherConfiguration.Colors.Error;
        Console.WriteLine($"[ERROR] {ex.Message}");
        Console.ResetColor();
    }
}

void RunCipher(CipherEnums.CipherType cipherType)
{
    Console.Clear();
    var cipher = CipherFactory.CreateCipher(cipherType);
    cipher.Run();
}

void RunVigenereBreaker()
{
    Console.Clear();
    var breaker = new VigenereBreaker();
    breaker.Run();
}

void ShowGoodbyeMessage()
{
    Console.ForegroundColor = CipherConfiguration.Colors.Success;
    Console.WriteLine("\n[SUCCESS] Thank you for using Cipher Algorithms!");
    Console.WriteLine("Stay secure!");
    Console.ResetColor();
}
