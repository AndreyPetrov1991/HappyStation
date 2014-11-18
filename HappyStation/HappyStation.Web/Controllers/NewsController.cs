﻿using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AutoMapper;

using HappyStation.Core.Constants;
using HappyStation.Core.Entities;
using HappyStation.Core.Services.Implementations;
using HappyStation.Web.Extensions;
using HappyStation.Web.Services;
using HappyStation.Web.Settings;
using HappyStation.Web.ViewModels;

namespace HappyStation.Web.Controllers
{
    public class NewsController : ControllerBase
    {
        public NewsController(NewsRepository newsRepository,
            IMappingEngine mapper,
            ApplicationSettings settings,
            FileUploadService fileUploadService,
            InstagramService instagramService)
            : base(fileUploadService)
        {
            Contract.Requires(newsRepository != null);
            Contract.Requires(mapper != null);
            Contract.Requires(settings != null);
            Contract.Requires(instagramService != null);

            this.newsRepository = newsRepository;
            this.mapper = mapper;
            this.settings = settings;
        }

        public ActionResult Index()
        {
            throw new NotImplementedException();
        }

        public ActionResult News(int id)
        {
            var news = newsRepository.Get(id);
            if (news == null)
            {
                return HttpNotFound();
            }

            return View(mapper.Map<NewsViewModel>(news));
        }

        public ActionResult HottestNews(int count = 4)
        {
            ViewData.Model = newsRepository.GetHottest(count).Select(n => mapper.Map<NewsViewModel>(n));

            return View();
        }

        [Authorize]
        public ActionResult ListAdmin(int pageNum = 1)
        {
            var skip = (pageNum - 1) * settings.ItemsPerPage;
            var news = newsRepository.GetBy(skip, settings.ItemsPerPage + 1).Select(n => mapper.Map<NewsViewModel>(n)).ToList();

            ViewData.Model = news.Take(settings.ItemsPerPage);
            ViewBag.HasPrevPage = pageNum > 1;
            ViewBag.HasNextPage = news.Count > settings.ItemsPerPage;
            ViewBag.PreviosPage = pageNum - 1;
            ViewBag.NextPage = pageNum + 1;

            return View();
        }

        public ActionResult List(int pageNum = 1)
        {
            var skip = (pageNum - 1) * settings.ItemsPerPage;
            var news = newsRepository.GetBy(skip, settings.ItemsPerPage + 1).Select(n => mapper.Map<NewsViewModel>(n)).ToList();

            ViewData.Model = news.Take(settings.ItemsPerPage);
            ViewBag.HasPrevPage = pageNum > 1;
            ViewBag.HasNextPage = news.Count > settings.ItemsPerPage;
            ViewBag.PreviosPage = pageNum - 1;
            ViewBag.NextPage = pageNum + 1;

            return View();
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            newsRepository.Delete(id);

            return RedirectToAction("ListAdmin");
        }

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            var model = id < 1
                ? new NewsViewModel()
                : mapper.Map<NewsViewModel>(newsRepository.Get(id));

            return View(model);
        }

        [HttpPost, Authorize]
        public ActionResult Save(NewsViewModel model, HttpPostedFileBase image)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }

            var news = new News();
            news = mapper.Map(model, news);
            if (image != null)
            {
                news.Image = UploadFile(image);
            }
            else if (!model.IsNew())
            {
                var oldNews = newsRepository.Get(model.Id);
                news.Image = oldNews.Image;
            }

            newsRepository.CreateOrUpdate(news);

            return RedirectToAction("ListAdmin");
        }

        [HttpGet]
        public ActionResult PreviewList(int count = Numbers.MaxGetCount)
        {
            ViewData.Model = newsRepository.GetBy(0, count).Select(n => mapper.Map<NewsViewModel>(n));

            return View();
        }

        private readonly NewsRepository newsRepository;
        private readonly IMappingEngine mapper;
        private readonly ApplicationSettings settings;
    }
}