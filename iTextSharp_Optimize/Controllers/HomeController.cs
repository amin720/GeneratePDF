using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp_Optimize.Models;
using iTextSharp_Optimize;
using iTextSharp_Optimize.PdfReportGenerator;

namespace iTextSharp_Optimize.Controllers
{
    public class HomeController : PdfViewController
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

	    public ActionResult PrintCustomers()
	    {
		    CustomerList customerList = CreateCustomerList();
		    FillImageUrl(customerList, "report.jpg");
		    return this.ViewPdf("Customer report", "PrintDemo", customerList);
	    }

	    private void FillImageUrl(CustomerList customerList, string imageName)
	    {
		    string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
		    customerList.ImageUrl = url + "Content/" + imageName;
	    }

	    private CustomerList CreateCustomerList()
	    {
		    return new CustomerList()
		    {
			    new Customer { Id = 1, Name = "Patrick", Address = "Geuzenstraat 29", Place = "Amsterdam" },
			    new Customer { Id = 2, Name = "Fred", Address = "Flink 9a", Place = "Rotterdam" },
			    new Customer { Id = 3, Name = "Sjonnie", Address = "Paternatenplaats 44", Place = "Enkhuizen" },
			    new Customer { Id = 4, Name = "Henk", Address = "Wakerdijk 74", Place = "Utrecht" },
			    new Customer { Id = 5, Name = "Klaas", Address = "Paternatenplaats 44", Place = "Plaantan" },
			    new Customer { Id = 6, Name = "Andre", Address = "Wolbrantskerkweg 90B ", Place = "Los Angeles" },
			    new Customer { Id = 7, Name = "Pieter", Address = "Sam van Houtenstraat 191H", Place = "Emmen" },
			    new Customer { Id = 8, Name = "Sjohn", Address = "Polostraat, M. 103-II", Place = "Kantens" },
			    new Customer { Id = 9, Name = "John", Address = "Paternatenplaats 44", Place = "Leiden" },
			    new Customer { Id = 10, Name = "Bruin", Address = "Anthony Spatzierhof 9", Place = "Maasbracht" },
			    new Customer { Id = 11, Name = "Sjonnie", Address = "Van Heuven Goedhartlaan 201", Place = "Potters" },
			    new Customer { Id = 12, Name = "Lumunon", Address = "Paternatenplaats 44", Place = "Utrecht" },
			    new Customer { Id = 13, Name = "Friese", Address = "Burgemeester Roellstr 243-4", Place = "Rotterdam" },
			    new Customer { Id = 14, Name = "Hudephol", Address = "Bilhamerstraat 4", Place = "Vlaardingen" },
			    new Customer { Id = 15, Name = "Postema", Address = "Mastbos 77", Place = "Schiedam" },
			    new Customer { Id = 16, Name = "Scharn", Address = "Marius Bauerstraat 123", Place = "Dordrecht" },
			    new Customer { Id = 17, Name = "Wagenmakers", Address = "Pieter Postsingel 16", Place = "Ede" },
			    new Customer { Id = 18, Name = "Gedikli", Address = "Burgemeester Hogguerstr 283", Place = "Twello" },
			    new Customer { Id = 19, Name = "Zwollo", Address = "Meervalweg 140", Place = "Terschelling" },
			    new Customer { Id = 20, Name = "Sjonnie", Address = "Ruys de Beerenbrouckstr 79A", Place = "Ter Aar" },
			    new Customer { Id = 21, Name = "Schimmelmann", Address = "Ritzema Bosstraat 28-2", Place = "Vierenman" },
			    new Customer { Id = 22, Name = "Makhlouf", Address = "Ln vd Helende Meesters 12", Place = "Eindhoven" },
			    new Customer { Id = 23, Name = "Meyer", Address = "Burgemeester v Leeuwenln 79H", Place = "Breda" },
		    };
	    }
	}
}