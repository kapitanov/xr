using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace AISTek.XRage.Content.VmfParsing
{
    internal class VmfTokenStreamParser
    {
        public IEnumerable<VmfToken> ReadTokens(string vmfContent)
        {
            var tokens = new List<VmfToken>();

            var enumerator = new VmfStreamEnumerator(vmfContent.TrimStart());
            enumerator.MoveNext();
            while (true)
            {
                if (ParseSection(enumerator, tokens))
                {
                    if (enumerator.MoveNext())
                        continue;
                }

                break;
            }

            return tokens;
        }

        private bool ParseSection(VmfStreamEnumerator enumerator, List<VmfToken> tokens)
        {
            var buffer = new StringBuilder();
            var currentState = VmfTokenStreamParserState.Title;

            do
            {
                var ch = enumerator.Current;

                switch (currentState)
                {
                    case VmfTokenStreamParserState.Title:
                        if (ch == ' ')
                            continue;

                        if (ch == '\r')
                            continue;

                        if (ch == '\n')
                        {
                            // Proceed only if section name has been parsed
                            // Otherwise this line is considered as empty line
                            if (buffer.Length > 0)
                            {
                                //Debug.Print("section: {0}", buffer);
                                tokens.Add(VmfToken.ClassName(buffer.ToString()));
                                buffer.Clear();
                                currentState = VmfTokenStreamParserState.SectionStart;
                            }
                        }
                        else
                        {
                            buffer.Append(ch);
                        }
                        break;

                    case VmfTokenStreamParserState.SectionStart:
                        if (char.IsWhiteSpace(ch))
                            continue;

                        if (ch == '{')
                        {
                            //Debug.Print("section_begin");
                            tokens.Add(VmfToken.ClassOpening());
                            currentState = VmfTokenStreamParserState.SectionContent;
                        }
                        break;

                    case VmfTokenStreamParserState.SectionContent:
                        // We are in the beginning of the section's line
                        // This line may be an attribute line, if its first meaning character is "\""
                        // Otherwise this line is a nested section name line.

                        if (char.IsWhiteSpace(ch))
                            continue;

                        if (ch == '\"')
                        {
                            // Consider this line as an attribute line
                            currentState = VmfTokenStreamParserState.AttributeName;
                        }
                        else if (char.IsLetter(ch))
                        {
                            // Consider this line as a nested section name
                            //Debug.Print("nested_section");
                            ParseSection(enumerator, tokens);
                        }
                        else if (ch == '}')
                        {
                            //Debug.Print("section_end");
                            // Consider this line as end of section
                            tokens.Add(VmfToken.ClassClosing());
                            return true;
                        }
                        break;

                    case VmfTokenStreamParserState.AttributeName:
                        if (ch == '\"')
                        {
                            // This was an end of attribute's name
                            //Debug.Print("attribute_name: {0}", buffer);
                            tokens.Add(VmfToken.PropertyName(buffer.ToString()));
                            currentState = VmfTokenStreamParserState.AttributeDelimitter;
                            buffer.Clear();
                        }
                        else
                        {
                            buffer.Append(ch);
                        }
                        break;

                    case VmfTokenStreamParserState.AttributeDelimitter:
                        if (ch == '\"')
                        {
                            // This was a start of attribute's value
                            currentState = VmfTokenStreamParserState.AttributeValue;
                        }

                        break;

                    case VmfTokenStreamParserState.AttributeValue:
                        if (ch == '\"')
                        {
                            // This was a start of attribute's value                            
                            //Debug.Print("attribute_value: {0}", buffer);
                            tokens.Add(VmfToken.PropertyValue(buffer.ToString()));
                            currentState = VmfTokenStreamParserState.SectionContent;
                            buffer.Clear();
                        }
                        else
                        {
                            buffer.Append(ch);
                        }
                        break;
                }

            }  while (enumerator.MoveNext());

            return false;
        }
    }
}
