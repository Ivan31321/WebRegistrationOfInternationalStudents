using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories;
using MonitoringTheProgressOfForeignStudents.Domain.Model;
using MonitoringTheProgressOfForeignStudents.Infrastructure.Repositories;
using MonitoringTheProgressOfForeignStudents.ViewModels.SpecialtyVM;

namespace MonitoringTheProgressOfForeignStudents.Controllers
{
    [Authorize(Policy = "RequireWorkerRoles")]
    [Route("[controller]")]
    public class SpecialtyController : Controller
    {
        private readonly EditSpecialtyVMConverter converter;
        private readonly ISpecialtyRepository specialtyRepository;
        private readonly IFacultyRepository facultyRepository;
        private readonly IQuestionnaireRepository questionnaireRepository;

        public SpecialtyController(EditSpecialtyVMConverter converter,
            ISpecialtyRepository specialtyRepository,
            IFacultyRepository facultyRepository,
            IQuestionnaireRepository questionnaireRepository)
        {
            this.converter = converter;
            this.specialtyRepository = specialtyRepository;
            this.facultyRepository = facultyRepository;
            this.questionnaireRepository = questionnaireRepository;
        }

        [Route("")]
        [HttpGet]
        public IActionResult GetAll()
        {
            ViewBag.Faculties = new SelectList(facultyRepository.GetAll(), nameof(Faculty.Id), nameof(Faculty.Name));
            var latestQuestionnaires = questionnaireRepository.GetAllFull()
                .GroupBy(x => x.PersonalDetails)
                .Select(x => x.OrderByDescending(f => f.Created).First()).ToList();

            return View(new GetSpecialtyListViewModel
            {
                SpecialtyViewModels = specialtyRepository.GetAllFull()
                .OrderByDescending(x=>x.Created).AsEnumerable()
                .Select(x => new GetSpecialtyViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Faculty = x.Faculty?.Name ?? string.Empty,
                    QuestionnairesCount = latestQuestionnaires.Count(q => q.SpecialtyId == x.Id)
                })
            });
        }

        [Route("")]
        [HttpPost]
        public IActionResult GetAll(GetSpecialtyListViewModel viewModel)
        {
            ViewBag.Faculties = new SelectList(facultyRepository.GetAll(), nameof(Faculty.Id), nameof(Faculty.Name));
            var latestQuestionnaires = questionnaireRepository.GetAllFull()
                .GroupBy(x => x.PersonalDetails)
                .Select(x => x.OrderByDescending(f => f.Created).First()).ToList();

            var specialties = specialtyRepository.GetAllFull().OrderByDescending(x => x.Created).AsEnumerable();

            if(viewModel.SearchSpecialty.FacultyId != Guid.Empty)
            {
                specialties = specialties.Where(x => x.FacultyId == viewModel.SearchSpecialty.FacultyId);
            }

            if (!string.IsNullOrEmpty(viewModel.SearchSpecialty.SearchString))
            {
                specialties = specialties.Where(x=>x.Name.ToLower().Contains(viewModel.SearchSpecialty.SearchString.ToLower()) ||
                                                x.Code.ToLower().Contains(viewModel.SearchSpecialty.SearchString.ToLower()));
            }

            viewModel.SpecialtyViewModels = specialties.Select(x=> new GetSpecialtyViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                Faculty = x.Faculty?.Name ?? string.Empty,
                QuestionnairesCount = latestQuestionnaires.Count(q => q.SpecialtyId == x.Id)
            });

            return View(viewModel);
        }

        [Route("add")]
        [HttpGet]
        public IActionResult AddSpecialty()
        {
            ViewBag.Faculties = new SelectList(facultyRepository.GetAll(), nameof(Faculty.Id), nameof(Faculty.Name));
            return View();
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> AddSpecialty(EditSpecialtyViewModel specialtyViewModel)
        {
            ViewBag.Faculties = new SelectList(facultyRepository.GetAll(), nameof(Faculty.Id), nameof(Faculty.Name));
            if (ModelState.IsValid)
            {
                if (!specialtyRepository.GetAll().Any(x => x.Name.ToLower() == specialtyViewModel.Name.ToLower() &&
                                                        x.Code.ToLower() == specialtyViewModel.Code.ToLower()))
                {
                    var specialty = converter.ConvertTo(specialtyViewModel);
                    await specialtyRepository.CreateAsync(specialty);
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Данная специальность уже есть в базе");
            }

            return View(specialtyViewModel);
        }

        [Route("edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditSpecialty(Guid id)
        {
            ViewBag.Faculties = new SelectList(facultyRepository.GetAll(), nameof(Faculty.Id), nameof(Faculty.Name));
            var specialty = await specialtyRepository.GetById(id);

            if (specialty != null)
            {
                return View(converter.ConvertFrom(specialty));
            }

            return NotFound();
        }

        [Route("edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> EditSpecialty(Guid id, EditSpecialtyViewModel specialtyViewModel)
        {
            ViewBag.Faculties = new SelectList(facultyRepository.GetAll(), nameof(Faculty.Id), nameof(Faculty.Name));
            if (ModelState.IsValid)
            {
                if (!specialtyRepository.GetAll().Any(x => x.Name.ToLower() == specialtyViewModel.Name.ToLower() &&
                                                            x.Code.ToLower() == specialtyViewModel.Code.ToLower() &&
                                                            id != x.Id))
                {
                    var specialty = await specialtyRepository.GetById(id);

                    if (specialty != null)
                    {
                        specialty = converter.Update(specialtyViewModel, specialty);
                        await specialtyRepository.UpdateAsync(specialty);
                        return RedirectToAction("Index");
                    }

                    return NotFound();
                }

                ModelState.AddModelError("", "Данная группа уже есть в базе");
            }

            return View(specialtyViewModel);
        }

        [Route("deletespecialty")]
        [HttpPost]
        public async Task<IActionResult> DeleteSpecialty(Guid id)
        {
            var specialty = await specialtyRepository.GetById(id);

            if (specialty != null)
            {
                await specialtyRepository.DeleteAsync(specialty);
            }
            return RedirectToAction("Index");
        }
    }
}
