using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GbfRaidfinder.Data {
    public class Translation {
        public string translatedText { get; set; }
    }

    public class Data {
        public List<Translation> translations { get; set; }
    }

    public class TranslationResponse {
        public Data data { get; set; }
    }
}
