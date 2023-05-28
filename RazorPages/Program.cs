using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RazorPagesGeneral.Models;
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

    public class Contact
    {
        public int Id { get; set; }
        public String first_name { get; set; }
        public String last_name { get; set; }
        public String email { get; set; }
        public String phone { get; set; }
        public String select_service { get; set; }
        public String select_price { get; set; }
        public String comments { get; set; }

        public Contact(string first_name, string last_name, string email, string phone, string select_service, string select_price, string comments)
        {
            this.first_name = first_name;
            this.last_name = last_name;
            this.email = email;
            this.phone = phone;
            this.select_service = select_service;
            this.select_price = select_price;
            this.comments = comments;
        }
    }

    public interface IContactsService
    {
        void writeToDBContacts(Contact contact);
    }

    public class ContactsService : IContactsService
    {
        void IContactsService.writeToDBContacts(Contact contact)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseSqlite(config.GetConnectionString("Default"))
                .Options;

            using (var context = new AppDBContext(options))
            {
                context.Database.EnsureCreated();

                context.Contacts.Add(contact);
                context.SaveChanges();
            }
        }
    }

    [Serializable]
    public class Testimonial
    {
        public int Id { get; set; }
        public string? CommentLabel { get; set; }
        public string? Comment { get; set; }
        public string? Name { get; set; }
        public string? JobTitle { get; set; }
        public string? ImageUrl { get; set; }
    }
    public interface ITestimonialService
    {
        IEnumerable<Testimonial> getAll();
    }

    public class TestimonialService : ITestimonialService
    {
        public IEnumerable<Testimonial> getAll()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseSqlite(config.GetConnectionString("Default"))
                .Options;

            var testimonials = new List<Testimonial>();
            using (var context = new AppDBContext(options))
            {
                context.Database.EnsureCreated();

                foreach (var testimonial in context.Testimonials)
                {
                    testimonials.Add(testimonial);
                }
            }
            return testimonials;
        }
    }
}
