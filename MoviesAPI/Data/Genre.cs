using MoviesAPI.Intefaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesAPI.Data
{
    public class Genre : ISoftDelete
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }

        public bool IsDeleted { get; private set; }
    }
}
