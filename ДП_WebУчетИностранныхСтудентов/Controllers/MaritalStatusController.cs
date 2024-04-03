using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories;
using MonitoringTheProgressOfForeignStudents.ViewModels.MaritalStatusVM;

namespace MonitoringTheProgressOfForeignStudents.Controllers
{
    [Route("[controller]")]
    [Authorize(Policy = "RequireWorkerRoles")]
    public class MaritalStatusController : Controller
    {
        private readonly EditMaritalStatusVMConverter converter;
        private readonly IMaritalStatusRepository repository;

        public MaritalStatusController(EditMaritalStatusVMConverter converter,
            IMaritalStatusRepository repository)
        {
            this.converter = converter;
            this.repository = repository;
        }

        [Route("")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return View(new GetMaritalStatusListViewModel
            {
                MaritalStatusViewModels = repository.GetAllFull()
                .OrderByDescending(x => x.Created).AsEnumerable()
                .Select(x => new GetMaritalStatusViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PersonsCount = x.Persons.Count()
                })
            });
        }

        [Route("")]
        [HttpPost]
        public IActionResult GetAll(GetMaritalStatusListViewModel viewModel)
        {
            var mStatuses = repository.GetAllFull().OrderByDescending(x=>x.Created).AsEnumerable();

            if (!string.IsNullOrEmpty(viewModel.SearchMaritalStatus.SearchString))
            {
                mStatuses = mStatuses.Where(x=>x.Name.ToLower().Contains(viewModel.SearchMaritalStatus.SearchString.ToLower()));
            }

            viewModel.MaritalStatusViewModels = mStatuses.Select(x => new GetMaritalStatusViewModel
            {
                Id = x.Id,
                Name = x.Name,
                PersonsCount = x.Persons.Count()
            });

            return View(viewModel);
        }

        [Route("add")]
        [HttpGet]
        public IActionResult AddMaritalStatus()
        {
            return View();
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> AddMaritalStatus(EditMaritalStatusViewModel maritalStatusViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!repository.GetAll().Any(x => x.Name.ToLower() == maritalStatusViewModel.Name.ToLower()))
                {
                    var maritalStatus = converter.ConvertTo(maritalStatusViewModel);
                    await repository.CreateAsync(maritalStatus);
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Данное семейное положение уже есть в базе");
            }

            return View(maritalStatusViewModel);
        }

        [Route("edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditMaritalStatus(Guid id)
        {
            var maritalStatus = await repository.GetById(id);

            if (maritalStatus != null)
            {
                return View(converter.ConvertFrom(maritalStatus));
            }

            return NotFound();
        }

        [Route("edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> EditMaritalStatus(Guid id, EditMaritalStatusViewModel maritalStatusViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!repository.GetAll().Any(x => x.Name.ToLower() == maritalStatusViewModel.Name.ToLower()
                                                                && id != x.Id))
                {
                    var maritalStatus = await repository.GetById(id);

                    if (maritalStatus != null)
                    {
                        maritalStatus = converter.Update(maritalStatusViewModel, maritalStatus);
                        await repository.UpdateAsync(maritalStatus);
                        return RedirectToAction("Index");
                    }

                    return NotFound();
                }

                ModelState.AddModelError("", "Данное семейное положение уже есть в базе");
            }

            return View(maritalStatusViewModel);
        }

        [Route("deletemarital")]
        [HttpPost]
        public async Task<IActionResult> DeleteMaritalStatus(Guid id)
        {
            var maritalStatus = await repository.GetById(id);

            if (maritalStatus != null)
            {
                await repository.DeleteAsync(maritalStatus);
            }
            return RedirectToAction("Index");
        }
    }
}
