using macrix_wk_backend.Data;
using macrix_wk_backend.Models;
using macrix_wk_backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Newtonsoft.Json;

[TestFixture]
public class PersonServiceTests
{

    private Mock<ApplicationDbContext> _mockContext;
    private PersonService _service;
    private Mock<DbSet<PersonModel>> _mockDbSet;

    [SetUp]
    public void SetUp()
    {
        var testData = LoadTestData().AsQueryable();
        _mockDbSet = CreateMockDbSet(testData);

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _mockContext = new Mock<ApplicationDbContext>(options);
        _mockContext.Setup(c => c.Persons).Returns(_mockDbSet.Object);
        _service = new PersonService(_mockContext.Object);

    }

    [Test]
    public async Task GetAllPersonsAsync_ShouldReturnAllPersons()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase_GetAllPersons")
            .Options;

        using var context = new ApplicationDbContext(options);

        var persons = LoadTestData();

        context.Persons.AddRange(persons);
        await context.SaveChangesAsync();

        var service = new PersonService(context);

        // Act
        var result = await service.GetAllPersonsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.That(result.Count(), Is.EqualTo(persons.Count));
        Assert.IsTrue(result.Any(p => p.FirstName == "Bob" && p.LastName == "Marley"));
        Assert.IsTrue(result.Any(p => p.FirstName == "Peter" && p.LastName == "Tosh"));
    }

    [Test]
    public async Task AddPersonAsync_AddsPerson()
    {
        // Arrange
        var person = LoadTestData().First();

        _mockContext.Setup(m => m.Persons).Returns(_mockDbSet.Object);

        // Act
        await _service.AddPersonAsync(person);

        // Assert
        _mockDbSet.Verify(m => m.AddAsync(person, It.IsAny<CancellationToken>()), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task UpdatePersonAsync_UpdatesPerson()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        using var context = new ApplicationDbContext(options);
        
        var person = LoadTestData().First();

        context.Persons.Add(person);
        await context.SaveChangesAsync();

        var service = new PersonService(context);

        var updatedPerson = person;
        updatedPerson.FirstName = "Robert Nesta";
        
        // Act
        await service.UpdatePersonAsync(person.Id, updatedPerson);

        // Assert
        var updatedEntity = await context.Persons.FindAsync(person.Id);

        Assert.NotNull(updatedEntity);
        Assert.That(updatedEntity.FirstName, Is.EqualTo("Robert Nesta"));
        Assert.That(updatedEntity.LastName, Is.EqualTo("Marley"));
    }


    [Test]
    public async Task DeletePersonAsync_DeletesPerson()
    {
        // Arrange
        var person = LoadTestData().First();

        _mockDbSet.Setup(m => m.FindAsync(person.Id)).ReturnsAsync(person);
        _mockContext.Setup(m => m.Persons).Returns(_mockDbSet.Object);

        // Act
        await _service.DeletePersonAsync(person.Id);

        // Assert
        _mockDbSet.Verify(m => m.Remove(person), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    private List<PersonModel> LoadTestData()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "TestData.json");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        var jsonData = File.ReadAllText(filePath);

        if (string.IsNullOrEmpty(jsonData))
        {
            throw new InvalidDataException("JSON data is null or empty.");
        }

        var result = JsonConvert.DeserializeObject<List<PersonModel>>(jsonData);

        if (result == null)
        {
            throw new InvalidOperationException("Deserialization resulted in null.");
        }

        return result;
    }
    private Mock<DbSet<T>> CreateMockDbSet<T>(IQueryable<T> data) where T : class
    {
        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        return mockSet;
    }
}