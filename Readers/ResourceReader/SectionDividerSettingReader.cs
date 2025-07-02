//Released under the MIT License.
//
//Copyright (c) 2015 Ntreev Soft co., Ltd.
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
//documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
//rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
//persons to whom the Software is furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
//Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
using Newtonsoft.Json.Linq;
using Ntreev.Library.Psd.Attributes;

namespace Ntreev.Library.Psd.Readers.ResourceReader;

[ResourceID("lsct", DisplayName = "SectionDividerSetting")]
internal class SectionDividerSettingReader(PsdBinaryReader reader, long length)
    : ValueReader<JToken>(reader, length, null)
    {
    protected override JObject ReadValue()
        {
        //var props = new Properties { ["SectionType"] = (SectionType)GlobalReader.ReadInt32() };
        var props = new JObject
            {
            ["SectionType"] = Enum.GetName((SectionType)GlobalReader.ReadInt32()),
            };
        return props;
        }
    }
