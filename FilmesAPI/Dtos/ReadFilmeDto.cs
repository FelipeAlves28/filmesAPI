using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Dtos
{
    public class ReadFilmeDto
    {
        public int id { get; set; }
        public string titulo { get; set; } 
        public string genero { get; set; }
        public int duracao { get; set; }
    }
}
