﻿using System.IO;
using System.Text;

namespace WCA.FirstTitle.Client.Resources
{
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding
        {
            get { return new UTF8Encoding(false); }
        }
    }
}
