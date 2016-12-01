namespace BlogSearch
{
    using Microsoft.Azure.Search.Models;
    using System;

    [SerializePropertyNamesAsCamelCase]
    public class Blog
    {
        public string Id { get; set; }

        public string Titulo { get; set; }

        public DateTimeOffset? Fecha { get; set; }

        public string Texto { get; set; }

        public string Autor { get; set; }
    }
}
