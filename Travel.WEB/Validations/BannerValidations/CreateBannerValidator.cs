using FluentValidation;
using Travel.WEB.DTOs.BannerDTOs;

namespace Travel.WEB.Validations.BannerValidations
{
    public class CreateBannerValidator:AbstractValidator<CreateBannerDto>
    {
        public CreateBannerValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık boş bırakılamaz.").MinimumLength(3).WithMessage("En az 3 karakter olmalıdır.");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama boş bırakılamaz.").MinimumLength(25).WithMessage("En az 25 karakter olmalıdır.");

            RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("Resim URL'si boş bırakılamaz.");
        }
    }
}
