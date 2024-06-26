﻿using HouseMates.Areas.Identity.Data;
using HouseMates.Business;
using HouseMates.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HouseMates.Controllers
{
    public class HouseController : Controller
    {
        private readonly IHouseBusiness _houseBusiness;
        public HouseController(IHouseBusiness houseBusiness)
        {
            _houseBusiness = houseBusiness;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddHouseViewModel houseViewModel)
        {

            await _houseBusiness.Add(houseViewModel, User);
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult ListUserHouses()
        {
            List<House> houses = _houseBusiness.ListUserHouses(User);

            return View(houses);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ListBrowseHouses()
        {
            List<House> houses = _houseBusiness.ListBrowseHouses(User);

            return View(houses);
        }

  

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var house = await _houseBusiness.EditGet(id);

            return View(house);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Location(Guid id)
        {
            var house = await _houseBusiness.EditGet(id);

            return View(house);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> OwnerDetails(string id)
        {
            var user = await _houseBusiness.UserGet(id);

            return View(user);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var house= await _houseBusiness.EditGet(id);

            return View(house);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(House viewModel)
        {
            var house = await _houseBusiness.EditAction(viewModel);

            return RedirectToAction("ListUserHouses", "House");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var house = await _houseBusiness.DeleteAction(id);

            return RedirectToAction("ListUserHouses", "House");
        }

    }
}
