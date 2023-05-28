using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages
{
    [IgnoreAntiforgeryToken]
    public class contactModel : PageModel
    {

        private IContactsService contactsService;

        public contactModel(IContactsService service)
        {
            this.contactsService = service;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var newContact = new Contact();
            newContact.first_name = Request.Form["first_name"];
            newContact.last_name = Request.Form["last_name"];
            newContact.email = Request.Form["email"];
            newContact.phone = Request.Form["phone"];
            newContact.select_service = Request.Form["select_service"];
            newContact.select_price = Request.Form["select_price"];
            newContact.comments = Request.Form["comments"];
            contactsService.writeContact(newContact);
            return Content("Done!");
        }
    }
}
