using System;
using UnityEngine;
using System.Collections;

public interface IAtribute
{
	event Action OnZero;
	string Name { get; set; }
	string Description { get; set; }


}
