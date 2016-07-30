using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Entities;
using AISTek.XRage.Content.SceneManagement;

namespace AISTek.XRage.SceneManagement
{
    public interface IEntityFactory
    {
        BaseEntity CreateEntity(CompiledEntity entity);
    }
}
