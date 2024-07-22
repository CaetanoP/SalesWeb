using FluentValidation;

namespace SalesWebMVc.Models.Validator
{
	public class DepartmentValidator : AbstractValidator<Department>
	{
		public DepartmentValidator()
		{
			//Validating the name of the department
			RuleFor(x => x.Name)
				.NotEmpty()
				.WithMessage("The name of the department is mandatory")			
				.MaximumLength(50)
				.WithMessage("Exceeded the number of characters, maximum 50");
			//Validating the id of the department é maior ou igual a zero
			RuleFor(x => x.Id)
				.GreaterThanOrEqualTo(0)
				.WithMessage("The id of the department must be greater than or equal to zero");
			
		}
	}
}
