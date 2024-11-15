using macrix_wk_backend.Models;

namespace macrix_wk_backend.Interfaces;

public interface IPersonService
{
    Task<IEnumerable<PersonModel>> GetAllPersonsAsync();
    Task<PersonModel> GetPersonByIdAsync(long id);
    Task AddPersonAsync(PersonModel person);
    Task UpdatePersonAsync(long id, PersonModel person);
    Task DeletePersonAsync(long id);
}