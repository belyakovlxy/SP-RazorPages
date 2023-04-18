using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorPages.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		private IParalaxSectionService _service;
		public IEnumerable<ParalaxSection> paralaxSection { get; set; }

		public IndexModel(ILogger<IndexModel> logger, IParalaxSectionService service)
		{
			_logger = logger;
			this._service = service;
			paralaxSection = service.getAll();
		}

		public void OnGet()
		{

		}
	}
}
