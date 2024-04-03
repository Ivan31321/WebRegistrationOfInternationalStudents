using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories;
using MonitoringTheProgressOfForeignStudents.Domain.Model;
using MonitoringTheProgressOfForeignStudents.ViewModels.PersonalDetailsVM;

namespace MonitoringTheProgressOfForeignStudents.Controllers
{
    [Route("[controller]")]
    [Authorize(Policy = "RequireWorkerRoles")]
    public class PersonalDetailsController : Controller
    {
        private readonly EditPersonalDetailsVMConverter converter;
        private readonly INationalityRepository nationalityRepository;
        private readonly IMaritalStatusRepository maritalStatusRepository;
        private readonly IGenderRepository genderRepository;
        private readonly ICountryRepository countryRepository;
        private readonly IPersonalDetailsRepository personalDetailsRepository;

        public PersonalDetailsController(EditPersonalDetailsVMConverter converter,
            INationalityRepository nationalityRepository,
            IMaritalStatusRepository maritalStatusRepository,
            IGenderRepository genderRepository,
            ICountryRepository countryRepository,
            IPersonalDetailsRepository personalDetailsRepository)
        {
            this.converter = converter;
            this.nationalityRepository = nationalityRepository;
            this.maritalStatusRepository = maritalStatusRepository;
            this.genderRepository = genderRepository;
            this.countryRepository = countryRepository;
            this.personalDetailsRepository = personalDetailsRepository;
        }

        [Route("")]
        [HttpGet]
        public IActionResult GetAll()
        {
            InitViewBag();
            return View(new GetPersonalDetailsListViewModel
            {
                PersonalDetailsViewModels = personalDetailsRepository.GetAllFull()
                .OrderByDescending(x => x.Created).AsEnumerable()
                .Select(x => new GetPersonalDetailsViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Surname = x.Surname ?? "",
                    Patronymic = x.Patronymic ?? "",
                    Email = x.Email ?? "",
                    Gender = x.Gender?.Name ?? string.Empty,
                    Country = x.Country?.Name ?? string.Empty,
                    MaritalStatus = x.MaritalStatus?.Name ?? string.Empty,
                    Nationality = x.Nationality?.Name ?? string.Empty,
                })
            });
        }

        [Route("")]
        [HttpPost]
        public IActionResult GetAll(GetPersonalDetailsListViewModel viewModel)
        {
            InitViewBag();
            var pDetails = personalDetailsRepository.GetAllFull().OrderByDescending(x => x.Created).AsEnumerable();

            if (viewModel.SearchPersonalDetails.MaritalStatusId != Guid.Empty)
            {
                pDetails = pDetails.Where(x => x.MaritalStatusId == viewModel.SearchPersonalDetails.MaritalStatusId);
            }
            if (viewModel.SearchPersonalDetails.NationalityId != Guid.Empty)
            {
                pDetails = pDetails.Where(x => x.NationalityId == viewModel.SearchPersonalDetails.NationalityId);
            }
            if (viewModel.SearchPersonalDetails.CountryId != Guid.Empty)
            {
                pDetails = pDetails.Where(x => x.CountryId == viewModel.SearchPersonalDetails.CountryId);
            }
            if (viewModel.SearchPersonalDetails.GenderId != Guid.Empty)
            {
                pDetails = pDetails.Where(x => x.GenderId == viewModel.SearchPersonalDetails.GenderId);
            }

            var res = pDetails.Select(x => new GetPersonalDetailsViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname ?? "",
                Patronymic = x.Patronymic ?? "",
                Email = x.Email ?? "",
                Gender = x.Gender?.Name ?? string.Empty,
                Country = x.Country?.Name ?? string.Empty,
                MaritalStatus = x.MaritalStatus?.Name ?? string.Empty,
                Nationality = x.Nationality?.Name ?? string.Empty,
            });

            if (!string.IsNullOrEmpty(viewModel.SearchPersonalDetails.SearchString))
            {
                res = res.Where(x => x.ToString().ToLower().Contains(viewModel.SearchPersonalDetails.SearchString.ToLower()));
            }

            viewModel.PersonalDetailsViewModels = res;

            return View(viewModel);
        }

        [Route("add")]
        [HttpGet]
        public IActionResult AddPersonalDetails()
        {
            InitViewBag();
            return View();
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> AddPersonalDetails(EditPersonalDetailsViewModel personalDetailsViewModel)
        {
            InitViewBag();

            if (ModelState.IsValid)
            {
                var personalDetails = converter.ConvertTo(personalDetailsViewModel);
                await personalDetailsRepository.CreateAsync(personalDetails);
                return RedirectToAction("Index");
            }

            return View(personalDetailsViewModel);
        }

        [Route("edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditPersonalDetails(Guid id)
        {
            InitViewBag();

            var personalDetails = await personalDetailsRepository.GetById(id);

            if (personalDetails != null)
            {
                return View(converter.ConvertFrom(personalDetails));
            }

            return NotFound();
        }

        [Route("edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> EditPersonalDetails(Guid id, EditPersonalDetailsViewModel personalDetailsViewModel)
        {
            InitViewBag();
            if (ModelState.IsValid)
            {
                var personalDetails = await personalDetailsRepository.GetById(id);

                if (personalDetails != null)
                {
                    personalDetails = converter.Update(personalDetailsViewModel, personalDetails);
                    await personalDetailsRepository.UpdateAsync(personalDetails);
                    return RedirectToAction("Index");
                }

                return NotFound();
            }

            return View(personalDetailsViewModel);
        }

        [Route("deletedetails")]
        [HttpPost]
        public async Task<IActionResult> DeletePersonalDetails(Guid id)
        {
            var personalDetails = await personalDetailsRepository.GetById(id);

            if (personalDetails != null)
            {
                await personalDetailsRepository.DeleteAsync(personalDetails);
            }
            return RedirectToAction("Index");
        }

        private void InitViewBag()
        {
            ViewBag.Nationalities = new SelectList(nationalityRepository.GetAll(), nameof(Nationality.Id), nameof(Nationality.Name));
            ViewBag.MaritalStatuses = new SelectList(maritalStatusRepository.GetAll(), nameof(MaritalStatus.Id), nameof(MaritalStatus.Name));
            ViewBag.Genders = new SelectList(genderRepository.GetAll(), nameof(Gender.Id), nameof(Gender.Name));
            ViewBag.Countries = new SelectList(countryRepository.GetAll(), nameof(Country.Id), nameof(Country.Name));
        }
    }
}
