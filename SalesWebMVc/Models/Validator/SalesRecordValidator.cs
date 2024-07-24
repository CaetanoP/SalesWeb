using FluentValidation;

namespace SalesWebMVc.Models.Validator
{
	public class SalesRecordValidator : AbstractValidator<SalesRecord>
	{

		public SalesRecordValidator()
		{
			//Validating the amount of the sale
			RuleFor(x => x.Amount)
				.NotEmpty()
				.WithMessage("The amount of the sale is mandatory")
				.GreaterThan(0)
				.WithMessage("The amount of the sale must be greater than zero");
			//Validating the enum
			RuleFor(x => x.Status)
				.IsInEnum()
				.WithMessage("The status may be only Pending, Billed or Canceled ");
		}
	}
}
