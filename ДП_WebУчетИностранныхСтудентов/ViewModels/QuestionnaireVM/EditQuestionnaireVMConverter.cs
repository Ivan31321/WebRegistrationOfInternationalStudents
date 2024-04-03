using MonitoringTheProgressOfForeignStudents.Domain.Model;
using MonitoringTheProgressOfForeignStudents.ViewModels.Abstract;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.QuestionnaireVM
{
    public class EditQuestionnaireVMConverter : ConverterVM<EditQuestionnaireViewModel, Questionnaire>
    {
        public override EditQuestionnaireViewModel ConvertFrom(Questionnaire obj)
        {
            return new EditQuestionnaireViewModel
            {
                PersonalDetailsId = obj.PersonalDetailsId,
                Name = obj.PersonalDetails.Name,
                Surname = obj.PersonalDetails.Surname,
                Patronymic = obj.PersonalDetails.Patronymic,
                Birthday = obj.PersonalDetails.Birthday,
                GenderId = obj.PersonalDetails.GenderId,
                CountryId = obj.PersonalDetails.CountryId,
                PlaceofBirth = obj.PersonalDetails.PlaceofBirth,
                NationalityId = obj.PersonalDetails.NationalityId,
                MaritalStatusId = obj.PersonalDetails.MaritalStatusId,
                PermamentHomeAddress = obj.PersonalDetails.PermamentHomeAddress,
                PhoneNumber = obj.PersonalDetails.PhoneNumber,
                PhoneNumberInBelarus = obj.PersonalDetails.PhoneNumberInBelarus,
                Email = obj.PersonalDetails.Email,
                //
                FathersName = obj.FathersName,
                FathersProfession = obj.FathersProfession,
                FathersPhone = obj.FathersPhone,
                MothersName = obj.MothersName,
                MothersProfession = obj.MothersProfession,
                MothersPhone = obj.MothersPhone,
                NameOfSchool = obj.NameOfSchool,
                AddressOfSchool = obj.AddressOfSchool,
                DateOfEntrance = obj.DateOfEntrance,
                DateOfCompletion = obj.DateOfCompletion,
                KnowledgeOfLanguages = obj.KnowledgeOfLanguages,
                RelativeToBeInformedInEmergency = obj.RelativeToBeInformedInEmergency,
                HowDidYouFindOutAboutBNTU = obj.HowDidYouFindOutAboutBNTU,
                //
                SpecialtyId = obj.SpecialtyId,
                EducationLanguageId = obj.EducationLanguageId,
                EducationTypeId = obj.EducationTypeId,
                //
                Orders = obj.Orders?.Where(x => x.OrderType == OrderType.Order).OrderByDescending(x=>x.Created).ToList() ?? new List<Order>(),
                Reprimands = obj.Orders?.Where(x => x.OrderType == OrderType.Reprimand).OrderByDescending(x => x.Created).ToList() ?? new List<Order>(),
                //
                Homes = obj.Homes?.OrderByDescending(x => x.Created).ToList() ?? new List<Home>(),
            };
        }

        public override Questionnaire ConvertTo(EditQuestionnaireViewModel obj)
        {
            var orders = new List<Order>();
            if (obj.Orders != null) orders.AddRange(obj.Orders);
            if (obj.Reprimands != null) orders.AddRange(obj.Reprimands);

            if (obj.Homes == null) obj.Homes = new List<Home>();

            var questionnaire = new Questionnaire
            {
                FathersName = obj.FathersName,
                FathersPhone = obj.FathersPhone,
                FathersProfession = obj.FathersProfession,
                MothersName = obj.MothersName,
                MothersPhone = obj.MothersPhone,
                MothersProfession = obj.MothersProfession,
                NameOfSchool = obj.NameOfSchool,
                AddressOfSchool = obj.AddressOfSchool,
                DateOfEntrance = obj.DateOfEntrance,
                DateOfCompletion = obj.DateOfCompletion,
                KnowledgeOfLanguages = obj.KnowledgeOfLanguages,
                RelativeToBeInformedInEmergency = obj.RelativeToBeInformedInEmergency,
                HowDidYouFindOutAboutBNTU = obj.HowDidYouFindOutAboutBNTU,
                SpecialtyId = obj.SpecialtyId,
                Orders = orders,
                Homes = obj.Homes,
                EducationLanguageId = obj.EducationLanguageId,
                EducationTypeId = obj.EducationTypeId
            };

            if (obj.PersonalDetailsId == null || obj.PersonalDetailsId == Guid.Empty)
            {
                questionnaire.PersonalDetails = new PersonalDetails
                {
                    Name = obj.Name,
                    Surname = obj.Surname,
                    Patronymic = obj.Patronymic,
                    Birthday = obj.Birthday,
                    PlaceofBirth = obj.PlaceofBirth,
                    PermamentHomeAddress = obj.PermamentHomeAddress,
                    PhoneNumber = obj.PhoneNumber,
                    PhoneNumberInBelarus = obj.PhoneNumberInBelarus,
                    Email = obj.Email,
                    NationalityId = obj.NationalityId,
                    MaritalStatusId = obj.MaritalStatusId,
                    GenderId = obj.GenderId,
                    CountryId = obj.CountryId,
                };
            }
            else
            {
                questionnaire.PersonalDetailsId = obj.PersonalDetailsId.Value;
            }

            return questionnaire;
        }

        public override Questionnaire Update(EditQuestionnaireViewModel from, Questionnaire to)
        {
            var orders = new List<Order>();
            if (from.Orders != null) orders.AddRange(from.Orders);
            if (from.Reprimands != null) orders.AddRange(from.Reprimands);

            if (from.Homes == null) from.Homes = new List<Home>();

            to.FathersName = from.FathersName;
            to.FathersProfession = from.FathersProfession;
            to.FathersPhone = from.FathersPhone;
            to.MothersName = from.MothersName;
            to.MothersProfession = from.MothersProfession;
            to.MothersPhone = from.MothersPhone;
            to.NameOfSchool = from.NameOfSchool;
            to.AddressOfSchool = from.AddressOfSchool;
            to.DateOfEntrance = from.DateOfEntrance;
            to.DateOfCompletion = from.DateOfCompletion;
            to.KnowledgeOfLanguages = from.KnowledgeOfLanguages;
            to.RelativeToBeInformedInEmergency = from.RelativeToBeInformedInEmergency;
            to.HowDidYouFindOutAboutBNTU = from.HowDidYouFindOutAboutBNTU;
            //
            to.SpecialtyId = from.SpecialtyId;
            to.EducationLanguageId = from.EducationLanguageId;
            to.EducationTypeId = from.EducationTypeId;
            //
            to.Orders = orders;
            //
            to.Homes = from.Homes;
            return to;
        }
    }
}
