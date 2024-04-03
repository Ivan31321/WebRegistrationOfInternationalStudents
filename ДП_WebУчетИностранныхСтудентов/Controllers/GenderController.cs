using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories;
using MonitoringTheProgressOfForeignStudents.ViewModels.GenderVM;

namespace MonitoringTheProgressOfForeignStudents.Controllers
{
    [Route("[controller]")]
    [Authorize(Policy = "RequireWorkerRoles")]
    public class GenderController : Controller
    {
        private readonly EditGenderVMConverter converter;
        private readonly IGenderRepository repository;

        public GenderController(EditGenderVMConverter converter,
            IGenderRepository repository)
        {
            this.converter = converter;
            this.repository = repository;
        }

        [Route("")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return View(new GetGenderListViewModel
            {
                GenderViewModels = repository.GetAllFull()
                .OrderByDescending(x => x.Created).AsEnumerable()
                .Select(x => new GetGenderViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PersonsCount = x.Persons.Count()
                })
            });
        }

        [Route("")]
        [HttpPost]
        public IActionResult GetAll(GetGenderListViewModel viewModel)
        {
            var genders = repository.GetAllFull().OrderByDescending(x=>x.Created).AsEnumerable();

            if (!string.IsNullOrEmpty(viewModel.SearchGender.SearchString))
            {
                genders = genders.Where(x => x.Name.ToLower().Contains(viewModel.SearchGender.SearchString.ToLower()));
            }

            viewModel.GenderViewModels = genders.Select(x => new GetGenderViewModel
            {
                Id = x.Id,
                Name = x.Name,
                PersonsCount = x.Persons.Count()
            });

            return View(viewModel);
        }

        [Route("add")]
        [HttpGet]
        public IActionResult AddGender()
        {
            return View();
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> AddGender(EditGenderViewModel genderViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!repository.GetAll().Any(x => x.Name.ToLower() == genderViewModel.Name.ToLower()))
                {
                    var gender = converter.ConvertTo(genderViewModel);
                    await repository.CreateAsync(gender);
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Данный пол уже есть в базе");
            }

            return View(genderViewModel);
        }

        [Route("edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditGender(Guid id)
        {
            var gender = await repository.GetById(id);

            if (gender != null)
            {
                return View(converter.ConvertFrom(gender));
            }

            return NotFound();
        }

        [Route("edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> EditGender(Guid id, EditGenderViewModel genderViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!repository.GetAll().Any(x => x.Name.ToLower() == genderViewModel.Name.ToLower()
                                                                && id != x.Id))
                {
                    var gender = await repository.GetById(id);

                    if (gender != null)
                    {
                        gender = converter.Update(genderViewModel, gender);
                        await repository.UpdateAsync(gender);
                        return RedirectToAction("Index");
                    }

                    return NotFound();
                }

                ModelState.AddModelError("", "Данный пол уже есть в базе");
            }

            return View(genderViewModel);
        }

        [Route("deleregender")]
        [HttpPost]
        public async Task<IActionResult> DeleteGender(Guid id)
        {
            var gender = await repository.GetById(id);

            if (gender != null)
            {
                await repository.DeleteAsync(gender);
            }
            return RedirectToAction("Index");
        }
    }
}
