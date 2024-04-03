using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories;
using MonitoringTheProgressOfForeignStudents.ViewModels.CountryVM;

namespace MonitoringTheProgressOfForeignStudents.Controllers
{
    [Route("[controller]")]
    [Authorize(Policy = "RequireWorkerRoles")]
    public class CountryController : Controller
    {
        private readonly ICountryRepository repository;
        private readonly EditCountryVMConverter converter;

        public CountryController(ICountryRepository repository,
                                EditCountryVMConverter converter)
        {
            this.repository = repository;
            this.converter = converter;
        }

        [Route("")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return View(new GetCountryListViewModel
            {
                CountryViewModels = repository.GetAllFull().OrderByDescending(x => x.Created).Select(x => new GetCountryViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PersonsCount = x.Persons.Count(),
                })
            });
        }

        [Route("")]
        [HttpPost]
        public IActionResult GetAll(GetCountryListViewModel viewModel)
        {
            var countries = repository.GetAllFull().OrderByDescending(x => x.Created).AsEnumerable();

            if (!string.IsNullOrEmpty(viewModel.SearchCountry.SearchString))
            {
                countries = countries.Where(x => x.Name.ToLower().Contains(viewModel.SearchCountry.SearchString.ToLower()));
            }

            viewModel.CountryViewModels = countries.Select(x => new GetCountryViewModel
            {
                Id = x.Id,
                Name = x.Name,
                PersonsCount = x.Persons.Count(),
            });

            return View(viewModel);
        }

        [Route("add")]
        [HttpGet]
        public IActionResult AddCountry()
        {
            return View();
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> AddCountry(EditCountryViewModel countryViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!repository.GetAll().Any(x => x.Name.ToLower() == countryViewModel.Name.ToLower()))
                {
                    var country = converter.ConvertTo(countryViewModel);
                    await repository.CreateAsync(country);
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Данная страна уже есть в базе");
            }

            return View(countryViewModel);
        }

        [Route("edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditCountry(Guid id)
        {
            var country = await repository.GetById(id);

            if (country != null)
            {
                return View(converter.ConvertFrom(country));
            }

            return NotFound();
        }

        [Route("edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> EditCountry(Guid id, EditCountryViewModel countryViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!repository.GetAll().Any(x => x.Name.ToLower() == countryViewModel.Name.ToLower()
                                                                && id != x.Id))
                {
                    var country = await repository.GetById(id);

                    if (country != null)
                    {
                        country = converter.Update(countryViewModel, country);
                        await repository.UpdateAsync(country);

                        return RedirectToAction("Index");
                    }

                    return NotFound();
                }

                ModelState.AddModelError("", "Данная страна уже есть в базе");
            }

            return View(countryViewModel);
        }

        [Route("deletecountry")]
        [HttpPost]
        public async Task<IActionResult> DeleteCountry(Guid id)
        {
            var country = await repository.GetById(id);

            if (country != null)
            {
                await repository.DeleteAsync(country);
            }
            return RedirectToAction("Index");
        }
    }
}
