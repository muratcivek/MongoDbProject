using FluentValidation;
using Travel.WEB.DTOs.ReviewDTOs;

namespace Travel.WEB.Validations.ReviewValidations
{
    public class CreateReviewValidator
        : AbstractValidator<CreateReviewDto>
    {
        public CreateReviewValidator()
        {
            RuleFor(x => x.RouteId)
                .NotEmpty()
                .WithMessage("Rota bilgisi bulunamadı.");

            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("Kullanıcı adı boş bırakılamaz.")
                .MinimumLength(2)
                .WithMessage(
                    "Kullanıcı adı en az 2 karakter olmalıdır.");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5)
                .WithMessage(
                    "Puan 1 ile 5 arasında olmalıdır.");

            RuleFor(x => x.Comment)
                .NotEmpty()
                .WithMessage("Yorum boş bırakılamaz.")
                .MinimumLength(10)
                .WithMessage(
                    "Yorum en az 10 karakter olmalıdır.")
                .MaximumLength(500)
                .WithMessage(
                    "Yorum en fazla 500 karakter olabilir.");
        }
    }
}