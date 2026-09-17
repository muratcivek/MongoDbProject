using FluentValidation;
using Travel.WEB.DTOs.ReservationDTOs;

namespace Travel.WEB.Validations.ReservationValidations
{
    public class CreateReservationValidator
        : AbstractValidator<CreateReservationDto>
    {
        public CreateReservationValidator()
        {
            RuleFor(x => x.RouteId)
                .NotEmpty()
                .WithMessage("Rota bilgisi bulunamadı.");

            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Ad soyad boş bırakılamaz.")
                .MinimumLength(3)
                .WithMessage("Ad soyad en az 3 karakter olmalıdır.")
                .MaximumLength(100)
                .WithMessage("Ad soyad en fazla 100 karakter olabilir.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email boş bırakılamaz.")
                .EmailAddress()
                .WithMessage("Geçerli bir email adresi giriniz.");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("Telefon boş bırakılamaz.")
                .MinimumLength(10)
                .WithMessage("Geçerli bir telefon numarası giriniz.");

            RuleFor(x => x.PersonCount)
                .InclusiveBetween(1, 10)
                .WithMessage("Kişi sayısı 1 ile 10 arasında olmalıdır.");

            RuleFor(x => x.TravelDate)
                .Must(date => date.Date >= DateTime.Today)
                .WithMessage("Seyahat tarihi geçmiş bir tarih olamaz.");
        }
    }
}