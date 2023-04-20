using DataLayer.Entities;
using DataLayer.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class KanjiNoteCardRepo : GenericRepo<KanjiNoteCard>, IKanjiNoteCardRepo
    {
        public KanjiNoteCardRepo(KanjiDbContext context) : base(context)
        {

        }
    }
}
