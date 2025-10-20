using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models
{
    public class Filme
    {
        [Key]
        [Required]
        public int id { get; set; }
        [Required(ErrorMessage = "o título do filme é obrigatório")]
        public string titulo { get; set; }
        [Required(ErrorMessage = "o gênero do filme é obrigatório")]
        [MaxLength(50, ErrorMessage = "o tamanho do gênero não pode exceder 50 caracteres")]
        public string genero { get; set; }
        [Required(ErrorMessage = "a duração do filme é obrigatória")]
        [Range(70, 600, ErrorMessage = "a duração deve ser entre 70 e 600 minutos")]
        public int duracao { get; set; }
    }
}
