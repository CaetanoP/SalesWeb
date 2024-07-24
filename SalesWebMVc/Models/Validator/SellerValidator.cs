using FluentValidation;

namespace SalesWebMVc.Models.Validator
{
	public class SellerValidator : AbstractValidator<Seller>
	{
		public SellerValidator()
		{
			//Validating the name of the Seller
			//The name need to have only characters and 1 space between the first name and the last name
			RuleFor(x => x.Name)
				.NotEmpty()
				.WithMessage("The name of the seller is mandatory")
				.MaximumLength(50)
				.WithMessage("Exceeded the number of characters, maximum 50")
				.Matches(@"^[A-Za-zÀ-ÿ]+(\s[A-Za-zÀ-ÿ]+)+$")
				.WithMessage("The name of the seller must have only characters and 1 space between the first name and the last name");
			//Validating the email of the seller
			RuleFor(x => x.Email)
				.NotEmpty()
				.WithMessage("The email of the seller is mandatory")
				.Matches(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$")
				.WithMessage("Enter a valid email");
			//Validating the BaseSalary of the seller
			RuleFor(x => x.BaseSalary)
				.GreaterThanOrEqualTo(1420.0)
				.WithMessage("The BaseSalary of the seller must be greater than or equal to 1420")
				.LessThanOrEqualTo(50000.0)
				.WithMessage("The BaseSalary of the seller must be less than or equal to 50000.0");
			//Validatting the DepartmentId of the seller
			RuleFor(x => x.DepartmentId)
				.NotEmpty()
				.WithMessage("The department id is mandatory")
				.GreaterThanOrEqualTo(1)
				.WithMessage("The department id must be greater than or equal to 1")
				.NotNull()
				.WithMessage("The department id is mandatory");
			//Validate if the BirthDate is a valid date +18 years
			RuleFor(x => x.BirthDate)
				.NotEmpty()
				.WithMessage("The birth date is mandatory")
				.LessThan(DateTime.Now.AddYears(-18))
				.WithMessage("The seller must be at least 18 years old");
			
		}
	}

}
