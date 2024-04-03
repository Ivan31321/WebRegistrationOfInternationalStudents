using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Services;
using MonitoringTheProgressOfForeignStudents.Domain.Model;
using MonitoringTheProgressOfForeignStudents.ViewModels.QuestionnaireVM;

namespace MonitoringTheProgressOfForeignStudents.Controllers
{
    [Route("[controller]")]
    [Authorize(Policy = "RequireWorkerRoles")]
    public class QuestionnaireController : Controller
    {
        private readonly EditQuestionnaireVMConverter converter;
        private readonly IOfficeService wordService;
        private readonly IQuestionnaireRepository questionnaireRepository;
        private readonly IGenderRepository genderRepository;
        private readonly ICountryRepository countryRepository;
        private readonly INationalityRepository nationalityRepository;
        private readonly IMaritalStatusRepository maritalStatusRepository;
        private readonly ISpecialtyRepository specialtyRepository;
        private readonly IEducationLanguageRepository educationLanguageRepository;
        private readonly IEducationTypeRepository educationTypeRepository;
        private readonly IPersonalDetailsRepository personalDetailsRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IHomeRepository homeRepository;

        public QuestionnaireController(EditQuestionnaireVMConverter converter,
            IOfficeService wordService,
            IQuestionnaireRepository questionnaireRepository,
            IGenderRepository genderRepository,
            ICountryRepository countryRepository,
            INationalityRepository nationalityRepository,
            IMaritalStatusRepository maritalStatusRepository,
            ISpecialtyRepository specialtyRepository,
            IEducationLanguageRepository educationLanguageRepository,
            IEducationTypeRepository educationTypeRepository,
            IPersonalDetailsRepository personalDetailsRepository,
            IOrderRepository orderRepository,
            IHomeRepository homeRepository)
        {
            this.converter = converter;
            this.wordService = wordService;
            this.questionnaireRepository = questionnaireRepository;
            this.genderRepository = genderRepository;
            this.countryRepository = countryRepository;
            this.nationalityRepository = nationalityRepository;
            this.maritalStatusRepository = maritalStatusRepository;
            this.specialtyRepository = specialtyRepository;
            this.educationLanguageRepository = educationLanguageRepository;
            this.educationTypeRepository = educationTypeRepository;
            this.personalDetailsRepository = personalDetailsRepository;
            this.orderRepository = orderRepository;
            this.homeRepository = homeRepository;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            InitViewBag();
            return View(new GetQuestionnaireListViewModel
            {
                QuestionnaireViewModels = questionnaireRepository.GetAllFull().AsEnumerable()
                .Select(x => new GetQuestionnaireViewModel
                {
                    Id = x.Id,
                    PersonalId = x.PersonalDetailsId,
                    Created = x.Created,
                    Name = x.PersonalDetails.Name,
                    Surname = x.PersonalDetails.Surname ?? string.Empty,
                    Patronymic = x.PersonalDetails.Patronymic ?? string.Empty,
                    Email = x.PersonalDetails.Email ?? string.Empty,
                    Gender = x.PersonalDetails.Gender?.Name ?? string.Empty,
                    Country = x.PersonalDetails.Country?.Name ?? string.Empty,
                    MaritalStatus = x.PersonalDetails.MaritalStatus?.Name ?? string.Empty,
                    Nationality = x.PersonalDetails.Nationality?.Name ?? string.Empty,
                    Faculty = x.Specialty?.Faculty?.Name ?? string.Empty,
                    Specialty = x.Specialty?.Name ?? string.Empty,
                    SpecialtyCode = x.Specialty?.Code ?? string.Empty
                })
                .GroupBy(x => x.PersonalId)
                .Select(x => x.OrderByDescending(f => f.Created).First()).ToList()
                .OrderByDescending(x => x.Created).ToList()
            });
        }

        [HttpPost]
        [Route("")]
        public IActionResult Index(GetQuestionnaireListViewModel viewModel)
        {
            InitViewBag();
            viewModel.QuestionnaireViewModels = SearchQuestionnaires(viewModel).ToList();
            return View(viewModel);
        }

