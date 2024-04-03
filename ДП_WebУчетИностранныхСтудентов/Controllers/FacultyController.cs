using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories;
using MonitoringTheProgressOfForeignStudents.ViewModels.FacultyVM;

namespace MonitoringTheProgressOfForeignStudents.Controllers
{
    [Route("[controller]")]
    [Authorize(Policy = "RequireWorkerRoles")]
    public class FacultyController : Controller
    {
        private readonly EditFacultyVMConverter converter;
        private readonly IFacultyRepository repository;

        public FacultyController(EditFacultyVMConverter converter,
            IFacultyRepository repository)
        {
            this.converter = converter;
            this.repository = repository;
        }

        [Route("")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return View(new GetFacultyListViewModel
            {
                FacultyViewModels = repository.GetAllFull()
                .OrderByDescending(x => x.Created).AsEnumerable()
                .Select(x => new GetFacultyViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    SpecialtiesCount = x.Specialties.Count(),
                })
            });
        }

        [Route("")]
        [HttpPost]
        public IActionResult GetAll(GetFacultyListViewModel viewModel)
        {
            var faculties = repository.GetAllFull().OrderByDescending(x => x.Created).AsEnumerable();

            if (!string.IsNullOrEmpty(viewModel.SearchFaculty.SearchString))
            {
                faculties = faculties.Where(x => x.Name.ToLower().Contains(viewModel.SearchFaculty.SearchString.ToLower()));
            }

            viewModel.FacultyViewModels = faculties.Select(x => new GetFacultyViewModel
            {
                Id = x.Id,
                Name = x.Name,
                SpecialtiesCount = x.Specialties.Count(),
            });

            return View(viewModel);
        }

        [Route("add")]
        [HttpGet]
        public IActionResult AddFaculty()
        {
            return View();
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> AddFaculty(EditFacultyViewModel facultyViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!repository.GetAll().Any(x => x.Name.ToLower() == facultyViewModel.Name.ToLower()))
                {
                    var faculty = converter.ConvertTo(facultyViewModel);
                    await repository.CreateAsync(faculty);
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Данный факультет уже есть в базе");
            }

            return View(facultyViewModel);
        }

        [Route("edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditFaculty(Guid id)
        {
            var faculty = await repository.GetById(id);

            if (faculty != null)
            {
                return View(converter.ConvertFrom(faculty));
            }

            return NotFound();
        }

        [Route("edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> EditFaculty(Guid id, EditFacultyViewModel facultyViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!repository.GetAll().Any(x => x.Name.ToLower() == facultyViewModel.Name.ToLower()
                                                                && id != x.Id))
                {
                    var faculty = await repository.GetById(id);

                    if (faculty != null)
                    {
                        faculty = converter.Update(facultyViewModel, faculty);
                        await repository.UpdateAsync(faculty);

                        return RedirectToAction("Index");
                    }

                    return NotFound();
                }

                ModelState.AddModelError("", "Данный факультет уже есть в базе");
            }

            return View(facultyViewModel);
        }

        [Route("deletefaculty")]
        [HttpPost]
        public async Task<IActionResult> DeleteFaculty(Guid id)
        {
            var faculty = await repository.GetById(id);

            if (faculty != null)
            {
                await repository.DeleteAsync(faculty);
            }
            return RedirectToAction("Index");
        }
    }
}
