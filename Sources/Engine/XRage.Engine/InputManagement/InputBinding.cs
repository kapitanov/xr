using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.InputManagement
{
    public abstract class InputBinding
    {
        public abstract bool EvaluateBinding(InputState inputState);
    }
}
