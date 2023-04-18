using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RazorPages
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}

    [Serializable]
    public class ParalaxSection
    {
        public string? Iconclass { get; set; }
        public string? StatCount { get; set; }
        public string? HeaderText { get; set; }
    }
    public interface IParalaxSectionService
    {
        IEnumerable<ParalaxSection> getAll();
    }

    public class ParalaxSectionService : IParalaxSectionService
    {
        public IEnumerable<ParalaxSection> getAll()
        {
            var streamReader = new StreamReader("paralax_section.json");

            string json = streamReader.ReadToEnd();
            return JsonSerializer.Deserialize<ParalaxSection[]>(json) ?? new ParalaxSection[] { };
        }
    }
}
