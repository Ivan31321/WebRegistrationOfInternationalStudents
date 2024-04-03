using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories;
using MonitoringTheProgressOfForeignStudents.ViewModels.NationalityVM;

namespace MonitoringTheProgressOfForeignStudents.Controllers
{
    [Route("[controller]")]
    [Authorize(Policy = "RequireWorkerRoles")]
    public class NationalityController : Controller
    {
        private readonly EditNationalityVMConverter converter;
        private readonly INationalityRepository repository;

        public NationalityController(EditNationalityVMConverter converter,
            INationalityRepository repository)
        {
            this.converter = converter;
            this.repository = repository;
        }

        [Route("")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return View(new GetNationalityListViewModel
            {
                NationalityViewModels = repository.GetAllFull()
                .OrderByDescending(x=>x.Created).AsEnumerable()
                .Select(x => new GetNationalityViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PersonsCount = x.Persons.Count()
                })
            });
        }

        [Route("")]
        [HttpPost]
        public IActionResult GetAll(GetNationalityListViewModel viewModel)
        {
            var nationalities = repository.GetAllFull().OrderByDescending(x=>x.Created).AsEnumerable();

            if (!string.IsNullOrEmpty(viewModel.SearchNationality.SearchString))
            {
                nationalities = nationalities.Where(x => x.Name.ToLower().Contains(viewModel.SearchNationality.SearchString.ToLower()));
            }

            viewModel.NationalityViewModels = nationalities.Select(x => new GetNationalityViewModel
            {
                Id = x.Id,
                Name = x.Name,
                PersonsCount = x.Persons.Count()
            });

            return View(viewModel);
        }

        [Route("add")]
        [HttpGet]
        public IActionResult AddNationality()
        {
            return View();
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> AddNationality(EditNationalityViewModel nationalityViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!repository.GetAll().Any(x=>x.Name.ToLower() == nationalityViewModel.Name.ToLower()))
                {
                    var nationality = converter.ConvertTo(nationalityViewModel);
                    await repository.CreateAsync(nationality);
                    return RedirectToAction("Index"); 
                }

                ModelState.AddModelError("", "Данная национальность уже есть в базе");
            }

            return View(nationalityViewModel);
        }

        [Route("edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditNationality(Guid id)
        {
            var nationality = await repository.GetById(id);

            if (nationality != null)
            {
                return View(converter.ConvertFrom(nationality));
            }

            return NotFound();
        }

        [Route("edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> EditNationality(Guid id, EditNationalityViewModel nationalityViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!repository.GetAll().Any(x => x.Name.ToLower() == nationalityViewModel.Name.ToLower()
                                                                && id != x.Id))
                {
                    var nationality = await repository.GetById(id);

                    if (nationality != null)
                    {
                        nationality = converter.Update(nationalityViewModel, nationality);
                        await repository.UpdateAsync(nationality);
                        return RedirectToAction("Index");
                    }

                    return NotFound(); 
                }

                ModelState.AddModelError("", "Данная национальность уже есть в базе");
            }

            return View(nationalityViewModel);
        }

        [Route("deletenationality")]
        [HttpPost]
        public async Task<IActionResult> DeleteNationality(Guid id)
        {
            var nationality = await repository.GetById(id);

            if (nationality != null)
            {
                await repository.DeleteAsync(nationality);
            }
            return RedirectToAction("Index");
        }
    }
}
