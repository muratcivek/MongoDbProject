using FluentValidation;
using Travel.WEB.DTOs.RouteDTOs;

namespace Travel.WEB.Validations.RouteValidations
{
    public class CreateRouteValidator : AbstractValidator<CreateRouteDto>
    {
        public CreateRouteValidator()
        {
            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("Şehir alanı boş bırakılamaz.")
                .MaximumLength(50)
                .WithMessage("Şehir alanı en fazla 50 karakter olabilir.");

            RuleFor(x => x.Country)
                .NotEmpty()
                .WithMessage("Ülke alanı boş bırakılamaz.")
                .MaximumLength(50)
                .WithMessage("Ülke alanı en fazla 50 karakter olabilir.");

            RuleFor(x => x.Duration)
                .NotEmpty()
                .WithMessage("Süre alanı boş bırakılamaz.")
                .MaximumLength(50)
                .WithMessage("Süre alanı en fazla 50 karakter olabilir.");

            RuleFor(x => x.ImageUrl)
                .NotEmpty()
                .WithMessage("Görsel URL alanı boş bırakılamaz.")
                .Must(x => Uri.TryCreate(x, UriKind.Absolute, out _))
                .WithMessage("Geçerli bir görsel URL'si giriniz.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Fiyat 0'dan büyük olmalıdır.");
        }
    }
}