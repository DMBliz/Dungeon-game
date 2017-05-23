using System;

public interface IModificator
{
	event Action<IModificator> OnEnd;
	Action a { get; set; }
	string Name { get; set; }
	string Description { get; set; }
	float WorkTime { get; set; }

	void Start();
}
