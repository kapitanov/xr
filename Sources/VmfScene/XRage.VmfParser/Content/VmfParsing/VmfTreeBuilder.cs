using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AISTek.XRage.Content.VmfParsing
{
    internal class VmfTreeBuilder
    {
        public VmfDocument BuildTree(IEnumerable<VmfToken> tokens)
        {
            var nodes = new List<VmfNode>();
            var enumerator = new VmfTokenEnumerator(tokens);
            enumerator.MoveNext();

            while (true)
            {
                var node = BuildSection(enumerator);
                if (node != null)
                {
                    nodes.Add(node);
                }
                else
                {
                    break;
                }

                if (!enumerator.MoveNext())
                {
                    Debug.Print("End of stream reached");
                    break;
                }
            }

            return new VmfDocument(nodes);
        }

        private VmfNode BuildSection(VmfTokenEnumerator enumerator)
        {
            var container = new List<VmfNode>();
            var containerName = string.Empty;
            var attributeName = string.Empty;
            var state = VmfTreeBuilderState.Title;

            do
            {
                var token = enumerator.Current;

                switch (state)
                {
                    case VmfTreeBuilderState.Title:
                        if (token.Type == VmfTokenType.ClassName)
                        {
                            containerName = token.Value;
                            state = VmfTreeBuilderState.Start;
                        }
                        else
                        {
                            throw new Exception(string.Format("Token {0} is unexpected, expected token of type ", token, VmfTokenType.ClassName));
                        }
                        break;

                    case VmfTreeBuilderState.Start:
                        if (token.Type == VmfTokenType.ClassOpening)
                        {
                            state = VmfTreeBuilderState.Content;
                        }
                        else
                        {
                            throw new Exception(string.Format("Token {0} is unexpected, expected token of type ", token, VmfTokenType.ClassOpening));
                        }
                        break;

                    case VmfTreeBuilderState.Content:
                        if (token.Type == VmfTokenType.PropertyName)
                        {
                            attributeName = token.Value;
                            state = VmfTreeBuilderState.Attribute;
                        } 
                        else if (token.Type == VmfTokenType.ClassName)
                        {
                            container.Add(BuildSection(enumerator));
                        }
                        else if(token.Type == VmfTokenType.ClassClosing)
                        {
                            return new VmfClassNode(containerName, container);
                        }
                        else
                        {
                            throw new Exception(string.Format("Token {0} is unexpected", token));
                        }
                        break;

                    case VmfTreeBuilderState.Attribute:
                        if (token.Type == VmfTokenType.PropertyValue)
                        {
                            container.Add(new VmfPropertyNode(attributeName, token.Value));
                            attributeName = token.Value;
                            state = VmfTreeBuilderState.Content;
                        }
                        else
                        {
                            throw new Exception(string.Format("Token {0} is unexpected, expected token of type ", token, VmfTokenType.PropertyName));
                        }
                        break;
                }

            } while (enumerator.MoveNext());

            return null;
        }   
    }
}
