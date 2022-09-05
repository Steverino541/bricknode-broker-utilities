using System.Text;
using BfsApi;
using Bricknode.Soap.Sdk.Services;
using Bricknode_Broker_Utilities.Contracts.Settings;
using ConsoleTables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bricknode_Broker_Utilities.Services;

public class MenuService
{
    private readonly UserSecrets _secrets;
    private readonly IBfsAuthenticationService _authenticationService;

    public MenuService(IOptions<UserSecrets> secrets, IBfsAuthenticationService authenticationService, ILogger<MenuService> logger)
    {
        _authenticationService = authenticationService;
        _secrets = secrets.Value;
    }

    public async Task ShowMenu()
    {
        var sb = new StringBuilder(514);
        sb.AppendLine(@"");
        sb.AppendLine(@" _____ _                      _             _____    ___  __  ");
        sb.AppendLine(@"/  ___| |                    (_)           |  ___|  /   |/  | ");
        sb.AppendLine(@"\ `--.| |_ _____   _____ _ __ _ _ __   ___ |___ \  / /| |`| | ");
        sb.AppendLine(@" `--. \ __/ _ \ \ / / _ \ '__| | '_ \ / _ \    \ \/ /_| | | | ");
        sb.AppendLine(@"/\__/ / ||  __/\ V /  __/ |  | | | | | (_) /\__/ /\___  |_| |_");
        sb.AppendLine(@"\____/ \__\___| \_/ \___|_|  |_|_| |_|\___/\____/     |_/\___/");
        sb.AppendLine(@"                                                              ");
        sb.AppendLine(@"                                                              ");

        Console.WriteLine(sb.ToString());

        Console.WriteLine($"--------------------------------------------------------------");
        Console.WriteLine($"                                                              ");
        Console.WriteLine($" Welcome to Bricknode Broker Utilities where you can find     ");
        Console.WriteLine($" useful hints and inspiration for how you can extend          ");
        Console.WriteLine($" Bricknode Broker                                             ");
        Console.WriteLine($"                                                              ");
        Console.WriteLine($" Select an item from the menu below                           ");
        Console.WriteLine($"                                                              ");
        Console.WriteLine($"--------------------------------------------------------------");

        Console.WriteLine($"");
        Console.WriteLine($"Start by logging in on an instance of Bricknode Broker below:");
        Console.WriteLine($"");

        var counter = 0;
        var bfsInstanceDictionary = new Dictionary<int, string>();

        foreach (var bfsCredentials in _secrets.BfsCredentials)
        {
            Console.WriteLine($"{counter}. {bfsCredentials.BfsInstanceKey} - {bfsCredentials.ApiEndpoint}");
            bfsInstanceDictionary.Add(counter, bfsCredentials.BfsInstanceKey);
            counter++;
        }

        Console.WriteLine($"");
        Console.WriteLine($"Enter the number of the instance and press enter:");
        var selectedBfsInstanceNumber = Console.ReadLine();

        var selectedBfsInstanceKey = bfsInstanceDictionary[int.Parse(selectedBfsInstanceNumber ?? string.Empty)];

        if (string.IsNullOrEmpty(selectedBfsInstanceKey))
        {
            throw new Exception($"The selected Bricknode Broker instance could not be found");
        }

        Console.WriteLine($"Enter your username and press enter:");
        var username = Console.ReadLine();

        Console.WriteLine($"Enter your password and press enter:");

        var pass = string.Empty;
        ConsoleKey key;
        do
        {
            var keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.Backspace && pass.Length > 0)
            {
                Console.Write("\b \b");
                pass = pass[0..^1];
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                Console.Write("*");
                pass += keyInfo.KeyChar;
            }
        } while (key != ConsoleKey.Enter);

        var password = pass;

        Console.WriteLine("");

        if (!(await _authenticationService.UsernamePasswordAuthenticationAsync(Domain.Admin, username, password,
                selectedBfsInstanceKey)).IsAuthenticated)
        {
            Console.WriteLine($"The credentials did not work, please start the application again.");
            return;
        }

        Console.WriteLine($"Congratulations, you are authenticated!");
        Console.WriteLine("");

        var menuTable = new ConsoleTable("Menu item", "Description");
        menuTable.AddRow(1, $"Create a transaction");
        menuTable.Write(Format.Alternative);

        Console.WriteLine($"Enter menu item and press enter:");
        var selectedMenuItem = Console.ReadLine();

        Console.WriteLine("Done");
        Console.ReadLine();
    }
}