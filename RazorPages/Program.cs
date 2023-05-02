using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Threading;

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

    public interface IContactsService
    {
        void writeContact(Contact newContact);
    }

    public class Contact
    {
        public String first_name { get; set; }
        public String last_name { get; set; }
        public String email { get; set; }
        public String phone { get; set; }
        public String select_service { get; set; }
        public String select_price { get; set; }
        public String comments { get; set; }
    }
    public class ContactsService : IContactsService
    {
        private Mutex mutexObj = new Mutex();
        private String csvFileName = @"csv\contacts.csv";
        public void writeContact(Contact newContact)
        {
            mutexObj.WaitOne();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };
            using (var stream = File.Open(csvFileName, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecord<Contact>(newContact);
                csv.NextRecord();
            }
            mutexObj.ReleaseMutex();
        }
    }
}
