// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using InternTrack.Portal.Web.Models.Interns;
using InternTrack.Portal.Web.Models.Interns.Exceptions;

namespace InternTrack.Portal.Web.Services.Foundations.Interns
{
    public partial class InternService
    {
        private void ValidateInternOnAdd(Intern intern)
        {
            ValidateInternIsNotNull(intern);

            Validate(
                (Rule: IsInvalid(intern.Id), Parameter: nameof(Intern.Id)),
                (Rule: IsInvalid(intern.FirstName), Parameter: nameof(Intern.FirstName)),
                (Rule: IsInvalid(intern.MiddleName), Parameter: nameof(Intern.MiddleName)),
                (Rule: IsInvalid(intern.LastName), Parameter: nameof(Intern.LastName)),
                (Rule: IsInvalid(intern.Email), Parameter: nameof(Intern.Email)),
                (Rule: IsInvalid(intern.PhoneNumber), Parameter: nameof(Intern.PhoneNumber)),
                (Rule: IsInvalid(intern.Status), Parameter: nameof(Intern.Status)),
                (Rule: IsInvalid(intern.UpdatedDate), Parameter: nameof(Intern.UpdatedDate)),
                (Rule: IsInvalid(intern.CreatedDate), Parameter: nameof(Intern.CreatedDate)),
                (Rule: IsInvalid(intern.JoinDate), Parameter: nameof(Intern.JoinDate)),
                (Rule: IsInvalid(intern.CreatedBy), Parameter: nameof(Intern.CreatedBy)),
                (Rule: IsInvalid(intern.UpdatedBy), Parameter: nameof(Intern.UpdatedBy)));
        }

        private void ValidateInternOnModify(Intern intern)
        {
            ValidateInternIsNotNull(intern);

            Validate(
                (Rule: IsInvalid(intern.Id), Parameter: nameof(Intern.Id)),
                (Rule: IsInvalid(intern.FirstName), Parameter: nameof(Intern.FirstName)),
                (Rule: IsInvalid(intern.LastName), Parameter: nameof(Intern.LastName)),
                (Rule: IsInvalid(intern.Email), Parameter: nameof(Intern.Email)),
                (Rule: IsInvalid(intern.PhoneNumber), Parameter: nameof(Intern.PhoneNumber)),
                (Rule: IsInvalid(intern.Status), Parameter: nameof(Intern.Status)),
                (Rule: IsInvalid(intern.UpdatedDate), Parameter: nameof(Intern.UpdatedDate)),
                (Rule: IsInvalid(intern.CreatedDate), Parameter: nameof(Intern.CreatedDate)),
                (Rule: IsInvalid(intern.JoinDate), Parameter: nameof(Intern.JoinDate)),
                (Rule: IsInvalid(intern.CreatedBy), Parameter: nameof(Intern.CreatedBy)),
                (Rule: IsInvalid(intern.UpdatedBy), Parameter: nameof(Intern.UpdatedBy)),
                (Rule: IsNotRecent(intern.UpdatedDate), Parameter: nameof(Intern.UpdatedDate)),

                (Rule: IsSame(
                        firstDate: intern.UpdatedDate,
                        secondDate: intern.CreatedDate,
                        secondDateName: nameof(Intern.CreatedDate)),

                Parameter: nameof(Intern.UpdatedDate)));
        }
        
        private static void ValidateInternIsNotNull(Intern intern)
        {
            if (intern is null)
            {
                throw new NullInternException();
            }
        }

        private static void ValidateInternId(Guid internId) =>
            Validate((Rule: IsInvalid(internId), Parameter: nameof(Intern.Id)));

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTimeOffset();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidInternException = new InvalidInternException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidInternException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidInternException.ThrowIfContainsErrors();
        }
    }
}
