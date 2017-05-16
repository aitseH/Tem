using UnityEngine;
using System.Collections;

using Tem;

namespace Tem
{

  [System.Serializable]

  public class Effect : Item
  {

    public string desp = "";
    public bool invincible;


    public Effect Clone()
    {

      Effect eff = new Effect();

      eff.ID = ID;
      eff.name = name;
      eff.icon = icon;

      eff.desp = desp;

      return eff;

    }
  }
}