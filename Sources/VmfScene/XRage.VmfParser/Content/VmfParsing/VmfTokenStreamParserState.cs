using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Content.VmfParsing
{
    internal enum VmfTokenStreamParserState
    {
        Title,
        SectionStart,
        SectionContent,
        SectionEnd,
        AttributeName,
        AttributeDelimitter,
        AttributeValue
    }
}
