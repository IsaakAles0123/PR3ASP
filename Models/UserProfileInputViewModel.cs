using System.ComponentModel.DataAnnotations;

namespace PR1_ASP.Models
{
    public class UserProfileInputViewModel
    {
        [Required(ErrorMessage = "Введите имя.")]
        [StringLength(80, ErrorMessage = "Имя не должно превышать 80 символов.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите email.")]
        [EmailAddress(ErrorMessage = "Некорректный формат email.")]
        [StringLength(120, ErrorMessage = "Email не должно превышать 120 символов.")]
        public string Email { get; set; } = string.Empty;

        [StringLength(80, ErrorMessage = "Город не должен превышать 80 символов.")]
        public string? City { get; set; }

        [Required(ErrorMessage = "Введите небольшое описание.")]
        [StringLength(500, ErrorMessage = "Описание не должно превышать 500 символов.")]
        public string Bio { get; set; } = string.Empty;
    }
}

