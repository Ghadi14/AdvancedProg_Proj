using HouseMates.Areas.Identity.Data;
using HouseMates.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HouseMates.Business
{
    public class HouseBusiness:IHouseBusiness
    {

        private readonly ApplicationDbContext _dbContext;

        public HouseBusiness(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        public async Task<StatusCode> Add(AddHouseViewModel houseViewModel, ClaimsPrincipal User)
        {

            string userId = _dbContext.Users.Where(u => u.UserName == User.Identity.Name).ToList().First().Id;

            House house = new House
            {
                location = houseViewModel.location,
                description = houseViewModel.description,
                numberOfBedrooms = houseViewModel.numberOfBedrooms,
                numberOfBathrooms = houseViewModel.numberOfBathrooms,
                roomMateDescription = houseViewModel.roomMateDescription,
                UserId = userId
            };
            await _dbContext.Houses.AddAsync(house);
            await _dbContext.SaveChangesAsync();

            StatusCode response = new StatusCode();
            response.message = "success";
            response.code = 0;
            return response;
        }

        public List<House> ListUserHouses(ClaimsPrincipal User)
        {
            string userId = _dbContext.Users.Where(u => u.UserName == User.Identity.Name).ToList().First().Id;

            List<House> list = _dbContext.Houses.Where(h => h.UserId == userId).ToList();

            return list;
        }

        public async Task<StatusCode> EditAction(House viewModel)
        {
            var house = await _dbContext.Houses.FindAsync(viewModel.Id);

            if (house is not null)
            {
                house.location = viewModel.location;
                house.description = viewModel.description;
                house.numberOfBathrooms = viewModel.numberOfBathrooms;
                house.numberOfBedrooms = viewModel.numberOfBedrooms;
                house.roomMateDescription = viewModel.roomMateDescription;

                await _dbContext.SaveChangesAsync();
            }

            StatusCode response = new StatusCode();
            response.message = "success";
            response.code = 0;

            return response;
        }

        public async Task<House> EditGet(Guid id)
        {
            var house = await _dbContext.Houses.FindAsync(id);

            return house;
        }
            
        public async Task<StatusCode> DeleteAction(Guid id)
        {
            var house = await _dbContext.Houses.FindAsync(id);

            if (house is not null)
            {
                _dbContext.Houses.Remove(house);
                await _dbContext.SaveChangesAsync();
            }

            StatusCode response = new StatusCode();
            response.message = "success";
            response.code = 0;

            return response;
        }
    }
}
