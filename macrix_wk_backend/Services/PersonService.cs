using macrix_wk_backend.Data;
using macrix_wk_backend.Interfaces;
using macrix_wk_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace macrix_wk_backend.Services;

public class PersonService : IPersonService
{
    private readonly ApplicationDbContext _context;
    public PersonService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<PersonModel>> GetAllPersonsAsync()
    {
        return await _context.Persons.ToListAsync();
    }
    public Task<PersonModel> GetPersonByIdAsync(long id)
    {
        throw new NotImplementedException();
    }
    public async Task AddPersonAsync(PersonModel model)
    {
        await _context.Persons.AddAsync(model);
        await _context.SaveChangesAsync();
    }
    public async Task UpdatePersonAsync(long id, PersonModel model)
    {
        if (id != model.Id)
        {
            throw new ArgumentException($"{id} mismatch.");
        }

        _context.Entry(model).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PersonExists(id))
            {
                throw new KeyNotFoundException($"Person with {id} not found.");
            }
            else
            {
                throw;
            }
        }
    }
    public async Task DeletePersonAsync(long id)
    {
        var person = await _context.Persons.FindAsync(id);
        if (person == null)
        {
            throw new KeyNotFoundException($"Person with {id} not found.");
        }
        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();
    }
    private bool PersonExists(long id)
    {
        return _context.Persons.Any(e => e.Id == id);
    }
}