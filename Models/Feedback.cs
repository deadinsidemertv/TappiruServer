using System.ComponentModel.DataAnnotations;

namespace TappiruServer.Models
{
    public class Feedback
    {
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "ты даун")]
        
        public string Name {  get; set; }

        [Display(Name = "Вид дауна")]
        [Required(ErrorMessage = "ты даун")]
        public string Surname { get; set; }

        [Display(Name = "Скок ммр мусор?")]
        [Required(ErrorMessage = "ты даун")]
        public int MMR { get; set; }

        [Display(Name = "Почту тврь вводи")]
        [Required(ErrorMessage = "ты даун")]
        public string Email { get; set; }

        [Display(Name = "Пиче what you want?")]
        [Required(ErrorMessage = "ты даун")]
        [StringLength(30,ErrorMessage = "бля это всё что ты хочешь сказать?")]
        public string Message { get; set; }

    }
}
