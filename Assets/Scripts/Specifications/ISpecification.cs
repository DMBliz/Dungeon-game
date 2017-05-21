public interface ISpecification
{
	string name { get; set; }

	void AddModificator(BaseSpecModificator modificator);

	void RemoveModificator(BaseSpecModificator modificator);
}
