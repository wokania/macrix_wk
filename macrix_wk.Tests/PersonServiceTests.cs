using macrix_wk_backend.Data;
using macrix_wk_backend.Models;
using macrix_wk_backend.Services;
using Microsoft.EntityFrameworkCore;
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
    public async Task GetAllPersonsAsync_ReturnsAllPersons()
    {
        // Arrange
        var persons = LoadTestData();

        _mockContext.Setup(c => c.Persons).Returns(_mockDbSet.Object);

        // Act
        var result = await _service.GetAllPersonsAsync();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2));
        foreach (var person in persons)
        {
            foreach (var element in result)
            {
                Assert.That(element.FirstName, Is.EqualTo(person.FirstName));
            }
        }
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
        var person = LoadTestData().First();

        _mockContext.Setup(m => m.Persons).Returns(_mockDbSet.Object);
        _mockContext.Setup(m => m.Entry(person).State == EntityState.Modified);

        // Act
        await _service.UpdatePersonAsync(1, person);

        // Assert
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task DeletePersonAsync_DeletesPerson()
    {
        // Arrange
        var person = LoadTestData().First();

        _mockDbSet.Setup(m => m.FindAsync(1)).ReturnsAsync(person);
        _mockContext.Setup(m => m.Persons).Returns(_mockDbSet.Object);

        // Act
        await _service.DeletePersonAsync(1);

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
    private void SeedDatabase()
    {
        var persons = LoadTestData();
        _mockContext.Object.Persons.AddRange(persons);
        _mockContext.Object.SaveChanges();
    }
    private Mock<DbSet<PersonModel>> CreateMockDbSet(IQueryable<PersonModel> data)
    {
        var mockSet = new Mock<DbSet<PersonModel>>();
        mockSet.As<IQueryable<PersonModel>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<PersonModel>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<PersonModel>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<PersonModel>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        return mockSet;
    }
}