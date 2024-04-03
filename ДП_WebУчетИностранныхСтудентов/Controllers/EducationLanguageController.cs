using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories;
using MonitoringTheProgressOfForeignStudents.Domain.Model;
using MonitoringTheProgressOfForeignStudents.ViewModels.EducationLanguageVM;

namespace MonitoringTheProgressOfForeignStudents.Controllers
{
    [Route("[controller]")]
    [Authorize(Policy = "RequireWorkerRoles")]
    public class EducationLanguageController : Controller
    {
        private readonly EditEducationLanguageVMConverter converter;
        private readonly IEducationLanguageRepository repository;
        private readonly IQuestionnaireRepository questionnaireRepository;

        public EducationLanguageController(EditEducationLanguageVMConverter converter,
                                            IEducationLanguageRepository repository,
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

            return View(new GetEducationLanguageListViewModel
            {
                EducationLanguageViewModels = repository.GetAllFull()
                .OrderByDescending(x => x.Created).AsEnumerable()
                .Select(x => new GetEducationLanguageViewModel
                {
                    Id = x.Id,
                    Language = x.Language,
                    QuestionnairesCount = latestQuestionnaires.Count(q => q.EducationLanguageId == x.Id),
                })
            });
        }

        [Route("")]
        [HttpPost]
        public IActionResult GetAll(GetEducationLanguageListViewModel viewModel)
        {
            var latestQuestionnaires = questionnaireRepository.GetAllFull()
                .GroupBy(x => x.PersonalDetails)
                .Select(x => x.OrderByDescending(f => f.Created).First()).ToList();

            var eduLangs = repository.GetAllFull().OrderByDescending(x => x.Created).AsEnumerable();

            if (!string.IsNullOrEmpty(viewModel.SearchEducationLanguage.SearchString))
            {
                eduLangs = eduLangs.Where(x => x.Language.ToLower().Contains(viewModel.SearchEducationLanguage.SearchString.ToLower()));
            }

            viewModel.EducationLanguageViewModels = eduLangs
                .Select(x => new GetEducationLanguageViewModel
                {
                    Id = x.Id,
                    Language = x.Language,
                    QuestionnairesCount = latestQuestionnaires.Count(q => q.EducationLanguageId == x.Id),
                });

            return View(viewModel);
        }

        [Route("add")]
        [HttpGet]
        public IActionResult AddEducationLanguage()
        {
            return View();
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> AddEducationLanguage(EditEducationLanguageViewModel educationLanguageViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!repository.GetAll().Any(x => x.Language.ToLower() == educationLanguageViewModel.Language.ToLower()))
                {
                    var language = converter.ConvertTo(educationLanguageViewModel);
                    await repository.CreateAsync(language);
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Данный язык уже есть в базе");
            }

            return View(educationLanguageViewModel);
        }

        [Route("edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditEducationLanguage(Guid id)
        {
            var language = await repository.GetById(id);

            if (language != null)
            {
                return View(converter.ConvertFrom(language));
            }

            return NotFound();
        }

        [Route("edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> EditEducationLanguage(Guid id, EditEducationLanguageViewModel educationLanguageViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!repository.GetAll().Any(x => x.Language.ToLower() == educationLanguageViewModel.Language.ToLower()
                                                                && id != x.Id))
                {
                    var language = await repository.GetById(id);

                    if (language != null)
                    {
                        language = converter.Update(educationLanguageViewModel, language);
                        await repository.UpdateAsync(language);

                        return RedirectToAction("Index");
                    }

                    return NotFound();
                }

                ModelState.AddModelError("", "Данный язык уже есть в базе");
            }

            return View(educationLanguageViewModel);
        }

        [Route("deleteedulang")]
        [HttpPost]
        public async Task<IActionResult> DeleteEducationLanguage(Guid id)
        {
            var language = await repository.GetById(id);

            if (language != null)
            {
                await repository.DeleteAsync(language);
            }
            return RedirectToAction("Index");
        }
    }
}
