using MonitoringTheProgressOfForeignStudents.Domain.Model;
using MonitoringTheProgressOfForeignStudents.ViewModels.Abstract;

namespace MonitoringTheProgressOfForeignStudents.ViewModels.PersonalDetailsVM
{
    public class EditPersonalDetailsVMConverter : ConverterVM<EditPersonalDetailsViewModel, PersonalDetails>
    {
        public override EditPersonalDetailsViewModel ConvertFrom(PersonalDetails obj)
        {
            return new EditPersonalDetailsViewModel
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

        public override PersonalDetails ConvertTo(EditPersonalDetailsViewModel obj)
        {
            return new PersonalDetails
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

        public override PersonalDetails Update(EditPersonalDetailsViewModel from, PersonalDetails to)
        {
            to.Name = from.Name;
            to.Surname = from.Surname;
            to.Patronymic = from.Patronymic;
            to.Birthday = from.Birthday;
            to.PlaceofBirth = from.PlaceofBirth;
            to.PermamentHomeAddress = from.PermamentHomeAddress;
            to.PhoneNumber = from.PhoneNumber;
            to.PhoneNumberInBelarus = from.PhoneNumberInBelarus;
            to.Email = from.Email;
            to.NationalityId = from.NationalityId;
            to.MaritalStatusId = from.MaritalStatusId;
            to.GenderId = from.GenderId;
            to.CountryId = from.CountryId;
            return to;
        }
    }
}
