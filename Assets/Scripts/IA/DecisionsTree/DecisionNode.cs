using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecisionNode : MonoBehaviour
{
    public abstract void Execute(Deadens deadens);
    public abstract void Execute(IllusionDemon i);

    public abstract void Execute(IllusionDuplications d);
}
