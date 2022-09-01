using System.Text;
using ConsoleTables;

namespace Bricknode_Broker_Utilities.Services;

public class MenuService
{
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

        var menuTable = new ConsoleTable("Menu item", "Description");
        menuTable.AddRow(1, $"Change acquisition value on a position");
        menuTable.Write(Format.Alternative);

        Console.WriteLine($"Enter menu item and press enter:");
        var selectedMenuItem = Console.ReadLine();

    }
}