        [HttpPost]
        [Route("get-file-list")]
        public FileResult GetFileList(GetQuestionnaireListViewModel questionnaireViewModel)
        {
            var viewmodels = SearchQuestionnaires(questionnaireViewModel).Select(x => new ExcelQuestionnaireViewModel
            {
                Surname = x.Surname,
                Name = x.Name,
                Patronymic = x.Patronymic,
                Email = x.Email,
                Country = x.Country,
                Nationality = x.Nationality,
                Gender = x.Gender,
                MaritalStatus = x.MaritalStatus,
                Specialty = x.Specialty,
                SpecialtyCode = x.SpecialtyCode,
                Faculty = x.Faculty,
            });
            var names = new List<string>
            {
                "Фамилия", "Имя", "Отчество", "Почта", "Страна", "Национальность", "Пол", "Семейное положение", "Спецальность", "Код специальности", "Факультет"
            };
            return File(wordService.ExportExcel(viewmodels, names), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{DateTime.Now}.xlsx");
        }

        [HttpGet]
        [Route("history/{personalId}")]
        public IActionResult History(Guid personalId)
        {
            return View(questionnaireRepository.GetAllFull()
                .Where(x => x.PersonalDetailsId == personalId)
                .OrderByDescending(f => f.Created).ToList()
                .Select(x => new GetQuestionnaireViewModel
                {
                    Id = x.Id,
                    PersonalId = x.PersonalDetailsId,
                    Created = x.Created,
                    Name = x.PersonalDetails.Name,
                    Surname = x.PersonalDetails.Surname ?? string.Empty,
                    Patronymic = x.PersonalDetails.Patronymic ?? string.Empty,
                    Email = x.PersonalDetails.Email ?? string.Empty,
                    Gender = x.PersonalDetails.Gender?.Name ?? string.Empty,
                    Country = x.PersonalDetails.Country?.Name ?? string.Empty,
                    MaritalStatus = x.PersonalDetails.MaritalStatus?.Name ?? string.Empty,
                    Nationality = x.PersonalDetails.Nationality?.Name ?? string.Empty,
                    Faculty = x.Specialty?.Faculty?.Name ?? string.Empty,
                    Specialty = x.Specialty?.Name ?? string.Empty,
                    SpecialtyCode = x.Specialty?.Code ?? string.Empty
                }));
        }

        [HttpPost]
        [Route("get-file")]
        public async Task<FileResult> GetFile(EditQuestionnaireViewModel questionnaireViewModel)
        {
            string fileName = @"АНКЕТА.docx";
            var questFile = new QuestionnaireFileViewModel(questionnaireViewModel);
            questFile.Gender = (await genderRepository.GetById(questionnaireViewModel.GenderId ?? Guid.Empty))?.Name ?? "Пол не определен";
            questFile.Country = (await countryRepository.GetById(questionnaireViewModel.CountryId ?? Guid.Empty))?.Name ?? "Страна не определена";
            questFile.MaritalStatus = (await maritalStatusRepository.GetById(questionnaireViewModel.MaritalStatusId ?? Guid.Empty))?.Name ?? "Семейное положение не определено";
            questFile.Nationality = (await nationalityRepository.GetById(questionnaireViewModel.NationalityId ?? Guid.Empty))?.Name ?? "Национальность не определена";
            questFile.Specialty = (await specialtyRepository.GetById(questionnaireViewModel.SpecialtyId ?? Guid.Empty))?.Name ?? "Специальность не определена";
            var dict = questFile.ToFileDict();
            return File(wordService.ReplaceTemplate(fileName, dict), "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                $"{questFile.Name} {questFile.Surname} {questFile.Patronymic} {DateTime.Now}.docx");
        }

        [Route("add/{PersonalDetailsId}")]
        [Route("add")]
        [HttpGet]
        public async Task<IActionResult> AddQuestionnaire(Guid PersonalDetailsId)
        {
            InitViewBag();

            if (PersonalDetailsId != Guid.Empty)
            {
                var questionnaire = questionnaireRepository.GetAll().OrderByDescending(x => x.Created).FirstOrDefault(x => x.PersonalDetailsId == PersonalDetailsId);
                if (questionnaire != null)
                {
                    return RedirectToAction("EditQuestionnaire", new { id = questionnaire.Id });
                }

                var personal = await personalDetailsRepository.GetById(PersonalDetailsId);
                if (personal != null)
                    return View(converter.ConvertFrom(new Questionnaire
                    {
                        PersonalDetails = personal
                    }));
            }

            return View();
        }

        [Route("add/{PersonalDetailsId}")]
        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> AddQuestionnaire(EditQuestionnaireViewModel questionnaireViewModel)
        {
            InitViewBag();
            if (ModelState.IsValid)
            {
                var questionnaire = converter.ConvertTo(questionnaireViewModel);
                await questionnaireRepository.CreateAsync(questionnaire);
                return RedirectToAction("Index");
            }

            return View(questionnaireViewModel);
        }

        [Route("edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditQuestionnaire(Guid id)
        {
            InitViewBag();

            var questionnaire = await questionnaireRepository.GetFullByIdAsync(id);

            if (questionnaire != null)
            {
                return View(converter.ConvertFrom(questionnaire));
            }

            return View();
        }

        [Route("edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> EditQuestionnaire(EditQuestionnaireViewModel questionnaireViewModel)
        {
            InitViewBag();
            ModelState.Remove("Birthday");
            ModelState.Remove("PlaceofBirth");
            ModelState.Remove("PermamentHomeAddress");
            if (ModelState.IsValid)
            {
                var quest = converter.ConvertTo(questionnaireViewModel);
                await questionnaireRepository.CreateAsync(quest);
                return RedirectToAction("Index");
            }

            return View(questionnaireViewModel);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteQuestionnaire(Guid id)
        {
            var questionnaire = await questionnaireRepository.GetFullByIdAsync(id);
            if (questionnaire != null)
            {
                await questionnaireRepository.DeleteAsync(questionnaire);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("addorder")]
        public async Task<IActionResult> AddOrderToQuestionnaire(OrderType orderType, EditQuestionnaireViewModel viewModel)
        {
            if (viewModel.Orders == null)
            {
                viewModel.Orders = new List<Order>();
            }

            viewModel.Orders.Add(new Order
            {
                OrderType = orderType,
            });

            var newQuest = converter.ConvertTo(viewModel);
            var questId = await questionnaireRepository.CreateAsync(newQuest);

            return RedirectToAction("EditQuestionnaire", new { id = questId });
        }

        [HttpPost]
        [Route("removeorder")]
        public async Task<IActionResult> RemoveOrderFromQuestionnaire(int index, OrderType orderType, EditQuestionnaireViewModel viewModel)
        {
            if (orderType == OrderType.Order && viewModel.Orders != null && viewModel.Orders.Count > index && index >= 0)
            {
                viewModel.Orders.RemoveAt(index);
                var newQuest = converter.ConvertTo(viewModel);
                var questId = await questionnaireRepository.CreateAsync(newQuest);
                return RedirectToAction("EditQuestionnaire", new { id = questId });
            }

            if (orderType == OrderType.Reprimand && viewModel.Reprimands != null && viewModel.Reprimands.Count > index && index >= 0)
            {
                viewModel.Reprimands.RemoveAt(index);
                var newQuest = converter.ConvertTo(viewModel);
                var questId = await questionnaireRepository.CreateAsync(newQuest);
                return RedirectToAction("EditQuestionnaire", new { id = questId });
            }

            var quest = await questionnaireRepository.GetAllFull()
                .Where(x => x.PersonalDetailsId == viewModel.PersonalDetailsId)
                .OrderByDescending(x => x.Created)
                .FirstOrDefaultAsync();

            return RedirectToAction("EditQuestionnaire", new { id = quest.Id });
        }

        [HttpPost]
        [Route("addhome")]
        public async Task<IActionResult> AddHomeToQuestionnaire(EditQuestionnaireViewModel viewModel)
        {
            if (viewModel.Homes == null)
            {
                viewModel.Homes = new List<Home>();
            }
            viewModel.Homes.Add(new Home());

            var newQuest = converter.ConvertTo(viewModel);
            var questId = await questionnaireRepository.CreateAsync(newQuest);

            return RedirectToAction("EditQuestionnaire", new { id = questId });
        }

        [HttpPost]
        [Route("removehome")]
        public async Task<IActionResult> RemoveHomeFromQuestionnaire(int index, EditQuestionnaireViewModel viewModel)
        {
            if (viewModel.Homes != null && viewModel.Homes.Count > index && index >= 0)
            {
                viewModel.Homes.RemoveAt(index);
                var newQuest = converter.ConvertTo(viewModel);
                var questId = await questionnaireRepository.CreateAsync(newQuest);
                return RedirectToAction("EditQuestionnaire", new { id = questId });
            }

            var quest = await questionnaireRepository.GetAllFull()
                .Where(x => x.PersonalDetailsId == viewModel.PersonalDetailsId)
                .OrderByDescending(x => x.Created)
                .FirstOrDefaultAsync();

            return RedirectToAction("EditQuestionnaire", new { id = quest.Id });
        }

        private void InitViewBag()
        {
            ViewBag.Nationalities = new SelectList(nationalityRepository.GetAll(), nameof(Nationality.Id), nameof(Nationality.Name));
            ViewBag.MaritalStatuses = new SelectList(maritalStatusRepository.GetAll(), nameof(MaritalStatus.Id), nameof(MaritalStatus.Name));
            ViewBag.Genders = new SelectList(genderRepository.GetAll(), nameof(Gender.Id), nameof(Gender.Name));
            ViewBag.Countries = new SelectList(countryRepository.GetAll(), nameof(Country.Id), nameof(Country.Name));
            ViewBag.Specialties = new SelectList(specialtyRepository.GetAll().Include(f => f.Faculty)
                .Select(x => new { Id = x.Id, Name = $"{x.Faculty.Name} {x.Name} {x.Code}" }), "Id", "Name");
            ViewBag.EduLang = new SelectList(educationLanguageRepository.GetAll(), nameof(EducationLanguage.Id), nameof(EducationLanguage.Language));
            ViewBag.EduType = new SelectList(educationTypeRepository.GetAll(), nameof(EducationType.Id), nameof(EducationType.Type));
        }

        private IEnumerable<GetQuestionnaireViewModel> SearchQuestionnaires(GetQuestionnaireListViewModel viewModel)
        {
            var questionnaires = questionnaireRepository.GetAllFull().ToList()
                .GroupBy(x => x.PersonalDetailsId)
                .Select(x => x.OrderByDescending(f => f.Created).First());

            if (viewModel.SearchQuestionnaire.GenderId != Guid.Empty)
            {
                questionnaires = questionnaires.Where(x => x.PersonalDetails.GenderId == viewModel.SearchQuestionnaire.GenderId);
            }
            if (viewModel.SearchQuestionnaire.CountryId != Guid.Empty)
            {
                questionnaires = questionnaires.Where(x => x.PersonalDetails.CountryId == viewModel.SearchQuestionnaire.CountryId);
            }
            if (viewModel.SearchQuestionnaire.NationalityId != Guid.Empty)
            {
                questionnaires = questionnaires.Where(x => x.PersonalDetails.NationalityId == viewModel.SearchQuestionnaire.NationalityId);
            }
            if (viewModel.SearchQuestionnaire.MaritalStatusId != Guid.Empty)
            {
                questionnaires = questionnaires.Where(x => x.PersonalDetails.MaritalStatusId == viewModel.SearchQuestionnaire.MaritalStatusId);
            }
            if (viewModel.SearchQuestionnaire.SpecialtyId != Guid.Empty)
            {
                questionnaires = questionnaires.Where(x => x.SpecialtyId == viewModel.SearchQuestionnaire.SpecialtyId);
            }
            if (viewModel.SearchQuestionnaire.EducationLanguageId != Guid.Empty)
            {
                questionnaires = questionnaires.Where(x => x.EducationLanguageId == viewModel.SearchQuestionnaire.EducationLanguageId);
            }
            if (viewModel.SearchQuestionnaire.EducationTypeId != Guid.Empty)
            {
                questionnaires = questionnaires.Where(x => x.EducationTypeId == viewModel.SearchQuestionnaire.EducationTypeId);
            }

            var res = questionnaires.Select(x => new GetQuestionnaireViewModel
            {
                Id = x.Id,
                PersonalId = x.PersonalDetailsId,
                Created = x.Created,
                Name = x.PersonalDetails.Name,
                Surname = x.PersonalDetails.Surname,
                Patronymic = x.PersonalDetails.Patronymic,
                Email = x.PersonalDetails?.Email,
                Gender = x.PersonalDetails?.Gender?.Name,
                Country = x.PersonalDetails?.Country?.Name,
                MaritalStatus = x.PersonalDetails?.MaritalStatus?.Name,
                Nationality = x.PersonalDetails?.Nationality?.Name,
                Faculty = x.Specialty?.Faculty?.Name,
                Specialty = x.Specialty?.Name,
                SpecialtyCode = x.Specialty?.Code
            });

            if (!string.IsNullOrEmpty(viewModel.SearchQuestionnaire.SearchString))
            {
                res = res.Where(x => x.ToString().ToLower().Contains(viewModel.SearchQuestionnaire.SearchString.ToLower()));
            }

            return res;
        }
    }
}
