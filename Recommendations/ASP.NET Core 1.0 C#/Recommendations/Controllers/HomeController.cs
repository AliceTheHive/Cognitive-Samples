﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.InternalAbstractions;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Text;
using Recommendations.Models;
using Recommendations.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recommendations.Services;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.WebUtilities;
using Recommendations.Interfaces;

namespace Recommendations.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _environment;
        private readonly IBooksRepository _books;
        private readonly IRecommendationsRepository _recommendations;

        public HomeController(IHostingEnvironment environment, IBooksRepository booksRepository, IRecommendationsRepository recommendationsRepository)
        {
            _environment = environment;
            _books = booksRepository;
            _recommendations = recommendationsRepository;
        }

        public IActionResult Index()
        {
            var allBooks = _books.List();
            return View(allBooks);
        }

        public async Task<IActionResult> Book(string id)
        {
            //get this book
            var book = _books.List().Where(o => o.Id == id).FirstOrDefault();

            //get recommended and FBT items
            var recomendedItems = await _recommendations.GetRecommendedItems(id, "5", "0");
            var fbtItems = await _recommendations.GetFBTItems(id, "5", "0");

            //construct view model
            var vm = new HomeBookViewModel()
            {
                Book = book,
                RecommendedItems = recomendedItems,
                FBTItems = fbtItems
            };

            //return view
            return View(vm);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
