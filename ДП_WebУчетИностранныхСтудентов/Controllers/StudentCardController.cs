using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Repositories;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Services;
using MonitoringTheProgressOfForeignStudents.Domain.Model;
using MonitoringTheProgressOfForeignStudents.ViewModels.QuestionnaireVM;
using MonitoringTheProgressOfForeignStudents.ViewModels.StudentCardVM;

namespace MonitoringTheProgressOfForeignStudents.Controllers
{
    [Route("[controller]")]
    [Authorize(Policy = "RequireWorkerRoles")]
    public class StudentCardController : Controller
    {
        private readonly EditStudentCardVMConverter converter;
        private readonly IOfficeService wordService;
        private readonly IStudentCardRepository studentCardRepository;
        private readonly IPersonalDetailsRepository personalDetailsRepository;
        private readonly IRegisterRepository registerRepository;
        private readonly IPassportInfoRepository passportInfoRepository;
        private readonly INationalityRepository nationalityRepository;
        private readonly IMaritalStatusRepository maritalStatusRepository;
        private readonly IGenderRepository genderRepository;
        private readonly ICountryRepository countryRepository;

        public StudentCardController(EditStudentCardVMConverter converter,
            IOfficeService wordService,
            IStudentCardRepository studentCardRepository,
            IPersonalDetailsRepository personalDetailsRepository,
            IRegisterRepository registerRepository,
            IPassportInfoRepository passportInfoRepository,
            INationalityRepository nationalityRepository,
            IMaritalStatusRepository maritalStatusRepository,
            IGenderRepository genderRepository,
            ICountryRepository countryRepository)
        {
            this.converter = converter;
            this.wordService = wordService;
            this.studentCardRepository = studentCardRepository;
            this.personalDetailsRepository = personalDetailsRepository;
            this.registerRepository = registerRepository;
            this.passportInfoRepository = passportInfoRepository;
            this.nationalityRepository = nationalityRepository;
            this.maritalStatusRepository = maritalStatusRepository;
            this.genderRepository = genderRepository;
            this.countryRepository = countryRepository;
        }

        [HttpGet]
        [Route("history/{personalId}")]
        public IActionResult History(Guid personalId)
        {
            return View(studentCardRepository.GetAllFull()
                .Where(x => x.PersonalDetailsId == personalId)
                .OrderByDescending(f => f.Created).ToList()
                .Select(x => new GetStudentCardViewModel
                {
                    Id = x.Id,
                    PersonalId = x.PersonalDetailsId,
                    Created = x.Created,
                    Name = x.PersonalDetails.Name,
                    Surname = x.PersonalDetails.Surname ?? "",
                    Patronymic = x.PersonalDetails.Patronymic ?? "",
                    Email = x.PersonalDetails.Email ?? "",
                    Gender = x.PersonalDetails.Gender?.Name ?? string.Empty,
                    Country = x.PersonalDetails.Country?.Name ?? string.Empty,
                    MaritalStatus = x.PersonalDetails.MaritalStatus?.Name ?? string.Empty,
                    Nationality = x.PersonalDetails.Nationality?.Name ?? string.Empty,
                }).OrderByDescending(x => x.Created));
        }

