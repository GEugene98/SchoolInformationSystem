using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkScheduler.Services.Monitoring
{
    public class TalentedChildrenService
    {
        protected Context Db;
        protected DictionaryService DictionaryService;

        public TalentedChildrenService(Context context, DictionaryService dictionaryService)
        {
            Db = context;
            DictionaryService = dictionaryService;
        }
    }
}
