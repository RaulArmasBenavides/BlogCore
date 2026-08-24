using BlogCore.Models;
using FluentValidation;

namespace BlogCore.Models.Validators
{
    public class ArticuloValidator : AbstractValidator<Articulo>
    {
        public ArticuloValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MinimumLength(3).WithMessage("El nombre debe tener mínimo 3 caracteres")
                .MaximumLength(200).WithMessage("El nombre no debe exceder 200 caracteres");

            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La descripción es obligatoria")
                .MinimumLength(10).WithMessage("La descripción debe tener mínimo 10 caracteres");

            RuleFor(x => x.CategoriaId)
                .GreaterThan(0).WithMessage("Debe seleccionar una categoría");
        }
    }
}
