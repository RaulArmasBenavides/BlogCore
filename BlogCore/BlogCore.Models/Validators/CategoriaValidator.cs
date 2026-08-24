using BlogCore.Models;
using FluentValidation;

namespace BlogCore.Models.Validators
{
    public class CategoriaValidator : AbstractValidator<Categoria>
    {
        public CategoriaValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre de la categoría es obligatorio")
                .MinimumLength(3).WithMessage("El nombre debe tener mínimo 3 caracteres")
                .MaximumLength(100).WithMessage("El nombre no debe exceder 100 caracteres");
        }
    }
}
