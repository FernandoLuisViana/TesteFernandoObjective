using System.Collections.Generic;

namespace TesteFernando.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUri { get; set; }
        public Thumbnail Thumbnail { get; set; }
        public string ResourceURI { get; set; }
        public List<Url> Urls { get; set; }
    }
}
