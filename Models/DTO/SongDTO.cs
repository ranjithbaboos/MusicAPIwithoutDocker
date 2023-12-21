using System.ComponentModel.DataAnnotations;

namespace MusicAPIwithoutDocker.Models.DTO
{
    public class SongDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Artist { get; set; }
        [Required]
        [MaxLength(50)]
        public string Album { get; set; }
        
    }
}
