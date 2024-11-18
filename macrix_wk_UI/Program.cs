using macrix_wk_UI.Services;

public class Program
{
    static async Task Main(string[] args)
    {
        var apiService = new ApiService();
        await DisplayGrid(apiService);
        while (true)
        {
            Console.WriteLine("Press any key to refresh the grid...");
            Console.ReadKey();
            await DisplayGrid(apiService);
        }
    }

    private static async Task DisplayGrid(ApiService apiService)
    {
        var persons = await apiService.GetAllPersonsAsync();
        Console.Clear();
        Console.WriteLine("Id/First Name/Last Name/Street Name/House Number/Apartment Number/Postal Code/Town/Phone Number/Date of Birth/Age");
        Console.WriteLine("------------------------------------------------------------------------------------------------------");

        foreach (var person in persons)
        {
            Console.WriteLine($@"{person.Id}\t{person.FirstName}\t{person.Id}\t{person.Address.Street}\t{person.Address.HouseNo}
                                \t{person.Address.ApartmentNo}\t{person.Address.PostalCode}\t{person.Address.Town}
                                \t{person.Address.PhoneNumber}\t{person.DateOfBirth}\t{person.Age}");
        }
    }
}