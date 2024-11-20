using System.Net.Http.Json;
using macrix_wk_backend.Models;

namespace macrix_wk_UI.Services;

public class ApiService
{
    private static readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5242/api/") };

    public static async Task GetAllPersonsAsync()
    {
        var response = await _httpClient.GetAsync("persons");
        if (response.IsSuccessStatusCode)
        {
            var persons = await response.Content.ReadFromJsonAsync<List<PersonModel>>();
            DrawGrid(persons);
        }
        else
        {
            Console.WriteLine("Error fetching persons.");
        }
    }

    public static async Task GetPersonByIdAsync()
    {
        Console.Write("Enter ID: ");
        if (long.TryParse(Console.ReadLine(), out long id))
        {
            var response = await _httpClient.GetAsync($"persons/{id}");
            if (response.IsSuccessStatusCode)
            {
                var person = await response.Content.ReadFromJsonAsync<PersonModel>();
                Console.WriteLine($@"Id: {person.Id}, First Name: {person.FirstName}, Last Name: {person.LastName}, 
                Street Name: {person.Address.Street}, House Number: {person.Address.HouseNo}, 
                Apartment Number: {person.Address.ApartmentNo}, Postal Code: {person.Address.PostalCode}, Town: {person.Address.Town}, 
                Phone Number: {person.Address.PhoneNumber}, Date of Birth: {person.DateOfBirth}, Age: {person.Age}");
            }
            else
            {
                Console.WriteLine("Person not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID.");
        }
    }

    public static async Task AddPersonAsync()
    {
        Console.Write("Enter Name: ");
        var input = Console.ReadLine();
        PersonModel person;
        if (!string.IsNullOrEmpty(input))
        {
            person = ReadFromInput(input);

            var response = await _httpClient.PostAsJsonAsync("persons", person);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Person added successfully.");
            }
            else
            {
                Console.WriteLine("Error adding person.");
            }
        }
    }

    public static async Task UpdatePersonAsync()
    {

        Console.Write("Enter ID: ");
        if (long.TryParse(Console.ReadLine(), out long id))
        {

            Console.Write("Enter Name: ");
            var input = Console.ReadLine();
            PersonModel person;
            if (!string.IsNullOrEmpty(input))
            {
                person = ReadFromInput(input);

                var response = await _httpClient.PutAsJsonAsync($"persons/{id}", person);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Person updated successfully.");
                }
                else
                {
                    Console.WriteLine("Error updating person.");
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid ID.");
        }
    }

    public static async Task DeletePersonAsync()
    {
        Console.Write("Enter ID: ");
        if (long.TryParse(Console.ReadLine(), out long id))
        {
            var response = await _httpClient.DeleteAsync($"persons/{id}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Person deleted successfully.");
            }
            else
            {
                Console.WriteLine("Error deleting person.");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID.");
        }
    }

    private static PersonModel ReadFromInput(string input)
    {

        if (!string.IsNullOrEmpty(input))
        {
            string[] fields = input.Split(',');

            if (fields.Length == 10 &&
                long.TryParse(fields[0], out long id) &&
                DateTime.TryParse(fields[9].Trim(), out DateTime dateOfBirth))
            {
                return new PersonModel
                {
                    Id = id,
                    FirstName = fields[1].Trim(),
                    LastName = fields[2].Trim(),
                    Address = new AddressModel
                    {
                        HouseNo = fields[3].Trim(),
                        PhoneNumber = fields[4].Trim(),
                        PostalCode = fields[5].Trim(),
                        Street = fields[6].Trim(),
                        Town = fields[7].Trim(),
                        ApartmentNo = fields[8].Trim()
                    },
                    DateOfBirth = dateOfBirth
                };
            }
            else
            {
                Console.WriteLine("Invalid input format. Please enter details in the format: Id, FirstName, LastName, HouseNo, PhoneNumber, PostalCode, Street, Town, ApartmentNo, DateOfBirth");
            }
        }
        else
        {
            Console.WriteLine("Input cannot be empty.");
        }
        return null;
    }

    public static void DrawGrid(List<PersonModel> people)
    {
        Console.WriteLine("Id/First Name/Last Name/Street Name/House Number/Apartment Number/Postal Code/Town/Phone Number/Date of Birth/Age");
        Console.WriteLine("------------------------------------------------------------------------------------------------------");

        foreach (var person in people)
        {
            Console.WriteLine($@"{person.Id}\t{person.FirstName}\t{person.Id}\t{person.Address.Street}\t{person.Address.HouseNo}
                                \t{person.Address.ApartmentNo}\t{person.Address.PostalCode}\t{person.Address.Town}
                                \t{person.Address.PhoneNumber}\t{person.DateOfBirth}\t{person.Age}");
        }
    }
}