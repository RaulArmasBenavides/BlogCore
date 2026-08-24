using Xunit;
using BlogCore.Models;
using BlogCore.Models.Validators;
using FluentValidation;

namespace BlogCore.Tests
{
    public class ValidatorTests
    {
        [Fact]
        public void ArticuloValidator_WithValidData_ShouldPass()
        {
            // Arrange
            var validator = new ArticuloValidator();
            var articulo = new Articulo
            {
                Nombre = \"Test Article\",
                Descripcion = \"This is a test description for the article\",
                CategoriaId = 1,
                UrlImagen = \"/images/test.jpg\"
            };

            // Act
            var result = validator.Validate(articulo);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void ArticuloValidator_WithEmptyNombre_ShouldFail()
        {
            // Arrange
            var validator = new ArticuloValidator();
            var articulo = new Articulo
            {
                Nombre = \"\",
                Descripcion = \"This is a test description for the article\",
                CategoriaId = 1
            };

            // Act
            var result = validator.Validate(articulo);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == \"Nombre\");
        }

        [Fact]
        public void CategoriaValidator_WithValidData_ShouldPass()
        {
            // Arrange
            var validator = new CategoriaValidator();
            var categoria = new Categoria { Nombre = \"Test Category\", Orden = 1 };

            // Act
            var result = validator.Validate(categoria);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void SliderValidator_WithValidData_ShouldPass()
        {
            // Arrange
            var validator = new SliderValidator();
            var slider = new Slider
            {
                Nombre = \"Test Slider\",
                UrlImagen = \"/images/slider.jpg\",
                Estado = true
            };

            // Act
            var result = validator.Validate(slider);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
