using System;
using System.IO;
using Spectre.Console;

static class Menu
{
    public static String[] ShowMenu()
    {
        Console.Clear();
        const string fileName = "/home/j4yden/HackNC2025/HackNC-2025-Roguelike/hacknc25/hacknc25.txt";
        String filePath = Path.Combine(AppContext.BaseDirectory, fileName);
        Console.WriteLine(AppContext.BaseDirectory);

        Console.WriteLine(File.ReadAllText(fileName));
        Console.ReadKey(intercept: true);

        var name = AnsiConsole.Prompt(
            new TextPrompt<string>("[green]Enter your name[/]:")
                .ValidationErrorMessage("[red]That's not a valid name[/]")
                .Validate(name =>
                {
                    return name.Length switch
                    {
                        < 1 => ValidationResult.Error("[red]Name must be at least one character[/]"),
                        > 12 => ValidationResult.Error("[red]Name must be at most 12 characters[/]"),
                        _ => ValidationResult.Success(),
                    };
                })
        );
    
        var classSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[cyan]Choose a class:[/]")
                .AddChoices(new[] { "Warrior", "Mage", "Rogue" })
        );

        var raceSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[magenta]Choose a race:[/]")
                .AddChoices(new[] { "Human", "Elf", "Orc", "Dwarf" })
        );

        var character = new[] { name, classSelection, raceSelection };

        Console.WriteLine($"You are {character[0]}, the {character[1]} {character[2]}!");
        Console.WriteLine("Press any key to start your adventure...");
        Console.ReadKey(intercept: true);

        return character;
    }
    

}