using System;

[Serializable]
public abstract class BaseSpecModificator
{
	public string name;
	public string description;
	public float value;
	public EStatModificatorType ModificatorType;

	public BaseSpecModificator(string description, float value, EStatModificatorType type)
	{
		this.name = this.GetType().ToString();
		this.description = description;
		this.value = value;
		ModificatorType = type;
	}

	public BaseSpecModificator(string name, string description, float value, EStatModificatorType type)
	{
		this.name = name;
		this.description = description;
		this.value = value;
		ModificatorType = type;
	}

}

public enum EStatModificatorType
{
	Addition,
	Multiply
}
