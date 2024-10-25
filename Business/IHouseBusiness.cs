using HouseMates.Areas.Identity.Data;
using HouseMates.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HouseMates.Business
{
    public interface IHouseBusiness
    {
        public Task<StatusCode> Add(AddHouseViewModel houseViewModel, ClaimsPrincipal User);
        public List<House> ListUserHouses(ClaimsPrincipal User);
        public Task<StatusCode> EditAction(House viewModel);
        public Task<House> EditGet(Guid id);
        public Task<StatusCode> DeleteAction(Guid id);
        public List<House> ListBrowseHouses(ClaimsPrincipal User);
        public Task<ApplicationUser> UserGet(string id);
        public List<House> SearchHouses(string searchedString);





    }
}
