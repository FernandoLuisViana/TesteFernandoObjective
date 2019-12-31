using System.Collections.Generic;

namespace TesteFernando.Models
{
    public class MarvelCharacter
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int Total { get; set; }
        public int Count { get; set; }
        public List<Character> Results { get; set; }
    }
}
