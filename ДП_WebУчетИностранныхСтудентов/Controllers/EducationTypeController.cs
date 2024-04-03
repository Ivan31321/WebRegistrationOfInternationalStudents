using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories;
using MonitoringTheProgressOfForeignStudents.ViewModels.EducationLanguageVM;
using MonitoringTheProgressOfForeignStudents.ViewModels.EducationTypeVM;

namespace MonitoringTheProgressOfForeignStudents.Controllers
{
    [Route("[controller]")]
    [Authorize(Policy = "RequireWorkerRoles")]
    public class EducationTypeController : Controller
    {
        private readonly EditEducationTypeVMConverter converter;
        private readonly IEducationTypeRepository repository;
        private readonly IQuestionnaireRepository questionnaireRepository;

        public EducationTypeController(EditEducationTypeVMConverter converter,
            IEducationTypeRepository repository,
            IQuestionnaireRepository questionnaireRepository)
        {
            this.converter = converter;
            this.repository = repository;
            this.questionnaireRepository = questionnaireRepository;
        }

        [Route("")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var latestQuestionnaires = questionnaireRepository.GetAllFull()
                .GroupBy(x => x.PersonalDetails)
                .Select(x => x.OrderByDescending(f => f.Created).First()).ToList();

            return View(new GetEducationTypeListViewModel
            {
                EducationTypeViewModels = repository.GetAllFull()
                .OrderByDescending(x => x.Created).AsEnumerable()
                .Select(x => new GetEducationTypeViewModel
                {
                    Id = x.Id,
                    Type = x.Type,
                    QuestionnairesCount = latestQuestionnaires.Count(q => q.EducationTypeId == x.Id)
                })
            });
        }

        [Route("")]
        [HttpPost]
        public IActionResult GetAll(GetEducationTypeListViewModel viewModel)
        {
            var latestQuestionnaires = questionnaireRepository.GetAllFull()
                .GroupBy(x => x.PersonalDetails)
                .Select(x => x.OrderByDescending(f => f.Created).First()).ToList();

            var eduTypes = repository.GetAllFull().OrderByDescending(x => x.Created).AsEnumerable();

            if (!string.IsNullOrEmpty(viewModel.SearchEducationType.SearchString))
            {
                eduTypes = eduTypes.Where(x => x.Type.ToLower().Contains(viewModel.SearchEducationType.SearchString.ToLower()));
            }

            viewModel.EducationTypeViewModels = eduTypes
                .Select(x => new GetEducationTypeViewModel
                {
                    Id = x.Id,
                    Type = x.Type,
                    QuestionnairesCount = latestQuestionnaires.Count(q => q.EducationLanguageId == x.Id),
                });

            return View(viewModel);
        }

        [Route("add")]
        [HttpGet]
        public IActionResult AddEducationType()
        {
            return View();
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> AddEducationType(EditEducationTypeViewModel educationTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!repository.GetAll().Any(x => x.Type.ToLower() == educationTypeViewModel.Type.ToLower()))
                {
                    var type = converter.ConvertTo(educationTypeViewModel);
                    await repository.CreateAsync(type);
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Данный тип уже есть в базе");
            }

            return View(educationTypeViewModel);
        }

        [Route("edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditEducationType(Guid id)
        {
            var type = await repository.GetById(id);

            if (type != null)
            {
                return View(converter.ConvertFrom(type));
            }

            return NotFound();
        }

        [Route("edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> EditEducationType(Guid id, EditEducationTypeViewModel educationTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!repository.GetAll().Any(x => x.Type.ToLower() == educationTypeViewModel.Type.ToLower()
                                                                    && id != x.Id))
                {
                    var type = await repository.GetById(id);

                    if (type != null)
                    {
                        type = converter.Update(educationTypeViewModel, type);
                        await repository.UpdateAsync(type);

                        return RedirectToAction("Index");
                    }

                    return NotFound();
                }

                ModelState.AddModelError("", "Данный тип уже есть в базе");
            }

            return View(educationTypeViewModel);
        }

        [Route("deleteedutype")]
        [HttpPost]
        public async Task<IActionResult> DeleteEducationType(Guid id)
        {
            var type = await repository.GetById(id);

            if (type != null)
            {
                await repository.DeleteAsync(type);
            }
            return RedirectToAction("Index");
        }
    }
}
