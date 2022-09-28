using System.ComponentModel.DataAnnotations;

namespace Parts4Cars_Demo.Models
{
    public class Image
    {
        public int ID { get; set; }

        [StringLength(100)]
        public string Url { get; set; }
        public virtual Part Part { get; set; }
    }
}
