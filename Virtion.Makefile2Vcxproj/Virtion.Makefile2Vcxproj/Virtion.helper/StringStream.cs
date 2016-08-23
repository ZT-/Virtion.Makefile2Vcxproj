using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Virtion.helper
{
    class StringStream
    {
        private string text;
        private int currentIndex;
        public StringStream(string s)
        {
            this.text = s;
        }

        public string ReadString()
        {
            bool needNext = false;
            List<char> list = new List<char>();
            for (int i = currentIndex; i < text.Length; i++, currentIndex++)
            {
                char c = text[i];
                if (c == ' ' || c == '\t')
                {
                    if (needNext == true)
                    {
                        break;
                    }
                    continue;
                }
                else
                {
                    needNext = true;
                    list.Add(c);
                }
            }
            return new string(list.ToArray());
        }

        public bool IsEof()
        {
            return currentIndex >= text.Length - 1;
        }

    }
}
