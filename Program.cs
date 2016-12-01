using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Configuration;
using System.Linq;

namespace BlogSearch
{

    class Program
    {
        static void Main(string[] args)
        {

            string serviceName = Environment.GetEnvironmentVariable(BlogSearchConstants.SEARCH_SERVICE_NAME, EnvironmentVariableTarget.Machine);
            string serviceAdminKey = Environment.GetEnvironmentVariable(BlogSearchConstants.SEARCH_SERVICE_ADMIN_KEY, EnvironmentVariableTarget.Machine);

            SearchServiceClient client = new SearchServiceClient(serviceName, new SearchCredentials(serviceAdminKey));
            SearchIndexClient indexClient = client.Indexes.GetClient("blogs");
            bool finished = false;
            while (!finished)
            {
                Console.WriteLine("0 - Terminar");
                Console.WriteLine("1 - Escribir un Blog");
                Console.WriteLine("2 - Buscar Blogs");
                string actionStr = Console.ReadLine();
                int action = 0;
                if (int.TryParse(actionStr, out action))
                {
                    if (action == 1)
                    {
                        Blog blog = new Blog() { Id = System.Guid.NewGuid().ToString() };
                        Console.WriteLine("Escriba el Titulo");
                        blog.Titulo = Console.ReadLine();
                        Console.WriteLine("Escriba el Cuerpo del Blog");
                        blog.Texto = Console.ReadLine();
                        Console.WriteLine("Escriba el Nombre del Autor del Blog");
                        blog.Autor = Console.ReadLine();
                        blog.Fecha = DateTimeOffset.UtcNow;
                        Console.WriteLine("Cargando su Post ... ");
                        var actions = new IndexAction<Blog>[]
                        {
                            IndexAction.Upload<Blog>(blog)
                        };
                        var batch = IndexBatch.New(actions);

                        try
                        {
                            indexClient.Documents.Index(batch);
                        }
                        catch (IndexBatchException e)
                        {
                            Console.WriteLine("Falló la Indexación del Blog.");
                        }
                    }
                    else if (action == 2)
                    {
                        Console.WriteLine("Ingrese la consulta.");
                        string term = Console.ReadLine();
                        var parameters = new SearchParameters
                        {
                            Select = new[] { "texto" }
                        };
                        var results = indexClient.Documents.Search<Blog>(term, parameters);

                        foreach (var res in results.Results)
                        {
                            Console.WriteLine(res.Document.Texto);
                        }
                    }
                    else if (action == 0)
                    {
                        finished = true;
                    }
                    else
                    {
                        Console.WriteLine("Comando Equivocado");
                    }
                }
                else
                {
                    Console.WriteLine("Comando Equivocado");
                }
            }
            Console.WriteLine("Saliendo ... ");
            Console.ReadLine();
        }
    }
}
