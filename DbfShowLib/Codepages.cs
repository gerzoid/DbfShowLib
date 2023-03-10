using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbfShowLib
{
    public struct CodePage
    {
        public string code { get; set; }
        public string codePage { get; set; }
        public string name { get; set; }
    }

    public class CodePages
    {
        List<CodePage> listCodePages;

        public CodePage codePage = new CodePage();
        //Инициализация
        public CodePages()
        {
            listCodePages = new List<CodePage>(){
                              new CodePage() { code = "0", codePage = "0", name = "None" },
                              new CodePage() { code = "1", codePage = "437", name = "US MS-DOS" },
                              new CodePage() { code = "2", codePage = "850", name = "Internatiol" },
                              new CodePage() { code = "3", codePage = "1252", name = "Windows ANSI Latin I" },
                              new CodePage() { code = "4", codePage = "10000", name = "Standard Macintosh" },
                              new CodePage() { code = "8", codePage = "865", name = "Danish OEM" },
                              new CodePage() { code = "9", codePage = "437", name = "Dutch OEM" },
                              new CodePage() { code = "10", codePage = "850", name = "Dutch OEM*" },
                              new CodePage() { code = "11", codePage = "437", name = "Finnish OEM" },
                              new CodePage() { code = "13", codePage = "437", name = "French OEM" },
                              new CodePage() { code = "14", codePage = "850", name = "French OEM*" },
                              new CodePage() { code = "15", codePage = "437", name = "German OEM" },
                              new CodePage() { code = "16", codePage = "850", name = "German OEM*" },
                              new CodePage() { code = "17", codePage = "437", name = "Italian OEM" },
                              new CodePage() { code = "18", codePage = "850", name = "Italian OEM*" },
                              new CodePage() { code = "19", codePage = "932", name = "Japanese" },
                              new CodePage() { code = "20", codePage = "850", name = "Spanish OEM*" },
                              new CodePage() { code = "21", codePage = "437", name = "Swedish OEM" },
                              new CodePage() { code = "22", codePage = "850", name = "Swedish OEM*" },
                              new CodePage() { code = "23", codePage = "865", name = "Norwegian OE" },
                              new CodePage() { code = "24", codePage = "437", name = "Spanish OEM" },
                              new CodePage() { code = "25", codePage = "437", name = "English OEM (Great Britain)" },
                              new CodePage() { code = "26", codePage = "850", name = "English OEM (Great Britain)*" },
                              new CodePage() { code = "27", codePage = "437", name = "English OEM (US)" },
                              new CodePage() { code = "28", codePage = "863", name = "French OEM (Canada)" },
                              new CodePage() { code = "29", codePage = "850", name = "French OEM*" },
                              new CodePage() { code = "31", codePage = "852", name = "Czech OEM" },
                              new CodePage() { code = "34", codePage = "852", name = "Hungarian" },
                              new CodePage() { code = "35", codePage = "852", name = "Polish OEM" },
                              new CodePage() { code = "36", codePage = "860", name = "Portuguese OEM" },
                              new CodePage() { code = "37", codePage = "850", name = "Portuguese OEM*" },
                              new CodePage() { code = "38", codePage = "866", name = "Russian OEM" },
                              new CodePage() { code = "55", codePage = "850", name = "English OEM (US)*" },
                              new CodePage() { code = "64", codePage = "852", name = "Romanian OEM" },
                              new CodePage() { code = "77", codePage = "936", name = "Chinese GBK (PRC)" },
                              new CodePage() { code = "78", codePage = "949", name = "Korean (ANSI/OEM)" },
                              new CodePage() { code = "79", codePage = "950", name = "Chinese Big5 (Taiwan)" },
                              new CodePage() { code = "80", codePage = "874", name = "Thai (ANSI/OEM)" },
                              new CodePage() { code = "87", codePage = "877", name = "ANSI" },
                              new CodePage() { code = "88", codePage = "1252", name = "Western European ANSI" },
                              new CodePage() { code = "89", codePage = "1252", name = "Spanish ANSI" },
                              new CodePage() { code = "100", codePage = "852", name = "Eastern European MS-DOS" },
                              new CodePage() { code = "101", codePage = "866", name = "Russian MS-DOS" },
                              new CodePage() { code = "102", codePage = "865", name = "Nordic MS-DOS" },
                              new CodePage() { code = "103", codePage = "861", name = "Icelandic MS-DOS" },
                              new CodePage() { code = "104", codePage = "895", name = "Kamenicky (Czech) MS-DOS" },
                              new CodePage() { code = "105", codePage = "620", name = "Mazovia (Polish) MS-DOS" },
                              new CodePage() { code = "106", codePage = "737", name = "Greek MS-DOS (437G)" },
                              new CodePage() { code = "107", codePage = "857", name = "Turkish MS-DOS" },
                              new CodePage() { code = "108", codePage = "863", name = "French-Canadian MS-DOS" },
                              new CodePage() { code = "120", codePage = "950", name = "Taiwan Big 5" },
                              new CodePage() { code = "121", codePage = "949", name = "Hangul (Wansung)" },
                              new CodePage() { code = "122", codePage = "936", name = "PRC GBK" },
                              new CodePage() { code = "123", codePage = "932", name = "Japanese Shift-JIS" },
                              new CodePage() { code = "124", codePage = "874", name = "Thai Windows/MS–DOS" },
                              new CodePage() { code = "134", codePage = "737", name = "Greek OEM" },
                              new CodePage() { code = "135", codePage = "852", name = "Slovenian OEM" },
                              new CodePage() { code = "136", codePage = "857", name = "Turkish OEM" },
                              new CodePage() { code = "150", codePage = "10007", name = "Russian Macintosh" },
                              new CodePage() { code = "151", codePage = "10029", name = "Eastern European Macintosh" },
                              new CodePage() { code = "152", codePage = "10006", name = "Greek Macintosh" },
                              new CodePage() { code = "200", codePage = "1250", name = "Eastern European Windows" },
                              new CodePage() { code = "201", codePage = "1251", name = "Russian Windows" },
                              new CodePage() { code = "202", codePage = "1254", name = "Turkish Windows" },
                              new CodePage() { code = "203", codePage = "1253", name = "Greek Windows" },
                              new CodePage() { code = "204", codePage = "1257", name = "Baltic Windows" },
                              new CodePage() { code = "254", codePage = "65001", name = "UTF-8" }};
        }

        public CodePage FindByCode(string code)
        {
            var t= listCodePages.Select(d => new CodePage() { code = d.code, codePage = d.codePage, name = d.name }).Where(p => p.code.Equals(code)).FirstOrDefault();
            return t;
        }
        public CodePage FindByCodePage(string codePage)
        {
            return listCodePages.Select(d => new CodePage() { code = d.code, codePage = d.codePage, name = d.name }).Where(p => p.codePage.Equals(codePage)).FirstOrDefault();
        }
        public CodePage FindByName(string name)
        {
            return listCodePages.Select(d => new CodePage() { code = d.code, codePage = d.codePage, name = d.name }).Where(p => p.name.Equals(name)).FirstOrDefault();
        }


    }
}
