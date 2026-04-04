using System.ComponentModel.DataAnnotations;

namespace TappiruServer.Models
{
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "Никнейм обязателен")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Никнейм должен быть от 3 до 20 символов")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть от 6 символов")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтвердите пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }


    }
}