        [HttpPost]
        [Route("get-file-list")]
        public FileResult GetFileList(GetStudentCardListViewModel viewModel)
        {
            var viewmodels = SearchStudentCards(viewModel).Select(x => new ExcelStudentCardViewModel
            {
                Surname = x.Surname,
                Name = x.Name,
                Patronymic = x.Patronymic,
                Email = x.Email,
                Country = x.Country,
                Nationality = x.Nationality,
                Gender = x.Gender,
                MaritalStatus = x.MaritalStatus,
                PassportValid = x.PassportValid,
                RegisterValid = x.RegisterValid,
            });
            var names = new List<string>
            {
                "Фамилия", "Имя", "Отчество", "Почта", "Страна", "Национальность", "Пол", "Семейное положение", "Пасспорт действителен?", "Регистрация действительна?"
            };
            return File(wordService.ExportExcel(viewmodels, names), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{DateTime.Now}.xlsx");
        }

        [HttpPost]
        [Route("get-file")]
        public FileResult GetFile(EditStudentCardViewModel studentCardViewModel)
        {
            string fileName = @"blank.docx";
            var cardFile = new StudentCardFileViewModel(studentCardViewModel);
            var dict = cardFile.ToFileDict();
            return File(wordService.ReplaceTemplate(fileName, dict), "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                $"{cardFile.Name} {cardFile.Surname} {cardFile.Patronymic} {DateTime.Now}.docx");
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAll()
        {
            ViewBag.Nationalities = new SelectList(nationalityRepository.GetAll(), nameof(Nationality.Id), nameof(Nationality.Name));
            ViewBag.MaritalStatuses = new SelectList(maritalStatusRepository.GetAll(), nameof(MaritalStatus.Id), nameof(MaritalStatus.Name));
            ViewBag.Genders = new SelectList(genderRepository.GetAll(), nameof(Gender.Id), nameof(Gender.Name));
            ViewBag.Countries = new SelectList(countryRepository.GetAll(), nameof(Country.Id), nameof(Country.Name));
            return View(new GetStudentCardListViewModel
            {
                StudentCardViewModels = studentCardRepository.GetAllFull().AsEnumerable()
                .Select(x => new GetStudentCardViewModel
                {
                    Id = x.Id,
                    PersonalId = x.PersonalDetailsId,
                    Created = x.Created,
                    Name = x.PersonalDetails.Name,
                    Surname = x.PersonalDetails.Surname ?? "",
                    Patronymic = x.PersonalDetails.Patronymic ?? "",
                    Email = x.PersonalDetails.Email ?? "",
                    Gender = x.PersonalDetails.Gender?.Name ?? string.Empty,
                    Country = x.PersonalDetails.Country?.Name ?? string.Empty,
                    MaritalStatus = x.PersonalDetails.MaritalStatus?.Name ?? string.Empty,
                    Nationality = x.PersonalDetails.Nationality?.Name ?? string.Empty,
                })
                .GroupBy(x => x.PersonalId)
                .Select(x => x.OrderByDescending(f => f.Created).First())
            });
        }

        [HttpPost]
        [Route("")]
        public IActionResult GetAll(GetStudentCardListViewModel viewModel)
        {
            ViewBag.Nationalities = new SelectList(nationalityRepository.GetAll(), nameof(Nationality.Id), nameof(Nationality.Name));
            ViewBag.MaritalStatuses = new SelectList(maritalStatusRepository.GetAll(), nameof(MaritalStatus.Id), nameof(MaritalStatus.Name));
            ViewBag.Genders = new SelectList(genderRepository.GetAll(), nameof(Gender.Id), nameof(Gender.Name));
            ViewBag.Countries = new SelectList(countryRepository.GetAll(), nameof(Country.Id), nameof(Country.Name));

            viewModel.StudentCardViewModels = SearchStudentCards(viewModel);

            return View(viewModel);
        }

        [Route("add/{PersonalDetailsId}")]
        [HttpGet]
        public async Task<IActionResult> AddStudentCard(Guid personalDetailsId)
        {
            var sCard = studentCardRepository.GetAll()
                .Where(x => x.PersonalDetailsId == personalDetailsId)
                .OrderByDescending(x => x.Created)
                .FirstOrDefault();

            if (sCard == null)
            {
                var personalDetails = await personalDetailsRepository.GetAllFull()
                    .FirstOrDefaultAsync(x => x.Id == personalDetailsId);

                if (personalDetails != null)
                {
                    var studentCardViewModel = converter.ConvertFrom(new StudentCard
                    {
                        PersonalDetails = personalDetails
                    });

                    return View(studentCardViewModel);
                }
            }
            else
            {
                return RedirectToAction("EditStudentCard", new { id = sCard.Id });
            }

            return NotFound();
        }

        [Route("add/{PersonalDetailsId}")]
        [HttpPost]
        public async Task<IActionResult> AddStudentCard(Guid personalDetailsId, EditStudentCardViewModel studentCardViewModel)
        {
            if (ModelState.IsValid)
            {
                var personalDetails = await personalDetailsRepository.GetAllFull()
                    .FirstOrDefaultAsync(x => x.Id == personalDetailsId);

                var pas = studentCardViewModel.Passports;
                var reg = studentCardViewModel.Registers;

                studentCardViewModel = converter.ConvertFrom(new Domain.Model.StudentCard
                {
                    PersonalDetails = personalDetails,
                });

                studentCardViewModel.Passports = pas;
                studentCardViewModel.Registers = reg;

                var studentCard = converter.ConvertTo(studentCardViewModel);

                await studentCardRepository.CreateAsync(studentCard);

                return RedirectToAction("Index");
            }

            return View(studentCardViewModel);
        }

        [Route("edit/{id}")]
        [HttpGet]
        public IActionResult EditStudentCard(Guid id)
        {
            var studentCard = studentCardRepository.GetAllFull()
                .FirstOrDefault(x => x.Id == id);

            if (studentCard != null)
            {
                return View(converter.ConvertFrom(studentCard));
            }

            return NotFound();
        }

        [Route("edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> EditStudentCard(EditStudentCardViewModel studentCardViewModel)
        {
            if (ModelState.IsValid)
            {
                var sCard = converter.ConvertTo(studentCardViewModel);
                await studentCardRepository.CreateAsync(sCard);
                return RedirectToAction("Index");
            }

            return View(studentCardViewModel);
        }

        [Route("deletecard")]
        [HttpPost]
        public async Task<IActionResult> DeleteStudentCard(Guid id)
        {
            var studentCard = await studentCardRepository.GetFullByIdAsync(id);

            if (studentCard != null)
            {
                if (studentCard.Passports != null && studentCard.Passports.Any())
                {
                    await passportInfoRepository.DeleteRange(studentCard.Passports);
                }
                if (studentCard.Registers != null && studentCard.Registers.Any())
                {
                    await registerRepository.DeleteRange(studentCard.Registers);
                }
                await studentCardRepository.DeleteAsync(studentCard);
            }
            return RedirectToAction("Index");
        }
        //--------------------------
        [HttpPost]
        [Route("addregister")]
        public async Task<IActionResult> AddRegisterToStudentCard(EditStudentCardViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.Registers == null)
                {
                    viewModel.Registers = new List<Register>();
                }

                viewModel.Registers.Insert(0, new Register());

                var newCard = converter.ConvertTo(viewModel);
                var questId = await studentCardRepository.CreateAsync(newCard);

                return RedirectToAction("EditStudentCard", new { id = questId });
            }

            var card = await studentCardRepository.GetAllFull()
                .Where(x => x.PersonalDetailsId == viewModel.PersonalDetailsId)
                .OrderByDescending(x => x.Created)
                .FirstOrDefaultAsync();

            return RedirectToAction("EditStudentCard", new { id = card.Id });
        }

        [HttpPost]
        [Route("removeregister")]
        public async Task<IActionResult> RemoveRegisterFromStudentCard(int index, EditStudentCardViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.Registers != null && viewModel.Registers.Count > index && index >= 0)
                {
                    viewModel.Registers.RemoveAt(index);
                    var newQuest = converter.ConvertTo(viewModel);
                    var questId = await studentCardRepository.CreateAsync(newQuest);
                    return RedirectToAction("EditStudentCard", new { id = questId });
                }
            }

            var quest = await studentCardRepository.GetAllFull()
                .Where(x => x.PersonalDetailsId == viewModel.PersonalDetailsId)
                .OrderByDescending(x => x.Created)
                .FirstOrDefaultAsync();

            return RedirectToAction("EditStudentCard", new { id = quest.Id });
        }
        //-----------
        [HttpPost]
        [Route("addpassportinfo")]
        public async Task<IActionResult> AddPassportInfoToStudentCard(EditStudentCardViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.Passports == null)
                {
                    viewModel.Passports = new List<PassportInfo>();
                }

                viewModel.Passports.Insert(0, new PassportInfo());

                var newCard = converter.ConvertTo(viewModel);
                var questId = await studentCardRepository.CreateAsync(newCard);

                return RedirectToAction("EditStudentCard", new { id = questId });
            }

