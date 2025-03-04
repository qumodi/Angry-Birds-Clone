using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal interface IData
{
    public void Load(GameData gameData);
    public void Save(GameData gameData);
}

