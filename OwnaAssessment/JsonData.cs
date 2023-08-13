using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OwnaAssessment
{
    public class JsonData
    {
        //[JsonPropertyName("authors")]
        public List<Author> Authors { get; set; }

        //[JsonPropertyName("books")]
        public List<Book> Books { get; set; }
    }
}