            var card = await studentCardRepository.GetAllFull()
                .Where(x => x.PersonalDetailsId == viewModel.PersonalDetailsId)
                .OrderByDescending(x => x.Created)
                .FirstOrDefaultAsync();

            return RedirectToAction("EditStudentCard", new { id = card.Id });
        }

        [HttpPost]
        [Route("removepassportinfo")]
        public async Task<IActionResult> RemovePassportInfoFromStudentCard(int index, EditStudentCardViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.Passports != null && viewModel.Passports.Count > index && index >= 0)
                {
                    viewModel.Passports.RemoveAt(index);
                    var newQuest = converter.ConvertTo(viewModel);
                    var questId = await studentCardRepository.CreateAsync(newQuest);
                    return RedirectToAction("EditStudentCard", new { id = questId });
                }
            }

            var quest = await studentCardRepository.GetAllFull()
                .Where(x => x.PersonalDetailsId == viewModel.PersonalDetailsId)
                .OrderByDescending(x => x.Created)
                .FirstOrDefaultAsync();

            return RedirectToAction("EditStudentCard", new { id = quest.Id });
        }

        public IEnumerable<GetStudentCardViewModel> SearchStudentCards(GetStudentCardListViewModel viewModel)
        {
            var studentCards = studentCardRepository.GetAllFull().GroupBy(x => x.PersonalDetailsId).Select(x => x.OrderByDescending(f => f.Created).First()).AsEnumerable();

            if (viewModel.SearchStudentCard.CountryId != Guid.Empty)
            {
                studentCards = studentCards.Where(x => x.PersonalDetails.CountryId == viewModel.SearchStudentCard.CountryId);
            }
            if (viewModel.SearchStudentCard.NationalityId != Guid.Empty)
            {
                studentCards = studentCards.Where(x => x.PersonalDetails.NationalityId == viewModel.SearchStudentCard.NationalityId);
            }
            if (viewModel.SearchStudentCard.GenderId != Guid.Empty)
            {
                studentCards = studentCards.Where(x => x.PersonalDetails.GenderId == viewModel.SearchStudentCard.GenderId);
            }
            if (viewModel.SearchStudentCard.MaritalStatusId != Guid.Empty)
            {
                studentCards = studentCards.Where(x => x.PersonalDetails.MaritalStatusId == viewModel.SearchStudentCard.MaritalStatusId);
            }
            if (viewModel.SearchStudentCard.PassportValid != null)
            {
                studentCards = studentCards.Where(x => x.Passports.Any(p => p.ValidUntil >= DateTime.Now.Date) == viewModel.SearchStudentCard.PassportValid);
            }
            if (viewModel.SearchStudentCard.RegisterValid != null)
            {
                studentCards = studentCards.Where(x => x.Registers.Any(p => p.ValidUntil >= DateTime.Now.Date) == viewModel.SearchStudentCard.RegisterValid);
            }

            var res = studentCards.Select(x => new GetStudentCardViewModel
            {
                Id = x.Id,
                PersonalId = x.PersonalDetailsId,
                Created = x.Created,
                Name = x.PersonalDetails.Name,
                Surname = x.PersonalDetails.Surname ?? "",
                Patronymic = x.PersonalDetails.Patronymic ?? "",
                Email = x.PersonalDetails.Email ?? "",
                Gender = x.PersonalDetails.Gender?.Name ?? string.Empty,
                Country = x.PersonalDetails.Country?.Name ?? string.Empty,
                MaritalStatus = x.PersonalDetails.MaritalStatus?.Name ?? string.Empty,
                Nationality = x.PersonalDetails.Nationality?.Name ?? string.Empty,
                PassportValid = x.Passports.Any(x => x.ValidUntil >= DateTime.Now.Date),
                RegisterValid = x.Registers.Any(x => x.ValidUntil >= DateTime.Now.Date),
            });

            if (!string.IsNullOrEmpty(viewModel.SearchStudentCard.SearchString))
            {
                res = res.Where(x => x.ToString().ToLower().Contains(viewModel.SearchStudentCard.SearchString.ToLower()));
            }

            return res;
        }
    }
}
