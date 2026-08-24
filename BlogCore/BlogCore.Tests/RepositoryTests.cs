using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using BlogCore.Data;
using BlogCore.Models;
using BlogCore.AccesoDatos.Data.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogCore.Tests
{
    public class RepositoryTests
    {
        private ApplicationDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: \"BlogCoreTestDb\")
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task Repository_AddAsync_ShouldAddEntity()
        {
            // Arrange
            var context = GetInMemoryContext();
            var repository = new Repository<Categoria>(context);
            var categoria = new Categoria { Nombre = \"Test Category\", Orden = 1 };

            // Act
            await repository.AddAsync(categoria);

            // Assert
            var result = context.Set<Categoria>().FirstOrDefault();
            Assert.NotNull(result);
            Assert.Equal(\"Test Category\", result.Nombre);
        }

        [Fact]
        public async Task Repository_GetAsync_ShouldReturnEntity()
        {
            // Arrange
            var context = GetInMemoryContext();
            context.Set<Categoria>().Add(new Categoria { Id = 1, Nombre = \"Category 1\", Orden = 1 });
            await context.SaveChangesAsync();

            var repository = new Repository<Categoria>(context);

            // Act
            var result = await repository.GetAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(\"Category 1\", result.Nombre);
        }

        [Fact]
        public async Task Repository_GetAllAsync_ShouldReturnAllEntities()
        {
            // Arrange
            var context = GetInMemoryContext();
            context.Set<Categoria>().AddRange(
                new Categoria { Id = 1, Nombre = \"Category 1\", Orden = 1 },
                new Categoria { Id = 2, Nombre = \"Category 2\", Orden = 2 }
            );
            await context.SaveChangesAsync();

            var repository = new Repository<Categoria>(context);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task Repository_RemoveAsync_ShouldDeleteEntity()
        {
            // Arrange
            var context = GetInMemoryContext();
            var categoria = new Categoria { Id = 1, Nombre = \"Category to Delete\", Orden = 1 };
            context.Set<Categoria>().Add(categoria);
            await context.SaveChangesAsync();

            var repository = new Repository<Categoria>(context);

            // Act
            await repository.RemoveAsync(1);
            context.Set<Categoria>().Remove(categoria);

            // Assert
            var result = context.Set<Categoria>().FirstOrDefault();
            Assert.Null(result);
        }
    }
}
