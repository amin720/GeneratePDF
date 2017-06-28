using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iTextSharp_Optimize.Models
{
	public class CustomerList : List<Customer>
	{
		public string ImageUrl { get; set; }
	}
}