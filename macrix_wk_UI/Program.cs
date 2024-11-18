using macrix_wk_UI.Services;

public class Program
{
    static async Task Main(string[] args)
    {
        ApiService apiService = new ApiService();
        while (true)
        {
            Console.Clear();
            await DisplayMenu();
            Console.WriteLine("\nPress any key to refresh the grid...\n");
            Console.ReadKey();
        }
    }

    private static async Task DisplayMenu()
    {
        Console.WriteLine("Choose an option:");
        Console.WriteLine("1 to display all persons.");
        Console.WriteLine("2 to display person with specific ID.");
        Console.WriteLine("3 to add Person.");
        Console.WriteLine("4 to update Person with specific ID.");
        Console.WriteLine("5 to delete Person with specific ID.");
        Console.Write("Option: ");
        var option = Console.ReadLine();

        switch (option)
        {
            case "1":
                await ApiService.GetAllPersonsAsync();
                break;

            case "2":
                await ApiService.GetPersonByIdAsync();
                break;

            case "3":
                await ApiService.AddPersonAsync();
                break;

            case "4":
                await ApiService.UpdatePersonAsync();
                break;

            case "5":
                await ApiService.DeletePersonAsync();
                break;

            default:
                Console.WriteLine($"Invalid option {option}");
                break;
        }
    }
}