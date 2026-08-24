using BlogCore.Models;
using FluentValidation;

namespace BlogCore.Models.Validators
{
    public class SliderValidator : AbstractValidator<Slider>
    {
        public SliderValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre del slider es obligatorio")
                .MinimumLength(3).WithMessage("El nombre debe tener mínimo 3 caracteres");

            RuleFor(x => x.UrlImagen)
                .NotEmpty().WithMessage("La URL de la imagen es obligatoria");
        }
    }
}
